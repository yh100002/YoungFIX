using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;
using QuickFix44;

namespace YoungServices
{
    public class OrderFIX
    {
        // TODO: encapsulate members
        public ClOrdID clOrdID;
        public OrigClOrdID origClOrdID; // used for Cancel request only
        public OrderID orderID;
        public SecurityID securityID;
        public OrdStatus ordStatus;
        public OrdType ordType;
        public Side side;
        public TimeInForce duration;
        public OrderQty orderQty;
        public Price price;
        public LastQty lastQty;
        public AvgPx avgPx;

        public String ClientOrderId
        {
            get { return clOrdID.getValue().ToString(); }
        }

        public String OriginalClientOrderId
        {
            get { return origClOrdID != null?origClOrdID.getValue().ToString():null; }
        }


        public String EXANTEId
        {
            get { return securityID.getValue().ToString(); }
        }

        public Int32 InitialQuantity
        {
            get
            {
                return (int)orderQty.getValue();
            }
        }

        public Double InitialPrice
        {
            get
            {
                return price.getValue();
            }
        }

        public Int32 FilledQuantity
        {
            get
            {
                return (int)(lastQty != null?lastQty.getValue(): 0);
            }
        }

        public Double FillPrice
        {
            get
            {
                return (avgPx != null ? avgPx.getValue() : 0.0);
            }
        }


        public OrderType OrderType
        {
            get
            {
                switch (ordType.getValue())
                {
                    case OrdType.MARKET:
                        return OrderType.MARKET;
                    case OrdType.LIMIT:
                        return OrderType.LIMIT;
                    default:
                        return OrderType.OTHER;
                }
            }
        }

        public Position Position
        {
            get
            {
                switch (side.getValue())
                {
                    case Side.BUY:
                        return Position.Long;
                    case Side.SELL:
                        return Position.Short;
                    default:
                        return Position.Zero;
                }
            }
        }


        public Duration Duration
        {
            get
            {
                switch (duration.getValue())
                {
                    case TimeInForce.GOOD_TILL_CANCEL:
                        return Duration.GTC;
                    case TimeInForce.DAY:
                        return Duration.Day;
                    default:
                        return Duration.Day;
                }
            }
        }


        public OrderStatus OrderStatus
        {
            get
            {
                switch (ordStatus.getValue())
                {
                    case OrdStatus.NEW:
                    case OrdStatus.PENDING_NEW:
                        return OrderStatus.WORKING;
                    case OrdStatus.FILLED:
                        return OrderStatus.FILLED;
                    case OrdStatus.PARTIALLY_FILLED:
                        return OrderStatus.PARTIALLY_FILLED;
                    case OrdStatus.REJECTED:
                        return OrderStatus.REJECTED;
                    case OrdStatus.CANCELED:
                        return OrderStatus.CANCELED;
                    default:
                        return OrderStatus.OTHER;
                }
            }
        }
    }
}
