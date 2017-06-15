using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoungServices
{
    public enum OrderStatus { WORKING, REJECTED, PARTIALLY_FILLED, FILLED, CANCELED, UNKNOWN, OTHER };
    public enum OrderType { MARKET, LIMIT, OTHER };

    public abstract class OrderBase
    {
        private String clientOrderId;
        private String newOrderId;
        private SymbolBase symbol;
        private int quantity;
        private Position position;
        private Duration duration;
        private DateTime dateTime;
        private OrderStatus? orderStatus;
        private void Init(String clientOrderId, DateTime dateTime, SymbolBase symbol, Position position, Duration duration, Int32 quantity)
        {
            this.clientOrderId = clientOrderId;
            this.symbol = symbol;
            this.dateTime = dateTime;
            this.quantity = quantity;
            if (quantity < 0)
            {
                throw new Exception("negative quantity order");
            }
            this.position = position;
            this.duration = duration;
            this.orderStatus = null;
        }

        public OrderBase(DateTime dateTime, SymbolBase symbol, Position position, Int32 quantity)
        {
            MoveOrderIds(); MoveOrderIds();
            Init(clientOrderId, dateTime, symbol, position, Duration.Day, quantity);
        }


        public OrderBase(DateTime dateTime, SymbolBase symbol, Position position, Duration duration, Int32 quantity)
        {
            MoveOrderIds(); MoveOrderIds();
            Init(clientOrderId, dateTime, symbol, position, duration, quantity);
        }

        public OrderBase(String clientOrderId, DateTime dateTime, SymbolBase symbol, Position position, Int32 quantity)
        {
            Init(clientOrderId, dateTime, symbol, position, Duration.Day, quantity);
        }
        
        public OrderBase(String clientOrderId, DateTime dateTime, SymbolBase symbol, Position position, Duration duration, Int32 quantity)
        {
            Init(clientOrderId, dateTime, symbol, position, duration, quantity);
        }

        public void MoveOrderIds()
        {
            this.clientOrderId = this.newOrderId;
            this.newOrderId = System.Guid.NewGuid().ToString();
        }
        public String ClientOrderId
        {
            get { return clientOrderId; }
        }
        public String NewOrderId
        {
            get { return newOrderId; }
        }
        public DateTime TimeStamp
        {
            get { return dateTime; }
        }
        public SymbolBase Symbol
        {
            get { return symbol; }
        }
        public int Quantity
        {
            get { return quantity; }
        }
        public Position Position
        {
            get { return position; }
        }
        public OrderStatus? OrderStatus
        {
            set { this.orderStatus = value; }
            get { return orderStatus; }
        }
        public Duration Duration
        {
            set { this.duration = value; }
            get { return this.duration; }
        }
    }
}
