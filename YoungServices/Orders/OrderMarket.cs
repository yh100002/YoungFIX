using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoungServices
{
    public class OrderMarket: OrderBase
    {
        public OrderMarket(DateTime dateTime, SymbolBase symbol, Position position, Int32 quantity)
            : base(dateTime, symbol, position, quantity)
        {

        }

        public OrderMarket(String clientOrderId, DateTime dateTime, SymbolBase symbol, Position position, Int32 quantity)
            : base(clientOrderId, dateTime, symbol, position, quantity)
        {
        }

    }
}
