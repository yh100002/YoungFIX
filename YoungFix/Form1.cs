using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Concurrent;
using OfficeOpenXml;
using System.IO;

using YoungServices;



namespace YoungFix
{
    public partial class Form1 : Form
    {
        private string targetMarket = "ODAX.EUREX";
        private String requestIdMD;
        private bool isCall;
        private int currentStrikePrice;
        private string currentExpirationDate;
        private string currentSavePath;
        private SymbolBase currentSymbol;

        private static QuoteFIXReceiver feedFIX = null;
        private static OrderFIXExecutor executionFIX = null;
        private string FIXIniVendorPath = @"FIX\fix_vendor.ini";
        private string FIXIniBrokerPath = @"FIX\fix_broker.ini";
        private Thread updateConnectionStatus;

        private String requestIDSymbols;
        private int totalSymbolsCount = 0;
        private ConcurrentBag<SymbolBase> allSymbols = new ConcurrentBag<SymbolBase>();        
        private ConcurrentDictionary<string, SymbolBase> odaxSymbolsDic = new ConcurrentDictionary<string, SymbolBase>();        

        private BackgroundWorker myWorker1 = new BackgroundWorker();

        public Form1()
        {
            InitializeComponent();

            myWorker1.DoWork += new DoWorkEventHandler(GeneratingWorker);
            myWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myWorker1_RunWorkerCompleted);
            myWorker1.ProgressChanged += new ProgressChangedEventHandler(myWorker1_ProgressChanged);
            myWorker1.WorkerReportsProgress = true;
            myWorker1.WorkerSupportsCancellation = true;

            button1.Enabled = false;
            button3.Enabled = false;

            labelConnectionFeedStatusValue.Text = "CONNECTION";
            labelConnectionFeedStatusValue.ForeColor = Color.Red;
            labelConnectionFeedStatusValue.BackColor = Color.Red;

            labelConnectionBrokerStatusValue.Text = "CONNECTION";
            labelConnectionBrokerStatusValue.ForeColor = Color.Red;
            labelConnectionBrokerStatusValue.BackColor = Color.Red;

            currentSavePath = Path.GetDirectoryName(Application.ExecutablePath);
            lblSavePath.Text = currentSavePath;

            cmbOpt.Items.Add("Call");
            cmbOpt.Items.Add("Put");

            InitPriceControl();

            InitOrdersControl();
        }

        private void InitFIXConnection()
        {
            feedFIX = new QuoteFIXReceiver(FIXIniVendorPath);
            executionFIX = new OrderFIXExecutor(FIXIniBrokerPath, OrderStatusUpdate);
            updateConnectionStatus = new Thread(UpdateConnectionStatus);
            updateConnectionStatus.Start();
        }

        private void KillFIXConnection()
        {
            if (feedFIX != null)
            {
                feedFIX.Dispose();
                feedFIX = null;
                GC.Collect();
            }
            if (executionFIX != null)
            {
                executionFIX.Dispose();
                executionFIX = null;
                GC.Collect();
            }
        }

        private void UpdateConnectionStatus()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (this.IsHandleCreated)
                {
                    if (feedFIX != null && feedFIX.IsConnected())
                    {
                        labelConnectionFeedStatusValue.Text = "CONNECTION";
                        labelConnectionFeedStatusValue.ForeColor = Color.Green;
                        labelConnectionFeedStatusValue.BackColor = Color.Green;
                    }
                    else
                    {
                        labelConnectionFeedStatusValue.Text = "CONNECTION";
                        labelConnectionFeedStatusValue.ForeColor = Color.Red;
                        labelConnectionFeedStatusValue.BackColor = Color.Red;
                    }

                    if (executionFIX != null && executionFIX.IsConnected())
                    {
                        labelConnectionBrokerStatusValue.Text = "CONNECTION";
                        labelConnectionBrokerStatusValue.ForeColor = Color.Green;
                        labelConnectionBrokerStatusValue.BackColor = Color.Green;
                    }
                    else
                    {
                        labelConnectionBrokerStatusValue.Text = "CONNECTION";
                        labelConnectionBrokerStatusValue.ForeColor = Color.Red;
                        labelConnectionBrokerStatusValue.BackColor = Color.Red;
                    }
                    /*
                    labelConnectionFeedStatusValue.Invoke(new Action(() =>
                    {
                        
                    }
                    ));
                    labelConnectionBrokerStatus.Invoke(new Action(() =>
                    {
                        
                    }
                    ));*/
                }
            }
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            KillFIXConnection();

            InitFIXConnection();
            // Demo - can be removed
            //Thread.Sleep(1000);
            //SetActiveSymbol(new SymbolBase(@"TEST-RANDOM.TEST"));
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            KillFIXConnection();
        }

        public void OrderStatusUpdate(string orderId, OrderStatus orderStatus, Double filledQauntity, Double fillPrice)
        {
            RedrawOrders();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetSymbolsList();
        }

        private void GetSymbolsList()
        {
            try
            {
                this.requestIDSymbols = System.Guid.NewGuid().ToString();

                executionFIX.GetSymbolsList(requestIDSymbols, UpdateSymbolsList);
            }
            catch(Exception)
            { }
            
        }
                

        public void UpdateSymbolsList(String requestId, List<SymbolBase> symbols, Int32 totalSymbolsCount)
        {
            if (this.requestIDSymbols != requestId)
                return;

            this.totalSymbolsCount = totalSymbolsCount;

            foreach (var elem in symbols)
            {
                allSymbols.Add(elem);   
            }

            UpdateProgressBar();           

        }

        private void UpdateProgressBar()
        {
            progressBarLoadSymbols.Invoke(new Action(() =>
            {
                progressBarLoadSymbols.Maximum = this.totalSymbolsCount;

                progressBarLoadSymbols.Value = allSymbols.Count;

                if (allSymbols.Count >= totalSymbolsCount)
                {                    
                    FilterSymbol(targetMarket);

                    UpdateSymbolsCombo();
                }
            }
            ));
        }

        private void UpdateSymbolsCombo()
        {
            cmbExp.DisplayMember = "Value";

            cmbExp.ValueMember = "Value";

            var groups = from item in odaxSymbolsDic
                         group item.Value.ExpirationDate by item.Value.ExpirationDate
                         into grouped
                         select grouped;

            
            cmbExp.Invoke(new Action(() =>
            {
                SuspendConrolRedraw(cmbExp);

                cmbExp.Items.Clear();

                foreach (var g in groups)
                {
                    cmbExp.Items.Add(g.Key);
                }

                ResumeControlRedraw(cmbExp);
            }
            
            ));

            cmbExp.SelectedIndex = 0;

            cmbOpt.SelectedIndex = 0;
        }
                
        public static void SuspendConrolRedraw(Control control)
        {
            Message msgSuspendUpdate = Message.Create(control.Handle, 0x000B, IntPtr.Zero,IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);

            window.DefWndProc(ref msgSuspendUpdate);
        }

        public static void ResumeControlRedraw(Control control)
        {            
            IntPtr wparam = new IntPtr(1);

            Message msgResumeUpdate = Message.Create(control.Handle, 0x000B, wparam,IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);

            window.DefWndProc(ref msgResumeUpdate);

            control.Invalidate();
        }

        private void FilterSymbol(string symbolName)
        {
            var odaxSymbols = allSymbols.Where(x => x.EXANTEId.Contains(symbolName)).ToList();

            foreach(var item in odaxSymbols)
            {
                string group = item.EXANTEId.Split('.').Skip(3).Take(1).SingleOrDefault();

                item.ExpirationDate = item.EXANTEId.Split('.').Skip(2).Take(1).SingleOrDefault();
                
                if(group != null)
                {
                    var optValue = group.ToArray().Take(1).SingleOrDefault();

                    item.StrikePrice = Int32.Parse(group.Substring(1));

                    if (optValue == 'C')
                    {
                        item.IsCall = true;
                    }
                }

                string fileName = item.EXANTEId;

                string end = "." + item.EXANTEId.Split('.').Skip(3).Take(1).SingleOrDefault();

                fileName = fileName.Replace(end, "") + "_" + item.StrikePrice + ".xlsx";

                item.QuotesReportName = fileName;

                odaxSymbolsDic.TryAdd(Guid.NewGuid().ToString(), item);

            }

            SubscribeForMarketData();

            button3.Enabled = true;
        }       

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.ShowNewFolderButton = true;

            fbd.SelectedPath = currentSavePath;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                lblSavePath.Text = fbd.SelectedPath;

                currentSavePath = fbd.SelectedPath;

                button1.Enabled = true;
            }
        }

        private void SubscribeForMarketData()
        {
            try
            {
                foreach (var key in odaxSymbolsDic.Keys)
                {
                    SymbolBase outsymbol;

                    bool re = odaxSymbolsDic.TryGetValue(key, out outsymbol);

                    if (re == true)
                    {
                        feedFIX.SubscribeForQuotes(key, outsymbol, updateQuotes);
                    }
                }
            }
            catch (Exception)
            {
                // not connected?
            }
        }

        private void SubscribeForMarketData(string key, SymbolBase symbol)
        {
            if(symbol != null)
            {
                requestIdMD = key;

                feedFIX.SubscribeForQuotes(key, symbol, updateQuotes);
            }
        }


        public void updateQuotes(String requestId, Dictionary<Double, Double> bids, Dictionary<Double, Double> asks, Dictionary<Double, Double> trades, DateTime timestamp)
        {           
            if (bids.Keys.Count > 0 && asks.Keys.Count > 0)
            {
                SymbolBase symbol;

                foreach (Double price in bids.Keys)
                {
                    bool got = odaxSymbolsDic.TryGetValue(requestId, out symbol);

                    if(got == true)
                    {
                        symbol.Bids[price] = bids[price];

                        updateCurrentSymbol(symbol);
                    }                   
                }
                foreach (Double price in asks.Keys)
                {                    

                    bool got = odaxSymbolsDic.TryGetValue(requestId, out symbol);

                    if (got == true)
                    {
                        symbol.Asks[price] = asks[price];

                        updateCurrentSymbol(symbol);
                    }                  
                }
                
            }


            if (trades.Keys.Count > 0)
            {
                foreach(var key in trades.Keys)
                {                   

                    SymbolBase symbol;

                    bool got = odaxSymbolsDic.TryGetValue(requestId, out symbol);

                    if (got == true)
                    {
                        symbol.Trades[key] = trades[key];

                        updateCurrentSymbol(symbol);                        
                    }

                    //string temp = string.Format("{0},{1},{2}", symbol.EXANTEId, key, symbol.Trades[key]);
                    //Console.WriteLine("Trade List ====> {0}", temp);
                }  
            }          
        }

        private void updateCurrentSymbol(SymbolBase symbol)
        {
            if (currentSymbol != null)
            {
                if (currentSymbol.EXANTEId == symbol.EXANTEId)
                {
                    currentSymbol = symbol;

                    listViewMarketDepth.Invoke(new Action(() =>
                    {
                        SuspendConrolRedraw(listViewMarketDepth);

                        listViewMarketDepth.BeginUpdate();

                        listViewMarketDepth.Items.Clear();

                        string[] bidRow = new string[] { "Bid", symbol.Bids.Keys.LastOrDefault().ToString() };

                        ListViewItem bid = new ListViewItem(bidRow);

                        bid.ForeColor = Color.Yellow;

                        bid.BackColor = Color.Blue;

                        listViewMarketDepth.Items.Add(bid);

                        string[] askRow = new string[] { "Ask", symbol.Asks.Keys.LastOrDefault().ToString() };

                        ListViewItem ask = new ListViewItem(askRow);

                        ask.ForeColor = Color.Blue;

                        ask.BackColor = Color.Yellow;

                        listViewMarketDepth.Items.Add(ask);                       

                        listViewMarketDepth.Sort();

                        listViewMarketDepth.EndUpdate();

                        ResumeControlRedraw(listViewMarketDepth);
                    }
                ));
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!myWorker1.IsBusy)//Check if the worker is already in progress
            {
                button2.Enabled = false;

                button1.Enabled = false;
                
                button3.Enabled = false;

                progressBarLoadSymbols.Maximum = 100;

                myWorker1.RunWorkerAsync(lblSavePath.Text);//Call the background worker
            }
        }


        private void GeneratingWorker(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker sendingWorker = (BackgroundWorker) sender;//Capture the BackgroundWorker that fired the event

            string param = (string)e.Argument;

            int total = odaxSymbolsDic.Count;

            int current = 0;

            double percent = 0;

            if(param == "MakeLastExecutionReport")
            {
                string templatePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "FIX"), "execution_template.xlsx");

                string newFilePath = Path.Combine(currentSavePath, "Last_Executions.xlsx");

                MakeLastExecutionReport(templatePath, newFilePath);
            }
            else
            {
                string templatePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "FIX"), "quotes_template.xlsx");                

                foreach (var key in odaxSymbolsDic.Keys)
                {
                    SymbolBase symbol;

                    if (odaxSymbolsDic.TryGetValue(key, out symbol))
                    {
                        string newfilePath = Path.Combine(currentSavePath, symbol.QuotesReportName);

                        MakeQuoteReport(templatePath, newfilePath, symbol);
                    }

                    current++;

                    percent = (int)Math.Round((double)(100 * current) / total);

                    sendingWorker.ReportProgress((int)percent);//Report our progress to the main thread
                }

                e.Result = "COMPLETED_QUOTE_REPORT";
            }            
        }

        private void myWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarLoadSymbols.Value = e.ProgressPercentage;
        }

        private void myWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {             
            if((string) e.Result == "COMPLETED_QUOTE_REPORT")
            {
                myWorker1.RunWorkerAsync("MakeLastExecutionReport");//Call the background worker
            }
            else
            {
                button2.Enabled = true;
                button1.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void MakeLastExecutionReport(string templatePath, string newFile)
        {
            try
            {
                if (!File.Exists(newFile))
                    File.Copy(templatePath, newFile);

                FileInfo newFileInfo = new FileInfo(newFile);
                using (ExcelPackage package = new ExcelPackage(newFileInfo))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets[1];

                    int rowoffset = 1;

                    foreach (var key in odaxSymbolsDic.Keys)
                    {
                        SymbolBase symbol;

                        if (odaxSymbolsDic.TryGetValue(key, out symbol))
                        {
                            var cell = sheet.Cells.Where(x => x.Value.ToString() == "Experation date").FirstOrDefault().Offset(rowoffset, 0);
                            cell.Value = symbol.ExpirationDate;

                            cell = sheet.Cells.Where(x => x.Value.ToString() == "Strike price").FirstOrDefault().Offset(rowoffset, 0);
                            cell.Value = symbol.StrikePrice;

                            cell = sheet.Cells.Where(x => x.Value.ToString() == "Last daily volume").FirstOrDefault().Offset(rowoffset, 0);
                            var trade = symbol.Trades.LastOrDefault();
                            var price = trade.Key;
                            var qty = trade.Value;
                            cell.Value = qty;

                            cell = sheet.Cells.Where(x => x.Value.ToString() == "Last (execution price)").FirstOrDefault().Offset(rowoffset, 0);
                            cell.Value = price;

                            rowoffset++;
                        }

                    }

                    package.Save();

                }
            }
            catch(Exception)
            {

            }

        }

        private void MakeQuoteReport(string templatePath, string newFile, SymbolBase symbol)
        {
            try
            {
                if(!File.Exists(newFile))
                    File.Copy(templatePath, newFile);                

                FileInfo newFileInfo = new FileInfo(newFile);

                using (ExcelPackage package = new ExcelPackage(newFileInfo))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets[1];

                    var cell = sheet.Cells.Where(x => x.Value.ToString() == "Experation date").FirstOrDefault().Offset(1, 0);
                    cell.Value = symbol.ExpirationDate;

                    cell = sheet.Cells.Where(x => x.Value.ToString() == "Strike price").FirstOrDefault().Offset(1, 0);
                    cell.Value = symbol.StrikePrice;

                    if(symbol.IsCall)
                    {
                        cell = sheet.Cells.Where(x => x.Value.ToString() == "Call Bid Qty Level1").FirstOrDefault().Offset(1, 0);
                        foreach (var price in symbol.Bids.Keys)
                        {
                            var qty = symbol.Bids[price];
                            cell.Value = qty;
                            cell = cell.Offset(1, 0);
                        }

                        cell = sheet.Cells.Where(x => x.Value.ToString() == "Call Bid Price Level1").FirstOrDefault().Offset(1, 0);
                        foreach (var price in symbol.Bids.Keys)
                        {
                            cell.Value = price;
                            cell = cell.Offset(1, 0);
                        }

                        cell = sheet.Cells.Where(x => x.Value.ToString() == "Call Ask Qty Level1").FirstOrDefault().Offset(1, 0);
                        foreach (var price in symbol.Asks.Keys)
                        {
                            var qty = symbol.Asks[price];
                            cell.Value = qty;
                            cell = cell.Offset(1, 0);
                        }

                        cell = sheet.Cells.Where(x => x.Value.ToString() == "Call Ask Price Level1").FirstOrDefault().Offset(1, 0);
                        foreach (var price in symbol.Asks.Keys)
                        {
                            cell.Value = price;
                            cell = cell.Offset(1, 0);
                        }
                    }
                    else
                    {
                        cell = sheet.Cells.Where(x => x.Value.ToString() == "Put Bid Qty Level1").FirstOrDefault().Offset(1, 0);
                        foreach (var price in symbol.Bids.Keys)
                        {
                            var qty = symbol.Bids[price];
                            cell.Value = qty;
                            cell = cell.Offset(1, 0);
                        }

                        cell = sheet.Cells.Where(x => x.Value.ToString() == "Put Bid Price Level1").FirstOrDefault().Offset(1, 0);
                        foreach (var price in symbol.Bids.Keys)
                        {
                            cell.Value = price;
                            cell = cell.Offset(1, 0);
                        }

                        cell = sheet.Cells.Where(x => x.Value.ToString() == "Put Ask Qty Level1").FirstOrDefault().Offset(1, 0);
                        foreach (var price in symbol.Asks.Keys)
                        {
                            var qty = symbol.Asks[price];
                            cell.Value = qty;
                            cell = cell.Offset(1, 0);
                        }

                        cell = sheet.Cells.Where(x => x.Value.ToString() == "Put Ask Price Level1").FirstOrDefault().Offset(1, 0);
                        foreach (var price in symbol.Asks.Keys)
                        {
                            cell.Value = price;
                            cell = cell.Offset(1, 0);
                        }
                    }                                    

                    package.Save();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {            
            updateConnectionStatus?.Abort();

            KillFIXConnection();
        }

        private void cmbExp_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentExpirationDate = (string)cmbExp.SelectedItem;

            cmbOpt.SelectedIndex = 0;

            cmbStp.DisplayMember = "Value";

            cmbStp.ValueMember = "Value";

            var groups1 = odaxSymbolsDic.Where(x => x.Value.ExpirationDate == currentExpirationDate).OrderBy(z => z.Value.StrikePrice);
            
            var groups2 = from item in groups1
                          group item.Value.StrikePrice by item.Value.StrikePrice
                          into grouped
                          select grouped;
            
            cmbStp.Invoke(new Action(() =>
            {
                SuspendConrolRedraw(cmbStp);

                cmbStp.Items.Clear();

                foreach (var g in groups2)
                {
                    cmbStp.Items.Add(g.Key);
                }

                ResumeControlRedraw(cmbStp);              
            }));

            cmbStp.SelectedIndex = 0;
        }       

        private void cmbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            var optVal = (string) cmbOpt.SelectedItem;

            isCall = (optVal.Equals("Call")) ? true : false;

        }

        private void cmbStp_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentStrikePrice = (int)cmbStp.SelectedItem;

            SetCurrentSymbol();
        }

        private void SetCurrentSymbol()
        {
            string partSymbol1 = string.Format("{0}.{1}", targetMarket, currentExpirationDate);

            string opt = isCall ? "C" : "P";

            string partSymbol2 = string.Format("{0}{1}", opt, currentStrikePrice);

            lblTargetSymbol.Text = partSymbol1 + "." + partSymbol2;

            currentSymbol = odaxSymbolsDic.Where(x => x.Value.EXANTEId == lblTargetSymbol.Text).SingleOrDefault().Value;

            ClearDispalay();

            SubscribeForMarketData(Guid.NewGuid().ToString(), currentSymbol);
        }

        private void ClearDispalay()
        {
            if (IsHandleCreated)
            {
                listViewMarketDepth.Invoke(new Action(() => listViewMarketDepth.Items.Clear()));
                //labelLastTradeValue.Invoke(new Action(() => labelLastTradeValue.Text = ""));
            }
        }

        private void PlaceOrder(Position position, OrderType orderType)
        {           
            try
            {
                switch(position)
                {
                    case Position.Long://Buy => Bid + 0.1
                    {
                        executionFIX.placeOrder(new OrderLimit(DateTime.Now, currentSymbol, position, CalcQuantity(), CalcPrice(true)), OrderStatusUpdate);
                        break;
                    }
                    case Position.Short://Sell => Ask - 0.1
                    {
                        executionFIX.placeOrder(new OrderLimit(DateTime.Now, currentSymbol, position, CalcQuantity(), CalcPrice(false)), OrderStatusUpdate);
                        break;
                    }
                }                
            }
            catch (Exception)
            {               
            }
        }

        private int CalcQuantity()
        {
            var qty = 1; //for test trade

            lblBuyQty.Text = qty.ToString();

            lblSellQty.Text = qty.ToString();

            return qty;
        }

        private double CalcPrice(bool isBuy)
        {
            double price = 0.0;

            if(isBuy)
            {
                price = currentSymbol.Bids.Keys.LastOrDefault() + 0.1;

                lblBuyPrice.Text = price.ToString();
            }
            else
            {
                price = currentSymbol.Asks.Keys.LastOrDefault() - 0.1;

                lblSellPrice.Text = price.ToString();
            }

            return price;
        }

        public void RedrawOrders()
        {            
            listViewOrders.Invoke(new Action(() =>
            {
                SuspendConrolRedraw(listViewOrders);
                listViewOrders.Items.Clear();
                foreach (OrderBase order in executionFIX.GetActiveOrdersCopy())
                {
                    string orderType = "Limit";
                    string side = "";
                    string price = "";
                    if (order.GetType() == typeof(OrderLimit))
                    {
                        orderType = "Limit";
                        price = ((OrderLimit)order).Price.ToString();
                    }

                    side = (order.Position == Position.Long) ? "Buy" : "Sell";


                    ListViewItem row = new ListViewItem(new string[] { order.Symbol.EXANTEId, side, orderType, order.Quantity.ToString(), price, order.OrderStatus.ToString() });
                    listViewOrders.Items.Add(row);
                }
                listViewOrders.Sort();
                ResumeControlRedraw(listViewOrders);
            }
            ));
        }

        private void InitOrdersControl()
        {            
            listViewOrders.Columns.Add(@"Instrument", 100);
            listViewOrders.Columns.Add("Side");
            listViewOrders.Columns.Add("Type");
            listViewOrders.Columns.Add("Qty");
            listViewOrders.Columns.Add("Price");
            listViewOrders.Columns.Add("Status", 100);
            listViewOrders.Sorting = SortOrder.Descending;
        }

        private void InitPriceControl()
        {
            listViewMarketDepth.Columns.Add(@"Type", 60);
            listViewMarketDepth.Columns.Add("Price",100);            
        }

        private void btnBuyLimit_Click(object sender, EventArgs e)
        {
            PlaceOrder(Position.Long, OrderType.LIMIT);
        }

        private void btnSellLimit_Click(object sender, EventArgs e)
        {
            PlaceOrder(Position.Short, OrderType.LIMIT);
        }
    }
}
