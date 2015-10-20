using System;
using System.Collections.Generic;

using System.Text;

using System.Collections;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;


namespace fxpa
{
  
   public  class DataHelper
    {
        public static string realTimeMsg;

        //static System.Timers.Timer  timer ;

        static Queue <string>msgQue=new Queue<string>();
        public static void DummyDataUpdate()
        {
            //timer = new System.Timers.Timer(1000);//实例化Timer类，设置间隔Time为10000毫秒； 
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(process);//到达Time的时候执行事件； 
            //timer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)； 
            //timer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；


            //timer2 = new System.Timers.Timer(1000);//实例化Timer类，设置间隔Time为10000毫秒； 
            //timer2.Elapsed += new System.Timers.ElapsedEventHandler(process2);//到达Time的时候执行事件； 
            //timer2.AutoReset = true;//设置是执行一次（false）还是一直执行(true)； 
            //timer2.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        //static int index=0;
        //static int t = 0;
        static int c = 0;
        static DateTime time = System.DateTime.Now;
        //public static void process(object sender, System.Timers.ElapsedEventArgs e)
        ////{
        //    string[] products = { "AUDUSD"
        //                            , "AUDJPY", "EURJPY", "EURUSD", "GBPUSD",
        //                            "GBPJPY", "NZDUSD", "USDJPY",
        //                            "USDCAD",  "USDCHF", "GOLD",  "SILVER"
        //                        };
            

        //    //while (true)
        //    //{
        //        if (index >= products.Length)
        //            index = 0;

        //        if (c > 11)
        //            c = 0;
        //        double d = 0;

        //        Random rand = new Random();
        //        double r= (double)rand.Next(1, 9);
        //        if (c % 2 == 0)
        //        {
        //             d = (1.2012 +( (double)r/1000)- (double)c / 100000);
        //        }
        //        else
        //        {
        //            d = (1.2012 - ((double)r / 1000) + (double)c / 100000);
        //        }
        //        Symbol symbol = (Symbol)Enum.Parse(typeof(Symbol), products[index].Trim());
        //        DateTime dateTime = time.Add(TimeSpan.FromMinutes(t));
        //        //DateTime dateTime = time.Add(TimeSpan.FromSeconds(t));
        //        RealTimeData realTimeData = new RealTimeData();
        //        realTimeData.symbol = symbol;
        //        realTimeData.datas = new double[] { d };
        //        realTimeData.dateTime = dateTime;

        //        if (DataService.symbolDatas.ContainsKey(symbol))
        //        {
        //            //List<RealTimeData> list = FXDataSource.RetrieveDataCache(symbol);
        //            //list.Add(realTimeData);

        //            if (null != DataService.providerList[symbol].DataCache)
        //            {
        //                if (!DataService.providerList[symbol].DataCache.IsInitialized)
        //                {
        //                    DataService.providerList[symbol].DataCache.StartTime = realTimeData.dateTime;
        //                    DataService.providerList[symbol].DataCache.Init();
        //                }
        //                DataService.providerList[symbol].DataCache.RealTimeDatas.Add(realTimeData);
        //            }
        //            //FXDataSource.WakeUpDataProvider(symbol);
        //            //FXDataSource.UpdateGraphicUI(FXDataSource.sourceHandler, realTimeData);
        //        }
        //       if(AppContext.IsSignalListInitialized)
        //        DataService.UpdateListView(DataService.priceListDelegate, realTimeData);
        //        //ThreadPool.QueueUserWorkItem(new WaitCallback(ProductListUpdate), realTimeData);
        //        //Thread.Sleep(1000);
        //        t++;
        //        index++;
        //        c++;            
        //}




        public static void ReceiveMsg(string realTimeMsg)
        {
            string rmsg = (string)realTimeMsg;

            List<RealTimeData> rtdList = AppUtil.ParseRtdMsg((string)realTimeMsg);
            if(rtdList.Count >0){
            foreach (RealTimeData realTimeData in rtdList)
            {
                lock (DataService.symbolDatas)
                {                    
                    if (DataService.symbolDatas.ContainsKey(realTimeData.symbol))
                    {   
                        DataService.GetProviderBySymbol(realTimeData.symbol).DataCache.RealTimeDatas.Add(realTimeData);
                    }
                    if (AppContext.IsPriceListInitialized)
                        DataService.UpdateListView(DataService.priceListDelegate, realTimeData);
                }
            }
            }
        }

        
       

        public static void ProductListUpdate(object obj)
        {
            RealTimeData realTimeData = (RealTimeData)obj;
            // TODO -- Add delegate method 
            DataService.UpdateListView(DataService.priceListDelegate, realTimeData);
        }


        public static void DataProviderUpdate(object obj)
        {
            RealTimeData realTimeData = (RealTimeData)obj;
            if (DataService.symList.Contains(realTimeData.symbol))
            {
                Console.WriteLine(" Symbol  " + realTimeData.symbol + " time " + realTimeData.dateTime+ "  Price ::" + realTimeData.datas[0]);
                DataService.isUpdated = true;
                //FXDataSource.UpdateGraphicUI(FXDataSource.sourceHandler, realTimeData);
            }
        }


   
   }
    
}