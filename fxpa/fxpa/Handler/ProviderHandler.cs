using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace fxpa
{

    public class ProviderHandler : MsgHandler
    {
        DataProvider provider;

        public DataProvider Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        public ProviderHandler(DataProvider provider)

            : base()
        {
            this.provider = provider;
            this.MsgUpdateEvent += new MsgUpdatedDelegate(provider.RetrieveCdlFileAndLatestRtd);
        }

        //bool IsGetHttpInfo;
        bool IsGetHisInfo;
        bool IsGetRealTimeInfo;
        bool IsGetSignalInfo;
        public override void Send(object msg)
        {
            Console.WriteLine(" ProviderHandler  Send ~~~~~~~~~~~ " + provider.Symbol + "  start " + System.DateTime.Now.ToLongTimeString());
            //Client.Send((string)msg);
        }

        public override void Execute()
        {
            IsProcessing = true;

            if ( AppContext.FirstDataProvider != null
                && AppContext.TradeAnalyzerControl==null)
            {
                // TO DO 
            }
            else
            {
                if (!IsGetHisInfo)
                {
                    IsGetHisInfo = true;
                    string request = "";

                    Console.WriteLine(" ProviderHandler  Send ~~~~~~~~~~~  " + Protocol.M0003_1);
                    if (provider.HasLocalLastCdlTime)
                        request = NetHelper.BuildMsg(Protocol.M0003_1, new string[] { provider.Symbol.ToString(), AppUtil.IntervalToString(provider.CurrentTInterval), provider.LocalLastCdlTime.Add(AppConst.AppTimeSpans[provider.CurrentTInterval]).ToString(FORMAT) });
                    else
                        request = NetHelper.BuildMsg(Protocol.M0003_1, new string[] { provider.Symbol.ToString(), AppUtil.IntervalToString(provider.CurrentTInterval), NULL });
                    Send(request);
                }
                else if (!IsGetSignalInfo)
                {
                    IsGetSignalInfo = true;
                    Console.WriteLine(" ProviderHandler  Send ~~~~~~~~~~~   " + Protocol.C0004_2);
                    string request ="";
                    DateTime time = DateTime.Now;
                    //time = time.AddMonths(-6);
                     time = DateTime.Parse("2009-09-01 00:00:00");
                    if(provider.HasLocalLastSigTime )
                        request = NetHelper.BuildMsg(Protocol.C0004_2, new string[] { provider.Symbol.ToString(), AppUtil.IntervalToString(AppContext.FirstDataProvider.CurrentTInterval), provider.LocalLastSigTime.ToString(FORMAT) });
                    else
                        request = NetHelper.BuildMsg(Protocol.C0004_2, new string[] { provider.Symbol.ToString(), AppUtil.IntervalToString(AppContext.FirstDataProvider.CurrentTInterval), time.ToString(FORMAT) });
                    Send(request);
                }
                else if (!IsGetRealTimeInfo)
                {
                    IsGetRealTimeInfo = true;
                    Console.WriteLine(" ProviderHandler  Send ~~~~~~~~~~~  " + Protocol.C0003_2);
                    string request = NetHelper.BuildMsg(Protocol.C0003_2, new string[] { provider.Symbol.ToString(), AppUtil.IntervalToString(provider.CurrentTInterval), provider.StartTime.ToString(FORMAT) });
                    Send(request);
                }
            }
        }

        public override void Receive(object msg)
        {
            Console.WriteLine(" ProviderHandler  Receive ~~~~~~~~~~~ " + provider.Symbol + "  Start  " + System.DateTime.Now.ToLongTimeString());

            if (provider != null)
            {
                string[] msgs = (string[])msg;

                Protocol protocol = AppUtil.ParseProtocol(msgs[1]);
                int paramAmount = AppUtil.StringToInt(msgs[2]);
                Console.WriteLine(" ProviderHandler  Receive ~~~~~~~~~~~ " + protocol);
                switch (protocol)
                {
                    case Protocol.M0003_1:
                        Execute();
                        if (paramAmount > 0 && !msgs.Contains<string>(NULL))
                        {
                            List<BarData> rtdDatas = new List<BarData>();
                            AppUtil.ParseRtd(ref rtdDatas, 3, msgs, protocol);
                            if (rtdDatas.Count > 0)
                            {
                                provider.StartTime = rtdDatas[rtdDatas.Count - 1].DateTime.Add(AppConst.AppTimeSpans[provider.CurrentTInterval]);
                                provider.DataCache.InitDataUnits(rtdDatas);
                                provider.InitCdlStatus = DataInitStatus.Initialized;

                            }
                            LogUtil.Info(" C0003_1  ::::: URL count " + rtdDatas.Count);
                        }
                        else
                        {
                            provider.HisUri = null;
                            provider.StartTime = provider.LocalLastCdlTime;
                            provider.CdlLoadingStatus = DownloadStatus.Finished;
                            provider.InitCdlStatus = DataInitStatus.Initialized;
                        }
                        IsGetHisInfo = true;
                        Execute();
                        //provider.DataCache.StartTime = provider.StartTime;
                        //provider.DataCache.Init();
                        Process(null);
                        break;
                    case Protocol.C0003_2:
                        if (paramAmount != AppUtil.PARSE_ERROR && paramAmount > 0)
                        {
                            List<BarData> rtdDatas = new List<BarData>();
                            AppUtil.ParseRtd(ref rtdDatas, 3, msgs, protocol);
                            provider.LatesRtds = rtdDatas;
                            provider.RtdLoadingStatus = DownloadStatus.Finished;
                            provider.InitRtdStatus = DataInitStatus.Initialized;
                        }
                        else
                        {
                            provider.RtdLoadingStatus = DownloadStatus.Failed;
                            provider.InitRtdStatus = DataInitStatus.Initialized;
                        }
                        IsGetRealTimeInfo = true;
                        break;
                    case Protocol.C0004_2:
                        if (paramAmount > 0)
                        {
                            List<Signal> sigalList = new List<Signal>();
                            for (int i = 3; i < msgs.Length; i++)
                            {
                                string signalInfo = msgs[i ];
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
                                    if (signal.Profit != 0 && NULL != info[12]) signal.ProfitTime = DateTime.Parse(info[12]);
                                    signal.ProfitPrice = double.Parse(info[13]);
                                    sigalList.Add(signal);
                                }
                            }
                            //provider.SignalList = sigalList;
                            //AppContext.FirstDataProvider.DataCache.InitSignalUnits(sigalList);
                            provider.DataCache.InitSignalUnits(sigalList);
                        }
                        provider.InitSigStatus = DataInitStatus.Initialized;
                        IsGetSignalInfo = true;
                        break;
                    default:
                        IsGetHisInfo = true;
                        provider.HisUri = null;
                        provider.StartTime = provider.LocalLastCdlTime;
                        provider.CdlLoadingStatus = DownloadStatus.Failed;
                        provider.InitCdlStatus = DataInitStatus.Initialized;
                        IsGetRealTimeInfo = true;
                        provider.RtdLoadingStatus = DownloadStatus.Failed;
                        provider.InitRtdStatus = DataInitStatus.Initialized;
                        IsProcessing = false;
                        break;
                }
                Console.WriteLine(" ProviderHandler  Receive ~~~~~~~~~~~ " + provider.Symbol + "  After operation  " + System.DateTime.Now.ToLongTimeString());
            }
        }


         }
}
