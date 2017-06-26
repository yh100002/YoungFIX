using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using QuickFix;
using QuickFix44;

namespace YoungServices
{
    public class QuickFixFeedApp : QuickFix.Application, IDisposable
    {
        private QuickFix.SessionID sessionID;
        //private UpdateSymbolQuotesCallback updateSymbolQuotesCallback = null;
        private Dictionary<String, UpdateSymbolQuotesCallback> updateSymbolQuotesCallbacks = new Dictionary<String, UpdateSymbolQuotesCallback>();
        private Dictionary<String, SymbolBase> symbols = new Dictionary<String, SymbolBase>();
        private String fixBrokerPassword;

        public QuickFixFeedApp(String fixBrokerPassword)
        {
            this.fixBrokerPassword = fixBrokerPassword;
        }

        public void Dispose()
        {
            try
            {
                Session.lookupSession(sessionID).logout("user requested");
            }
            catch (Exception)
            {
            }
        }


        public void AddSymbol(String requestId, SymbolBase symbol, int depth, UpdateSymbolQuotesCallback updateSymbolQuotesCallback)
        {
            this.updateSymbolQuotesCallbacks[requestId] = updateSymbolQuotesCallback;
            symbols[requestId] = symbol;
            subscribeForQuotes(symbol, requestId, depth);
        }

        public void Start()
        {
        }

        public void toApp(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            //Console.WriteLine($"QuickFixFeedApp toApp : {message}");
        }

        public void onCreate(QuickFix.SessionID sessionID)
        {
            Console.WriteLine($"QuickFixFeedApp onCreate : {sessionID}");
        }

        public void onLogout(QuickFix.SessionID sessionID)
        {
            Console.WriteLine($"QuickFixFeedApp onLogout : {sessionID}");
        }

        public void onLogon(QuickFix.SessionID sessionID)
        {
            this.sessionID = sessionID;

            Console.WriteLine($"QuickFixFeedApp onLogon : {sessionID}");
        }


        public void toAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Console.WriteLine($"QuickFixFeedApp toAdmin : {message}");

            MsgType msgType = new MsgType();
            message.getHeader().getField(msgType);
            if (msgType.getValue() == MsgType.Logon)
            {
                message.setField(new Password(this.fixBrokerPassword));
            }
        }

        public void fromApp(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Console.WriteLine($"QuickFixFeedApp fromApp : {message}");

            MsgType msgType = new MsgType();
            message.getHeader().getField(msgType);
            if (msgType.getValue() == MsgType.MarketDataSnapshotFullRefresh)
            {
                onMDSFR(message as MarketDataSnapshotFullRefresh, sessionID);
            }
        }

        public void fromAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Console.WriteLine($"QuickFixFeedApp fromAdmin : {message}");

        }

        public void onMDSFR(QuickFix44.MarketDataSnapshotFullRefresh message, QuickFix.SessionID sid)
        {
            try
            {
                String requestId = message.getMDReqID().ToString();
                NoMDEntries noMDEntries = message.getNoMDEntries();
                QuickFix44.MarketDataSnapshotFullRefresh.NoMDEntries group = new QuickFix44.MarketDataSnapshotFullRefresh.NoMDEntries();
                MDEntryType mdEntryType = new MDEntryType();
                MDEntryPx mdEntryPx = new MDEntryPx();
                MDEntrySize mDEntrySize = new MDEntrySize();
                Dictionary<Double, Double> bids = new Dictionary<Double, Double>();
                Dictionary<Double, Double> asks = new Dictionary<Double, Double>();
                Dictionary<Double, Double> trades = new Dictionary<Double, Double>();

                for (uint i = 1; i <= noMDEntries.getValue(); i++)
                {
                    message.getGroup(i, group);
                    group.get(mdEntryType);
                    group.get(mdEntryPx);
                    group.get(mDEntrySize);
                    switch (mdEntryType.getValue())
                    {
                        case MDEntryType.BID:
                            Double bid = mdEntryPx.getValue();
                            Double bidVolume = mDEntrySize.getValue();
                            bids[bid] = bidVolume;
                            break;
                        case MDEntryType.OFFER:
                            Double ask = mdEntryPx.getValue();
                            Double askVolume = mDEntrySize.getValue();
                            asks[ask] = askVolume;
                            break;
                        case MDEntryType.TRADE:
                            Double tradePrice = mdEntryPx.getValue();
                            Double tradeVolume = mDEntrySize.getValue();
                            trades[tradePrice] = tradeVolume;
                            break;

                    }
                }
                updateSymbolQuotesCallbacks[requestId].Invoke(requestId, bids, asks, trades, DateTime.Now);
            }
            catch (Exception)
            {                
            }
        }


        public void subscribeForQuotes(SymbolBase symbol, String requestId, int depth)
        {
            QuickFix44.MarketDataRequest marketDataRequest = new QuickFix44.MarketDataRequest();
            marketDataRequest.set(new MDReqID(requestId));
            marketDataRequest.set(new SubscriptionRequestType(SubscriptionRequestType.SNAPSHOT_PLUS_UPDATES));
            marketDataRequest.set(new MarketDepth(depth));
            marketDataRequest.set(new MDUpdateType(MDUpdateType.FULL_REFRESH));
            marketDataRequest.set(new AggregatedBook(true));
            QuickFix44.MarketDataRequest.NoMDEntryTypes group = new QuickFix44.MarketDataRequest.NoMDEntryTypes();
            group.set(new MDEntryType(MDEntryType.BID));
            marketDataRequest.addGroup(group);
            group.set(new MDEntryType(MDEntryType.OFFER));
            marketDataRequest.addGroup(group);
            group.set(new MDEntryType(MDEntryType.TRADE));
            marketDataRequest.addGroup(group);
            QuickFix44.MarketDataRequest.NoRelatedSym group2 = new QuickFix44.MarketDataRequest.NoRelatedSym();
            group2.set(new SecurityIDSource("111"));
            group2.set(new SecurityID(symbol.EXANTEId));
            group2.set(new Symbol(symbol.EXANTEId));
            marketDataRequest.addGroup(group2);
            Session.sendToTarget(marketDataRequest, sessionID);

            //Console.WriteLine($"QuickFixFeedApp subscribeForQuotes : {marketDataRequest}");
        }
    }
}