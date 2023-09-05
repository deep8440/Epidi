using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Amqp.Encoding;

namespace WebSocket
{
    internal class Program
    {
        public static string _apiKey;
        public static string _apiSecret;
        public static string eventHubConnectionString;
        public static string ConnectionString;
        private ClientWebSocket _socket = null;
        //private EventHubClient _eventHubClient;
        //this._Configuration.GetSection("DocumentUploadCred")["BlobAccountName"];
      
        static async Task Main(string[] args)
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsetting.json", optional: false);

            IConfiguration config = builder.Build();

            _apiKey = config["CommonUrl:ApiKey"];
            _apiSecret = config["CommonUrl:ApiSecret"];
            eventHubConnectionString = config["CommonUrl:EventHubConnectionString"];
            ConnectionString = config["CommonUrl:ConnectionString"];
            

            List<string> instruments = new List<string>();
            string channel, events;

            if (!string.IsNullOrWhiteSpace(args[0]))
            {
                foreach (var item in args[0].Split(","))
                {
                    instruments.Add(item);
                }
            }
            channel = args[1].ToString();
            events = args[2].ToString();
            Program prg = new Program();
            await Task.Run(() => prg.Connect(instruments, channel, events));
            Task.WaitAll();
        }

        public async Task Connect(List<string> instruments, string channel, string events)
        {
            _socket = new ClientWebSocket();
            Uri uri = new Uri(@"wss://api.gateio.ws/ws/v4/");
            Console.WriteLine("start connecting websocket");
            await _socket.ConnectAsync(uri, CancellationToken.None);
            Console.WriteLine("finished connecting websocket");
            var cancelToken = CancellationToken.None;

            try
            {
                TimeSpan t = DateTime.Now - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;

                var payload = instruments;
                //string channel = "spot.tickers";
                //string events = "subscribe";

                var data = new Dictionary<object, object> {
                        {
                            "time",
                            secondsSinceEpoch},
                        {
                            "channel",
                            channel},
                        {
                            "event",
                            events},
                        {
                            "payload",
                            payload}};

                var message = "channel=" + channel + "&event=" + events + "&time=" + secondsSinceEpoch;
                data["auth"] = new Dictionary<object, object> {
                                {
                                    "method",
                                    "api_key"},
                                {
                                    "KEY",
                                    _apiKey},
                                {
                                    "SIGN",
                                    this.getSignIn(message)}};
                string jsondata = JsonConvert.SerializeObject(data);
                await Send(_socket, jsondata, cancelToken);
                await Receive(_socket, cancelToken);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR - {ex.Message}");
            }
        }

        private async Task Send(ClientWebSocket socket, string data, CancellationToken stoppingToken) =>
        await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, stoppingToken);

        private async Task Receive(ClientWebSocket socket, CancellationToken stoppingToken)
        {
            //_eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString);

            //userfavorite database call
            SqlConnection sqlCon;
            sqlCon = new SqlConnection(ConnectionString);

            SqlCommand sql_cmnd = new SqlCommand("GetFavoriteInstruments", sqlCon);
            sql_cmnd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adpter = new SqlDataAdapter(sql_cmnd);
            DataTable instrumentList = new DataTable();
            adpter.Fill(instrumentList);

            List<UserFavoritesInstruments> userFavoritesInstruments = ConvertDataTable<UserFavoritesInstruments>(instrumentList);



            var buffer = new ArraySegment<byte>(new byte[2048]);

            while (!stoppingToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, stoppingToken);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    ms.Seek(0, SeekOrigin.Begin);
                    try
                    {
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            string data = await reader.ReadToEndAsync();
                            Console.WriteLine(data);
                            Root account = JsonConvert.DeserializeObject<Root>(data);

                            //var eventData = new EventData(Encoding.UTF8.GetBytes(data));
                            //await _eventHubClient.SendAsync(eventData);
                            var connectionString = "Endpoint=sb://streaminggateio.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=bAYCAaWM2pbA2CHUunH07YPP1K/qB9RA8/kOUdba1WU=";
                            var eventHubName = "demoinputhub";

                            //var connectionString = "Endpoint=sb://streaminggateio.servicebus.windows.net/;SharedAccessKeyName=key;SharedAccessKey=KMBO2sFF3VeZMGjrYCdbz7uCqRKojjom4+AEhKuMqbQ=";
                            //var eventHubName = "usergateio";

                            if (account != null)
                            {
                                if (account.result != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(account.result.currency_pair))
                                    {
                                        if (userFavoritesInstruments.Where(x => x.LPInstrumentName == account.result.currency_pair) != null)
                                        {
                                            await using (var producer = new EventHubProducerClient(connectionString,
                                            eventHubName))
                                            {
                                                using EventDataBatch eventBatch = await producer.CreateBatchAsync();
                                                //Root account = JsonConvert.DeserializeObject<Root>(data);
                                                account.result.GUID = Guid.NewGuid();
                                                //account.result.UserId = 1;
                                                string json = JsonConvert.SerializeObject(account);

                                                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(json)));
                                                await producer.SendAsync(eventBatch);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            };
        }
        private string getSignIn(string text)
        {
            using (var hmacsha256 = new HMACSHA512(Encoding.UTF8.GetBytes(_apiSecret)))
            {
                var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return Convert.ToBase64String(hash);
            }

        }

        public class Result
        {
            public string currency_pair { get; set; }
            public string last { get; set; }
            public string lowest_ask { get; set; }
            public string highest_bid { get; set; }
            public string change_percentage { get; set; }
            public string base_volume { get; set; }
            public string quote_volume { get; set; }
            public string high_24h { get; set; }
            public string low_24h { get; set; }

            public Guid GUID { get; set; }

        }

        public class Root
        {
            public int time { get; set; }
            public long time_ms { get; set; }
            public string channel { get; set; }
            public string @event { get; set; }
            public Result result { get; set; }
        }

        public class UserFavoritesInstruments
        {
            public string LPInstrumentName { get; set; }

            public int Userid { get; set; }

        }

        static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        object value = dr[column.ColumnName];
                        if (value == DBNull.Value)
                            value = null;
                        pro.SetValue(obj, value, null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }



    }
}

