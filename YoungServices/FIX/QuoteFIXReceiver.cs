using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using QuickFix;
using YoungServices.FIX;

namespace YoungServices
{
    public delegate void UpdateSymbolQuotesCallback(String requestId, Dictionary<Double, Double> bids, Dictionary<Double, Double> asks, Dictionary<Double, Double> trades, DateTime timestamp);

    public class QuoteFIXReceiver : IQouteReceiver
    {
        private SocketInitiator initiator = null;
        private QuickFixFeedApp application = null;

        public void SubscribeForQuotes(String requestId, SymbolBase symbol, UpdateSymbolQuotesCallback updateQuotesCallback)
        {
            SubscribeForQuotes(requestId, symbol, 0, updateQuotesCallback); // FULL BOOK by default
        }

        public void SubscribeForQuotes(String requestId, SymbolBase symbol, int depth, UpdateSymbolQuotesCallback updateQuotesCallback)
        {
            ((QuickFixFeedApp)application).AddSymbol(requestId, symbol, depth, updateQuotesCallback);
        }

        ~QuoteFIXReceiver()
        {
            Dispose();
        }

        public QuoteFIXReceiver(String fixFeedIniPath)
        {
            try
            {
                SessionSettings settings = new SessionSettings(fixFeedIniPath);
                ArrayList sessions = settings.getSessions();
                QuickFix.Dictionary dict = settings.get((QuickFix.SessionID) sessions[0]);
                String fixFeedPassword = dict.getString("password");
                FileStoreFactory storeFactory = new FileStoreFactory(settings);
                application = new QuickFixFeedApp(fixFeedPassword);
                FileLogFactory logFactory = new FileLogFactory(settings);
                MessageFactory messageFactory = new DefaultMessageFactory();
                initiator = new SocketInitiator(application, storeFactory, settings, logFactory /*optional*/, messageFactory);
                initiator.start();
            }
            catch (ConfigError e)
            {
                Console.WriteLine(e);
            }
        }

        public void Dispose()
        {
            if (initiator != null)
            {
                application.Dispose();
                application = null;
                initiator = null;
                GC.Collect();             
            }
        }

        public bool IsConnected()
        {
            if (initiator == null)
                return false;
            return initiator.isLoggedOn();
        }
    }
}

