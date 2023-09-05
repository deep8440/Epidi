using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Com.Lmax.Api.OrderBook;

namespace Com.Lmax.Api
{
    class MarketDataClient
    {
        private Dictionary<long, InstrumentInfo> _instrumentInfoById;
        private int _failureCount = 5;

        private ISession _session;
        private long[] _instrumentIds;

        public MarketDataClient(long[] instrumentIds)
        {
            _instrumentIds = instrumentIds;
            if (_instrumentIds.Length == 0)
            {
                throw new ArgumentException("Must have at least one instrumentId");
            }
        }

        private void SessionDisconnected()
        {
            Console.WriteLine("Session Disconnected");

        }

        private void StreamFailure(Exception e)
        {
            Console.WriteLine("Error occured on the stream " + e.Message);
            Console.WriteLine(e.StackTrace);

            if (e is FileNotFoundException)
            {
                _session.Stop();
            }

            if (--_failureCount == -1)
            {
                _session.Stop();
            }
        }

        private void MarketDataUpdate(OrderBookEvent orderBookEvent)
        {
            long instrumentId = orderBookEvent.InstrumentId;
            decimal bestBid = GetBestPrice(orderBookEvent.BidPrices);
            decimal bestAsk = GetBestPrice(orderBookEvent.AskPrices);

            if (_instrumentInfoById.ContainsKey(instrumentId))
            {
                InstrumentInfo instrument;
                _instrumentInfoById.TryGetValue(instrumentId, out instrument);
                instrument.Update(bestBid, bestAsk, Convert.ToString(orderBookEvent.InstrumentId), orderBookEvent.MktClosePrice, orderBookEvent.HasLastTradedPrice ? orderBookEvent.LastTradedPrice : -1,
                    orderBookEvent.DailyHighestTradedPrice, orderBookEvent.DailyLowestTradedPrice,
                    orderBookEvent.HasValuationAskPrice ? orderBookEvent.ValuationAskPrice : -1, orderBookEvent.HasValuationBidPrice ? orderBookEvent.ValuationBidPrice : -1);
            }
        }

        private static decimal GetBestPrice(List<PricePoint> prices)
        {
            return prices.Count != 0 ? prices[0].Price : 0m;
        }

        private void OnLoginSuccess(ISession session)
        {
            Console.WriteLine("My accountId is: " + session.AccountDetails.AccountId);

            _session = session;
            _session.MarketDataChanged += MarketDataUpdate;
            _session.EventStreamFailed += StreamFailure;
            _session.EventStreamSessionDisconnected += SessionDisconnected;


            InitialiseInstruments();

            foreach (long instrumentId in _instrumentIds)
            {
                SubscribeToInstrument(instrumentId);
            }

            session.Start();
        }

        private void InitialiseInstruments()
        {
            _instrumentInfoById = new Dictionary<long, InstrumentInfo>();
            foreach (long instrumentId in _instrumentIds)
            {
                List<Instrument> instruments = new List<Instrument>();
                _session.SearchInstruments(new SearchRequest("id:" + instrumentId),
                    (i, b) => { instruments = i; },
                    FailureCallback("Search for instrument: " + instrumentId));
                if (instruments.Count > 0)
                {
                    //massive assumption that there's only one instrument since we searched by ID
                    _instrumentInfoById.Add(instrumentId, new InstrumentInfo(instruments[0]));

                }
            }


        }

        private void SubscribeToInstrument(long instrumentId)
        {
            Console.WriteLine("Subscribing to: {0}", instrumentId);

            _session.Subscribe(new OrderBookSubscriptionRequest(instrumentId),
                                               () => Console.WriteLine("Subscribed to instrument: " + instrumentId),
                                               failureResponse =>
                                               {
                                                   Console.WriteLine("Failed to subscribe to instrument: " +
                                                                     instrumentId);
                                                   throw new Exception("Error subscribing to instrument: " +
                                                                       instrumentId);
                                               });

        }

        private static OnFailure FailureCallback(string failedFunction)
        {
            return
                failureResponse =>
                Console.Error.WriteLine("Failed to " + failedFunction + " due to: " + failureResponse.Message);
        }

        public static void Main(string[] args)
        {
            //if (args.Length != 5)
            //{
            //    Console.WriteLine("Usage MarketDataClient <url> <username> <password> [CFD_DEMO|CFD_LIVE] instrumentId");
            //    Environment.Exit(-1);
            //}

            String url = "https://api.lmaxtrader.com/";
            String username = "EpidiNZ";
            String password = "Kleanthis123!@";
            ProductType productType = ProductType.CFD_LIVE;

            do
            {
                Console.WriteLine("Attempting to login to: {0} as {1}", url, username);

                LmaxApi lmaxApi = new LmaxApi(url);

                MarketDataClient marketDataClient = new MarketDataClient(new long[] { 100667, 100619, 4008, 100615, 4007 });

                lmaxApi.Login(new LoginRequest(username, password, productType), marketDataClient.OnLoginSuccess,
                    failureResponse => Console.WriteLine("Failed to log in"));

                Console.WriteLine("Logged out, pausing for 10s before retrying");
                Thread.Sleep(10000);
            }
            while (true);

        }

    }


    class InstrumentInfo
    {
        private long _instrumentId;
        private long _lastUpdate = 0;

        public InstrumentInfo(Instrument instrument)
        {
            _instrumentId = instrument.Id;
        }

        public long InstrumentId
        {
            get { return _instrumentId; }
            set { _instrumentId = value; }
        }

        public long LastUpdate
        {
            get { return _lastUpdate; }
            set { _lastUpdate = value; }
        }

        public decimal BestBid { get; set; }

        public decimal BestAsk { get; set; }

        public void Update(decimal bid, decimal ask, string instrumentid,
            decimal MktClosePrice, decimal LastTradedPrice, decimal DailyHighestTradedPrice, decimal DailyLowestTradedPrice, decimal ValuationAskPrice, decimal ValuationBidPrice)
        {
            BestBid = bid;
            BestAsk = ask;

            _lastUpdate = GetCurrentMilliseconds();

            Console.WriteLine("instrumentid: " + instrumentid + "; BestBid: " + BestBid + "; BestAsk: " + BestAsk + "; MktClosePrice:" + MktClosePrice + "; LastTradedPrice:" + LastTradedPrice +
                "; DailyHighestTradedPrice:" + DailyHighestTradedPrice +
                "; DailyLowestTradedPrice:" + DailyLowestTradedPrice +
                "; ValuationAskPrice:" + ValuationAskPrice +
                "; ValuationBidPrice:" + ValuationBidPrice +
                " At:" + _lastUpdate);
        }

        private static long GetCurrentMilliseconds()
        {
            DateTime staticDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - staticDate;
            return (long)timeSpan.TotalMilliseconds;
        }

    }

}
