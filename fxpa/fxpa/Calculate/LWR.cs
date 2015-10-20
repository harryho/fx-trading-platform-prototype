using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *LWR威廉指标实际上是KD指标的补数，即(100-KD)。 LWR1线 (100-线K) LWR2线
 * (100-线D)  parameter ：N、M1、M2 天数，一般取9、3、3 用法：
 * 1.LWR2<30，超买；LWR2>70，超卖。
 * 2.线LWR1向下跌破线LWR2，买进Signal；线LWR1向上突破线LWR2，卖出Signal。
 * 3.线LWR1与线LWR2的交叉发生在30以下，70以上，才有效。
 * 4.LWR指标不适于发行量小，交易不活跃的股票； 5.LWR指标对大盘和热门大盘股有极高准确性。
 * 
 * @author herry
 */
namespace fxpa
{
    public class LWR
    {
        /**
         * LWR:= (HHV(HIGH,N)-CLOSE)/(HHV(HIGH,N)-LLV(LOW,N))*100;
         *  HHV:HHV(HIGH,N); LLV:LLV(LOW,N); 
         *  
         *  LWR1:SMA(LWR,M1,1);
         *  LWR2:SMA(LWR1,M2,1);
         *  */

        public static string LWR1 = "LWR1";
        public static string LWR2 = "LWR2";

        // Initialize necessary candles 
        private int initCandleSize;
        private int n = 9;

        //private int N = 14;
        private int m1 = 3;
        private int m2 = 3;
        private int m1size =0;
        private int m2size =0;
        private double THREE = 3;
        private double FACTOR = 0.015;

        string name = "";
        string forexCode = "";
        private int interval;
        //List<BarData> initCandleList = new List<BarData>();
        protected List<BarData> lwr1List = new List<BarData>();
        protected List<BarData> lwr2List = new List<BarData>();

        public LWR(String code, int interval)
        {
            this.forexCode = code;
            this.name = "LWR";
            this.interval = interval;
            //this.initCandleSize = this.n = this.N * 2 - 1;
                  this.m1size = n + m1 - 1;
        this.m2size =n + m1 + m2 - 2;
        }

        public LWR(String code, int interval, int initSize, int m1, int m2)
        {
            this.forexCode = code;
            this.name = "LWR";
            this.interval = interval;
            this.initCandleSize = this.n = initSize;
            //this.N = initSize;
            this.m1 = m1;
            this.m2 = m2;
            this.m1size = n + m1 - 1;
            this.m2size = n + m1 + m2 - 1;
        }



        public Dictionary<string, double> handleFullCandle(ref BarData candle, int field)
        {

            Dictionary<string, double> lwr = new Dictionary<string, double>();
            lwr.Add(LWR1, 0);
            lwr.Add(LWR2, 0);

            bool rtn = false;
            if (lwr1List.Count >= this.m1size)
            {
                lwr1List.RemoveAt(0);
                lwr1List.Add(candle);
            }
            else
            {
                lwr1List.Add(candle);
                rtn = true;
            }

            if (lwr2List.Count >= this.m2size)
            {
                lwr2List.RemoveAt(0);
                lwr2List.Add(candle);
            }
            else
            {
                lwr2List.Add(candle);
                rtn = true;
            }

            if (rtn)
            {
                lwr[LWR1] = 0;
                lwr[LWR2] = 0;
                candle.Lwr = lwr;

                return lwr;
            }

            // HHV(HIGH,N)
            // The highest value in last N candles
            double hhv = 0;

            // LLV(LOW,N)
            // The lowest value in last N candles
            double llv = 0;

            // RSV= (HHV(HIGH,N)-CLOSE)/(HHV(HIGH,N)-LLV(LOW,N))*100;
            double rsv = 0;

            // LWR1: SMA(RSV,M1,1);
            // LWR1=(RSV1+RSV2+ ...+RSVn)/ N
            // N =M1, M1>=1
            double lwr1 = 0;
            rsv = 0;
            for (int i = m1size - 1; i > m1size - m1 - 1; i--)
            {
                BarData cdl = lwr1List[i];
                double high = Double.MinValue;
                double low = Double.MaxValue;
                hhv = 0;
                llv = 0;
                for (int k = i; k > i -n ; k--)
                {
                    BarData c = lwr1List[k];
                    high = high < c.High ? c.High : high;
                    low = low > c.Low ? c.Low : low;
                }     
                hhv = high;
                double hcd = high - cdl.Close;
                llv = low;
                double hld = hhv-llv;
                double rsvn = hcd/hld*100;
                rsv = rsv+rsvn;
            }
            lwr1 = rsv/m1;

            // LWR2: SMA(LWR1,M2,1);
            // LWR2=(LWR1+LWR2+ ...+LWRn)/ N
            // N =M2, M2>=1
            double lwr2 = 0;

            for (int t = m2size - 1; t > m2size - m2 - 1; t--)
            {
                rsv = 0;
                for (int i = t; i >t- m1; i--)
                {
                    BarData cdl = lwr2List[i];
                    double high = Double.MinValue;
                    double low = Double.MaxValue;
                    hhv = 0;
                    llv = 0;
                    for (int k = i; k > i - n; k--)
                    {
                        BarData c = lwr2List[k];
                        high = high < c.High ? c.High : high;
                        low = low > c.Low ? c.Low : low;
                    }
                    hhv = high;
                    double hcd = high - cdl.Close;
                    llv = low;
                    double hld = hhv - llv;
                    double rsvn = hcd / hld * 100;
                    rsv = rsv + rsvn;
                }

                double lwrn = rsv/m1;
                lwr2+= lwrn;
            }

            lwr2 = lwr2/m2;


            lwr1 = Double.Parse(String.Format("{0:00.00000}", lwr1));
            lwr2 = Double.Parse(String.Format("{0:00.00000}", lwr2));


            lwr[LWR1] = lwr1;
            lwr[LWR2] = lwr2;

            candle.Lwr = lwr;
            return lwr;
        }


        public Dictionary<string, double> handleFragCandle(ref BarData candle, int field)
        {

            Dictionary<string, double> lwr = new Dictionary<string, double>();

            // cp the previous candle list
            List<BarData> cpLwr1List = new List<BarData>();
            cpLwr1List.AddRange(lwr1List);
            cpLwr1List.RemoveAt(0);
            cpLwr1List.Add(candle);

            List<BarData> cpLwr2List = new List<BarData>();
            cpLwr2List.AddRange(lwr2List);
            cpLwr2List.RemoveAt(0);
            cpLwr2List.Add(candle);

            // HHV(HIGH,N)
            // The highest value in last N candles
            double hhv = 0;

            // LLV(LOW,N)
            // The lowest value in last N candles
            double llv = 0;

            // RSV= (HHV(HIGH,N)-CLOSE)/(HHV(HIGH,N)-LLV(LOW,N))*100;
            double rsv = 0;

            // LWR1: SMA(RSV,M1,1);
            // LWR1=(RSV1+RSV2+ ...+RSVn)/ N
            // N =M1, M1>=1
            double lwr1 = 0;
            rsv = 0;
            for (int i = m1size - 1; i > m1size - m1 - 1; i--)
            {
                BarData cdl = cpLwr1List[i];
                double high = Double.MinValue;
                double low = Double.MaxValue;
                hhv = 0;
                llv = 0;
                for (int k = i; k > i - n; k--)
                {
                    BarData c = cpLwr1List[k];
                    high = high < c.High ? c.High : high;
                    low = low > c.Low ? c.Low : low;
                }

                hhv = high;
                //  Console.WriteLine(" high " + hhv);
                double hcd = high - cdl.Close;
                llv = low;
                //  Console.WriteLine(" low  " + llv);
                double hld = hhv - llv;
                //  Console.WriteLine(" hhhhcccc " + hcd);
                //  Console.WriteLine(" hhhhhlllllll " + hld);
                double rsvn = hcd / hld * 100;
                rsv = rsv + rsvn;
                //  Console.WriteLine(" rsv=======  " + rsv );
            }
            //  Console.WriteLine(rsv);
            lwr1 = rsv / m1;
            //  Console.WriteLine(lwr1);

            // LWR2: SMA(LWR1,M2,1);
            // LWR2=(LWR1+LWR2+ ...+LWRn)/ N
            // N =M2, M2>=1
            double lwr2 = 0;

            for (int t = m2size - 1; t > m2size - m2 - 1; t--)
            {
                rsv = 0;
                for (int i = t; i > t - m1; i--)
                {
                    BarData cdl = cpLwr2List[i];
                    double high = Double.MinValue;
                    double low = Double.MaxValue;
                    hhv = 0;
                    llv = 0;
                    for (int k = i; k > i - n; k--)
                    {
                        BarData c = cpLwr2List[k];
                        high = high < c.High ? c.High : high;
                        low = low > c.Low ? c.Low : low;
                    }
                    hhv = high;
                    double hcd = high - cdl.Close;
                    llv = low;
                    double hld = hhv - llv;
                    //  Console.WriteLine(" hhhhvvv " + hcd);
                    //  Console.WriteLine(" llllllvvvvv " + hld );
                    double rsvn = hcd / hld * 100;
                    rsv = rsv + rsvn;
                }
                //  Console.WriteLine(rsv);
                double lwrn = rsv / m1;
                lwr2 += lwrn;
            }
            //  Console.WriteLine(lwr2);
            lwr2 = lwr2 / m2;
            //  Console.WriteLine(lwr2);

            lwr1 = Double.Parse(String.Format("{0:00.00000}", lwr1));
            lwr2 = Double.Parse(String.Format("{0:00.00000}", lwr2));
            //contrainer.setOtherIndValues(map);

            lwr[LWR1] = lwr1;
            lwr[LWR2] = lwr2;

            candle.Lwr = lwr;
            return lwr;
        }
    }

}
