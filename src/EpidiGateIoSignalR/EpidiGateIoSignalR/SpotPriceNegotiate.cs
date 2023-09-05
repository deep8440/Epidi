using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azure.Messaging.EventHubs;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace EpidiGateIoSignalR
{
    class MsgObject
    {
        //public string id;
        public string message;
    };

    public static class SpotPriceNegotiate
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "gateioSignalRHub")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

    
    }

    public static class SpotPricesEventHubTrigger
    {
        [FunctionName("onconnect")]
        public static async Task<IActionResult> onConnect(
               [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            //Add User to the table storage
            try
            {

                //Used Upsert to check If a user record already exist in the table then update it with new connection id. If does not exist then insert the user.
                //This helps if the discoonect failed and the record was unable to be deleted.

                return new OkResult();
            }
            catch
            {
                return new BadRequestObjectResult("Error: Something went wrong!");
            }

            return new OkResult();
        }


        //[FunctionName("ondisconnect")]
        //public static async Task<IActionResult> onDisconnect([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        //    [Table("UserContext", Connection = "AzureTableStorage")] TableClient userTable)
        //{
        //    //Remove user from table storage
        //    try
        //    {
        //        if (String.IsNullOrEmpty(req.Query["userId"])) { return new BadRequestObjectResult("An error has occurred"); }
        //        if (String.IsNullOrEmpty(req.Query["deviceId"])) { return new BadRequestObjectResult("An error has occurred"); }
        //        await userTable.DeleteEntityAsync(req.Query["userId"], req.Query["deviceId"], ETag.All);
        //    }
        //    catch
        //    {
        //        return new BadRequestObjectResult("Error: Something went wrong!");
        //    }
        //    return new OkResult();
        //}

        [FunctionName("SpotPricesEventHubTriggerSignalR")]
        public static async Task Run([EventHubTrigger("demoinputhub", Connection = "streaminggateioEventHubConnStr")] EventData[] events,
            [SignalR(HubName = "gateioSignalRHub")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            var exceptions = new List<Exception>();

            
            foreach (EventData eventData in events)
            {

                if (eventData == null) { return; }

                try
                {
                    //message processing starts here

                    //var messageBody = JsonConvert.DeserializeObject<CurrentDeviceData>(Encoding.UTF8.GetString(eventData.EventBody));

                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {Encoding.UTF8.GetString(eventData.EventBody)}");


                    Root eventdata = JsonConvert.DeserializeObject<Root>(Encoding.UTF8.GetString(eventData.EventBody));

                    SqlConnection sqlCon;
                    sqlCon = new SqlConnection(@"Server=epidi-sql-server.database.windows.net;Database=Epidi_Dev;User Id=epidiadmin;Password=*aA123123;MultipleActiveResultSets=true;");

                    SqlCommand sql_cmnd = new SqlCommand("GET_MasterInstrumentId_By_InstrumentName", sqlCon);
                    sql_cmnd.CommandType = CommandType.StoredProcedure;
                    sql_cmnd.Parameters.Add(new SqlParameter("@InstrumentName", SqlDbType.VarChar)
                    {
                        Direction = ParameterDirection.Input,
                        Value = eventdata.result.currency_pair
                    });

                    SqlDataAdapter adpter = new SqlDataAdapter(sql_cmnd);
                    DataTable instrumentList = new DataTable();
                    adpter.Fill(instrumentList);

                    List<MasterInstrumentData> masterInstrumentData = ConvertDataTable<MasterInstrumentData>(instrumentList);


                    SqlCommand sql_cmnd2 = new SqlCommand("GET_Quote_MarkUp_Rule_By_UsersFavourite", sqlCon);
                    sql_cmnd2.CommandType = CommandType.StoredProcedure;
                    sql_cmnd2.Parameters.Add(new SqlParameter("@MasterInstrumentId", SqlDbType.Int){Direction = ParameterDirection.Input,Value = masterInstrumentData[0].MasterInstrumentId });
                    sql_cmnd2.Parameters.Add(new SqlParameter("@LPId", SqlDbType.Int){Direction = ParameterDirection.Input,Value = masterInstrumentData[0].LpId });

                    SqlDataAdapter adpter2 = new SqlDataAdapter(sql_cmnd2);
                    DataTable instrumentList2 = new DataTable();
                    adpter2.Fill(instrumentList2);

                    //foreach (TableEntity userEntity in userResults)
                    //{

                    //    MsgObject[] msgArgs = new[] { new MsgObject { id = messageBody.deviceid, message = JsonConvert.SerializeObject(messageBody) } };

                    string[] result = new string[] { Encoding.UTF8.GetString(eventData.EventBody) };

                    //    log.LogInformation(userEntity.GetString("ConnectionId") + " " + userEntity.GetString("RowKey") + " " + userEntity.GetString("PartitionKey"));

                    await signalRMessages.AddAsync(new SignalRMessage
                    {
                        //ConnectionId = "ItVDQcK-Tvf0GUxb2qIAIQe6340a601",
                        Target = "notify",
                        Arguments = result
                    });

                    await Task.Yield();
                    //}
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
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

        public int UserId { get; set; }
        public Guid GUID { get; set; }

        public double updatedprice { get; set; }

    }

    public class Root
    {
        public int time { get; set; }
        public long time_ms { get; set; }
        public string channel { get; set; }
        public string @event { get; set; }
        public Result result { get; set; }
    }

    public class MasterInstrumentData
    {
        public int MasterInstrumentId { get; set; }
        public int LpId { get; set; }
    }

}