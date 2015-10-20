using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace fxpa
{

    public class LoadingHandler : MsgHandler
    {
        LoadingForm loadingForm;

        public LoadingForm LoadingForm
        {
            get { return loadingForm; }
            set { loadingForm = value; }
        }

        public LoadingHandler()
        {
            timer = new System.Timers.Timer(500);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(ProcessLoading);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public override void Send(object msg)
        {
            //timer.Start();
            Console.WriteLine(" LoadingHandler  Send ~~~~~~~~~~~ " + msg);
            //Client.Send((string)msg);
        }


        public override void Receive(object msg)
        {
            Console.WriteLine(" LoadingHandler  Receive ~~~~~~~~~~~ " + msg);

            /// C0001_3
            string strIntervals = " MIN1";//,       MIN3,        MIN5,        MIN15,   MIN30,         MIN60,";// msgs[3];
            string[] intervals = strIntervals.Split(',');
            AppSetting.INTEVALS = new Interval[intervals.Length];
            int n = 0;
            foreach (string s in intervals)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    AppSetting.INTEVALS[n] = AppUtil.StringToInterval(s.Trim());
                    n++;
                }
            }

            string strSymbol = "  GBPUSD,        NZDUSD,        USDCAD,        USDCHF,        USDJPY,        GBPJPY,        AUDJPY,        EURJPY,        GOLD,        SILVER,";// msgs[4];
            LogUtil.Info(" C0004  ::::: symbols " + strSymbol);
            string[] symbols = strSymbol.Split(',');
            foreach (string s in symbols)
            {
                Symbol symbol = Symbol.UNKNOWN;
                if ((symbol = AppUtil.ParseSymbol(s.Trim())) != Symbol.UNKNOWN)
                {
                    FxpaSource.AddAvailableSession(s.Trim());
                }
            }

            int k = 0;
            AppSetting.SYMBOLS = new Symbol[FxpaSource.AvailableSessionList.Count];
            foreach (SessionInfo sin in FxpaSource.AvailableSessionList)
            {
                AppSetting.SYMBOLS[k] = AppUtil.ParseSymbol(sin.Symbol);
                k++;
            }
            if (AppSetting.INTEVALS != null && AppSetting.INTEVALS.Length > 0 &&
                 AppSetting.SYMBOLS != null && AppSetting.SYMBOLS.Length > 0)
            {

                {
                    DataProvider provider = new DataProvider(AppSetting.SYMBOLS[0].ToString());
                    provider.Init4Loading();
                    AppContext.FirstDataProvider = provider;
                }
                AppContext.IsGetSymbols = true;

            }


            AppSetting.LOGIN_TOKEN = "LOGIN_TOKEN";
            AppSetting.DS_SERVER_IP = "DS_SERVER_IP";//  msgs[4].Trim();
            AppSetting.DS_SERVER_PORT = 1234;// "DS_SERVER_PORT"; //int.Parse(msgs[5].Trim());

            AppContext.IsGetDSInfo = true;
            LogUtil.Info(" C0005  ::::: server ");

            AppContext.FirstDataProvider.InitCdlStatus = DataInitStatus.Initialized;

            AppContext.FirstDataProvider.CdlLoadingStatus = DownloadStatus.Finished;

            AppContext.IsGetInitProviderFile = true;

            AppContext.FirstDataProvider.RtdLoadingStatus = DownloadStatus.Finished;
            AppContext.FirstDataProvider.InitRtdStatus = DataInitStatus.Initialized;

            AppContext.IsGetSignalList = true;

            AppContext.FirstDataProvider.InitSigStatus = DataInitStatus.Initialized;
            AppContext.IsGetInitProviderSignals = true;

            AppContext.IsGetSymbolPrices = true;

            AppContext.IsGetPaymentInfo = true;


        }
        bool isTimeStart = false;
        bool isLoadingHisCdls = false;
        public override void Execute()
        {

            Receive(null);
            CloseForm();

        }

        System.Timers.Timer timer;

        static int count = 0;
        bool isDSConnecting = false;

        public void ProcessLoading(object obj, System.Timers.ElapsedEventArgs e)
        {
            if (loadingForm != null && !loadingForm.IsDisposed)
            {
                MsgUpdatedDelegate d = new MsgUpdatedDelegate(ProgresstUpdate);
                int status = loadingForm.GetLoadingStatus();
                if (status < 10)// && AppClient.IsReady)
                {
                    loadingForm.BeginInvoke(d, 10);
                }
                else if (status < 20 && AppContext.IsGetSymbols)
                {
                    loadingForm.BeginInvoke(d, 20);
                }
                else if (status < 30 && AppContext.IsGetDSInfo)
                {
                    loadingForm.BeginInvoke(d, 30);
                }
                else if (status >= 30 && status < 40)//&& DSClient.IsReady)
                {
                    loadingForm.BeginInvoke(d, 40);
                }
                else if (status >= 40 && status < 50 && AppContext.IsGetInitProviderFile)
                {
                    loadingForm.BeginInvoke(d, 50);
                }
                else if (status >= 50 && status < 60 && AppContext.IsGetInitProviderRtd)
                {
                    loadingForm.BeginInvoke(d, 60);
                }
                else if (status >= 60 && status < 70 && AppContext.IsGetInitProviderSignals)
                {
                    AppContext.FirstDataProvider.InitStart();
                    loadingForm.BeginInvoke(d, 70);
                }
                else if (status >= 70 && status < 80 && AppContext.IsGetSymbolPrices)
                {
                    loadingForm.BeginInvoke(d, 80);
                }
                else if (status >= 80 && status < 90 && AppContext.IsGetSignalList)
                {
                    loadingForm.BeginInvoke(d, 90);
                }
                //else if (status >= 85 && status < 95 && AppContext.IsLoadInitProviderFile)
                //{
                //    AppContext.FirstDataProvider.InitStart();
                //    loadingForm.BeginInvoke(d, 95);
                //}
                else if (status >= 90 && status < 100 && AppContext.IsFirstProviderInit)
                {
                    timer.Stop();
                    loadingForm.BeginInvoke(d, 100);
                    AppContext.IsLoading = false;
                }
            }
        }

        void ProgresstUpdate(object obj)
        {
            loadingForm.UpdateLoadingProgress((int)obj);
        }

        private void CloseForm()
        {
            lock (loadingForm)
            {
                if (loadingForm != null)
                {
                    loadingForm.DialogResult = DialogResult.OK;
                    loadingForm.Close();
                    loadingForm.Dispose();
                    loadingForm = null;
                }
            }
        }


        private void NetworkChecking(object obj, System.Timers.ElapsedEventArgs e)
        {
            //MethodInvoker mi = new MethodInvoker(CloseForm);
            //if (DSClient.IsReady)
            //{
            //    if (loadingForm != null)
            //        loadingForm.BeginInvoke(mi);
            //}
            //else
            //{
            //    count++;
            //    if (count > 10)
            //    {
            //        MessageBox.Show("数据服务未能接通! please try 再尝试.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        if (loadingForm != null)
            //            loadingForm.BeginInvoke(mi);
            //    }
            //}
        }
    }
}
