using System;
using System.Collections.Generic;
using System.Text;

namespace fxpa
{
    public enum OperationalStateEnum
    {
        UnInitialized,
        Initialized,
        Operational,
        NotOperational
    }

    public enum DownloadStatus
    {
        NotStart,
        Loading,
        Finished,
        Failed
    }
    public enum DataInitStatus
    {
        NotStart,
        Initializing,
        Initialized
        //Failed
    }


    public enum Symbol
    {
        AUDUSD,
        EURUSD,
        GBPUSD,
        NZDUSD,
        USDCAD,
        USDCHF,
        USDJPY,
        GBPJPY,
        AUDJPY,
        EURJPY,
        GOLD,
        SILVER,
        UNKNOWN
    }

    public enum BaseCurrency
    {
        USD,
        JPY,
        GBP,
        AUD,
        EUR,
        NZD,
        GOLD,
        SILVER
    }

    public enum Interval
    {
        MIN1,
        MIN3,
        MIN5,
        MIN15,   // 15 mins
        MIN30,  //  30 mins
        MIN60,
        MIN120,
        DAY1,
    }

    public enum PaymentType
    {
        Point,
        Year,
        Month,
        Free,
        Unknow
    }


   public class AppConst
    {
       //public static string VERSION = "01";
       public static object SignalDatasLocker = new object();
        public static object PriceListLocker = new object();
        public static object StatListLocker = new object();
        public static object SignalListLocker = new object();
        public static object InfoListLocker = new object();
       public static Dictionary<Interval, TimeSpan> AppTimeSpans = new Dictionary<Interval,TimeSpan>();
        public static Dictionary<Symbol, string> AppCnSymbols = new Dictionary<Symbol, string>();
        public static Dictionary<Symbol, string> AppCurrencies = new Dictionary<Symbol, string>();
        //public static string[] Day = { "星期 day ", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        public static string[] Days = { "Sun", "Mon", "Tue", "wed", "Thu", "Fri", "Sat" };

        public static List<Symbol> JPYGOLD = new List<Symbol>();

        public static void InitAppSettings()
        {
            AppTimeSpans.Add(Interval.MIN1, TimeSpan.FromMinutes(1));
            AppTimeSpans.Add(Interval.MIN3, TimeSpan.FromMinutes(3));
            AppTimeSpans.Add(Interval.MIN5, TimeSpan.FromMinutes(5));
            AppTimeSpans.Add(Interval.MIN15, TimeSpan.FromMinutes(15));
            AppTimeSpans.Add(Interval.MIN30, TimeSpan.FromMinutes(30));
            AppTimeSpans.Add(Interval.MIN60, TimeSpan.FromMinutes(60));
            AppTimeSpans.Add(Interval.MIN120, TimeSpan.FromMinutes(120));
            AppTimeSpans.Add(Interval.DAY1, TimeSpan.FromDays(1));

            AppCnSymbols.Add(Symbol.AUDUSD, "AUDUSD");
            AppCnSymbols.Add(Symbol.EURUSD, "EURUSD");
            AppCnSymbols.Add(Symbol.GBPUSD, "GBPUSD");
            AppCnSymbols.Add(Symbol.NZDUSD, "NZDUSD");
            AppCnSymbols.Add(Symbol.USDCAD, "USDCAD");
            AppCnSymbols.Add(Symbol.USDCHF, "USDCHF");
            AppCnSymbols.Add(Symbol.USDJPY, "USDJPY");
            AppCnSymbols.Add(Symbol.AUDJPY, "AUDJPY");
            AppCnSymbols.Add(Symbol.EURJPY, "EURJPY");
            AppCnSymbols.Add(Symbol.GBPJPY, "GBPJPY");
            AppCnSymbols.Add(Symbol.GOLD, "GOLD");
            AppCnSymbols.Add(Symbol.SILVER, "SILVER");

            JPYGOLD.Add(Symbol.AUDJPY);
            JPYGOLD.Add(Symbol.USDJPY);
            JPYGOLD.Add(Symbol.EURJPY);
            JPYGOLD.Add(Symbol.GBPJPY);
            JPYGOLD.Add(Symbol.GOLD);

        }

       

    }
}
