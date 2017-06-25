using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungServices.FIX
{
    // Interface is required here to make transparent backtest execution
    public interface IOrderExecutor : IDisposable
    {
        void placeOrder(OrderBase order, OrderStatusCallback orderStatusCallback);
        void cancelOrder(String orderId);
        void UpdateQuote(Quote quote);
        Boolean IsConnected();
    }
}
