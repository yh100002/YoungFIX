using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungServices.FIX
{
    public interface IQouteReceiver : IDisposable
    {
        //void SubscribeForQuotes(String requestId, SymbolBase symbol, UpdateSymbolQuotesCallback updateQuotesCallback);
        void SubscribeForQuotes(String requestId, SymbolBase symbol, int depth, UpdateSymbolQuotesCallback updateQuotesCallback);
        Boolean IsConnected();
    }
}
