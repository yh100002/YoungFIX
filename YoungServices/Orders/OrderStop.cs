using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoungServices
{
    public class OrderStop : OrderBase
    {
        private Double stopPrice;

        public OrderStop(DateTime dateTime, SymbolBase symbol, Position position, Int32 quantity, Double stopPrice)
            : base(dateTime, symbol, position, quantity)
        {
            this.stopPrice = stopPrice;
        }

        //public OrderStop(DateTime dateTime, SymbolBase symbol, Position position, Int32 quantity, Double stopPrice, OrderStatusCallback orderStatusCallback)
        //    : base(clientOrderId, dateTime, symbol, position, quantity, orderStatusCallback)
        //{
        //    this.stopPrice = stopPrice;
        //}

        public Double StopPrice
        {
            get { return stopPrice; }
        }
    }
}
