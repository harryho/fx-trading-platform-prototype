using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace fxpa
{
    public class AppContext
    {
       public  static Double[] method(Double[] inReal, int optIn, Double[] outReal)
        {
            //Double[] outReal = inReal;
            return null;
        }
        public static void cleanPrevData()
        {
            if (Directory.Exists(FxpaSource.folderPath))
            {
                DateTime dt =Directory.GetCreationTime(FxpaSource.folderPath);
                if (dt.CompareTo(AppSetting.RELEASE_DATE) < 0)
                {
                    Directory.Delete(FxpaSource.folderPath, true);
                    Directory.Delete(AppContext.APP_PATH + "\\logs", true);
                }
            }
        }

        /** Local var **/
        static bool IsRelogin;
        static bool IsAppConnecting = false;
        static bool IsDSConnecting = false;
        static string CONNECTION_AVAILABLE = " Network is connected";
        static string CONNECTION_NOT_AVAILABLE = " Network is disconnected";
        static string WELCOME = "Welcome ";
        static string SIGH = " ! ";
        //static ServerCheckHandler scHandler = new ServerCheckHandler(AppClient.GetInstance);
        /** Local var **/

        /** Global app  status  **/
        public static bool IsGetLatestVersion;
        public static bool IsUpdated;
        public static bool IsFinishVerChk;
        public static bool IsOpen;
        public static bool IsPriceListInitialized;
        public static bool IsSignalListInitialized;
        public static bool IsStatListInitialized;
        public static bool IsPriceListInitializing;
        public static bool IsSignalListInitializing;
        public static bool NoSubscription;
        //public static bool IsAppInitialized;
        //public static bool IsAppInitializing;
        public static bool IsLoading;
        public static bool IsReconnecting;
        public static bool IsLogin;
        public static bool IsLoginning;

        public static bool IsGetDSInfo;
        public static bool IsGetSymbols;
        public static bool IsGetSymbolPrices;
        public static bool IsGetInitProviderFile;
        public static bool IsLoadInitProviderFile;
        public static bool IsGetInitProviderRtd;
        public static bool IsGetInitProviderSignals;
        public static bool IsGetSignalList;
        public static bool IsGetPaymentInfo;
        public static bool IsFirstProviderInit;
        public static bool IsProviderInitializing;
        public static bool IsProviderInitialized;
        public static bool IsRefreshProviders;
        public static bool IsRefreshSignals;
        public static bool IsReloginFailure;
        public static bool IsOpenSpeaker = true;
        /** Global app  status  **/

        /** Global object **/
        public static string APP_PATH = Application.StartupPath;
        public static DateTime CURRENT_TIME;
        public static int DAY;
        public static Dictionary<string, double> SymbolPrices = new Dictionary<string, double>();
        public static Form FxpaMain;

        public static List<Signal> SignalDatas = new List<Signal>();
        public static Dictionary<Symbol, Dictionary<Interval, List<Signal>>> SymbolSignals = new Dictionary<Symbol, Dictionary<Interval, List<Signal>>>();

        public static DoubleBufferListView PriceListView;
        public static DoubleBufferListView SignalListView;
        public static DoubleBufferListView StatListView;
        public static FxAnalyzerControl TradeAnalyzerControl;
        public static DataProvider FirstDataProvider;
        static public ToolStripStatusLabel welcomeStatusLabel;
        static public ToolStripStatusLabel warningStatusLabel;
        static public Label timeLabel;
        public static Label RestLabel;
        public static Label TotalLabel;
        /** Global object **/

        /**Global Time object  **/
        public static System.Timers.Timer appTimer = new System.Timers.Timer(60000);
        public static System.Timers.Timer initTimer = new System.Timers.Timer(500);
        public static System.Timers.Timer reConnTimer = new System.Timers.Timer(20000);
        public static DateTime AppTime = System.DateTime.Now;
        /**Global Timer **/

        static public object appTokenLock = new object();

        public static Dictionary<string, string> ServToken = new Dictionary<string, string>();
        static bool disconneted;
     

        static int rec = 0;


        static bool IsAllProviderInitialized()
        {
            if (AppSetting.SYMBOLS != null)
            {
                if (DataService.GetActiveProviderCount() > 0)
                {
                    foreach (DataProvider provider in DataService.GetAllActiveProvider())
                    {
                        if (!provider.Initialized)
                        {
                            return provider.Initialized;
                        }
                    }
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        private static void ProcessTA(object obj, System.Timers.ElapsedEventArgs e)
        {
            AppTime = AppTime.Add(TimeSpan.FromSeconds(5));
        }

        static DateTime lastCheckTime;


   }
}
