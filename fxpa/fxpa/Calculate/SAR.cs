using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
    public class SAR
    {

        private String code;

        private String name;
        private Interval interval;


        // Initialize necessary candles 
        private int initCandleSize;

        // 是否已经初始化
        private bool isInitialized = false;

        private double maxAccFactor = 0.2D;

        // 每次因子的增加量
        private double accFactorIncreatement = 0.02D;

        // Last  extreme Price
        private double prevEPrice;

        // Extreme of highPrice
        private double maxEPrice = double.MinValue;

        // Extreme of highPrice
        private double minEPrice = double.MaxValue;

        // Last SAR
        private double prevSAR;



        // 上一个因子
        private double prevAccFactor;

        //  Current trend  1 表示多头，0表示空头
        // 多头1就是表示SAR value上升了 空头0就是表示SAR value下降了
        private int sarArea;

        // 是否是走势的拐点
        private bool turnPoint;

        private double nextSAR;

        private int n;

        private List<double> lastNCandleHigh = new List<double>();

        private List<double> lastNCandleLow = new List<double>();

        private string formater = null;

        public static  int AREA_DOWN = 0;
	    public static  int AREA_UP = 1;


        /**
     * 
     */
        public SAR(String code, Interval interval)
        {
            this.code = code;
            this.name = "SAR";
            this.interval = interval;
            this.initCandleSize = 1;

            // Default is long term, growing up
            sarArea = AREA_UP;

            this.formater = this.getSARDecimalFormat(code);

            turnPoint = false;

            // 取10跟蜡烛的最高最低
            this.n = 10;
        }

        /**
         * 
         * @param first
         * @return
         */
        private double init(BarData first)
        {
            // Calculate第一个值

            // 初始化的时候，Since positionArea = 1，所以取Lowest price
            prevEPrice = first.High;
            prevSAR = first.Low;

            // <---------------为下一个蜡烛Calculate --------------------->
            prevAccFactor = maxAccFactor;

            // 记录当期的Highest Price和Lowest price
            maxEPrice = minEPrice = prevEPrice;

            lastNCandleHigh.Add(prevEPrice);
            lastNCandleLow.Add(prevSAR);

            isInitialized = true;

            //IndValue contrainer = new IndValue(first);
            //contrainer.setIndValue(prevSAR);

            return prevSAR;
        }


        public double handleFullCandle(ref BarData candle)
        {

            //IndValue contrainer = new IndValue(candle);

            // 最初的一 candles
            if (!isInitialized)
            {
                return this.init(candle);
            }

            // 1: CalculatePrice极值
            double currentEprice = -1;
            double candleLow = candle.Low;
            bool isNewLow = false;
            double candleHigh = candle.High;
            bool isNewHigh = false;

            if (sarArea == AREA_UP)
            {// 多头的时候，拿当前这个阶段的最高的Price

                if (candleHigh > maxEPrice)
                {
                    isNewHigh = true;
                    currentEprice = candleHigh;

                    if (!turnPoint)
                    {// 在没有区间转变的时候，才能Calculate到本期间的最高值
                        maxEPrice = candleHigh;
                    }
                }
                else
                {
                    currentEprice = maxEPrice;
                }
            }
            else if (sarArea == AREA_DOWN)
            {// 空头的时候，拿当前这个阶段的最低的Price

                if (candleLow < minEPrice)
                {
                    isNewLow = true;
                    currentEprice = candleLow;
                    if (!turnPoint)
                    {// 在没有区间转变的时候，才能Calculate到本期间的最低值
                        minEPrice = candleLow;
                    }
                }
                else
                {
                    currentEprice = minEPrice;
                }
            }



            // 2: Calculate当前因子
            double currentAccFactor;

            if (turnPoint)
            {// 当前是一个拐点
                currentAccFactor = accFactorIncreatement;
            }
            else if ((sarArea == AREA_UP && isNewHigh)
                    || (sarArea == AREA_DOWN && isNewLow))
            {// 多头,并且Price创出新的最高的时候|或者空头，并且Price是新低

                currentAccFactor = prevAccFactor+accFactorIncreatement;

                if (currentAccFactor >= maxAccFactor)
                {// 不能超过0.2
                    currentAccFactor = maxAccFactor;
                }
            }
            else
            {// 保持上一个不变
                currentAccFactor = prevAccFactor;
            }

            // 3: Calculate当前的SAR
            double sar = 0;
            if (turnPoint)
            {// 当前是一个拐点
                if (sarArea == AREA_UP)
                {// 当前是多头,SAR是前N candles的最低值
                    sar = this.getLowestInN();
                }
                else
                {
                    sar = this.getHighestInN();
                }

                minEPrice = candleLow;
                maxEPrice = candleHigh;

                // 下一根是否是拐点
                turnPoint = false;
            }
            else
            {
                // SAR(i) = SAR(i-1)+ACCELERATION(i-1)*(EPRICE(i-1)-SAR(i-1))
                double tmp = prevAccFactor * (prevEPrice - prevSAR);
                sar = prevSAR + tmp;
            }

            //double ttt = TypeFormat.parseDouble(formater.format(sar.doubleValue()));
            //sar = new double(ttt);

            //contrainer.setIndValue(ttt);

            sar = Double.Parse(String.Format(formater, sar));

            candle.Sar = sar;

            prevEPrice = currentEprice;
            prevSAR = sar;
            prevAccFactor = currentAccFactor;

            // 为下一 candlesCalculate做准备,主要是，是否是拐点
            if (sarArea == AREA_DOWN && candleHigh > sar)
            {
                turnPoint = true;
                sarArea = AREA_UP;
            }
            else if (sarArea == AREA_UP && candleLow < sar)
            {
                turnPoint = true;
                sarArea = AREA_DOWN;
            }

            if (this.lastNCandleHigh.Count >= this.n)
            {
                this.lastNCandleHigh.RemoveAt(0);
            }
            this.lastNCandleHigh.Add(candleHigh);

            if (this.lastNCandleLow.Count >= this.n)
            {
                this.lastNCandleLow.RemoveAt(0);
            }
            this.lastNCandleLow.Add(candleLow);

            // Calculate下一 candles value
            if (turnPoint)
            {// 当前是一个拐点
                if (sarArea == AREA_UP)
                {// 当前是多头,SAR是上个区间的最低值
                    nextSAR = this.getLowestInN();
                }
                else
                {
                    nextSAR = this.getHighestInN();
                }
            }
            else
            {
                // SAR(i) = SAR(i-1)+ACCELERATION(i-1)*(EPRICE(i-1)-SAR(i-1))
                double tmp = prevAccFactor * (prevEPrice - prevSAR);
                nextSAR = prevSAR + tmp;
            }

            nextSAR = Double.Parse(String.Format(formater, nextSAR));

            //ttt = TypeFormat.parseDouble(formater.format(nextSAR.doubleValue()));
            //nextSAR =ttt;

            return nextSAR;
        }

        /**
         * 拿下一个SAR value
         * 
         * @return
         */
        public double getNextSAR()
        {
            return nextSAR;
        }

        public int getSarArea()
        {
            return sarArea;
        }

        public bool isTurnPoint()
        {
            return turnPoint;
        }

        private double getHighestInN()
        {
            double ret = 0D;
            for (int i = 0, k = this.lastNCandleHigh.Count; i < k; i++)
            {
                double high = lastNCandleHigh[i];

                if (high > ret)
                {
                    ret = high;
                }
            }
            return ret;
        }

        private double getLowestInN()
        {
            double ret = Double.MaxValue;
            for (int i = 0, k = this.lastNCandleLow.Count; i < k; i++)
            {
                double low = lastNCandleLow[i];
                if (low < ret)
                {
                    ret = low;
                }
            }
            return ret;
        }

        public string getSARDecimalFormat(string code)
        {
            if (code.IndexOf("JPY") != -1 || "SILVER".Equals(code))
            {
                return "{0:00.000}";
            }
            if ("GOLD".Equals(code))
            {
                return "{0:00.00}";
            }
            return "{0:00.00000}";
        }
    }

}
