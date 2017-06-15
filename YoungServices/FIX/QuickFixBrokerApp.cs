using System;
using System.Threading;
using System.Linq;
using System.Text;
using QuickFix;
using QuickFix44;
using System.Collections.Generic;


namespace YoungServices
{
    public delegate void OrderStatusCallbackFIX(OrderFIX orderFIX);
    public delegate void MassOrderStatusCallbackFIX(OrderFIX orderFIX, String massStatusRequestID, Boolean isLastStatusMessage);


    class QuickFixBrokerApp : QuickFix.Application, IDisposable
    {
        private MassOrderStatusCallbackFIX massOrderStatusCallbackFIX = null;
        private OrderStatusCallbackFIX orderStatusCallbackFIX = null;
        private UpdateSymbolsListCallback symbolsCallback = null;
        private UpdateAccountSummaryCallback accountSummaryCallback = null;
        private SessionID sessionID;
        private String fixBrokerPassword;

        public QuickFixBrokerApp(MassOrderStatusCallbackFIX massOrderStatusCallback, OrderStatusCallbackFIX orderStatusCallback, String fixBrokerPassword)
        {
            this.fixBrokerPassword = fixBrokerPassword;
            this.massOrderStatusCallbackFIX = massOrderStatusCallback;
            this.orderStatusCallbackFIX = orderStatusCallback;
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
 
        public void placeOrder(OrderBase order)
        {
            ClOrdID clOrdID = new ClOrdID(order.ClientOrderId);
            Side side = GetOrderSide(order);

            NewOrderSingle placeOrder = null;
            if (order.GetType() == typeof(OrderMarket))
            {
                placeOrder = new NewOrderSingle(clOrdID, side, new TransactTime(order.TimeStamp), new OrdType(OrdType.MARKET));
            }
            if (order.GetType() == typeof(OrderLimit))
            {
                placeOrder = new NewOrderSingle(clOrdID, side, new TransactTime(order.TimeStamp), new OrdType(OrdType.LIMIT));
                placeOrder.set(new Price(((OrderLimit)order).Price));
            }
            if (order.GetType() == typeof(OrderStop))
            {
                placeOrder = new NewOrderSingle(clOrdID, side, new TransactTime(order.TimeStamp), new OrdType(OrdType.STOP));
                placeOrder.set(new StopPx(((OrderStop)order).StopPrice));
            }

            if (order.Duration == Duration.GTC)
            {
                placeOrder.set(new TimeInForce(TimeInForce.GOOD_TILL_CANCEL));
            }
            else
            {
                placeOrder.set(new TimeInForce(TimeInForce.DAY));
            }
            
            placeOrder.set(new SecurityIDSource("111"));
            placeOrder.set(new SecurityID(order.Symbol.EXANTEId));
            placeOrder.set(new Symbol(order.Symbol.EXANTEId));

            placeOrder.set(new OrderQty(order.Quantity));
            QuickFix.Session.sendToTarget(placeOrder, sessionID);
        }


        public void cancelOrder(OrderBase order)
        {
            OrigClOrdID clOrdID = new OrigClOrdID(order.ClientOrderId);
            order.MoveOrderIds();
            ClOrdID NewClOrdID = new ClOrdID(order.NewOrderId);
            OrderCancelRequest cancelOrder = new OrderCancelRequest(clOrdID, NewClOrdID, GetOrderSide(order), new TransactTime(order.TimeStamp));
            cancelOrder.set(new SecurityIDSource("111"));
            cancelOrder.set(new SecurityID(order.Symbol.EXANTEId));
            cancelOrder.set(new Symbol(order.Symbol.EXANTEId));
            cancelOrder.set(new OrderQty(order.Quantity));
            Session.sendToTarget(cancelOrder, sessionID);
        }


        private Side GetOrderSide (OrderBase order)
        {
            if (order.Position == Position.Long)
                return new Side(Side.BUY);
            else
            {
                return new Side(Side.SELL);
            }
        }

        public void AccountSummaryRequest(String requestId, UpdateAccountSummaryCallback callbackFun)
        {
            this.accountSummaryCallback = callbackFun;
            QuickFix44.Message message = new QuickFix44.Message(new MsgType("UASQ"));
            message.setString(20020, requestId);
            Session.sendToTarget(message, sessionID);
        }


        public void MassOrderStatusRequest(String requestId)
        {
            OrderMassStatusRequest request = new OrderMassStatusRequest(new MassStatusReqID(requestId), new MassStatusReqType(7));
            Session.sendToTarget(request, sessionID);
        }

        public void onCreate(QuickFix.SessionID sessionID)
        {
            Console.WriteLine("QuickFixBrokerApp onCreate");
        }


        public void onLogon(QuickFix.SessionID sessionID)
        {
            Console.WriteLine("QuickFixBrokerApp onLogon");
        }


        public void onLogout(QuickFix.SessionID sessionID)
        {
            Console.WriteLine("QuickFixBrokerApp onLogout");
        }


        public void toAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Console.WriteLine("QuickFixBrokerApp toAdmin");
            MsgType msgType = new MsgType();
            message.getHeader().getField(msgType);
            if (msgType.getValue() == MsgType.Logon)
            {
                onLogonMessage(message as Logon, sessionID);
            }
            else
            {
                onMessage(message, sessionID);
            }
        }


        public void fromAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Console.WriteLine("QuickFixBrokerApp fromAdmin");
        }


        public void toApp(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
        }


        public void fromApp(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            try
            {
                MsgType msgType = new MsgType();
                message.getHeader().getField(msgType);
                if (msgType.getValue() == MsgType.ExecutionReport)
                {
                    ParseExecutionReport(message);
                }
                else if (msgType.getValue() == MsgType.OrderCancelReject)
                {

                }
                else if (msgType.getValue() == MsgType.Reject)
                {

                }
                else if (msgType.getValue() == new MsgType("y").getValue())
                {
                    ParseSymbolsListResponse(message);
                }
                else if (msgType.getValue() == new MsgType("UASR").getValue())
                {
                    ParseAccountSummaryResponse(message);
                }
                else
                {
                    throw new Exception("unknown message type received");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private void ParseExecutionReport(QuickFix.Message message)
        {
            OrderFIX orderFIX = null;

            OrderID orderID = new OrderID();
            message.getField(orderID);
            if (orderID.getValue() != "NONE")
            {
                orderFIX = new OrderFIX();
                orderFIX.orderID = orderID;

                ClOrdID clOrdID = new ClOrdID();
                if (message.isSetField(clOrdID))
                {
                    message.getField(clOrdID);
                    orderFIX.clOrdID = clOrdID;
                }

                OrigClOrdID origClOrdID = new OrigClOrdID();
                if (message.isSetField(origClOrdID))
                {
                    message.getField(origClOrdID);
                    orderFIX.origClOrdID = origClOrdID;
                }

                SecurityID securityID = new SecurityID();
                if (message.isSetField(securityID))
                {
                    message.getField(securityID);
                    orderFIX.securityID = securityID;
                }

                OrdStatus ordStatus = new OrdStatus();
                message.getField(ordStatus);
                orderFIX.ordStatus = ordStatus;

                Side side = new Side();
                message.getField(side);
                orderFIX.side = side;

                TimeInForce duration = new TimeInForce();
                message.getField(duration);
                orderFIX.duration = duration;

                OrderQty orderQty = new OrderQty();
                message.getField(orderQty);
                orderFIX.orderQty = orderQty;

                OrdType ordType = new OrdType();
                message.getField(ordType);
                orderFIX.ordType = ordType;

                // TODO: Add STOP LIMIT etc support
                if (ordType.getValue() == OrdType.LIMIT)
                {
                    Price price = new Price();
                    message.getField(price);
                    orderFIX.price = price;
                }

                if (ordStatus.getValue() == OrdStatus.FILLED || ordStatus.getValue() == OrdStatus.PARTIALLY_FILLED)
                {
                    LastQty lastQty = new LastQty();
                    message.getField(lastQty);
                    orderFIX.lastQty = lastQty;
                    AvgPx avgPx = new AvgPx();
                    message.getField(avgPx);
                    orderFIX.avgPx = avgPx;
                }
            }

            MassStatusReqID massStatusRequestId = new MassStatusReqID();
            if (message.isSetField(massStatusRequestId))
            {
                // Part of mass order status request 
                message.getField(massStatusRequestId);
                LastRptRequested lastRptRequested = new LastRptRequested();
                message.getField(lastRptRequested);
                Boolean isLastMessage = lastRptRequested.getValue();
                massOrderStatusCallbackFIX(orderFIX, massStatusRequestId.getValue(), isLastMessage);
            }
            else
            {
                // Single Order
                orderStatusCallbackFIX(orderFIX);
            }
        }

        private void ParseSymbolsListResponse(QuickFix.Message message)
        {
            String requestId = message.getString(320);
            List<SymbolBase> symbolsList = new List<SymbolBase>();
            TotNoRelatedSym totalNumberOfSymbols = new TotNoRelatedSym();
            message.getField(totalNumberOfSymbols);
            NoRelatedSym numberOfSymbols = new NoRelatedSym();
            message.getField(numberOfSymbols);            
            SecurityList.NoRelatedSym securityListGroup = new SecurityList.NoRelatedSym();
            for (uint i = 1; i <= numberOfSymbols.getValue(); i++)
            {
                message.getGroup(i, securityListGroup);

                SecurityID exanteId = securityListGroup.getSecurityID();

                var symbol = new SymbolBase(exanteId.ToString());

                symbolsList.Add(symbol);

            }

            symbolsCallback(requestId, symbolsList, totalNumberOfSymbols.getValue());
        }



        private void ParseAccountSummaryResponse(QuickFix.Message message)
        {
            Position position = Position.Zero;
            Double quantity = 0;
            String requestId = message.getString(20020);
            Int32 totalNumberOfPositions = message.getInt(20021);
            SecurityID exanteId = new SecurityID();
            message.getField(exanteId);
            LongQty longQty = new LongQty();
            message.getField(longQty);
            ShortQty shortQty = new ShortQty();
            message.getField(shortQty);
            if (longQty.getValue() > 0)
            {
                position = Position.Long;
                quantity = longQty.getValue();
            }
            else if (shortQty.getValue() > 0)
            {
                position = Position.Short;
                quantity = shortQty.getValue();
            }
            accountSummaryCallback(requestId, exanteId.getValue(), position, quantity, totalNumberOfPositions, DateTime.Now);
        }


        public void onLogonMessage(QuickFix44.Logon logon, QuickFix.SessionID sessionID)
        {
            this.sessionID = sessionID;
            logon.setField(new Password(this.fixBrokerPassword));
        }

        public void onMessage(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Console.WriteLine("QuickFixBrokerApp onMessage");
            Console.WriteLine(message);
        }

        public void GetSymbolsList(String requestIDSymbols, UpdateSymbolsListCallback symbolsCallback)
        {
            this.symbolsCallback = symbolsCallback;
            QuickFix44.Message message = new QuickFix44.Message(new MsgType("x"));
            message.setString(320, requestIDSymbols);
            message.setInt(559, 4);
            Session.sendToTarget(message, sessionID);
        }
    }
}
