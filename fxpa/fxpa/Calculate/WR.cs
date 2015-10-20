using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
    class WR
    {
        private String code;

        private int interval;

        // Initialize necessary candles 
        private int initCandleSize;

        // 是否已经初始化
        private bool isInitialized = false;

        private int n = 14;

        List<BarData> initCandleList = new List<BarData>();

        public WR(String code, int interval)
        {
            this.code = code;
            this.interval = interval;
            this.n = 14;
            this.initCandleSize = this.n;
        }

        public WR(String code, int interval, int initSize)
        {
            this.code = code;
            this.interval = interval;
            //this.n = 14;
            this.initCandleSize = this.n=initSize;
        }

        public double handleFullCandle(ref BarData candle, int field)
        {
            if (!this.isInitialized)
            {
                this.initCandleList.Add(candle);
                if (initCandleList.Count == this.n)
                {
                    isInitialized = true;
                }
                return 0;
            }

            if (initCandleList.Count >= this.n)
            {
                initCandleList.RemoveAt(0);
            }
            this.initCandleList.Add(candle);

            double[] arr = this.getHighLowest(initCandleList);
            double highest = arr[0];
            double lowest = arr[1];
           
            // [( N期内Highest Price - 当期Close) / (N期内Highest Price-n期内Lowest price)] * (-100)           
            double tmp1 = highest - candle.Close;
            //Console.WriteLine(" tmp1  " + tmp1 +"  " + candle.Close);
            double tmp2 = highest - lowest;
            //Console.WriteLine(" tmp2  " + tmp2);
            double wr = (tmp1 / tmp2) * (100);
            //Console.WriteLine(" wr "+wr);
            

            switch (field)
            {
                case 1:
                    candle.Wr = wr;
                    break;
                case 2:
                    candle.Wr2= wr;
                    break;
            }
            return wr;
        }

        private double[] getHighLowest(List<BarData> list)
        {
            double[] ret = new double[2];
            double highest = 0, lowest = 0;
            for (int i = 0, k = list.Count; i < k; i++)
            {
                BarData bd = list[i];
                if (bd.High > highest)
                {
                    highest = bd.High;
                }
                if(lowest == 0 || lowest > bd.Low)
                {
                    lowest = bd.Low;
                }

            }
            ret[0] = highest;
            ret[1] = lowest;
            //Console.WriteLine(" highest  "+highest + "  lowest  " + lowest + " cc "  + list.Count);
            return ret;
        }

    }
}

