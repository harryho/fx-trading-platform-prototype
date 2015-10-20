using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Resources;
using System.Drawing;

namespace fxpa
{
    public class SignalHandler : MsgHandler
    {
        ManualTradeExpertControl expertControl;

        public ManualTradeExpertControl ExpertControl
        {
            get { return expertControl; }
            set { expertControl = value; }
        }

        DoubleBufferListView priceListView;

        public DoubleBufferListView PriceListView
        {
            get { return priceListView; }
            set { priceListView = value; }
        }

        DoubleBufferListView signalListView;

        public DoubleBufferListView SignalListView
        {
            get { return signalListView; }
            set { signalListView = value; }
        }

        DoubleBufferListView statListView;

        public DoubleBufferListView StatListView
        {
            get { return statListView; }
            set { statListView = value; }
        }

        public SignalHandler(Client client)
            : base(client)
        {
        }

        public override void Send(object msg)
        {
            Console.WriteLine(" SignalHandler  Send ~~~~~~~~~~~ " + msg);
            Client.Send((string)msg);
        }


        public override void Receive(object msg)
        {
            string[] msgs = (string[])msg;

            Protocol protocol = AppUtil.ParseProtocol(msgs[1]);
            int paramAmount = AppUtil.StringToInt(msgs[2]);
            if (protocol != Protocol.UNKNOWN)
            {
                if ( msgs.Length >= 4)
                {
                    LogUtil.Info(" Signal content " + msgs[3]);
                    string[] info = msgs[3].Split(',');
                    if (info.Length >= 2)
                    {
                        Interval interval = AppUtil.StringToInterval(info[0]);
                        Symbol symbol = AppUtil.ParseSymbol(info[1]);
                        try
                        {
                            switch (protocol)
                            {
                                case Protocol.K0004_1: // Arrow Signal 
                                    if (!AppContext.IsReconnecting && AppContext.IsPriceListInitialized &&
                                        AppContext.IsSignalListInitialized)
                                    {
                                        if (!msgs.Contains(NULL))
                                        {
                                            Signal signal = new Signal();
                                            signal.Interval = interval;
                                            signal.Symbol = symbol;
                                            signal.ActPrice = double.Parse(info[2]);
                                            signal.Arrow = int.Parse(info[3]);
                                            signal.ActTime = DateTime.Parse(info[4]);
                                            signal.StopLossPrice = double.Parse(info[5]);
                                            Console.WriteLine(Protocol.K0004_1+"  " + signal);
                                            LogUtil.Info(Protocol.K0004_1 + "  " + signal);
                                            if (AppSetting.SYMBOLS.Contains(signal.Symbol) && AppSetting.INTEVALS.Contains(signal.Interval))
                                            {
                                                SignalProcess(signal, protocol);
                                            }
                                        }
                                    }
                                    break;
                                case Protocol.K0004_2:  // Stop Loss Signal 
                                    if (!AppContext.IsReconnecting && AppContext.IsPriceListInitialized &&
                                        AppContext.IsSignalListInitialized)
                                    {
                                        if (!msgs.Contains(NULL))
                                        {
                                            Signal signal = new Signal();
                                            signal.Interval = interval;
                                            signal.Symbol = symbol;
                                            signal.StopLossBidPrice = double.Parse(info[2]);
                                            signal.StopLoss = int.Parse(info[3]);
                                            signal.Arrow = int.Parse(info[3]);
                                            signal.StopLossTime = DateTime.Parse(info[4]);
                                            signal.ActTime = DateTime.Parse(info[5]);
                                            Console.WriteLine(Protocol.K0004_2 + "  " + signal);
                                            LogUtil.Info(Protocol.K0004_2 + "  " + signal);
                                            if (signal.StopLossBidPrice > 0 && signal.StopLoss != 0)
                                            SignalProcess(signal, protocol);
                                        }
                                    }
                                    break;
                                case Protocol.K0004_3:  // Gain Tip Signal
                                    if (!AppContext.IsReconnecting && AppContext.IsPriceListInitialized &&
                                        AppContext.IsSignalListInitialized)
                                    {
                                        if (!msgs.Contains(NULL))
                                        {
                                            Signal signal = new Signal();
                                            signal.Interval = interval;
                                            signal.Symbol = symbol;
                                            signal.GainTip = int.Parse(info[2]);
                                            signal.Arrow = int.Parse(info[2]);
                                            signal.GainTipTime = DateTime.Parse(info[3]);
                                            signal.ActTime = DateTime.Parse(info[4]);
                                            signal.GainTipPrice = double.Parse(info[5]);
                                            Console.WriteLine(Protocol.K0004_3 + "  " + signal);
                                            LogUtil.Info(Protocol.K0004_3 + "  " + signal);
                                            if (signal.GainTipPrice > 0)
                                            SignalProcess(signal, protocol);
                                        }
                                    }
                                    break;
                                case Protocol.K0004_4:  // Stop Gain Signal 
                                    if (!AppContext.IsReconnecting && AppContext.IsPriceListInitialized &&
                                        AppContext.IsSignalListInitialized)
                                    {
                                        if (!msgs.Contains(NULL))
                                        {
                                            Signal signal = new Signal();
                                            signal.Interval = interval;
                                            signal.Symbol = symbol;
                                            signal.StopGainPrice = double.Parse(info[2]);
                                            signal.StopGain = int.Parse(info[3]);
                                            signal.Arrow = int.Parse(info[3]);
                                            signal.StopGainTime = DateTime.Parse(info[4]);
                                            signal.ActTime = DateTime.Parse(info[5]);
                                            Console.WriteLine(Protocol.K0004_4 + "  " + signal);
                                            LogUtil.Info(Protocol.K0004_4 + "  " + signal);
                                            if (signal.StopGainPrice > 0)
                                            SignalProcess(signal, protocol);
                                        }
                                    }
                                    break;
                                case Protocol.K0004_5:  // Profit Signal
                                    if (!AppContext.IsReconnecting && AppContext.IsPriceListInitialized &&
                                        AppContext.IsSignalListInitialized)
                                    {
                                        if (!msgs.Contains(NULL))
                                        {
                                            Signal signal = new Signal();
                                            signal.Interval = interval;
                                            signal.Symbol = symbol;
                                            signal.Arrow = int.Parse(info[2]);
                                            signal.ActTime = DateTime.Parse(info[3]);
                                            signal.Profit = int.Parse(info[4]);
                                            signal.ProfitTime = DateTime.Parse(info[5]);
                                            signal.ProfitPrice = double.Parse(info[6]);
                                            Console.WriteLine(Protocol.K0004_5 + "  " + signal);
                                            LogUtil.Info(Protocol.K0004_5 + "  " + signal);
                                            if(signal.ProfitPrice >0)
                                            SignalProcess(signal, protocol);
                                        }
                                    }
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            LogUtil.Error(" Protocol " + protocol.ToString()+ e.StackTrace);
                        }
                    }
                }
            }
        }

        static Queue<Signal> signalQueue = new Queue<Signal>();

        private void SignalProcess(Signal signal, Protocol protocol)
        {
            Console.WriteLine(" Before Invoke control update ......   " + signal);
            bool dupli = false;
            bool isValid = false;
            for (int i = 0; i < AppContext.SignalDatas.Count; i++)
            {
                Signal sig = AppContext.SignalDatas[i];
                if (signal.Equals(sig))
                {
                    if (signal.Arrow == sig.Arrow && protocol == Protocol.K0004_1)
                    {
                        dupli = true;
                    }
                    else if (signal.StopLoss == sig.StopLoss && protocol == Protocol.K0004_2)
                    {
                        dupli = true;
                    }
                    else if (signal.GainTip == sig.GainTip && protocol == Protocol.K0004_3)
                    {
                        dupli = true;
                    }
                    else if (signal.StopGain == sig.StopGain && protocol == Protocol.K0004_4)
                    {
                        dupli = true;
                    }
                    else if (signal.ProfitPrice == sig.ProfitPrice && protocol == Protocol.K0004_5)
                    {
                        dupli = true;
                    }
                    else
                    {
                        if (signal.StopLoss != 0) { sig.StopLoss = signal.StopLoss; sig.StopLossTime = signal.StopLossTime; sig.StopLossBidPrice = signal.StopLossBidPrice; isValid = true; }
                        else if (signal.GainTip != 0) { sig.GainTip = signal.GainTip; sig.GainTipPrice = signal.GainTipPrice; sig.GainTipTime = signal.GainTipTime; isValid = true; }
                        else if (signal.StopGain != 0) { sig.StopGain = signal.StopGain; sig.StopGainPrice = signal.StopGainPrice; sig.StopGainTime = signal.StopGainTime; isValid = true; }
                        else if (signal.ProfitPrice > 0) { sig.Profit = signal.Profit; sig.ProfitPrice = signal.ProfitPrice; sig.ProfitTime = signal.ProfitTime; isValid = true; 
                               DataProvider provider = DataService.GetProviderBySymbol(signal.Symbol);
                               if (provider != null && !provider.DataCache.SignalUnits[provider.CurrentTInterval].Contains(sig))
                               { provider.DataCache.SignalUnits[provider.CurrentTInterval].Add(sig); }
                        }
                        AppContext.SignalDatas[i] = sig;
                        LogUtil.Info(" AppContext.SignalDatas[i]  " + i +"  "+AppContext.SignalDatas[i]);
                    }
                    LogUtil.Info(" dupli ==============  " + dupli);
                    break;
                }
                if (i == AppContext.SignalDatas.Count - 1 && !dupli)
                {
                    isValid = true;
                }
            }

            if (!dupli)
            {
                if (protocol == Protocol.K0004_1)
                    AppContext.SignalDatas.Add(signal);

                if (isValid)
                {
                    Dictionary<Protocol, Signal> protocolSignal = new Dictionary<Protocol, Signal>();
                    protocolSignal.Add(protocol, signal);
                    MsgUpdatedDelegate d = null;
                    if (protocol != Protocol.K0004_5)
                    {
                        d = new MsgUpdatedDelegate(ProviderUpdate);
                        d.Invoke(protocolSignal);
                    }
                    if (protocol != Protocol.K0004_2 && protocol != Protocol.K0004_3)
                    {
                        d = new MsgUpdatedDelegate(SignalListUpdate);
                        signalListView.BeginInvoke(d, protocolSignal);
                    }
                    if (protocol == Protocol.K0004_5 || protocol == Protocol.K0004_1)
                    {
                        d = new MsgUpdatedDelegate(PriceListUpdate);
                        priceListView.BeginInvoke(d, protocolSignal);
                    }
                    if (protocol == Protocol.K0004_5 || protocol == Protocol.K0004_1)
                    {
                        d = new MsgUpdatedDelegate(StatlListUpdate);
                        statListView.BeginInvoke(d, protocolSignal);
                    }
                }
            }
        }


        private void ProviderUpdate(object obj)
        {
            Dictionary<Protocol, Signal> protocolSignal = (Dictionary<Protocol, Signal>)obj;
            Protocol protocol = (Protocol)protocolSignal.Keys.First();
            Signal signal = (Signal)protocolSignal.Values.First();
            DataProvider provider = DataService.GetProviderBySymbol(signal.Symbol);
            if (provider !=null)
            {
                lock (provider)
                {
                   provider.SignalUpdate(protocolSignal);
                }
            }
        }

        private void PriceListUpdate(object obj)
        {
            Dictionary<Protocol, Signal> protocolSignal = (Dictionary<Protocol, Signal>)obj;
            Protocol protocol = (Protocol)protocolSignal.Keys.First();
            Signal signal = (Signal)protocolSignal.Values.First();
            if (AppSetting.SYMBOLS.Contains(signal.Symbol))
            {
                int signalCount = 0, profit = 0;
                if (signal.Arrow != 0 && protocol == Protocol.K0004_1)
                {
                    lock (AppConst.PriceListLocker)
                    {
                        //    if (priceListView.Items[iForexIndex].SubItems.Count > 0)
                        //    {
                        //        if (string.IsNullOrEmpty(priceListView.Items[iForexIndex].SubItems[2].Text))
                        //        {
                        //            dOldForexValue = 0;
                        //        }
                        //        else
                        //        {
                        //            dOldForexValue = Convert.ToInt16(priceListView.Items[iForexIndex].SubItems[2].Text);
                        //        }
                        //        priceListView.Items[iForexIndex].SubItems[2].Text = (dOldForexValue + 1).ToString();
                        //    }
                        //}

                        for (int i = 0; i < priceListView.Items.Count; i++)
                        {
                            string strSymbol = priceListView.Items[i].SubItems[0].Text;
                            Symbol symbol = signal.Symbol;
                            string cnSymbol = AppConst.AppCnSymbols[symbol];
                            if (cnSymbol != null && strSymbol != null && cnSymbol.Trim() == strSymbol.Trim())
                            {
                                string c = priceListView.Items[i].SubItems[2].Text;
                                signalCount = !string.IsNullOrEmpty(c) ? Convert.ToInt16(c) : 0;
                                priceListView.Items[i].SubItems[2].Text = (signalCount + 1).ToString();
                                break;
                            }
                        }
                    }
                }
                else if (signal.ProfitPrice != 0 && protocol == Protocol.K0004_5)
                {
                    LogUtil.Info(" Protocol :  " + Protocol.K0004_5);
                    lock (AppConst.PriceListLocker)
                    {
                        //if (priceListView.Items[iForexIndex].SubItems.Count > 0)
                        //{
                        //    if (string.IsNullOrEmpty(priceListView.Items[iForexIndex].SubItems[3].Text))
                        //    {
                        //        dOldForexValue = 0;
                        //    }
                        //    else
                        //    {
                        //        dOldForexValue = Convert.ToInt16(priceListView.Items[iForexIndex].SubItems[3].Text);
                        //    }
                        //    LogUtil.Info(" dOldForexValue :  " + dOldForexValue);
                        //    priceListView.Items[iForexIndex].SubItems[3].Text = (dOldForexValue + signal.Profit).ToString();
                        //}

                        for (int i = 0; i < priceListView.Items.Count; i++)
                        {
                            string strSymbol = priceListView.Items[i].SubItems[0].Text;
                            Symbol symbol = signal.Symbol;
                            string cnSymbol = AppConst.AppCnSymbols[symbol];
                            if (cnSymbol != null && strSymbol != null && cnSymbol.Trim() == strSymbol.Trim())
                            {
                                lock (AppConst.PriceListLocker)
                                {
                                    string p = priceListView.Items[i].SubItems[3].Text;
                                    profit = !string.IsNullOrEmpty(p) ? Convert.ToInt16(p) : 0;
                                    priceListView.Items[i].SubItems[3].Text = (profit + signal.Profit).ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        private void SignalListUpdate(object obj)
        {
            Dictionary<Protocol, Signal> protocolSignal = (Dictionary<Protocol, Signal>)obj;
            Protocol protocol = (Protocol)protocolSignal.Keys.First();
            Signal signal = (Signal)protocolSignal.Values.First();
            if (AppSetting.SYMBOLS.Contains(signal.Symbol))
            {
                if (signal.Arrow != 0 && protocol == Protocol.K0004_1)
                {
                    LogUtil.Info(" signal.Arrow != 0 && protocol == " + Protocol.K0004_1);
                    Signal[] signals = null;
                    lock (AppConst.SignalDatasLocker)
                    {
                        signals = new Signal[AppContext.SignalDatas.Count];
                        AppContext.SignalDatas.CopyTo(signals, 0);
                        Array.Sort(signals);
                    }
                    lock (AppConst.SignalListLocker)
                    {
                        if (signals != null)
                        {
                            signalListView.Items.Clear();
                            lock (AppConst.PriceListLocker)
                            {
                                foreach (Signal sig in signals)
                                {
                                    if (AppSetting.SYMBOLS.Contains(sig.Symbol))
                                    {
                                        string strSignal = AppUtil.GetSignalChinese(sig.Arrow);
                                        ListViewItem item = new ListViewItem(
                                            new string[] {sig.Symbol.ToString(), 
                                        sig.ActPrice.ToString(), strSignal, sig.ActTime.ToString(), sig.StopLossPrice.ToString(), ""}, 25 + sig.Arrow);
                                        signalListView.Items.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (signal.StopGain != 0 && protocol == Protocol.K0004_4)
                {
                    LogUtil.Info(" (signal.StopGain != 0 && protocol ==  " + Protocol.K0004_4);
                    ListView.ListViewItemCollection items = signalListView.Items;
                    for (int i = 0; i < items.Count; i++)
                    {
                        ListViewItem item = items[i];
                        string strDate = item.SubItems[3].Text ?? "";
                        DateTime actTime;
                        DateTime.TryParse(strDate, out actTime);
                        if (actTime.CompareTo(signal.ActTime) == 0)
                        {
                            lock (AppConst.SignalListLocker)
                            {
                                item.SubItems[5].Text = signal.StopGainPrice.ToString();
                                break;
                            }
                        }
                        //items[i] = item;
                    }
                }
                else if (signal.ProfitPrice != 0 && protocol == Protocol.K0004_5)
                {
                    LogUtil.Info(" signal.Profit != 0  && protocol ==" + Protocol.K0004_5);
                    ListView.ListViewItemCollection items = signalListView.Items;
                    for (int i = 0; i < items.Count; i++)
                    {
                        ListViewItem item = items[i];
                        if (item.SubItems.Count >= 5)
                        {
                            string strDate = item.SubItems[3].Text ?? "";
                            DateTime actTime;
                            DateTime.TryParse(strDate, out actTime);
                            if (actTime.CompareTo(signal.ActTime) == 0)
                            {
                                lock (AppConst.SignalListLocker)
                                {
                                    int count = item.SubItems.Count - 1;
                                    while (count >= 0)
                                    {
                                        item.SubItems[count].BackColor = Color.Azure;
                                        count--;
                                    }
                                    items[i] = item;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void StatlListUpdate(object obj)
        {
            Dictionary<Protocol, Signal> protocolSignal = (Dictionary<Protocol, Signal>)obj;
            Protocol protocol = (Protocol)protocolSignal.Keys.First();
            Signal signal = (Signal)protocolSignal.Values.First();
            if (AppSetting.SYMBOLS.Contains(signal.Symbol))
            {
                if (signal.Arrow != 0 && protocol == Protocol.K0004_1)
                {
                    LogUtil.Info("StatlListUpdate signal.Arrow != 0 && protocol ==" + Protocol.K0004_1);
                    Signal[] signals = null;
                    lock (AppConst.SignalDatasLocker)
                    {
                        signals = new Signal[AppContext.SignalDatas.Count];
                        AppContext.SignalDatas.CopyTo(signals, 0);
                        Array.Sort(signals);
                    }

                    if (signals != null)
                    {
                        lock (AppConst.StatListLocker)
                        {
                            statListView.Items.Clear();
                            foreach (Signal sig in signals)
                            {
                                if (AppSetting.SYMBOLS.Contains(sig.Symbol))
                                {
                                    string strSignal = AppUtil.GetSignalChinese(sig.Arrow);
                                    ListViewItem item = new ListViewItem(
                                        new string[] {sig.Symbol.ToString(), 
                                        sig.ActPrice.ToString(), strSignal, sig.ActTime.ToString(), "","", ""}, 25 + sig.Arrow);
                                    statListView.Items.Add(item);
                                }
                            }
                        }
                    }
                }
                else if (signal.ProfitPrice != 0 && protocol == Protocol.K0004_5)
                {
                    LogUtil.Info(" signal.Profit != 0  && protocol ==" + Protocol.K0004_5);
                    ListView.ListViewItemCollection items = statListView.Items;
                    for (int i = 0; i < items.Count; i++)
                    {
                        ListViewItem item = items[i];
                        if (item.SubItems.Count >= 5)
                        {
                            string strDate = item.SubItems[3].Text ?? "";
                            DateTime actTime;
                            DateTime.TryParse(strDate, out actTime);
                            if (actTime.CompareTo(signal.ActTime) == 0)
                            {
                                lock (AppConst.StatListLocker)
                                {
                                    item.SubItems[4].Text = signal.ProfitTime.ToString();
                                    item.SubItems[5].Text = signal.ProfitPrice.ToString();
                                    item.SubItems[6].Text = signal.Profit.ToString();
                                    int count = item.SubItems.Count - 1;
                                    while (count >= 0)
                                    {
                                        item.SubItems[count].BackColor = Color.Azure;
                                        count--;
                                    }
                                    items[i] = item;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
