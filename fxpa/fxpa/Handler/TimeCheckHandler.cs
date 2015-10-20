using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    public class TimeCheckHandler : MsgHandler
    {
        FXPA fxpa;

        public FXPA FXPA
        {
            get { return fxpa; }
            set { fxpa = value; }
        }

        DoubleBufferListView signalListView;
        DoubleBufferListView priceListView;
        DoubleBufferListView infoListView;


        public DoubleBufferListView InfoListView
        {
            get { return infoListView; }
            set { infoListView = value; }
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

        public TimeCheckHandler(Client client)
            : base(client)
        {
        }

        public override void Send(object msg)
        {
            Console.WriteLine(" TimeCheckHandler  Send ~~~~~~~~~~~ " + msg);
            Client.Send(msg);
        }


        public override void Receive(object msg)
        {
            Console.WriteLine(" TimeCheckHandler  Receive ~~~~~~~~~~~ " + msg);
            string[] msgs = (string[])msg;

            Protocol protocol = AppUtil.ParseProtocol(msgs[1]);
            int paramAmount = AppUtil.StringToInt(msgs[2]);
            if (protocol != Protocol.UNKNOWN)
            {
                try
                {
                    switch (protocol)
                    {
                        case Protocol.S0002_1:
                            string strServerTime = msgs[3];
                            DateTime serverTime;
                            serverTime = DateTime.Parse(strServerTime);
                           
                            if (FXPA != null && !FXPA.IsDisposed)
                            {
                                if (!msgs.Contains(NULL))
                                {
                                    if (serverTime.CompareTo(AppSetting.END_TIME) > 0)
                                    {
                                        errCode = -1;
                                        MethodInvoker mi = new MethodInvoker(ShowErrorMsg);
                                        FXPA.BeginInvoke(mi);

                                        mi = new MethodInvoker(CloseApp);
                                        FXPA.BeginInvoke(mi);
                                    }
                                }
                            }

                            int DAY = Convert.ToInt16(serverTime.Day);
                            AppContext.CURRENT_TIME = serverTime;
                            
                            DateTime startTimeOfNormalDay = AppUtil.GetStartTimeOfNormalDay(AppContext.CURRENT_TIME);

                            if (DAY > LogUtil.DAY && serverTime.CompareTo(startTimeOfNormalDay) > 0)
                            {
                                MethodInvoker mi = new MethodInvoker(TimeLabelUpdate);
                                AppContext.TradeExpertControl.BeginInvoke(mi);
                                LogUtil.DAY = DAY;
                                LogUtil.Close();
                                LogUtil.init(true, true, true, true);
                            }
                            else if (DAY > AppContext.DAY )
                            {
                                AppContext.DAY = DAY;
                                DateTime startTimeOfWeek=AppUtil.GetStartTimeOfWeek(AppContext.CURRENT_TIME);
                                DateTime startTimeOfBizDay = AppUtil.GetStartTimeOfBizToday(AppContext.CURRENT_TIME);
                                List<Signal> obsoletes = new List<Signal>();
                                if (AppSetting.STATUS == AppSetting.TEST_ACCOUNT && serverTime.CompareTo(startTimeOfWeek) > 0)
                                {
                                    if (AppContext.SignalDatas.Count > 0) 
                                    {
                                        lock (AppConst.SignalDatasLocker)
                                        {
                                            foreach (Signal signal in AppContext.SignalDatas)
                                            {
                                                if (signal.ProfitPrice != 0 && signal.ProfitTime.CompareTo(startTimeOfWeek) < 0)
                                                {
                                                    obsoletes.Add(signal);
                                                }
                                            }
                                            foreach (Signal signal in obsoletes)
                                                AppContext.SignalDatas.Remove(signal);
                                        }                                 
                                    }
                                }
                                else if (serverTime.CompareTo(startTimeOfBizDay) > 0)
                                {
                                    if (AppContext.SignalDatas.Count > 0)
                                    {
                                        lock (AppConst.SignalDatasLocker)
                                        {
                                            foreach (Signal signal in AppContext.SignalDatas)
                                            {
                                                if (signal.ProfitPrice != 0 && signal.ProfitTime.CompareTo(startTimeOfBizDay) < 0)
                                                {
                                                    obsoletes.Add(signal);
                                                }
                                            }
                                            foreach (Signal signal in obsoletes)
                                                AppContext.SignalDatas.Remove(signal);
                                        }
                                    }
                                }

                                if (obsoletes != null && obsoletes.Count > 0)
                                {
                                    MethodInvoker mi = new MethodInvoker(SignalListUpdate);
                                    signalListView.BeginInvoke(mi);
                                    mi = new MethodInvoker(StatListUpdate);
                                    AppContext.StatListView.BeginInvoke(mi);
                                    mi = new MethodInvoker(PriceListUpdate);
                                    priceListView.BeginInvoke(mi);
                                }

                                obsoletes.Clear();
                                List<PublishInfo> obsoletes2 = new List<PublishInfo>();
                                if (AppContext.PublishInfos.Count > 0)
                                {
                                    lock (AppContext.PublishInfos)
                                    {
                                       foreach (PublishInfo info in AppContext.PublishInfos)
                                        {
                                             if (AppContext.CURRENT_TIME.Subtract(info.DateTime) > AppConst.AppTimeSpans[Interval.DAY1])
                                            {
                                                obsoletes2.Add(info);
                                            }
                                        }

                                        foreach (PublishInfo info in obsoletes2)
                                                AppContext.PublishInfos.Remove(info);
                                        if (obsoletes2 != null && obsoletes2.Count > 0)
                                        {
                                            MethodInvoker mi = new MethodInvoker(InfoListUpdate);
                                            infoListView.BeginInvoke(mi);
                                        }
                                    }
                                }
                                obsoletes2.Clear();                            
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    LogUtil.Info(" TimeCheckHandler  Receive  Exception~~~~~~~~~~~ " + e.StackTrace);
                }
            }
        }

        private void SignalListUpdate()
        {
            if (AppContext.SignalDatas.Count > 0)
            {
                Signal[] signals = new Signal[AppContext.SignalDatas.Count];
                AppContext.SignalDatas.CopyTo(signals);
                Array.Sort(signals);
                lock (AppConst.SignalListLocker)
                {
                    AppContext.SignalListView.Items.Clear();
                    foreach (Signal signal in signals)
                    {
                        if (AppSetting.SYMBOLS.Contains(signal.Symbol))
                        {
                            string strSignal = AppUtil.GetSignalChinese(signal.Arrow);
                            ListViewItem item = new ListViewItem(
                                new string[] {signal.Symbol.ToString(), 
                          signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), signal.StopLossPrice.ToString() }, 0);
                            AppContext.SignalListView.Items.Add(item);
                        }
                    }
                }
            }
            else
            {
                AppContext.SignalListView.Items.Clear();
            }
        }


        private void PriceListUpdate()
        {
            if (AppContext.SignalDatas.Count > 0)
            {
                Signal[] signals = new Signal[AppContext.SignalDatas.Count];
                AppContext.SignalDatas.CopyTo(signals);

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
                                        break;
                                    }
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
        }

        private void StatListUpdate()
        {
            if (AppContext.SignalDatas.Count > 0)
            {
                Signal[] signals = new Signal[AppContext.SignalDatas.Count];
                AppContext.SignalDatas.CopyTo(signals);

                if (signals.Length > 0)
                {
                    lock (AppConst.StatListLocker)
                    {
                        AppContext.StatListView.Items.Clear();
                        foreach (Signal signal in signals)
                        {
                            if (AppSetting.SYMBOLS.Contains(signal.Symbol))
                            {
                                string strSignal = AppUtil.GetSignalChinese(signal.Arrow);
                                ListViewItem item = null; int arrow = signal.Arrow == -1 ? 0 : 1;
                                if (signal.ProfitPrice <= 0)
                                    item = new ListViewItem(new string[] { signal.Symbol.ToString(), signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), "", "" }, arrow);
                                else
                                    item = new ListViewItem(new string[] { signal.Symbol.ToString(), signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), signal.ProfitTime.ToString(), signal.Profit.ToString() }, arrow);
                                AppContext.StatListView.Items.Add(item);
                            }
                        }
                    }
                }
            }
            else
            {
                AppContext.StatListView.Items.Clear();
            }
        }

        private void InfoListUpdate()
        {
            if (AppContext.PublishInfos.Count > 0)
            {
                PublishInfo[] infos = new PublishInfo[AppContext.PublishInfos.Count];
                AppContext.PublishInfos.CopyTo(infos);
                Array.Sort(infos);
                lock (AppConst.InfoListLocker)
                {
                    infoListView.Items.Clear();
                    foreach (PublishInfo info in infos)
                    {
                        if (AppSetting.SYMBOLS.Contains(info.Symbol))
                        {
                            ListViewItem item = new ListViewItem(
                              new string[] { info.DateTime.ToString(), info.Type.ToString(), info.Content.ToString() }, 0);
                            infoListView.Items.Add(item);
                        }
                    }
                }
            }
            else
            {
                infoListView.Items.Clear();
            }
        }

        private void TimeLabelUpdate()
        {
            AppContext.TradeExpertControl.TimeLabel.Text = AppContext.CURRENT_TIME.ToString("MM'月'dd' day ' " + AppConst.Days[Convert.ToInt16(AppContext.CURRENT_TIME.DayOfWeek)]);
            AppContext.TradeExpertControl.TimeLabel.Refresh();
        }

        int errCode = 0;

        private void ShowErrorMsg()
        {
            switch (errCode)
            {
                case -1:
                    MessageBox.Show(fxpa, "Your account has been expired!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void CloseApp()
        {
            if (fxpa != null)
            {
                FXPA.Close();
                Application.ExitThread();
            }
        }

    }
}
