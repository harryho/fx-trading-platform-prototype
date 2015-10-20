using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Threading;

namespace fxpa
{
    public class  IntervalData
    {
        public double Max;
        public double Min;
        public double Open;
        public double Close;

        public List<double> PriceList = new List<double>();
        public List<CandleSignal> SignalList = new List<CandleSignal>();

        public IntervalData(int a)
        {
            Max = Double.MinValue;
            Min = Double.MaxValue;
            Open = 0;
            Close = 0;
        }
        public IntervalData(double open, double close, double min, double max )
        {
            this.Max = max;
            this.Min = min;
            this.Open = open;
            this.Close = close;
            PriceList.Add(open);
            PriceList.Add( close);
            PriceList.Add( max);
            PriceList.Add( min);
        }

        public override string ToString()
        {
            string s=base.ToString() + "[ Open: " +Open +" Close: " +Close+ " Max: " + Max+ " Min : " + Min+ "]";
            foreach (CandleSignal cs in SignalList)
            {
                s += "\n" + cs.ToString();
            }
            return s;
        }
    }

    public class RealTimeData : IComparer, IComparable
    {
        public Symbol symbol;
        public DateTime dateTime;
        public double[] datas;
        public RealTimeData() { }
        public RealTimeData(Symbol symbol, DateTime dateTime, double[] datas)
        {
            this.symbol = symbol;
            this.dateTime = dateTime;
            this.datas = datas;
        }

        public override string ToString()
        {
            return base.ToString() + "[DateTime: " + dateTime.ToString() + " symbol : " + symbol + " datas : " + datas[0] +"]";
        }

        #region IComparer 成员

        public int Compare(object x, object y)
        {
            return ((RealTimeData)x).dateTime.CompareTo(((RealTimeData)y).dateTime);
        }

        public int CompareTo(object obj)
        {
            return this.dateTime.CompareTo(((RealTimeData)obj).dateTime);
        }
        #endregion
    }

    class DataService
    {
        private static RealTimeData _realTimeData;
        public static RealTimeData RealTimeData
        {
            get { return _realTimeData; }
            set { _realTimeData = value; }
        }
        public static ListView priceList;  //Window form object 
        public static ImageList ImageList;

        public delegate void DataChangedDelegate(RealTimeData realTimeData);

        static Dictionary<Symbol, DataProvider> activeProviders = new Dictionary<Symbol, DataProvider>();
        static public Dictionary<Symbol, List<RealTimeData>> symbolDatas = new Dictionary<Symbol, List<RealTimeData>>();
        public static DataChangedDelegate priceListDelegate = new DataChangedDelegate(UpdatePriceList);
        public static DataChangedDelegate priceChangedtDelegate = new DataChangedDelegate(UpdateRealTimeListView);

        public static List<Symbol> symList = new List<Symbol>();
        public static bool isUpdated = false;

        public static void UpdateListView(DataChangedDelegate dataHandler, RealTimeData realTimeData)
        {
            dataHandler(realTimeData);
        }

        public static void UpdatePriceList(RealTimeData rtd)
        {
            RealTimeData = rtd;
            //MethodInvoker mi = new MethodInvoker(UpdateRealTimeListView);
            if (priceList != null && priceList.IsHandleCreated)
            {
                //priceList.BeginInvoke(mi);
                //priceChangedtDelegate.Invoke(rtd);
                priceList.BeginInvoke(priceChangedtDelegate, rtd);
            }
        }

        public static int GetActiveProviderCount()
        {
            lock (activeProviders)
            {
                return activeProviders.Count;
            }
        }

        public static ICollection<DataProvider>  GetAllActiveProvider()
        {
            lock (activeProviders)
            {
                return (ICollection<DataProvider>)activeProviders.Values;
            }
        }

        public static DataProvider GetProviderBySymbol(string s)
        {
            Symbol symbol = AppUtil.ParseSymbol(s);
            if (symbol != Symbol.UNKNOWN)
                return GetProviderBySymbol(symbol);
            else
                return null;
        }


        public static DataProvider GetProviderBySymbol(Symbol symbol)
        {
            if (activeProviders.ContainsKey(symbol))
            {
                return activeProviders[symbol];
            }
            else
            {
                return null;
            }
        }

        const double zero = 0;

        public static void UpdateRealTimeListView(object obj)
        {
            if (obj != null)
            {
                RealTimeData rtd = (RealTimeData)obj;
                double oldPrice = 0, newPrice = 0;
                newPrice = rtd.datas[rtd.datas.Length - 1];

                for (int i = 0; i < priceList.Items.Count; i++)
                {
                    string strSymbol = priceList.Items[i].SubItems[0].Text;
                    Symbol symbol = rtd.symbol;
                    string cnSymbol = AppConst.AppCnSymbols[symbol];
                    if (cnSymbol != null && strSymbol != null && cnSymbol.Trim() == strSymbol.Trim())
                    {
                        lock (AppConst.PriceListLocker)
                        {
                            string p = priceList.Items[i].SubItems[1].Text;
                            oldPrice = !string.IsNullOrEmpty(p) ? Convert.ToDouble(p) : 0;
                            //Console.WriteLine("  Symbol := " + strSymbol + " Time:= " + rtd.dateTime + "  OldPrice  := " + oldPrice + "  NewPrice := " + newPrice);

                            if ((oldPrice - newPrice) < zero)
                            {
                                priceList.Items[i].SubItems[1].Text = newPrice.ToString();
                                priceList.Items[i].SubItems[1].ForeColor = Color.Red;
                                priceList.Items[i].ImageIndex = 0;
                            }
                            else if ((oldPrice - newPrice) > zero)
                            {
                                priceList.Items[i].SubItems[1].Text = newPrice.ToString();
                                priceList.Items[i].SubItems[1].ForeColor = Color.Green;
                                priceList.Items[i].ImageIndex = 1;
                            }
                            //Console.WriteLine("  Symbol ==== " + symbol);
                            break;
                        }
                    }
                }
            }       
        }
 
        static TimeSpan updateTimeSpan = TimeSpan.FromSeconds(5);


        public static void RegisterDataProvider(DataProvider provider)
        {
            lock (activeProviders)
            {
                //Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@   " + provider.Symbol);
                if (!activeProviders.ContainsKey(provider.Symbol))
                {
                    activeProviders.Add(provider.Symbol, provider);
                    symbolDatas.Add(provider.Symbol, new List<RealTimeData>());
                    activeProvider = provider;
                    symList.Add(provider.Symbol);
                }

            }
        }

        public static void UnRegisterDataProvider(DataProvider dataProvider)
        {
            lock (activeProviders)
            {
                if (activeProviders.ContainsKey(dataProvider.Symbol))
                {
                    activeProviders.Remove(((DataProvider)dataProvider).Symbol);
                    symbolDatas.Remove(((DataProvider)dataProvider).Symbol);
                    activeProvider = null;
                    symList.Remove(((DataProvider)dataProvider).Symbol);
                }
            }
        }

        //==========================================================================================================

        public static void Initialze()
        {
        }

        static List<Symbol> providerRecords = new List<Symbol>();
        static DataProvider activeProvider;
    }
    
}