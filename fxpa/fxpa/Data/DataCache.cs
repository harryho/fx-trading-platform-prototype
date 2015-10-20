using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace fxpa
{
    public class DataCache
    {
        static int MAXIMUM = 5000;
        static int BASE = 1000;
        static int OFFSET = 20;
        System.Timers.Timer timer;
        bool isSavingFile = false;
        bool isInitialized = false;
        DateTime startTime;
        Dictionary<Interval, DateTime> lastCdlTimes = new Dictionary<Interval, DateTime>();
        Dictionary<Interval, DateTime> currentTimes = new Dictionary<Interval, DateTime>();
        Dictionary<Interval, IntervalData> tmpIntervDatas = new Dictionary<Interval, IntervalData>();
        List<RealTimeData> realTimeDatas = new List<RealTimeData>();
        Dictionary<Interval, IntervalData> currentDatas = new Dictionary<Interval, IntervalData>();
        Dictionary<Interval, List<BarData>> dataUnits = new Dictionary<Interval, List<BarData>>();
        Dictionary<Interval, List<Signal>> signalUnits = new Dictionary<Interval, List<Signal>>();
        Dictionary<Interval, int> totalProfitLoss = new Dictionary<Interval, int>();
        Dictionary<Interval, SAR> iSARs = new Dictionary<Interval, SAR>();
        Dictionary<Interval, CR> iCRs = new Dictionary<Interval, CR>();
        Dictionary<Interval, ARBR> iARBRs = new Dictionary<Interval, ARBR>();
        Dictionary<Interval, ARBR> iARBRs2 = new Dictionary<Interval, ARBR>();
        Dictionary<Interval, RSI> iRISs = new Dictionary<Interval, RSI>();
        Dictionary<Interval, RSI> iRISs2 = new Dictionary<Interval, RSI>();
        Dictionary<Interval, RSI> iRISs3 = new Dictionary<Interval, RSI>();
        Dictionary<Interval, RSI> iRISs4 = new Dictionary<Interval, RSI>();
        Dictionary<Interval, CCI> iCCIs = new Dictionary<Interval, CCI>();
        Dictionary<Interval, CCI> iCCIs2 = new Dictionary<Interval, CCI>();
        Dictionary<Interval, CCI> iCCIs3 = new Dictionary<Interval, CCI>();
        Dictionary<Interval, WR> iWRs = new Dictionary<Interval, WR>();
        Dictionary<Interval, WR> iWRs2 = new Dictionary<Interval, WR>();
        Dictionary<Interval, LWR> iLWRs = new Dictionary<Interval, LWR>();
        Dictionary<Interval, BOLL> iBOLLs = new Dictionary<Interval, BOLL>();

  

        DataProvider provider;

        public DataCache(DataProvider dp)
        {
            provider = dp;
            //  180,000  means 30 mins 
            //  360,000  means 1 hour 
            //timer = new System.Timers.Timer(5000);
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(SaveDataToFile);
            //timer.AutoReset = true;
            //timer.Enabled = true;
            //timer.Stop();
        }

        public delegate void DataChangedHandler(RealTimeData realTimeData);

        public void DataUpdate()
        {
            RealTimeData[] rtds;
            lock (realTimeDatas)
            {
                if (this.realTimeDatas.Count == 0) return;
                rtds = new RealTimeData[realTimeDatas.Count];
                realTimeDatas.CopyTo(rtds);
                realTimeDatas.Clear();
            }

            if (isInitialized)
            {
                foreach (RealTimeData rtd in rtds)
                {
                    Console.WriteLine("  RTD " + rtd.symbol + "  time  " + rtd.dateTime + " price " + rtd.datas[0]);
                    LogUtil.Info(" RTD " + rtd.ToString());
                    UpdateDataSource(rtd);
                    UpdateDataProvider(rtd);
                }
            }
        }

        public void UpdateDataSource(RealTimeData rtd)
        {
            Symbol symbol = rtd.symbol;
            double[] prices = rtd.datas;
            DateTime time = rtd.dateTime;

            if (prices != null)
            {
                Dictionary<Interval, IntervalData> currentlData = currentDatas;

                lock (this)
                {
                    foreach (Interval intv in AppSetting.INTEVALS)
                    {
                        IntervalData intvData;
                        if (currentlData.ContainsKey(intv))
                            intvData = currentlData[intv];
                        else
                            intvData = new IntervalData(0);

                        UpdateIntervalData(intv, ref  intvData, prices, time, provider);

                        tmpIntervDatas[intv] = intvData;
                        if (currentlData.ContainsKey(intv))
                            currentlData[intv] = intvData; // tmpIntervDatas[intv];
                        else
                            currentlData.Add(intv, intvData);
                        LogUtil.Info("  intvData  " + intvData.ToString());
                    }
                }
            }
        }

        private void UpdateIntervalData(Interval intervSpan, ref IntervalData intvData, double[] prices, DateTime time, DataProvider provider)
        {
            if (currentTimes.ContainsKey(intervSpan) && currentDatas.ContainsKey(intervSpan))
            {
                DateTime lastTime = currentTimes[intervSpan];
                DateTime currTime = AppUtil.GetCurrentIntervalTime(lastTime, time, intervSpan);
                TimeSpan span = time.Subtract(lastTime);
                Console.WriteLine("  currentTimes[TInterval.MIN1]  span  " + span + "  lastTime " + lastTime + " real  time " + time + " currTime " + currTime);
                LogUtil.Info("  currentTimes[TInterval.MIN1]  span  " + span + "  lastTime " + lastTime + " real  time " + time + " currTime " + currTime);
                if (lastTime.CompareTo(time) > 0)
                {
                    return;
                }
                else if (span >= AppConst.AppTimeSpans[intervSpan])
                {
                    lastTime = AppUtil.GetLatestIntervalTime(lastTime, time, intervSpan);
                    Console.WriteLine("   span > interval  span:::   " + span + "  lastTime " + lastTime + " real  time " + time);
                    BarData barData = dataUnits[intervSpan][dataUnits[intervSpan].Count - 1];
                    barData.DateTime = lastTime;
                    barData.Open = intvData.Open;
                    barData.Close = intvData.Close;
                    barData.Low = intvData.Min;
                    barData.High = intvData.Max;
                    barData.SignalList = intvData.SignalList;

                    Console.WriteLine(" BarData  #### time " + barData);

                    List<CandleSignal> currSignalList = new List<CandleSignal>();
                    Signal[] signals = null;
                    lock (AppConst.SignalDatasLocker)
                    {
                        signals = new Signal[AppContext.SignalDatas.Count];
                        AppContext.SignalDatas.CopyTo(signals);
                    }
                    if (signals != null)
                    {
                        int count = 0;
                        Array.Sort(signals);
                        foreach (Signal s in signals)
                        {
                            if (s.Symbol == provider.Symbol && s.Interval == provider.CurrentTInterval)
                            {
                                if (s.ActTime.CompareTo(lastTime) == 0 && s.Arrow != 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 1);
                                    if (!barData.SignalList.Contains(cs))
                                        barData.SignalList.Add(cs);
                                }
                                if (s.Arrow != 0 && s.ActTime.CompareTo(currTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 1);
                                    currSignalList.Add(cs);
                                }
                                if (s.StopLoss != 0 && s.StopLossTime.CompareTo(lastTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 3);
                                    if (!barData.SignalList.Contains(cs))
                                        barData.SignalList.Add(cs);
                                }
                                if (s.StopLoss != 0 && s.StopLossTime.CompareTo(currTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 3);
                                    currSignalList.Add(cs);
                                }
                                if (s.GainTip != 0 && s.GainTipTime.CompareTo(lastTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 2);
                                    if (!barData.SignalList.Contains(cs))
                                        barData.SignalList.Add(cs);
                                }
                                if (s.GainTip != 0 && s.GainTipTime.CompareTo(currTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 2);
                                    currSignalList.Add(cs);
                                }
                                if (s.StopGain != 0 && s.StopGainTime.CompareTo(lastTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 4);
                                    if (!barData.SignalList.Contains(cs))
                                        barData.SignalList.Add(cs);
                                }
                                if (s.StopGain != 0 && s.StopGainTime.CompareTo(currTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 4);
                                    currSignalList.Add(cs);
                                }
                                if (count > 5) break;
                                count++;
                            }
                        }
                    }
                    barData.RefreshExValues();
                    Console.WriteLine(" BarData  ############ time " + barData);
                    LogUtil.Info(" Last BarData  #####  " + barData);
                    SAR sar = null;
                    CR cr = null;
                    RSI rsi = null;
                    RSI rsi2 = null;
                    RSI rsi3 = null;
                    RSI rsi4 = null;
                    ARBR arbr = null;
                    ARBR arbr2 = null;
                    CCI cci = null;
                    CCI cci2 = null;
                    CCI cci3 = null;
                    WR wr = null;
                    WR wr2 = null;
                    LWR lwr = null;
                    BOLL boll = null;
                    if (!provider.Initializing && provider.Initialized)
                    {
                        sar = iSARs[intervSpan];
                        cr = iCRs[intervSpan];
                        rsi = iRISs[intervSpan];
                        rsi2 = iRISs2[intervSpan];
                        rsi3 = iRISs3[intervSpan];
                        rsi4 = iRISs4[intervSpan];
                        arbr = iARBRs[intervSpan];
                        arbr2 = iARBRs2[intervSpan];
                        cci = iCCIs[intervSpan];
                        cci2 = iCCIs2[intervSpan];
                        cci3 = iCCIs3[intervSpan];
                        wr = iWRs[intervSpan];
                        wr2 = iWRs2[intervSpan];
                        lwr = iLWRs[intervSpan];
                        boll = iBOLLs[intervSpan];

                        sar.handleFullCandle(ref barData);
                        rsi.handleFullCandle(ref barData, true,1);
                        rsi2.handleFullCandle(ref barData, true,2);
                        rsi3.handleFullCandle(ref barData, true,3);
                        rsi4.handleFullCandle(ref barData, true,4);
                        cr.handleFullCandle(ref barData);
                        arbr.handleFullCandle(ref barData,1);
                        arbr2.handleFullCandle(ref barData,2);
                        cci.handleFullCandle(ref barData,1);
                        cci2.handleFullCandle(ref barData,2);
                        cci3.handleFragCandle(ref barData, 3);
                        wr.handleFullCandle(ref barData,1);
                        wr2.handleFullCandle(ref barData,2);
                        lwr.handleFullCandle(ref barData, 0);
                        boll.handleFullCandle(ref barData, 0);
                    }

                    if (dataUnits.ContainsKey(intervSpan))
                    {
                        dataUnits[intervSpan][dataUnits[intervSpan].Count - 1] = (barData);
                    }
                    else
                    {
                        List<BarData> list = new List<BarData>();
                        list.Add(barData);
                        dataUnits.Add(intervSpan, list);
                    }

                    if (provider.CurrentTInterval == intervSpan)
                    {
                        provider.IsUpdateCandle = false;
                        provider.IsAddCandle = true;
                    }

                    Console.WriteLine(" New BarData  #### time " + currTime);
                    currentTimes[intervSpan] = currTime;
                    Console.WriteLine("  currentTimes[" + intervSpan + "]  " + currentTimes[intervSpan]);
                    Console.WriteLine(" NEW RTD.symbol    time  " + time + " price " + prices[0]);
                    intvData = new IntervalData(0);
                    double p = prices[0];
                    intvData.Max = p;
                    intvData.Min = p;
                    intvData.Open = p;
                    intvData.Close = p;
                    intvData.PriceList.Clear();
                    foreach (double price in prices)
                    {
                        intvData.PriceList.Add(price);
                        intvData.Max = Math.Max(intvData.Max, price);
                        intvData.Min = Math.Min(intvData.Min, price);
                    }

                    barData = new BarData(currTime, intvData.Open, intvData.Close, intvData.Min, intvData.Max, 0);
                    barData.SignalList = currSignalList;
                    barData.RefreshExValues();


                    if (!provider.Initializing && provider.Initialized)
                    {
                        barData.Sar = sar.getNextSAR();
                        rsi.handleFullCandle(ref barData, false, 1);
                        rsi2.handleFullCandle(ref barData, false, 2);
                        rsi3.handleFullCandle(ref barData, false, 3);
                        rsi4.handleFullCandle(ref barData, false, 4);
                        cr.handleFullCandle(ref barData);
                        cci.handleFragCandle(ref barData,2);
                        cci2.handleFragCandle(ref barData,2);


                        iSARs[intervSpan] = sar;
                        iRISs[intervSpan] = rsi;
                        iRISs2[intervSpan] = rsi2;
                        iRISs3[intervSpan] = rsi3;
                        iRISs4[intervSpan] = rsi4;
                        iCRs[intervSpan] = cr;
                        iARBRs[intervSpan] = arbr;
                        iARBRs2[intervSpan] = arbr2;
                        iCCIs[intervSpan] = cci;
                        iCCIs2[intervSpan] = cci2;
                        iCCIs3[intervSpan] = cci3;
                        iWRs[intervSpan] = wr;
                        iWRs2[intervSpan] = wr2;
                        iLWRs[intervSpan] = lwr;
                        iBOLLs[intervSpan]=boll;
                    }

                    LogUtil.Info(" New BarData  ####  " + barData);
                    dataUnits[intervSpan].Add(barData);

                    Console.WriteLine(" New  intvData  ############ op " + intvData.Open + " cls " + intvData.Close + "  max  " + intvData.Max + "  low " + intvData.Min);
                    Console.WriteLine("   allDataUnits[provider.Symbol][intervSpan]..Count  ::::::::::::::::::::::::::: " + dataUnits[intervSpan].Count);
                }
                else
                {
                    Console.WriteLine(" IN  currentTimes[" + intervSpan + "]  " + currentTimes[intervSpan]);
                    foreach (double price in prices)
                    {
                        intvData.PriceList.Add(price);
                        intvData.Max = Math.Max(intvData.Max, price);
                        intvData.Min = Math.Min(intvData.Min, price);
                    }
                    Signal[] signals = null;
                    lock (AppConst.SignalDatasLocker)
                    {
                        signals = new Signal[AppContext.SignalDatas.Count];
                        AppContext.SignalDatas.CopyTo(signals);
                    }
                    if (signals != null)
                    {
                        Array.Sort(signals);
                        int count = 0;
                        foreach (Signal s in signals)
                        {
                            if (s.Symbol == provider.Symbol && s.Interval == provider.CurrentTInterval)
                            {
                                if (s.Arrow != 0 && s.ActTime.CompareTo(lastTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 1);
                                    if (!intvData.SignalList.Contains(cs))
                                        intvData.SignalList.Add(cs);
                                }
                                if (s.GainTip != 0 && s.GainTipTime.CompareTo(lastTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 2);
                                    if (!intvData.SignalList.Contains(cs))
                                        intvData.SignalList.Add(cs);
                                }
                                if (s.StopLoss != 0 && s.StopLossTime.CompareTo(lastTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 3);
                                    if (!intvData.SignalList.Contains(cs))
                                        intvData.SignalList.Add(cs);
                                }
                                if (s.StopGain != 0 && s.StopGainTime.CompareTo(lastTime) == 0)
                                {
                                    CandleSignal cs = new CandleSignal(s, 4);
                                    if (!intvData.SignalList.Contains(cs))
                                        intvData.SignalList.Add(cs);
                                }
                                if (count > 5) break;
                                count++;
                            }
                        }
                    }

                    intvData.Close = intvData.PriceList.Last<double>();

                    //intvData.isInitialized = true;
                    if (provider.CurrentTInterval == intervSpan)
                    {
                        provider.IsUpdateCandle = true;
                        provider.IsAddCandle = false;
                    }
                    LogUtil.Info(" Updated intvData  ############  " + intvData);
                }
                Console.WriteLine(" Update  intvData  ############ op " + intvData.Open + " cls " + intvData.Close + "  max  " + intvData.Max + "  low " + intvData.Min);
                //}
                //intvData.isUpdated = true;
            }
        }


        public void UpdateDataProvider(RealTimeData rtd)
        {
            Console.WriteLine("  provider.IsPause  >>>>>>>>>: " + provider.IsPause);
            //if(!provider.IsPause)
            provider.Process(null, null);
        }

        public void GetDataUnits(Symbol symbol, Interval tinterv, ref List<BarData> dataUnits)
        {
            Dictionary<Interval, List<BarData>> productDatas = this.dataUnits;
            if (productDatas.ContainsKey(tinterv) && productDatas[tinterv].Count > 0)
            {
                List<BarData> units = productDatas[tinterv];
                Console.WriteLine("   units.Count  ::::::::::::::::::::::::::: " + units.Count + "  dataUnits.Count ::::::::::::::::::::::  " + dataUnits.Count);
                dataUnits = units;
            }
        }

        public void RefreshCurrentTime(Interval interval, DateTime dateTime)
        {
            currentTimes[interval] = dateTime;
            isInitialized = true;
        }

        public void InitCurrentData(Interval interval, IntervalData data)
        {
            currentDatas[interval] = data;
            //currentTimes[interval]= 
        }

        Thread t = null;
        private void SaveDataToFile(object sender, System.Timers.ElapsedEventArgs e)
        {
            SaveDataToFile();
        }
        static int off = 200;
        private void SaveDataToFile() //object sender, System.Timers.ElapsedEventArgs e)
        {
            if (provider.Initialized && !isSavingFile && dataUnits.ContainsKey(provider.CurrentTInterval))
            {
                isSavingFile = true;
                DateTime now = DateTime.Now;
                Console.WriteLine(" isSavingFile%%%%%%%%%%% ::: " + isSavingFile + " " + now.Second + " " + now.Millisecond);

                Interval interval;
                string tmpPath;
                string tmpCdlFile;
                string tmpSigFile;
                string dataFile;
                lock (provider)
                {
                    //ProviderHelper.CleanTmpDirs(provider);
                    tmpPath = ProviderHelper.GetTmpPath(provider);
                    tmpCdlFile = ProviderHelper.GetTmpCdlFilePath(provider);
                    tmpSigFile = ProviderHelper.GetTmpSigFilePath(provider);
                    dataFile = ProviderHelper.GetDataFilePath(provider);
                    Console.WriteLine(" dataFile ::: " + dataFile + " tmpFile ::: " + tmpCdlFile + " tmpPath :: " + tmpPath + " tmpSigFile   " + tmpSigFile);
                    interval = provider.CurrentTInterval;
                }

                List<BarData> intervDatas = new List<BarData>();
                lock (dataUnits[interval])
                {
                    intervDatas.AddRange(dataUnits[interval]);
                }

                List<Signal> signalDatas = new List<Signal>();
                lock (signalUnits[interval])
                {
                    signalDatas.AddRange(signalUnits[interval]);
                }

                if (intervDatas.Count - OFFSET > BASE)
                {
                    if (intervDatas.Count - OFFSET > MAXIMUM)
                    {
                        int rmc = intervDatas.Count - OFFSET - MAXIMUM;
                        intervDatas.RemoveRange(0, rmc);
                    }
                    intervDatas.RemoveRange(intervDatas.Count - OFFSET, OFFSET);
                    BarData firstData = intervDatas.First();


                    List<Signal> ctxSignals = new List<Signal>();

                    lock (AppConst.SignalDatasLocker)
                    {
                        foreach (Signal s in AppContext.SignalDatas)
                        {
                            if (s.Symbol == provider.Symbol && s.Interval == provider.CurrentTInterval)
                                ctxSignals.Add(s);
                        }
                    }

                    ctxSignals.Sort();
                    ctxSignals.Reverse();

                    foreach (Signal s in ctxSignals)
                    {
                        if (s.ProfitPrice == 0)
                            break;
                        if (!signalDatas.Contains(s))
                            signalDatas.Add(s);
                    }

                    List<Signal> obsletes = new List<Signal>();
                    foreach (Signal s in signalDatas)
                    {
                        if (s.ProfitTime.CompareTo(firstData.DateTime) < 0)
                            obsletes.Add(s);
                    }

                    foreach (Signal s in obsletes)
                    {
                        signalDatas.Remove(s);
                    }
                }

                if (intervDatas.Count > 0) // Initialized
                {
                    Console.WriteLine(" intervDatas.Count ::: " + intervDatas.Count);
                    BarData[] barDatas = new BarData[intervDatas.Count];
                    intervDatas.CopyTo(barDatas);
                    Console.WriteLine(" barDatas.Count ::: " + barDatas.Length);
                    DateTime newLastTime = intervDatas.Last<BarData>().DateTime;

                    IFileWorker fw = FileUtil.GetFileWorker();
                    Signal[] signals = null;
                    lock (signalUnits[interval])
                    {
                        signals = new Signal[signalUnits[interval].Count];
                        signalUnits[interval].CopyTo(signals);
                    }

                    signalUnits[interval].CopyTo(signals);

                    fw.SaveCdlFile(tmpCdlFile, barDatas, true);
                    fw.SaveSigFile(tmpSigFile, signals, true);
                    fw.ZipFiles(tmpPath, dataFile, null);
                    fw = null;
                    lastCdlTimes[interval] = newLastTime;
                    ProviderHelper.CleanTmpDirs(provider);
                }
                isSavingFile = false;
                now = DateTime.Now;
                Console.WriteLine(" isSavingFile%%%%%%%%%%% ::: " + isSavingFile + " " + now.Second + " " + now.Millisecond);
            }
        }

        public void InitLocalData()
        {
            string file;
            string localPath;
            string localCdlFile;
            string localSigFile;
            Interval interval;

            lock (provider)
            {
                file = ProviderHelper.GetDataFilePath(provider);
                localPath = ProviderHelper.GetLocalPath(provider);
                localCdlFile = ProviderHelper.GetLocalCdlFilePath(provider);
                localSigFile = ProviderHelper.GetLocalSigFilePath(provider);
                interval = provider.CurrentTInterval;
            }

            Console.WriteLine(" InitLocalData  " + file + " local " + localPath);

            List<BarData> cdlList = new List<BarData>();
            List<Signal> sigList = new List<Signal>();
           // if (File.Exists(file))
            {
                IFileWorker fw = FileUtil.GetFileWorker();
       //         if (fw.UnZipFiles(file, localPath, null, false))
                {
                    Console.WriteLine(localCdlFile);
                    if (File.Exists(localCdlFile) && fw.LoadCdlFile(localCdlFile, 0, int.MaxValue, out cdlList, true, true))
                    {
                        if (cdlList.Count > 0)
                        {
                            provider.LocalLastCdlTime = cdlList[cdlList.Count - 1].DateTime;
                            provider.HasLocalLastCdlTime = true;
                        }
                    }

                    if (cdlList.Count > 0 && File.Exists(localSigFile) && fw.LoadSigFile(localSigFile, 0, int.MaxValue, out sigList, true, true))
                    {
                        sigList.ForEach(SupplementSignals);
                        if (sigList.Count > 0)
                        {
                            provider.LocalLastSigTime = sigList[sigList.Count - 1].ActTime;
                            provider.HasLocalLastSigTime = true;
                        }
                    }
                }
                fw = null;
            }
            dataUnits.Add(interval, cdlList);
            signalUnits.Add(interval, sigList);
        }

        private void SupplementSignals(Signal s)
        {
            s.Symbol = provider.Symbol;
            s.Interval = provider.CurrentTInterval;
        }

        public void InitHisCdls()
        {
            List<BarData> hisList;
            string fpath = provider.CdlFilePath;

            FileUtil.LoadFromFileCSV(fpath, 0, int.MaxValue, out hisList);
            InitDataUnits(hisList);
        }

        public void InitDataUnits(List<BarData> hisList)
        {
            if (dataUnits.ContainsKey(provider.CurrentTInterval))
            {
                dataUnits[provider.CurrentTInterval].AddRange(hisList);
                Console.WriteLine("   His Candles count >>>>>>>>>>>>>>>>>> " + dataUnits[provider.CurrentTInterval].Count);
            }
            else
            {
                dataUnits.Add(provider.CurrentTInterval, hisList);
                Console.WriteLine("   His Candles count >>>>>>>>>>>>>>>>>>  " + dataUnits[provider.CurrentTInterval].Count);
            }
        }


        public void InitSignalUnits(List<Signal> sigList)
        {
            if (signalUnits.ContainsKey(provider.CurrentTInterval))
            {
                Signal[] signals = new Signal[sigList.Count];
                sigList.CopyTo(signals);
                Array.Sort(signals);
                Array.Reverse(signals);
                foreach (Signal s in signals)
                {
                    if (!signalUnits[provider.CurrentTInterval].Contains(s))
                        signalUnits[provider.CurrentTInterval].Add(s);
                }

                //signalUnits[provider.CurrentTInterval].AddRange(sigList);
                Console.WriteLine("   His signalUnits count >>>>>>>>>>>>>>>>>> " + signalUnits[provider.CurrentTInterval].Count);
            }
            else
            {
                signalUnits.Add(provider.CurrentTInterval, sigList);
                Console.WriteLine("   His signalUnits count >>>>>>>>>>>>>>>>>>  " + signalUnits[provider.CurrentTInterval].Count);
            }
        }

        public void Init()
        {
            //Console.WriteLine("  data cache init time >>>>>>>>>>>>>>>>>>  StartTime " + StartTime);
            //foreach (Interval interv in AppSetting.INTEVALS)
            //{
            //    if (!currentTimes.ContainsKey(interv))
            //    {
            //        currentTimes.Add(interv, StartTime);
            //    }
            //}
            //isInitialized = true;
        }

        public void Start()
        {
            //timer.Start();
        }


        public void RefreshDataUnits(List<BarData> dataList)
        {
            dataUnits.Clear();
            dataUnits.Add(provider.CurrentTInterval, dataList);
            Console.WriteLine("   His Candles count >>>>>>>>>>>>>>>>>>  " + dataUnits[provider.CurrentTInterval].Count);
        }


        public void UnInitialize()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
            currentTimes.Clear();
            currentDatas.Clear();
            dataUnits.Clear();
            int i = 0;
            SaveDataToFile();
            ProviderHelper.CleanTmpDirs(provider);
        }

        public void CleanCache()
        {
            currentTimes.Clear();
            currentDatas.Clear();
            dataUnits.Clear();
            //statDatas.Clear();
            if (timer != null)
            {
                timer.Stop();
            }
        }


        public List<RealTimeData> RealTimeDatas
        {
            get { return realTimeDatas; }
            set { realTimeDatas = value; }
        }

        public Dictionary<Interval, DateTime> CurrentTimes
        {
            get { return currentTimes; }
            set { currentTimes = value; }
        }

        public Dictionary<Interval, IntervalData> CurrentDatas
        {
            get { return currentDatas; }
            set { currentDatas = value; }
        }

        public Dictionary<Interval, List<BarData>> DataUnits
        {
            get { return dataUnits; }
            set { dataUnits = value; }
        }
        public bool IsInitialized
        {
            get { return isInitialized; }
            set { isInitialized = value; }
        }
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public Dictionary<Interval, int> TotalProfitLoss
        {
            get { return totalProfitLoss; }
            set { totalProfitLoss = value; }
        }

        public Dictionary<Interval, List<Signal>> SignalUnits
        {
            get { return signalUnits; }
            set { signalUnits = value; }
        }

        public Dictionary<Interval, RSI> RISs
        {
            get { return iRISs; }
            set { iRISs = value; }
        }

        public Dictionary<Interval, RSI> RISs2
        {
            get { return iRISs2; }
            set { iRISs2 = value; }
        }

        public Dictionary<Interval, RSI> RISs3
        {
            get { return iRISs3; }
            set { iRISs3 = value; }
        }

        public Dictionary<Interval, RSI> RISs4
        {
            get { return iRISs4; }
            set { iRISs4 = value; }
        }
        internal Dictionary<Interval, CR> CRs
        {
            get { return iCRs; }
            set { iCRs = value; }
        }

        internal Dictionary<Interval, ARBR> ARBRs
        {
            get { return iARBRs; }
            set { iARBRs = value; }
        }

        internal Dictionary<Interval, ARBR> ARBRs2
        {
            get { return iARBRs2; }
            set { iARBRs2 = value; }
        }

        internal Dictionary<Interval, CCI> CCIs
        {
            get { return iCCIs; }
            set { iCCIs = value; }
        }

        internal Dictionary<Interval, CCI> CCIs2
        {
            get { return iCCIs2; }
            set { iCCIs2 = value; }
        }

        internal Dictionary<Interval, CCI> CCIs3
        {
            get { return iCCIs3; }
            set { iCCIs3 = value; }
        }

        internal Dictionary<Interval, WR> WRs
        {
            get { return iWRs; }
            set { iWRs = value; }

        }

        internal Dictionary<Interval, WR> WRs2
        {
            get { return iWRs2; }
            set { iWRs2 = value; }

        }

        internal Dictionary<Interval, LWR> LWRs
        {
            get { return iLWRs; }
            set { iLWRs= value; }
        }
        public Dictionary<Interval, SAR> SARs
        {
            get { return iSARs; }
            set { iSARs = value; }
        }

        public Dictionary<Interval, BOLL> BOLLs
        {
            get { return iBOLLs; }
            set { iBOLLs = value; }
        }
    }
}
