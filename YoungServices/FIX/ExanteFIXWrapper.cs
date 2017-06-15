using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoungServices
{
    // Simple Quote
    public class Quote
    {
        private SymbolBase symbol;
        private DateTime timestamp;
        private Double? bid;
        private Double? ask;

        public Double? Bid
        {
            get { return bid; }
            private set { this.bid = value; }
        }

        public Double? Ask
        {
            get { return ask; }
            private set { this.ask = value; }
        }

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { this.timestamp = value; }
        }

        public SymbolBase Symbol
        {
            get { return symbol; }
            private set { this.symbol = value; }
        }

        public Double? Middle()
        {
            return ((this.Bid + this.Ask) ?? (this.Bid * 2) ?? (this.Ask * 2)) / 2;
        }


        public Quote(SymbolBase symbol, Double? bid, Double? ask, DateTime timestamp)
        {
            this.symbol = symbol;
            this.Bid = bid;
            this.Ask = ask;
            this.Timestamp = timestamp;
        }
    }

    public enum Position { Long, Short, Zero };
    public enum Duration { Day, GTC };


    // Interface is required here to make transparent backtest execution
    public interface IOrderExecutor: IDisposable
    {
        void placeOrder(OrderBase order, OrderStatusCallback orderStatusCallback);
        void cancelOrder(String orderId);
        void UpdateQuote(Quote quote);
        Boolean IsConnected();
    }

    public interface IQouteReceiver : IDisposable
    {
        //void SubscribeForQuotes(String requestId, SymbolBase symbol, UpdateSymbolQuotesCallback updateQuotesCallback);
        void SubscribeForQuotes(String requestId, SymbolBase symbol, int depth, UpdateSymbolQuotesCallback updateQuotesCallback);
        Boolean IsConnected();
    }
}
