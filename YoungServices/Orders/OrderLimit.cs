using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoungServices
{
    public class OrderLimit: OrderBase
    {
        private Double price;

        public OrderLimit(DateTime dateTime, SymbolBase symbol, Position position, Int32 quantity, Double price)
            : base(dateTime, symbol, position, Duration.Day, quantity)
        {
            this.price = price;
        }

        
        public OrderLimit(DateTime dateTime, SymbolBase symbol, Position position, Duration duration, Int32 quantity, Double price)
            : base(dateTime, symbol, position, duration, quantity)
        {
            this.price = price;
        }

        public OrderLimit(String clientOrderId, DateTime dateTime, SymbolBase symbol, Position position, Int32 quantity, Double price)
            : base(clientOrderId, dateTime, symbol, position, Duration.Day, quantity)
        {
            this.price = price;
        }


        public OrderLimit(String clientOrderId, DateTime dateTime, SymbolBase symbol, Position position, Duration duration, Int32 quantity, Double price)
            : base(clientOrderId, dateTime, symbol, position, duration, quantity)
        {
            this.price = price;
        }


        public Double Price
        {
            get { return price; }
        }
    }
}
