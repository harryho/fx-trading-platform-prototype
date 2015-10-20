using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace fxpa
{
    public class SignalListHandler : MsgHandler
    {

        DoubleBufferListView priceListView;
        DoubleBufferListView signalListView;
        DoubleBufferListView statListView;

        public DoubleBufferListView StatListView
        {
            get { return statListView; }
            set { statListView = value; }
        }
        public DoubleBufferListView PriceListView
        {
            get { return priceListView; }
            set { priceListView = value; }
        }

       public DoubleBufferListView SignalListView
        {
            get { return signalListView; }
            set { signalListView = value; }
        }

        public SignalListHandler(Client client)
            : base(client)
        {     
        }

        public override void Send(object msg)
        {
            Console.WriteLine(" SignalListHandler  Send ~~~~~~~~~~~ " + msg);
            Client.Send((string)msg);
        }

        public override void Execute()
        {
            string msg = "";
            if (AppContext.IsAppInitializing && AppContext.IsGetSignalList && AppContext.SignalDatas.Count > 0)
            {
                signals = new Signal[AppContext.SignalDatas.Count];
                AppContext.SignalDatas.CopyTo(signals);
                MethodInvoker mi = new MethodInvoker(InitPriceList);
                priceListView.Invoke(mi);
                mi = new MethodInvoker(InitSignalList);
                signalListView.Invoke(mi);
                mi = new MethodInvoker(InitStatList);
                statListView.Invoke(mi);
            }
            else if (AppContext.IsAppInitializing && AppContext.IsGetSymbolPrices && AppContext.SymbolPrices.Count > 0)
            {
                MethodInvoker mi = new MethodInvoker(InitPriceList);
                priceListView.Invoke(mi);
                AppContext.IsStatListInitialized = true;
                AppContext.IsSignalListInitialized = true;
            }
            else
            {
                if (AppSetting.STATUS == AppSetting.TEST_ACCOUNT)  //  || AppSetting.STATUS == AppSetting.ADMIN_ACCOUNT
                {
                    msg = NetHelper.BuildMsg(Protocol.C0004_1, new string[] { "2" });
                }
                else
                {
                    msg = NetHelper.BuildMsg(Protocol.C0004_1, new string[] { "0" });
                }
                Send(msg);
            }
        }

        public void GetSignalsByTime(string time)
        {
            string request = NetHelper.BuildMsg(Protocol.C0004_1, new string[] { time });
            Send(request);
        }

        public override void Receive(object msg)
        {
            Console.WriteLine(" SignalListHandler  Receive ~~~~~~~~~~~ " + msg);
            string[] msgs = (string[])msg;
            Protocol protocol = AppUtil.ParseProtocol(msgs[1]);

            int paramAmount = int.Parse(msgs[2]);
            if (paramAmount > 0)
            {
                AppContext.SignalDatas.Clear();
                try
                {
                    for (int i = 3; i < msgs.Length; i++)
                    {
                        string signalInfo = msgs[i];
                        if (!string.IsNullOrEmpty(signalInfo))
                        {
                            string[] info = signalInfo.Split(',');
                            Signal signal = new Signal();
                            signal.Interval = AppUtil.StringToInterval(info[0]);
                            signal.Symbol = AppUtil.ParseSymbol(info[1]);
                            signal.ActPrice = double.Parse(info[2]);
                            signal.Arrow = AppUtil.StringToInt(info[3]);
                            signal.ActTime = DateTime.Parse(info[4]);
                            signal.StopLossPrice = double.Parse(info[5]);
                            if (NULL != info[6])
                            {
                                signal.StopLossTime = DateTime.Parse(info[6]);
                                signal.StopLoss = signal.Arrow;
                                signal.StopLossBidPrice = double.Parse(info[13]);
                            }
                            signal.GainTipPrice = double.Parse(info[7]);
                            if (NULL != info[8])
                            {
                                signal.GainTipTime = DateTime.Parse(info[8]);
                                signal.GainTip = signal.Arrow;
                            }
                            signal.StopGainPrice = double.Parse(info[9]);
                            if (NULL != info[10])
                            {
                                signal.StopGainTime = DateTime.Parse(info[10]);
                                signal.StopGain = signal.Arrow;
                            }
                            signal.Profit = int.Parse(info[11]);
                            signal.ProfitPrice = double.Parse(info[13]);
                            if (signal.ProfitPrice != 0 && NULL != info[12]) signal.ProfitTime = DateTime.Parse(info[12]);
                            AppContext.SignalDatas.Add(signal);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogUtil.Error("  M0004  " + e.StackTrace);
                }
            }
            signals = new Signal[AppContext.SignalDatas.Count];
            AppContext.SignalDatas.CopyTo(signals);
            MethodInvoker mi = new MethodInvoker(InitPriceList);
            priceListView.Invoke(mi);
            mi = new MethodInvoker(InitSignalList);
            signalListView.Invoke(mi);
            mi = new MethodInvoker(InitStatList);
            statListView.Invoke(mi);
            mi = new MethodInvoker(RefreshStatBtn);
            AppContext.TradeExpertControl.Invoke(mi);
        }

        static Signal[] signals = null;


        private void InitPriceList()
        {
            string[] fxList = Enum.GetNames(Symbol.AUDJPY.GetType());
            List<string> initSymbols = new List<string>();
            if (signals != null && signals.Length > 0)
            {
                for (int i = 0; i < AppContext.PriceListView.Items.Count; i++)
                {
                    if (!string.IsNullOrEmpty(priceListView.Items[i].SubItems[0].Text))
                    {
                        AppContext.PriceListView.Items[i].SubItems[2].Text = "";
                        AppContext.PriceListView.Items[i].SubItems[3].Text = "";
                    }
                }

                foreach (Signal signal in signals)
                {
                    string symbol = signal.Symbol.ToString();
                    if (AppSetting.SYMBOLS.Contains(signal.Symbol))
                    {
                        int arrowCount, profit;
                        for (int i = 0; i < priceListView.Items.Count; i++)
                        {
                            string strSymbol = priceListView.Items[i].SubItems[0].Text;
                            string cnSymbol = AppConst.AppCnSymbols[signal.Symbol];
                            if (cnSymbol != null && strSymbol != null && cnSymbol.Trim() == strSymbol.Trim())
                            {
                                lock (AppConst.PriceListLocker)
                                {
                                    string ac = priceListView.Items[i].SubItems[2].Text;
                                    arrowCount = string.IsNullOrEmpty(ac) ? 0 : int.Parse(ac.Trim());
                                    string pft = priceListView.Items[i].SubItems[3].Text;
                                    profit = string.IsNullOrEmpty(pft) ? 0 : int.Parse(pft.Trim());

                                    Console.WriteLine("  Symbol := " + strSymbol + " Count:= " + arrowCount + "  Profit  := " + profit);
                                    priceListView.Items[i].SubItems[2].Text = (arrowCount + 1).ToString();
                                    priceListView.Items[i].SubItems[3].Text = (profit + signal.Profit).ToString();
                                    Console.WriteLine("  Symbol ==== " + symbol);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {  // Clean obslete datas
                for (int i = 0; i < AppContext.PriceListView.Items.Count; i++)
                {
                    if (!string.IsNullOrEmpty(priceListView.Items[i].SubItems[0].Text))
                    {
                        AppContext.PriceListView.Items[i].SubItems[2].Text = "";
                        AppContext.PriceListView.Items[i].SubItems[3].Text = "";
                    }
                }
            }

            if (AppContext.IsGetSymbolPrices && AppContext.SymbolPrices.Count > 0)
            {
                foreach (KeyValuePair<string, double> pair in AppContext.SymbolPrices)
                {
                    string symbol = pair.Key;
                    double price = pair.Value;

                    for (int i = 0; i < priceListView.Items.Count; i++)
                    {
                        string strSymbol = priceListView.Items[i].SubItems[0].Text;
                        string cnSymbol = AppConst.AppCnSymbols[AppUtil.ParseSymbol(symbol)];
                        if (cnSymbol != null && strSymbol != null && cnSymbol.Trim() == strSymbol.Trim())
                        {
                            lock (AppConst.PriceListLocker)
                            {
                                Console.WriteLine("  Symbol := " + strSymbol + "  Price := " + price);
                                priceListView.Items[i].SubItems[1].Text = price.ToString();
                                break;
                            }
                        }
                    }

                }
                AppContext.IsGetSymbolPrices = false;
                AppContext.SymbolPrices.Clear();
            }
            AppContext.IsPriceListInitialized = true;
        }

        private void InitSignalList()
        {
            if (signals != null && signals.Length > 0)
            {
                lock (AppConst.SignalListLocker)
                {
                    signalListView.Items.Clear();
                    foreach (Signal signal in signals)
                    {
                        if (AppSetting.SYMBOLS.Contains(signal.Symbol))
                        {
                            string strSignal = AppUtil.GetSignalChinese(signal.Arrow);
                            ListViewItem item = null;
                            int arrow = signal.Arrow == -1 ? 0 : 1;
                            if (signal.StopGainPrice > 0)
                            {
                                item = new ListViewItem(
                                new string[] {signal.Symbol.ToString(), 
                          signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), signal.StopLossPrice.ToString(), signal.StopGainPrice.ToString()  }, arrow);
                            }
                            else
                            {
                                item = new ListViewItem(new string[] {signal.Symbol.ToString(), 
                          signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), signal.StopLossPrice.ToString(),"" }, arrow);
                            }
                            signalListView.Items.Add(item);
                        }
                    }
                }
            }
            else if (signals == null || signals.Length == 0)
            {
                signalListView.Items.Clear();
            }
            AppContext.IsSignalListInitialized = true;
        }

        private void InitStatList()
        {
            if (signals != null && signals.Length > 0)
            {

                lock (AppConst.StatListLocker)
                {
                    statListView.Items.Clear();
                    foreach (Signal signal in signals)
                    {
                        if (AppSetting.SYMBOLS.Contains(signal.Symbol))
                        {
                            string strSignal = AppUtil.GetSignalChinese(signal.Arrow);
                            ListViewItem item = null; int arrow = signal.Arrow == -1 ? 0 : 1;
                            if (signal.ProfitPrice == 0)
                                item = new ListViewItem(new string[] { signal.Symbol.ToString(), signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), "", "", "" }, arrow);
                            else
                                item = new ListViewItem(new string[] { signal.Symbol.ToString(), signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), signal.ProfitTime.ToString(), signal.ProfitPrice.ToString(), signal.Profit.ToString() }, arrow);
                            statListView.Items.Add(item);
                        }
                    }
                }
            }
            else if (signals == null || signals.Length == 0)
            {
                statListView.Items.Clear();
            }
            AppContext.IsStatListInitialized = true;
        }
        private void RefreshStatBtn()
        {
            AppContext.TradeExpertControl.DayStatBtn.Enabled =true;
            AppContext.TradeExpertControl.TriDayStatBtn.Enabled = true;
            AppContext.TradeExpertControl.WeekStatBtn.Enabled = true;

        }
    }
}
