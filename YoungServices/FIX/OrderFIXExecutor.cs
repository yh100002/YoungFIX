using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using QuickFix;

namespace YoungServices
{
    public delegate void UpdateSymbolsListCallback(String requestIDSymbols, List<SymbolBase> symbols, Int32 totalNumberOfSymbols);
    public delegate void UpdateAccountSummaryCallback(String requestId, String ExanteId, Position position, Double quantity, Int32 totalNumberOfPositions, DateTime timestamp);
    public delegate void OrderStatusCallback(string orderId, OrderStatus orderStatus, Double filledQauntity, Double fillPrice);
    public delegate void MassOrderStatusCallback(String massStatusRequestID, Boolean isLastStatusMessage);
    
    public class OrderFIXExecutor: IOrderExecutor
    {
        private SocketInitiator initiator = null;
        private QuickFixBrokerApp application = null;
        private Dictionary<String, OrderBase> activeOrders = new Dictionary<String, OrderBase>();
        private Dictionary<String, OrderStatusCallback> orderStatusCallbacks = new Dictionary<string,OrderStatusCallback>();
        private OrderStatusCallback defaultOrderStatusCallback;
        private MassOrderStatusCallback massOrderStatusCallback = null;

        public OrderFIXExecutor(String fixBrokerIniPath, OrderStatusCallback defaultOrderStatusCallback)
        {
            this.defaultOrderStatusCallback = defaultOrderStatusCallback;
            try
            {
                SessionSettings settings = new SessionSettings(fixBrokerIniPath);
                ArrayList sessions = settings.getSessions();
                QuickFix.Dictionary dict = settings.get((QuickFix.SessionID) sessions[0]);
                String fixBrokerPassword = dict.getString("password");
                FileStoreFactory storeFactory = new FileStoreFactory(settings);
                application = new QuickFixBrokerApp(MassOrderStatusCallbackFIX, OrderStatusCallbackFIX, fixBrokerPassword);
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
        
        public OrderBase[] GetActiveOrdersCopy()
        {
            return activeOrders.Values.ToArray();
        }

        private bool HasCallback(OrderFIX orderFIX)
        {
            return (orderStatusCallbacks.Keys.Contains(orderFIX.ClientOrderId));
        }

        private void UpdateOrder(OrderFIX orderFIX)
        {
            if (orderFIX == null)
                return;
            OrderStatus status = orderFIX.OrderStatus;
            if (status == OrderStatus.CANCELED) // TODO: Add Replace
            {
                OrderBase canceledOrder = activeOrders[orderFIX.OriginalClientOrderId];
                activeOrders.Remove(orderFIX.OriginalClientOrderId);
                activeOrders[orderFIX.ClientOrderId] = canceledOrder;
                if (orderStatusCallbacks.Keys.Contains(orderFIX.OriginalClientOrderId))
                {
                    OrderStatusCallback orderStatusCallback = orderStatusCallbacks[orderFIX.OriginalClientOrderId];
                    orderStatusCallbacks.Remove(orderFIX.OriginalClientOrderId);
                    orderStatusCallbacks[orderFIX.ClientOrderId] = orderStatusCallback;
                }
            }
            if (!activeOrders.ContainsKey(orderFIX.ClientOrderId))
            {
                OrderBase tempOrder;
                // 1. Create new order obeject
                OrderType type = orderFIX.OrderType;
                Position position = orderFIX.Position;
                Duration duration = orderFIX.Duration;
                switch (type)
                {
                    case OrderType.MARKET:
                        tempOrder = new OrderMarket(orderFIX.ClientOrderId, DateTime.Now, new SymbolBase(orderFIX.EXANTEId), position, orderFIX.InitialQuantity);
                        break;
                    case OrderType.LIMIT:
                        tempOrder = new OrderLimit(orderFIX.ClientOrderId, DateTime.Now, new SymbolBase(orderFIX.EXANTEId), position, duration, orderFIX.InitialQuantity, orderFIX.InitialPrice);
                        break;
                    default:
                        throw new Exception("unsupported order type");
                }
                activeOrders[orderFIX.ClientOrderId] = tempOrder;
            }
            OrderBase order = activeOrders[orderFIX.ClientOrderId];
            order.OrderStatus = status;
        }


        public void MassOrderStatusCallbackFIX(OrderFIX orderFIX, String massStatusRequestID, Boolean isLastStatusMessage)
        {
            if (massOrderStatusCallback == null)
                return;
            UpdateOrder(orderFIX);
            massOrderStatusCallback(massStatusRequestID, isLastStatusMessage);
        }


        public void OrderStatusCallbackFIX(OrderFIX orderFIX)
        {
            UpdateOrder(orderFIX);
            OrderBase order = activeOrders[orderFIX.ClientOrderId];
            if (HasCallback(orderFIX))
            {
                orderStatusCallbacks[orderFIX.ClientOrderId](orderFIX.ClientOrderId, orderFIX.OrderStatus, orderFIX.FilledQuantity, orderFIX.FillPrice);
                //TODO: clear old data from orderStatusCallbacks on final state update
            }
            else
            {
                if (defaultOrderStatusCallback != null)
                {
                    defaultOrderStatusCallback(orderFIX.ClientOrderId, orderFIX.OrderStatus, orderFIX.FilledQuantity, orderFIX.FillPrice);
                }
            }
        }


        public void placeOrder(OrderBase order, OrderStatusCallback orderStatusCallback)
        {
            activeOrders[order.ClientOrderId] = order;
            orderStatusCallbacks[order.ClientOrderId] = orderStatusCallback;
            application.placeOrder(order);
        }

        public void cancelOrder(String clientOrderId)
        {
            if (activeOrders.Keys.Contains(clientOrderId))
                cancelOrder(activeOrders[clientOrderId]);
        }

        public void cancelOrder(OrderBase order)
        {
            application.cancelOrder(order);
        }

        public void AccountSummaryRequest(String requestId, UpdateAccountSummaryCallback callbackFun)
        {
            application.AccountSummaryRequest(requestId, callbackFun);
        }

        public void MassOrderStatusRequest(String requestId, MassOrderStatusCallback massOrderStatusCallback)
        {
            this.massOrderStatusCallback = massOrderStatusCallback;
            application.MassOrderStatusRequest(requestId);
        }


        public void UpdateQuote(Quote quote)
        {            
        }


        public void GetSymbolsList(String requestIDSymbols, UpdateSymbolsListCallback symbolsCallback)
        {
            ((QuickFixBrokerApp)application).GetSymbolsList(requestIDSymbols, symbolsCallback);
        }
    }
}
