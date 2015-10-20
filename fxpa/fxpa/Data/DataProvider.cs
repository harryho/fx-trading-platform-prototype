using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Resources;
using System.Timers;
using System.IO;




namespace fxpa
{
    public class DataProvider : RemoteDataProvider
    {
        //------------------------ New -------------------------------//

        double realTimePrice;
        double _spread = 0;
        string hisUri;
        string cdlFilePath;

        bool onFocus;
        bool isUpdateCandle;
        bool isAddCandle;
        bool hasLocalLastCdlTime;
        bool hasLocalLastSigTime;

        bool isPause;
        bool isUpdateSignal = false;
        bool isUpdateCandleSignal = false;
        public bool Initialized;

        //bool isAddSignal = false;
        DateTime localLastCdlTime;
        DateTime localLastSigTime;
        DateTime startTime;
        DateTime lastTime = System.DateTime.Now;
        DateTime lastLocalTime = System.DateTime.Now;
        DateTime lastActionTime = System.DateTime.Now;

        public DateTime LastActionTime
        {
            get { return lastActionTime; }
            set { lastActionTime = value; }
        }

        public System.Timers.Timer timerInit;
        public System.Timers.Timer timerAuto;
        
        Interval currentTInterval;
        DataInitStatus initCdlStatus = DataInitStatus.NotStart;
        DataInitStatus initRtdStatus = DataInitStatus.NotStart;
        DataInitStatus initSigStatus = DataInitStatus.NotStart;

        DownloadStatus cdlLoadingStatus = DownloadStatus.NotStart;
        DownloadStatus rtdLoadingStatus = DownloadStatus.NotStart;
        protected Symbol symbol=Symbol.UNKNOWN;

        private ProviderHandler handler;
        DataCache dataCache;
        //Stat stat;
        Signal signal;

        List<BarData> latesRtds=new List<BarData>();
        //List<Signal> signalList=new List<Signal>();



        public DataProvider(string symbol)
        {
            this.symbol = (Symbol)Enum.Parse(typeof(Symbol), symbol);
            onFocus = true;
            currentTInterval = AppSetting.INTEVALS[0];
            dataCache = new DataCache(this);
            //stat = new Stat(this);
            Register();
            timerInit = new System.Timers.Timer(500);
            timerInit.Elapsed += new System.Timers.ElapsedEventHandler(CheckInitStatus);
            timerInit.AutoReset = true;
            timerInit.Enabled = true;
            timerInit.Stop();
            timerAuto = new System.Timers.Timer(5000);
            timerAuto.Elapsed += new System.Timers.ElapsedEventHandler(Execute);
            timerAuto.AutoReset = true;
            timerAuto.Enabled = true;
            timerAuto.Stop();
        }

        public ProviderHandler Handler
        {
            get { return handler; }
            set { handler = value; }
        }

        //DateTime startTime;
        public void Init()
        {
            lock (this)
            {
                Console.WriteLine(" Start  Now   ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + DateTime.Now.ToLongTimeString());
                Initialized = false;
                Initializing = false;
                initCdlStatus = DataInitStatus.NotStart;
                initRtdStatus = DataInitStatus.NotStart;
                cdlLoadingStatus = DownloadStatus.NotStart;
                rtdLoadingStatus = DownloadStatus.NotStart;                

                //currentTInterval = AppSetting.INTEVALS[0];
                isUpdateCandle = false;
                isAddCandle = false;
                dataCache.InitLocalData();
   
                    handler = new ProviderHandler( this);
                    ProviderService.RegisterHandler(handler);
                    ProviderService.KickOff();
            }
            InitStart();
        }


        public void Init4Loading()
        {
            Initialized = false;
            Initializing = false;
            initCdlStatus = DataInitStatus.NotStart;
            initRtdStatus = DataInitStatus.NotStart;
            cdlLoadingStatus = DownloadStatus.NotStart;
            rtdLoadingStatus = DownloadStatus.NotStart;

            //currentTInterval = AppSetting.INTEVALS[0];
            isUpdateCandle = false;
            isAddCandle = false;
            dataCache.InitLocalData();
            //List<BarData> datas = new List<BarData>();
            //if (dataCache.DataUnits.ContainsKey(currentTInterval))
            //{
            //    datas = dataCache.DataUnits[currentTInterval];
            //    Console.WriteLine(" datas   count  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + datas.Count);
            //    if (datas.Count > 0)
            //    {
            //        localLastCdlTime = datas[datas.Count - 1].DateTime;
            //        hasLocalLastCdlTime = true;
            //    }
            //}
            //else
            //{               
            //    dataCache.DataUnits.Add(currentTInterval, datas);
            //}
        }

        public void InitStart()
        {
            timerInit.Start();

        }

        public void Run()
        {
            //Process(null, null);
            timerInit.Stop();
            timerAuto.Start();
        }


        public void Reconnect()
        {
            lock (this)
            {
                timerAuto.Stop();
                
                Initialized = false;
                Initializing = false;
                initCdlStatus = DataInitStatus.NotStart;
                initRtdStatus = DataInitStatus.NotStart;
                cdlLoadingStatus = DownloadStatus.NotStart;
                rtdLoadingStatus = DownloadStatus.NotStart;

                //currentTInterval = AppSetting.INTEVALS[0];
                isUpdateCandle = false;
                isAddCandle = false;

                if (dataCache != null)
                    dataCache.CleanCache();
                else
                    dataCache = new DataCache(this);

                handler = new ProviderHandler(this);
                ProviderService.RegisterHandler(handler);

                dataCache.InitLocalData();
                if (dataCache.DataUnits.ContainsKey(currentTInterval))
                {
                    List<BarData> datas = dataCache.DataUnits[currentTInterval];
                    Console.WriteLine(" datas   count  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + datas.Count);
                    if (datas.Count > 0)
                    {
                        localLastCdlTime = datas[datas.Count - 1].DateTime;
                       hasLocalLastCdlTime = true;
                    }
                }
                else
                {
                    List<BarData> datas = new List<BarData>();
                    dataCache.DataUnits.Add(currentTInterval, datas);
                }
                ProviderService.KickOff();
            }
            timerInit.Start();
            //RaiseValuesUpdateEvent(UpdateType.NewBar, 1, 0);
       }

        public string CdlFilePath
        {
            get { return cdlFilePath; }
        }
        
       public  bool Initializing = false;

       public void CheckInitStatus(object obj, System.Timers.ElapsedEventArgs e)
       {
           if (!Initializing)
           {
               lock (this)
               {
                   if (!Initialized && initCdlStatus == DataInitStatus.Initialized  &&
                      initRtdStatus == DataInitStatus.Initialized && initSigStatus == DataInitStatus.Initialized )
                   {
                       Initializing = true;
                       if( handler !=null )
                          handler.IsProcessing = false;
                       if (initRtdStatus == DataInitStatus.Initialized)
                       {
                           Console.WriteLine(" LatesRtds  count  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + LatesRtds.Count);
                           LogUtil.Info(" LatesRtds  count  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + LatesRtds.Count);
                           if (LatesRtds.Count > 0)
                           {
                              if (dataCache.DataUnits.ContainsKey(CurrentTInterval))
                               {
                                     dataCache.DataUnits[CurrentTInterval].AddRange(LatesRtds);
                               }
                               else
                               {
                                   dataCache.DataUnits.Add(CurrentTInterval, LatesRtds);
                               }
                               Console.WriteLine(" dataCache.DataUnits[CurrentTInterval].Count ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + dataCache.DataUnits[CurrentTInterval].Count);
                               LogUtil.Info(" dataCache.DataUnits[CurrentTInterval].Count  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + dataCache.DataUnits[CurrentTInterval].Count);
                           }
                       }

                       RealTimeData[] rtds = null;
                       lock (dataCache.RealTimeDatas)
                       {
                           if (dataCache.RealTimeDatas.Count > 0)
                           {
                               rtds = new RealTimeData[dataCache.RealTimeDatas.Count];
                               dataCache.RealTimeDatas.CopyTo(rtds);
                               dataCache.RealTimeDatas.Clear();
                           }
                       }

                       if (dataCache.DataUnits[CurrentTInterval].Count > 0)
                       {
                           List<BarData> datas = dataCache.DataUnits[CurrentTInterval];
						   BarData lastBar = dataCache.DataUnits[CurrentTInterval][datas.Count - 1];
                           dataCache.RefreshCurrentTime(currentTInterval, lastBar.DateTime);
                           IntervalData data = new IntervalData(lastBar.Open, lastBar.Close, lastBar.Low, lastBar.High);
                           dataCache.InitCurrentData(currentTInterval, data);
                           Console.WriteLine(" Last BAR  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + lastBar );
                           LogUtil.Info(" Last BAR  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + lastBar);
                           if (rtds != null && rtds.Length > 0)
                           {
                               Console.WriteLine(" dataCache.RealTimeDatas.  count  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + rtds.Length);
                               LogUtil.Info(" dataCache.RealTimeDatas.  count  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + rtds.Length);
                               Console.WriteLine(lastBar);
                               if (lastBar.IsCompleted)
                               {
                                   foreach (RealTimeData rtd in rtds)
                                   {
                                       dataCache.UpdateDataSource(rtd);
                                   }
                               }
                               else
                               {
                                   bool isCompleted = false;
                                   double max = lastBar.High;
                                   double min = lastBar.Low;
                                   List<double> prices = new List<double>();

                                   prices.Add(lastBar.Open);
                                   prices.Add(lastBar.Close);
                                   prices.Add(lastBar.Low);
                                   prices.Add(lastBar.High);

                                   for (int k = 0; k < rtds.Length; k++)
                                   {
                                       RealTimeData rtd = rtds[k];
                                       Console.WriteLine(" RealTimeData   ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + rtd);
                                       LogUtil.Info(" RealTimeData   ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + rtd);
                                       if (rtd.dateTime.CompareTo(lastBar.DateTime) < 0)
                                           continue;

                                       if (rtd.dateTime.Subtract(AppConst.AppTimeSpans[CurrentTInterval]) < lastBar.DateTime)
                                       {
                                           foreach (double price in rtd.datas)
                                           {
                                               prices.Add(price);
                                               max = Math.Max(max, price);
                                               min = Math.Min(min, price);
                                           }
                                           if (k == rtds.Length - 1 && prices.Count > 0)
                                           {
                                               lastBar.Close = prices[prices.Count - 1];
                                               lastBar.High = max;
                                               lastBar.Low = min;
                                               IntervalData newdata = new IntervalData(lastBar.Open, lastBar.Close, lastBar.Low, lastBar.High);
                                               dataCache.InitCurrentData(currentTInterval, newdata);
                                               dataCache.DataUnits[CurrentTInterval][datas.Count - 1] = lastBar;
                                           }
                                       }
                                       else if (rtd.dateTime.Subtract(AppConst.AppTimeSpans[CurrentTInterval]) >= lastBar.DateTime)
                                       {
                                           if (!isCompleted)
                                           {
                                               if (prices != null && prices.Count > 0)
                                               {
                                                   lastBar.Close = prices[prices.Count - 1];
                                                   lastBar.High = max;
                                                   lastBar.Low = min;
                                                   prices.Clear();
                                               }
                                               lastBar.IsCompleted = true;
                                               dataCache.DataUnits[CurrentTInterval][datas.Count - 1] = lastBar;
                                               IntervalData newdata = new IntervalData(lastBar.Open, lastBar.Close, lastBar.Low, lastBar.High);
                                               dataCache.InitCurrentData(currentTInterval, newdata);
                                               isCompleted = true;
                                               dataCache.UpdateDataSource(rtd);
                                           }
                                           else
                                           {
                                               dataCache.UpdateDataSource(rtd);
                                           }
                                       }
                                   }
                               }
                           }

                           SAR sar = new SAR(symbol.ToString(), currentTInterval);
                           RSI rsi = new RSI(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)),6);
                           RSI rsi2 = new RSI(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)),42);
                           RSI rsi3 = new RSI(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)),14);
                           RSI rsi4 = new RSI(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)),22);
                           CR cr = new CR(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)));
                           ARBR arbr = new ARBR(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)), 26);
                           ARBR arbr2 = new ARBR(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)),42);
                           CCI cci = new CCI(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)), 14);
                           //CCI cci2 = new CCI(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)), 42);
                           CCI cci2 = new CCI(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)), 24);
                           CCI cci3 = new CCI(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)), 42);
                           WR wr=new WR(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)),14);
                           WR wr2 = new WR(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)), 24);
LWR lwr=new LWR(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)), 14, 3, 3);
BOLL boll = new BOLL(symbol.ToString(), int.Parse(AppUtil.IntervalToString(currentTInterval)), 20);
                           for (int i = 0; i < datas.Count; i++)
                           {
                               BarData bar = datas[i];
                               if (bar.IsCompleted)
                               {
                                   sar.handleFullCandle(ref bar);
                                   rsi.handleFullCandle(ref bar, true, 1);
                                   rsi2.handleFullCandle(ref bar, true,2);
                                   rsi3.handleFullCandle(ref bar, true,3);
                                   rsi4.handleFullCandle(ref bar, true,4);
                                   cr.handleFullCandle(ref bar);
                                   arbr.handleFullCandle(ref bar, 1);
                                   arbr2.handleFullCandle(ref bar,2);
                                   cci.handleFullCandle(ref bar,1);
                                   cci2.handleFullCandle(ref bar,2);
                                   cci3.handleFullCandle(ref bar, 3);
                                   wr.handleFullCandle(ref bar, 1);
                                   wr2.handleFullCandle(ref bar,2);
                                   lwr.handleFullCandle(ref bar, 0);
                                   boll.handleFullCandle(ref bar, 0);
                                   datas[i] = bar;
                               }
                           }
                           dataCache.SARs[currentTInterval] = sar;
                           dataCache.RISs[currentTInterval] = rsi;
                           dataCache.RISs2[currentTInterval] = rsi2;
                           dataCache.RISs3[currentTInterval] = rsi3;
                           dataCache.RISs4[currentTInterval] = rsi4;
                           dataCache.CRs[currentTInterval] = cr;
                           dataCache.ARBRs[currentTInterval] = arbr;
                           dataCache.ARBRs2[currentTInterval] = arbr2;
                           dataCache.CCIs[currentTInterval] = cci;
                           dataCache.CCIs2[currentTInterval] = cci2;
                           dataCache.CCIs3[currentTInterval] = cci3;
                           dataCache.WRs[currentTInterval] = wr;
                           dataCache.WRs2[currentTInterval] = wr2;
                           dataCache.LWRs[currentTInterval] = lwr;
                           dataCache.BOLLs[currentTInterval] = boll;
                           
                           List<Signal> signalList = dataCache.SignalUnits[currentTInterval];
                           LogUtil.Info(" SignalList  count  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + signalList.Count);

                           if (signalList.Count > 0)
                           {
                               Signal[] signals = new Signal[signalList.Count];
                               signalList.CopyTo(signals, 0);
                               Array.Sort(signals);
                               Array.Reverse(signals);
                               DateTime currentTime = dataCache.CurrentTimes[currentTInterval];
                               int startIndex = 0;
                               foreach (Signal signal in signals)
                               {
                                   DateTime actTime = signal.ActTime;

                                   for (int i = startIndex; i < dataCache.DataUnits[currentTInterval].Count; i++)
                                   {
                                       bool isUpdate = false;
                                       List<CandleSignal> sigList = new List<CandleSignal>();
                                       BarData candle = dataCache.DataUnits[currentTInterval][i];
                                       if (actTime.CompareTo(candle.DateTime) > 0)
                                       {
                                           continue;
                                       }
                                       else if (actTime.CompareTo(candle.DateTime) == 0)
                                       {
                                           startIndex = i;
                                           isUpdate = true;
                                           LogUtil.Info(" Signal " + signal );
                                           LogUtil.Info(" Candle " + candle);
                                          // LogUtil.Info(" bf1st " + dataCache.DataUnits[currentTInterval][i-1]);
                                           //LogUtil.Info(" bf2nd " + dataCache.DataUnits[currentTInterval][i-2]);
                                           CandleSignal cs = new CandleSignal(signal, 1);
                                           sigList.Add(cs);
                                       }
                                       if (signal.GainTip!=0 && signal.GainTipPrice > 0 && candle.DateTime.CompareTo(signal.GainTipTime) == 0)
                                       {
                                           CandleSignal cs = new CandleSignal(signal, 2);
                                           sigList.Add(cs);
                                           isUpdate = true;
                                       }
                                       if (signal.StopLoss != 0 && signal.StopLossBidPrice > 0 && candle.DateTime.CompareTo(signal.StopLossTime) == 0)
                                       {
                                           CandleSignal cs = new CandleSignal(signal, 3);
                                           sigList.Add(cs);
                                           isUpdate = true;
                                       }
                                       if (signal.StopGain != 0 &&  signal.StopGainPrice > 0 && candle.DateTime.CompareTo(signal.StopGainTime) == 0)
                                       {
                                           CandleSignal cs = new CandleSignal(signal, 4);
                                           sigList.Add(cs);
                                           isUpdate = true;
                                       }
                                       if (isUpdate)
                                       {
                                           if (candle.SignalList != null)
                                               candle.SignalList.AddRange(sigList);
                                           else
                                               candle.SignalList = sigList;
                                           candle.RefreshExValues();
                                           dataCache.DataUnits[currentTInterval][i] = candle;
                                       }
                                   }
                                   List<CandleSignal> sList = new List<CandleSignal>();
                                   if (signal.ActTime.CompareTo(currentTime) <= 0)
                                   {
                                       if (signal.ActTime.CompareTo(currentTime) == 0)
                                       {
                                           CandleSignal cs = new CandleSignal(signal, 1);
                                           sList.Add(cs);
                                       }
                                       if (signal.GainTipPrice > 0 && signal.GainTipTime.CompareTo(currentTime) == 0)
                                       {
                                           CandleSignal cs = new CandleSignal(signal, 2);
                                           sList.Add(cs);
                                       }
                                       if (signal.StopLossBidPrice > 0 && signal.StopLossTime.CompareTo(currentTime) == 0)
                                       {
                                           CandleSignal cs = new CandleSignal(signal, 3);
                                           sList.Add(cs);
                                       }
                                       if (signal.StopGainPrice > 0 && signal.StopGainTime.CompareTo(currentTime) == 0)
                                       {
                                           CandleSignal cs = new CandleSignal(signal, 4);
                                           sList.Add(cs);
                                       }
                                       dataCache.CurrentDatas[currentTInterval].SignalList.AddRange(sList);
                                   }
                               }
                           }
						   Console.WriteLine(" LAST ###### "+lastBar);
                           LogUtil.Info(" LAST ###### " + lastBar);

                       

                           dataCache.RefreshDataUnits(datas);
                       }
                       //dataCache.Start();
                       if (AppContext.IsLoading)
                       {
                           AppContext.IsFirstProviderInit = true;
                           isAddCandle = true;
                           Initialized = true;
                           Initializing = false;
                           ProviderHelper.CleanInitDirs(this);
                       }
                       else
                       {
                           isAddCandle = true;
                           Initialized = true;
                           Process(null, null);
                           timerInit.Stop();
                           timerAuto.Start();
                           Initializing = false;
                           ProviderHelper.CleanInitDirs(this);
                           Console.WriteLine(" End  Now   ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;  " + DateTime.Now.ToLongTimeString());
                       }
                   }
               }
           }
       }

       public void SignalUpdate(Dictionary<Protocol, Signal> protocolSignal)
       {
           isUpdateSignal = false;
           isUpdateCandleSignal = false;
           Protocol protocol = protocolSignal.Keys.First();
           Signal signal = (Signal)protocolSignal.Values.First();
           Console.WriteLine("  signal   " + signal);
           LogUtil.Info("  signal   " + signal);
           if (dataCache.DataUnits.ContainsKey(currentTInterval))
           {
               lock (dataCache.DataUnits[currentTInterval])
               {
                   if (timerAuto != null)
                   {
                       timerAuto.Stop();
                   }
                   for (int i = dataCache.DataUnits[currentTInterval].Count - 1; i >= 0; i--)
                   {
                       BarData data = dataCache.DataUnits[currentTInterval][i];
                       if (data.SignalList == null)
                           data.SignalList = new List<CandleSignal>();
                       if (protocol==Protocol.K0004_1 && signal.Arrow != 0 && data.DateTime.CompareTo(signal.ActTime) == 0)
                       {
                           isUpdateCandleSignal = true;
                           CandleSignal cs = new CandleSignal(signal, 1);
                           data.SignalList.Add(cs);
                           dataCache.DataUnits[currentTInterval][i] = data;
                           break;
                       }
                       if (protocol == Protocol.K0004_3 &&  signal.GainTip != 0 && data.DateTime.CompareTo(signal.GainTipTime) == 0)
                       {
                           isUpdateCandleSignal = true;
                           CandleSignal cs = new CandleSignal(signal, 2);
                           data.SignalList.Add(cs);
                           dataCache.DataUnits[currentTInterval][i] = data;
                           break;
                       }
                       if (protocol == Protocol.K0004_2 && signal.StopLoss != 0 && data.DateTime.CompareTo(signal.StopLossTime) == 0)
                       {
                           isUpdateCandleSignal = true;
                           CandleSignal cs = new CandleSignal(signal, 3);
                           data.SignalList.Add(cs);
                           dataCache.DataUnits[currentTInterval][i] = data;
                           break;
                       }
                       if (protocol == Protocol.K0004_4 && signal.StopGain != 0 && data.DateTime.CompareTo(signal.StopGainTime) == 0)
                       {
                           isUpdateCandleSignal = true;
                           CandleSignal cs = new CandleSignal(signal, 4);
                           data.SignalList.Add(cs);
                           dataCache.DataUnits[currentTInterval][i] = data;
                           break;
                       }
                       if (data.DateTime.CompareTo(signal.ActTime) < 0)
                       {
                           break;
                       }
                   }
               }
           }
           if (isUpdateCandleSignal)
           {
               Process(null, null);
           }
           else
           {
               RealTimeData rtd = new RealTimeData();
               rtd.symbol = symbol;
               if (protocol == Protocol.K0004_1)
               {
                   rtd.dateTime = signal.ActTime;
                   rtd.datas = new double[] { signal.ActPrice };
               }
               else if (protocol == Protocol.K0004_2)
               {
                   rtd.dateTime = signal.StopLossTime;
                   rtd.datas = new double[] { signal.StopLossBidPrice };
               }
               else if (protocol == Protocol.K0004_3)
               {
                   rtd.dateTime = signal.GainTipTime;
                   rtd.datas = new double[] { signal.GainTipPrice };
               }
               else if (protocol == Protocol.K0004_4)
               {
                   rtd.dateTime = signal.StopGainTime;
                   rtd.datas = new double[] { signal.StopGainPrice };
               }
               dataCache.UpdateDataSource(rtd);
               isUpdateCandleSignal = true;
           }

           LogUtil.Info("  dataCache.CurrentTimes[currentTInterval]   " + dataCache.CurrentTimes[currentTInterval]);
           if (dataCache.CurrentTimes[currentTInterval].CompareTo(signal.ActTime) == 0)
           {
               lock (dataCache.CurrentDatas[currentTInterval])
               {
                   if (protocol == Protocol.K0004_1 && signal.Arrow != 0)
                   {
                       CandleSignal cs = new CandleSignal(signal, 1);
                       if (!dataCache.CurrentDatas[currentTInterval].SignalList.Contains(cs))
                       dataCache.CurrentDatas[currentTInterval].SignalList.Add(cs);
                       
                       isUpdateSignal = true;
                   }
               }
           }         
           if (dataCache.CurrentTimes[currentTInterval].CompareTo(signal.GainTipTime) == 0)
           {
               lock ( dataCache.CurrentDatas[currentTInterval])
               {
                   if (protocol == Protocol.K0004_3 && signal.GainTip != 0)
                   {
                       CandleSignal cs = new CandleSignal(signal, 2);
                       if (!dataCache.CurrentDatas[currentTInterval].SignalList.Contains(cs))
                       dataCache.CurrentDatas[currentTInterval].SignalList.Add(cs);
                       isUpdateSignal = true;
                   }
               }
           }
           if (dataCache.CurrentTimes[currentTInterval].CompareTo(signal.StopLossTime) == 0)
           {
               lock (dataCache.CurrentDatas[currentTInterval])
               {
                   if (protocol == Protocol.K0004_2 && signal.StopLoss != 0)
                   {
                       CandleSignal cs = new CandleSignal(signal, 3);
                       if (!dataCache.CurrentDatas[currentTInterval].SignalList.Contains(cs))
                       dataCache.CurrentDatas[currentTInterval].SignalList.Add(cs);
                       isUpdateSignal = true;
                   }
               }
           }
           if (dataCache.CurrentTimes[currentTInterval].CompareTo(signal.StopGainTime) == 0)
           {
               lock (dataCache.CurrentDatas[currentTInterval])
               {
                   if (protocol==Protocol.K0004_1 &&signal.StopGain != 0)
                   {
                       CandleSignal cs = new CandleSignal(signal, 4);
                       if (!dataCache.CurrentDatas[currentTInterval].SignalList.Contains(cs))
                       dataCache.CurrentDatas[currentTInterval].SignalList.Add(cs);
                       isUpdateSignal = true;
                   }
               }
           }
           LogUtil.Info(" isUpdateSignal   " + isUpdateSignal);
           if (isUpdateSignal || isUpdateCandleSignal)
           {
               AppContext.TradeAnalyzerControl.AddSignalSymbol(signal.Symbol);
               AppContext.TradeAnalyzerControl.ActiveTimer();
               if (AppContext.IsOpenSpeaker)
               {
                   System.Media.SoundPlayer player = new SoundPlayer(Properties.Resources.chimes);
                   player.LoadAsync();
                   player.Play();
               }
           }
           Process(null, null);
           isUpdateSignal = false;
           isUpdateCandleSignal = false;
           if (timerAuto != null)
           {
               timerAuto.Start();
           }
       }

        public void Register()
        {
            DataService.RegisterDataProvider(this);
        }

        public void Execute(object sender, ElapsedEventArgs e)
        {
            dataCache.DataUpdate();
        }

        public void SwitchTimeInterval()
        {
            dataCache.GetDataUnits(symbol, currentTInterval, ref _dataUnits);
            Console.WriteLine("  _dataUnits  " + _dataUnits.Count);
            RaiseValuesUpdateEvent(UpdateType.NewBar, 1, 0);
            isAddCandle = false;
            isUpdateCandle = false;
        }

        public void Process(object sender, ElapsedEventArgs e)
        {
            lock (this)
            {
                if (isUpdateCandle && !isAddCandle && _dataUnits.Count > 0)
                {
                    Dictionary<Interval, IntervalData> currentDatas = dataCache.CurrentDatas;
                    Dictionary<Interval, DateTime> currentTimes = dataCache.CurrentTimes;
                    if (currentDatas.ContainsKey(currentTInterval))
                    {
                        BarData barData;
                        IntervalData intervData = currentDatas[currentTInterval];
                        DateTime currentTime = currentTimes[currentTInterval];
                        Console.WriteLine("  currentTime   " + currentTime);
                        barData = new BarData(currentTime, intervData.Open, intervData.Close, intervData.Min, intervData.Max, 0);
                        barData.SignalList = intervData.SignalList;
                        //barData.RefreshExValues();
                        if (_dataUnits.Count > 0 && _dataUnits[_dataUnits.Count - 1].DateTime.CompareTo(barData.DateTime) == 0)
                        {
                            barData.RefreshExValues();
                            RSI rsi = dataCache.RISs[currentTInterval];
                            RSI rsi2 = dataCache.RISs2[currentTInterval];
                            RSI rsi3 = dataCache.RISs3[currentTInterval];
                            RSI rsi4 = dataCache.RISs4[currentTInterval];
                            SAR sar= dataCache.SARs[currentTInterval];
                            CCI cci = dataCache.CCIs[currentTInterval];
                            CCI cci2 = dataCache.CCIs2[currentTInterval];

                            rsi.handleFullCandle(ref barData, false, 1);
                            rsi2.handleFullCandle(ref barData, false,2);
                            rsi3.handleFullCandle(ref barData, false,3);
                            rsi4.handleFullCandle(ref barData, false,4);
                            cci.handleFragCandle(ref barData,2);
                            cci2.handleFragCandle(ref barData,2);
                            barData.Sar=sar.getNextSAR();
                            LogUtil.Info(" Current Candle   " + barData);
                            _dataUnits[_dataUnits.Count - 1] = barData;
                            dataCache.RISs[currentTInterval]= rsi;
                        }
                    }
                    isUpdateCandle = false;
                }
                else if (isAddCandle)
                {
                    dataCache.GetDataUnits(symbol, currentTInterval, ref _dataUnits);
                    Console.WriteLine("  _dataUnits  " + _dataUnits.Count);
                    isAddCandle = false;
                    //isUpdateCandle = false;
                }
                else if (isUpdateCandleSignal)
                {
                    dataCache.GetDataUnits(symbol, currentTInterval, ref _dataUnits);
                    isUpdateCandleSignal = false;
                }
                else if (isUpdateSignal)
                {
                    dataCache.GetDataUnits(symbol, currentTInterval, ref _dataUnits);
                    Dictionary<Interval, DateTime> currentTimes = dataCache.CurrentTimes;
                    Dictionary<Interval, IntervalData> currentDatas = dataCache.CurrentDatas;
                    if (currentDatas.ContainsKey(currentTInterval))
                    {
                        BarData barData;
                        IntervalData intervData = currentDatas[currentTInterval];
                        DateTime currentTime = currentTimes[currentTInterval];
                        Console.WriteLine("  currentTime   " + currentTime);
                        barData = new BarData(currentTime, intervData.Open, intervData.Close, intervData.Min, intervData.Max, 0);
                        barData.SignalList = intervData.SignalList;
                        if (_dataUnits.Count > 0 && _dataUnits[_dataUnits.Count - 1].DateTime.CompareTo(barData.DateTime) == 0)
                        {
                            barData.RefreshExValues();
                            RSI rsi = dataCache.RISs[currentTInterval];
                            RSI rsi2 = dataCache.RISs2[currentTInterval];
                            RSI rsi3 = dataCache.RISs3[currentTInterval];
                            RSI rsi4 = dataCache.RISs4[currentTInterval];
                            SAR sar = dataCache.SARs[currentTInterval];
                            CCI cci = dataCache.CCIs[currentTInterval];
                            CCI cci2 = dataCache.CCIs2[currentTInterval];

                            rsi.handleFullCandle(ref barData, false, 1);
                            rsi2.handleFullCandle(ref barData, false, 2);
                            rsi3.handleFullCandle(ref barData, false, 3);
                            rsi4.handleFullCandle(ref barData, false, 4);
                            cci.handleFragCandle(ref barData,2);
                            cci2.handleFragCandle(ref barData,2);

                            barData.Sar = sar.getNextSAR();
                            LogUtil.Info(" isUpdateSignal Candle   " + barData);
                            _dataUnits[_dataUnits.Count - 1] = barData;
                        }
                    }
                }

                Console.WriteLine("  _dataUnits  " + _dataUnits.Count);
                isAddCandle = false;
                isUpdateCandle = false;

            }

            if (isPause && DateTime.Now.Subtract(lastActionTime) > TimeSpan.FromMinutes(1))
                isPause = false;


            Console.WriteLine("  isPause   " + isPause);
            //if (!isPause)
            RaiseValuesUpdateEvent(UpdateType.NewBar, 1, 0);

        }

        private void NewThreadUpdate()
        {
            RaiseValuesUpdateEvent(UpdateType.NewBar, 1, 0);
        }

        //------------------------ New -------------------------------//

        int _initialAvailablePeriodsCount = 12;
        public int InitialAvailablePeriodsCount
        {
            get { lock (this) { return _initialAvailablePeriodsCount; } }
            set { lock (this) { _initialAvailablePeriodsCount = value; } }
        }

        public List<BarData> _totalUnits = new List<BarData>();

        public List<BarData> TotalUits
        {
            get
            {
                return _totalUnits;
            }
            set
            {
                _totalUnits = value;
            }
        }

        #region ITimeControl Members

        public int PeriodsCount
        {
            get { lock (this) { return _totalUnits.Count; } }
        }

        public int CurrentPeriod
        {
            get { lock (this) { return _dataUnits.Count; } }
        }

        #endregion

        public override double Ask
        {
            get
            {
                lock (this)
                {
                    if (CurrentDataUnit == null)
                    {
                        return double.NaN;
                    }

                    return CurrentDataUnit.Value.Close;
                }
            }
        }

        public override double Bid
        {
            get
            {
                lock (this)
                {
                    if (CurrentDataUnit == null)
                    {
                        return double.NaN;
                    }
                    return CurrentDataUnit.Value.Close - Spread;
                }
            }
        }

        bool DoStepForward(int stepsRemaining)
        {
            lock (this)
            {
                // Modified by Harry for update real time data
                if (_totalUnits.Count == 0)
                {
                    return false;
                }
                else if (_totalUnits.Count > 0 && _dataUnits.Count == _totalUnits.Count)
                {
                    _dataUnits[_totalUnits.Count - 1] = _totalUnits[_totalUnits.Count - 1];
                }
                else if (_totalUnits.Count > 0 && _dataUnits.Count < _totalUnits.Count)
                {
                    _dataUnits.Add(_totalUnits[_dataUnits.Count]);
                }

                //_dataUnits.Add(_dataUnits[_dataUnits.Count-1]);
            }

            RaiseValuesUpdateEvent(UpdateType.NewBar, 1, stepsRemaining);

            return true;
        }

        public bool StepForward()
        {
            return DoStepForward(0);
        }

        public bool StepBack()
        {
            lock (this)
            {
                return false;
            }
        }

        public bool StepTo(int index)
        {
            if (index < CurrentPeriod)
            {
                return false;
            }
            // This is before the start.
            int current = CurrentPeriod;
            for (int i = current; i < index; i++)
            {
                // Last step is not marked as multi step to indicate stepping is over.
                if (DoStepForward(index - 1 - i) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public override double Spread
        {
            get
            {
                lock (this)
                {
                    return _spread;
                }
            }
        }

        public void RetrieveCdlFileAndLatestRtd( object obj )
        {
            if (!string.IsNullOrEmpty(hisUri) && cdlLoadingStatus == DownloadStatus.NotStart)
            {
                // download 
                DSWebClient webClient = new DSWebClient(this);
                cdlFilePath =  ProviderHelper.GetCdlFilePath(this);
                Console.WriteLine(" DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD  uri  " + hisUri);
                Console.WriteLine(" DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD  output file   " + cdlFilePath);
                webClient.LoadingMark = "H";
                webClient.DownloadFile(hisUri, cdlFilePath);
            }
        }

        public void InitCdlFile()
        {
            if (!string.IsNullOrEmpty(cdlFilePath) && cdlLoadingStatus == DownloadStatus.Finished
                && initCdlStatus == DataInitStatus.NotStart)
            {
                Console.WriteLine(" IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII  uri  " + hisUri);
                Console.WriteLine(" IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII  output file   " + cdlFilePath);

                IFileWorker fw = FileUtil.GetFileWorker();
                List<BarData> datas;
                fw.LoadCdlFile(cdlFilePath, 0, int.MaxValue, out datas, true, false);               

                if (datas.Count > 0)
                {
                    Console.WriteLine(" HIs ..................... datas.Count    " + datas.Count);
                    dataCache.InitDataUnits(datas);
                }
                initCdlStatus = DataInitStatus.Initialized;
                fw = null;
            }
            else if (!string.IsNullOrEmpty(cdlFilePath) && cdlLoadingStatus == DownloadStatus.Failed
                && initCdlStatus == DataInitStatus.NotStart)
            {
                //initCdlStatus = DataInitStatus.Failed;
            }

            if (AppContext.IsLoading)
            {
                AppContext.IsLoadInitProviderFile = true;
            }
        }


        public override void UnInitialize()
        {
            Console.WriteLine(" @@@@@@@@@@@@@@@@@@@@ UnInitialize  @@@@@@@@@@@@@@@@@@ ");
            dataCache.UnInitialize();            
            
            if (timerInit != null)
            {
                timerInit.Stop();
                timerInit = null;
            }
            if (timerAuto != null)
            {
                timerAuto.Stop();
                timerAuto = null;
            }
            DataService.UnRegisterDataProvider(this);
            base.UnInitialize();
        }

        public Interval CurrentTInterval
        {
            get { return currentTInterval; }
            set { currentTInterval = value; }
        }
        public bool OnFocus
        {
            get { return onFocus; }
            set { onFocus = value; }
        }
        public Symbol Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        public double RealTimePrice
        {
            get { return realTimePrice; }
            set { realTimePrice = value; }
        }
        public bool IsUpdateCandle
        {
            get { return isUpdateCandle; }
            set { isUpdateCandle = value; }
        }

        public bool IsAddCandle
        {
            get { return isAddCandle; }
            set { isAddCandle = value; }
        }
        public DataCache DataCache
        {
            get { return dataCache; }
            set { dataCache = value; }
        }
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public string HisUri
        {
            get { return hisUri; }
            set { hisUri = value; }
        }

        public bool IsPause
        {
            get { return isPause; }
            set { isPause = value; }
        }

        public Signal Signal
        {
            get { return signal; }
            set { signal = value; }
        }

        public bool HasLocalLastCdlTime
        {
            get { return hasLocalLastCdlTime; }
            set { hasLocalLastCdlTime = value; }
        }

        public DateTime LocalLastCdlTime
        {
            get { return localLastCdlTime; }
            set { localLastCdlTime = value; }
        }

        public DateTime LocalLastSigTime
        {
            get { return localLastSigTime; }
            set { localLastSigTime = value; }
        }

        public DataInitStatus InitCdlStatus
        {
            get { return initCdlStatus; }
            set { initCdlStatus = value; }
        }

        public DataInitStatus InitRtdStatus
        {
            get { return initRtdStatus; }
            set { initRtdStatus = value; }
        }

        public DownloadStatus CdlLoadingStatus
        {
            get { return cdlLoadingStatus; }
            set { cdlLoadingStatus = value; }
        }

        public DownloadStatus RtdLoadingStatus
        {
            get { return rtdLoadingStatus; }
            set { rtdLoadingStatus = value; }
        }

        public List<BarData> LatesRtds
        {
            get { return latesRtds; }
            set { latesRtds = value; }
        }

        public bool HasLocalLastSigTime
        {
            get { return hasLocalLastSigTime; }
            set { hasLocalLastSigTime = value; }
        } 

        public DataInitStatus InitSigStatus
        {
            get { return initSigStatus; }
            set { initSigStatus = value; }
        }
    }
}
