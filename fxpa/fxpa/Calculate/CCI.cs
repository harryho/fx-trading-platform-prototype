using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * 算法： 　中价与中价的N day Moving Average的差 is divided byWithin N day(s)中价的平均绝对偏差  parameter ：N　设定CalculateMoving Average的天数，一般为14
 * 用法： 当CCI小于-100时为买入Signal，CCI大于100时为卖出Signal。
 * 
 * N -- -- Max: 100; Min: 2; Default: 14
 * 
 * REFLINE: -100, 0, 100; TP := (HIGH + LOW + CLOSE)/3;
 * (TP-MA(TP,N))/(0.015*AVEDEV(TP,N));
 * 
 * @author herry 
 */

namespace fxpa
{
    public class CCI
    {

        // Initialize necessary candles 
        private int initCandleSize;
        private int n = 14;

        private int N = 14;
        private double THREE = 3;
        private double FACTOR = 0.015;

        string name = "";
        string forexCode = "";
        private int interval;
        List<BarData> initCandleList = new List<BarData>();


        public CCI(String code, int interval)
        {
            this.forexCode = code;
            this.name = "CCI";
            this.interval = interval;
            this.initCandleSize = this.n =this.N*2-1;
        }

        public CCI(String code, int interval, int initSize)
        {
            this.forexCode = code;
            this.name = "CCI";
            this.interval = interval;
            this.initCandleSize = this.n = initSize*2-1;
            this.N = initSize;
        }



        public double handleFullCandle(ref BarData candle, int field)
        {

            double cciVal = 0;

            if (initCandleList.Count < this.n)
            {
                initCandleList.Add(candle);
                return cciVal;
            }

            initCandleList.RemoveAt(0);
            initCandleList.Add(candle);


            //TP = (Hight + Lowt + Closet) / 3
            double tp = (candle.High + candle.Low + candle.Close) / THREE;

            // MA= (TP1 + TP2 +... + TPn) / n
            // MA: The moving average of the typical price. 
            // TPn: The typical price for the nth interval. 
            // N: Number of intervals for the average.
            double ma = 0;
            for (int i = n - 1; i > this.N - 2; i--)
            {
                // calculate MA
                BarData c = initCandleList[i];
                ma += ((c.High + c.Low + c.Close) / THREE);
            }
            ma = ma / N;


            // MD= (|TPAVG1 - TP1| + ... + |TPAVG1 - TPn |) / n
            // MD: The mean deviation for this interval. 
            //TPn: The typical price for the nth interval. 
            // N: Number of intervals.
            double md = 0;
                    

            for (int i = n - 1; i > this.N-2; i--)
            {
                // calculate MD
                BarData c = initCandleList[i];
                double tpn = 0;
                tpn = (c.High + c.Low + c.Close) / THREE;
                double tpavg = 0;
                for (int k = i; k > i - this.N; k--)
                {
                    BarData b = initCandleList[k];
                    tpavg += ((b.High + b.Low + b.Close) / THREE);
                }

                tpavg = tpavg / N;
                //Console.WriteLine(" tpavg " + tpavg + " tpn " + tpn );
                md += Math.Abs(tpavg - tpn);
            }

            md = md / N;

            double cci = 0;
            //Console.WriteLine(" tp " + tp + " ma " + ma + " md " + md);

            // CCI = (TP - MA) / (.015 x MD)
            cci = (tp - ma) / (md * FACTOR);

            cciVal = Double.Parse(String.Format("{0:00.00000}", cci));

            switch (field)
            {
                case 1:
                    candle.Cci = cciVal;
                    break;
                case 2:
                    candle.Cci2 = cciVal;
                    break;
                case 3:
                    candle.Cci3 = cciVal;
                    break;
            }
            return cciVal;
        }


        public double handleFragCandle(ref BarData candle, int field)
        {

            double cciVal = 0;

            // cp the previous candle list
            List<BarData> cpList = new List<BarData>();
            cpList.AddRange(initCandleList);
            cpList.RemoveAt(0);
            cpList.Add(candle);



            //TP = (Hight + Lowt + Closet) / 3
            double tp = (candle.High + candle.Low + candle.Close) / THREE;

            // MA= (TP1 + TP2 +... + TPn) / n
            // MA: The moving average of the typical price. 
            // TPn: The typical price for the nth interval. 
            // N: Number of intervals for the average.

            double ma = 0;
            for (int i = n - 1; i > 12; i--)
            {
                // calculate MA
                BarData c = initCandleList[i];
                ma += ((c.High + c.Low + c.Close) / THREE);
            }
            ma = ma / N;

            // MD = (|TPAVG1 - TP1| + ... + |TPAVG1 - TPn |) / n
            // MD: The mean deviation for this interval. 
            //TPn: The typical price for the nth interval. 
            // N: Number of intervals.
            double md = 0;
            for (int i = n - 1; i > 12; i--)
            {
                // calculate MD
                BarData c = initCandleList[i];
                double tpn = 0;
                tpn = (c.High + c.Low + c.Close) / THREE;
                double tpavg = 0;
                for (int k = i; k > i - 14; k--)
                {
                    BarData b = initCandleList[k];
                    tpavg += ((b.High + b.Low + b.Close) / THREE);
                }

                tpavg = tpavg / N;
                //Console.WriteLine(" tpavg " + tpavg + " tpn " + tpn );
                md += Math.Abs(tpavg - tpn);
            }
            md = md / N;

            double cci = 0;
            //Console.WriteLine(" tp " + tp + " ma " + ma + " md " + md);

            // CCIt = (TPt - TPAVGt) / (.015 x MDT)
            cci = (tp - ma) / (md * FACTOR);

            cciVal = Double.Parse(String.Format("{0:00.00000}", cci));

            switch (field)
            {
                case 1:
                    candle.Cci = cciVal;
                    break;
                case 2:
                    candle.Cci2 = cciVal;
                    break;
            }
            return cciVal;
        }
    }

}
