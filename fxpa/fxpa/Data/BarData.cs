// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace fxpa
{
    //[Serializable]
    public struct BarData : IComparer, IComparable
    {
        public enum DataValueSourceEnum
        {
            High,
            Low,
            Average,
            Open,
            Volume,
            Close
        }

        // Harry -- Add
        bool isHigherPrev;
        public bool IsHigherPrev
        {
            get
            {
                return isHigherPrev;
            }
            set
            {
                isHigherPrev = true;
            }
        }


        /**
         * 0 means complete，1 means incomplete
         * */
        bool isCompleted;
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        double sar;
        public double Sar
        {
            get { return sar; }
            set { sar = value; }
        }

        double ar;

        public double Ar
        {
            get { return ar; }
            set { ar = value; }
        }

        double ar2;

        public double Ar2
        {
            get { return ar2; }
            set { ar2 = value; }
        }

        double br;

        public double Br
        {
            get { return br; }
            set { br = value; }
        }

        double cr;
        public double Cr
        {
            get { return cr; }
            set { cr = value; }
        }

        bool isReverse;

        public bool IsReverse
        {
            get { return isReverse; }
            set { isReverse = value; }
        }

        double rsi;
        double rsi2;

        public double Rsi2
        {
            get { return rsi2; }
            set { rsi2 = value; }
        }
        double rsi3;

        public double Rsi3
        {
            get { return rsi3; }
            set { rsi3 = value; }
        }
        double rsi4;

        public double Rsi4
        {
            get { return rsi4; }
            set { rsi4 = value; }
        }
        public double Rsi
        {
            get { return rsi; }
            set { rsi = value; }
        }

        double cci;

        public double Cci
        {
            get { return cci; }
            set { cci = value; }
        }

        double cci2;

        public double Cci2
        {
            get { return cci2; }
            set { cci2 = value; }
        }

        double cci3;

        public double Cci3
        {
            get { return cci3; }
            set { cci3 = value; }
        }

        double wr2;

        public double Wr2
        {
            get { return wr2; }
            set { wr2 = value; }
        }

        Dictionary<string, double> indicators;

        public Dictionary<string, double> Indicators
        {
            get { return indicators; }
            set { indicators = value; }
        }

        Dictionary<string, double> lwr;

        public Dictionary<string, double> Lwr
        {
            get { return lwr; }
            set { lwr = value; }
        }

        Dictionary<string, double> boll;




        public Dictionary<string, double> Boll
        {
            get { return boll; }
            set { boll = value; }
        }
        double wr;

        public double Wr
        {
            get { return wr; }
            set { wr = value; }
        }
        double actPrice;

        public double ActPrice
        {
            get { return (double.IsNaN(actPrice) || actPrice == 0) ? Average : actPrice; }
            set { actPrice = value; }
        }

        double stopLossPrice;

        public double StopLossPrice
        {
            get { return stopLossPrice; }
            set { stopLossPrice = value; }
        }

        double stopGainPrice;

        public double StopGainPrice
        {
            get { return stopGainPrice; }
            set { stopGainPrice = value; }
        }

        List<CandleSignal> signalList; //= new List<CandleSignal>();

        public List<CandleSignal> SignalList
        {
            get { return signalList; }
            set { signalList = value; }
        }

        double exHigh;

        public double ExHigh
        {
            get { return exHigh >= High ? exHigh : High; }
            set { exHigh = value; }
        }

        double exLow;

        public double ExLow
        {
            get { return exLow <= Low ? exLow : Low; }
            set { exLow = value; }
        }


        //Harry -- Add

        long _dateTime;
        public DateTime DateTime
        {
            get { return DateTime.FromFileTime(_dateTime); }
            set { _dateTime = value.ToFileTime(); }
        }

        public double Average
        {
            get
            {
                double sum = Open + Low + High + Close;
                return sum / 4d;
            }
        }

        public double AverageOpenClose
        {
            get
            {
                double sum = Open + Close;
                return sum / 2d;
            }
        }

        double _open;
        public double Open
        {
            get { return _open; }
            set { _open = value; }
        }

        double _close;
        public double Close
        {
            get { return _close; }
            set { _close = value; }
        }

        double _low;
        public double Low
        {
            get { return _low; }
            set { _low = value; }
        }

        double _high;
        public double High
        {
            get { return _high; }
            set { _high = value; }
        }

        double _volume;
        public double Volume
        {
            get { return _volume; }
        }

        public bool HasDataValues
        {
            get { return double.IsNaN(_open) == false && double.IsNaN(_close) == false && double.IsNaN(_low) == false && double.IsNaN(_high) == false; }
        }

        public double LowerOpenClose
        {
            get
            {
                return Math.Min(Open, Close);
            }
        }

        public double HigherOpenClose
        {
            get
            {
                return Math.Max(Open, Close);
            }
        }

        public double BarTopShadowLength
        {
            get
            {
                return High - Math.Max(Open, Close);
            }
        }

        public double BarBottomShadowLength
        {
            get
            {
                return Math.Min(Open, Close) - Low;
            }
        }

        public double BarTotalLength
        {
            get
            {
                return Math.Abs(High - Low);
            }
        }

        /// <summary>
        /// Did the bar close higher that it opened.
        /// </summary>
        public bool BarIsRising
        {
            get
            {
                return Close > Open;
            }
        }

        public double BarBodyLength
        {
            get
            {
                return Math.Abs(Open - Close);
            }
        }

        /// <summary>
        /// Construct empty BarData, for this dateTime.
        /// </summary>
        public BarData(DateTime dateTime)
        {
            _dateTime = dateTime.ToFileTime();
            _volume = double.NaN;
            _open = double.NaN;
            _close = double.NaN;
            _high = double.NaN;
            _low = double.NaN;
            isHigherPrev = false;
            //_arrow = 0;
            isCompleted = true;
            //_stopLoss = 0;
            //_gainTip = 0;
            //_stopGain = 0;
            sar = double.NaN;
            cr = double.NaN;
            isReverse = false;
            rsi = double.NaN;

            rsi2 = double.NaN;
            rsi3 = double.NaN;
            rsi4 = double.NaN;
            actPrice = double.NaN;
            stopLossPrice = double.NaN;
            stopGainPrice = double.NaN;
            signalList = new List<CandleSignal>();
            exHigh = _high;
            exLow = _low;
            ar = double.NaN;
            ar2 = double.NaN;
            br = double.NaN;
            cci = double.NaN;
            cci2 = double.NaN;
            cci3 = double.NaN;
            indicators = new Dictionary<string, double>();
            lwr = new Dictionary<string, double>();
            wr = double.NaN;
            wr2 = double.NaN;
            lwr.Add(LWR.LWR1, 0);
            lwr.Add(LWR.LWR2, 0);
            boll = new Dictionary<string, double>();
            boll.Add(BOLL.UPPER, 0);
            boll.Add(BOLL.MID, 0);
            boll.Add(BOLL.LOWER, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public BarData(DateTime dateTime, double open, double close, double min, double max, double volume)
        {
            _dateTime = dateTime.ToFileTime();
            _open = open;
            _close = close;
            _low = min;
            _high = max;
            _volume = volume;
            isHigherPrev = false;
            isCompleted = true;
            sar = double.NaN;
            cr = double.NaN;
            ar = double.NaN;
            ar2 = double.NaN;
            br = double.NaN;
            isReverse = false;
            rsi = double.NaN;
            actPrice = double.NaN;
            stopLossPrice = double.NaN;
            stopGainPrice = double.NaN;
            signalList = new List<CandleSignal>();
            exHigh = _high;
            exLow = _low;
            rsi2 = double.NaN;
            rsi3 = double.NaN;
            rsi4 = double.NaN;
            cci = double.NaN;
            cci2 = double.NaN;
            cci3 = double.NaN;
            indicators = new Dictionary<string, double>();
            lwr = new Dictionary<string, double>();
            wr = double.NaN;
            wr2 = double.NaN;
            lwr.Add(LWR.LWR1, 0);
            lwr.Add(LWR.LWR2, 0);
            boll = new Dictionary<string, double>();
            boll.Add(BOLL.UPPER, 0);
            boll.Add(BOLL.MID, 0);
            boll.Add(BOLL.LOWER, 0);
        }

        public BarData(DateTime dateTime, double open, double close, double min, double max, double volume, int signal)
        {
            _dateTime = dateTime.ToFileTime();
            _open = open;
            _close = close;
            _low = min;
            _high = max;
            _volume = volume;
            isHigherPrev = false;
            //this._arrow = signal;
            isCompleted = true;
            //_stopLoss = 0;
            //_gainTip = 0;
            //_stopGain = 0;
            sar = double.NaN;
            cr = double.NaN;
            isReverse = false;
            rsi = double.NaN;
            actPrice = double.NaN;
            stopLossPrice = double.NaN;
            stopGainPrice = double.NaN;
            signalList = new List<CandleSignal>();
            exHigh = _high;
            exLow = _low;
            ar = double.NaN;
            ar2 = double.NaN;
            br = double.NaN;
            rsi2 = double.NaN;
            rsi3 = double.NaN;
            rsi4 = double.NaN;
            cci = double.NaN;
            cci2 = double.NaN;
            cci3 = double.NaN;
            indicators = new Dictionary<string, double>();
            lwr = new Dictionary<string, double>();
            wr = double.NaN;
            wr2 = double.NaN;
            lwr.Add(LWR.LWR1, 0);
            lwr.Add(LWR.LWR2, 0);
            boll = new Dictionary<string, double>();
            boll.Add(BOLL.UPPER, 0);
            boll.Add(BOLL.MID, 0);
            boll.Add(BOLL.LOWER, 0);
        }

        /// <summary>
        /// The bar will encompas the values of the input ones, extending its values where needed.
        /// </summary>
        static public BarData CombinedBar(BarData[] bars)
        {
            BarData result = bars[0];

            double _volumeSum = 0;
            foreach (BarData bar in bars)
            {
                result._low = Math.Min(bar.Low, result.Low);
                result._high = Math.Max(bar.High, result.High);
                _volumeSum += bar.Volume;
            }

            result._close = bars[bars.Length - 1].Close;
            result._volume = _volumeSum / (float)bars.Length;

            return result;
        }

        public double GetValue(DataValueSourceEnum valueSource)
        {
            if (HasDataValues == false)
            {
                return double.NaN;
            }

            switch (valueSource)
            {
                case DataValueSourceEnum.High:
                    return this.High;
                case DataValueSourceEnum.Low:
                    return this.Low;
                case DataValueSourceEnum.Average:
                    return this.Average;
                case DataValueSourceEnum.Open:
                    return this.Open;
                case DataValueSourceEnum.Volume:
                    return this.Volume;
                case DataValueSourceEnum.Close:
                    return this.Close;
                default:
                    System.Diagnostics.Debug.Fail("Unexpected case.");
                    break;
            }
            return 0;
        }

        public string[] ToStrings()
        {
            return new string[] { DateTime.FromFileTime(_dateTime).ToString(), Open.ToString(), Close.ToString(), 
                Low.ToString(), High.ToString(), Volume.ToString() };
        }

        public override string ToString()
        {
            string s = base.ToString() + "[DateTime: " + DateTime.FromFileTime(_dateTime).ToString() + " Open: "
                + Open + " Close: " + Close + " Low: " + _low + " High : " + _high + " ExHigh: " + ExHigh + " ExLow: "
                + ExLow + " Complete: " + isCompleted + " SAR: " + sar + " RSI: " + rsi + " CR: " + cr + " ]";
            if (SignalList != null)
            {
                foreach (CandleSignal cs in SignalList)
                {
                    s += "\n" + cs.ToString();
                }
            }
            return s;
        }


        public static BarData[] GenerateTestSeries(int length)
        {
            BarData[] result = new BarData[length];
            for (int i = 0; i < result.Length; i++)
            {
                if (i == 0)
                {
                    result[i] = new BarData(DateTime.Now, GeneralHelper.Random(50), GeneralHelper.Random(50), GeneralHelper.Random(50), GeneralHelper.Random(50), 10);
                }
                else
                {
                    double open = result[i - 1].Close + GeneralHelper.Random(-2, 3);
                    double close = open + GeneralHelper.Random(-30, 31);
                    result[i] = new BarData(result[i - 1].DateTime + TimeSpan.FromSeconds(1), open, close, Math.Min(open, close) - GeneralHelper.Random(15), Math.Max(open, close) + GeneralHelper.Random(10), 10);
                }
            }

            return result;
        }

        public void RefreshExValues()
        {
            if (signalList != null && signalList.Count > 0)
            {
                exHigh = _high;
                exLow = _low;
                foreach (CandleSignal cs in signalList)
                {
                    // BarBodyLength
                    if (cs.Code == -1 || cs.Code == -3)
                        this.ExHigh = (1 - (0.0015f * cs.Arrow)) * this.ExHigh;
                    else if (cs.Code == 1 || cs.Code == 3)
                        this.ExLow = (1 - (0.0015f * cs.Arrow)) * this.ExLow;
                    else if (cs.Code == -2 || cs.Code == -4)
                        this.ExLow = (1 + (0.0015f * cs.Arrow)) * this.ExLow;
                    else if (cs.Code == 2 || cs.Code == 4)
                        this.ExHigh = (1 + (0.0015f * cs.Arrow)) * this.ExHigh;
                }
            }

            //Console.WriteLine(ToString());

        }

        //Dictionary<string, Indicator> Indicators = new Dictionary<string, Indicator>();

        #region IComparable 成员
        public int CompareTo(object obj)
        {
            return this.DateTime.CompareTo(((BarData)obj).DateTime);
        }


        #endregion

        #region IComparer 成员

        public int Compare(object x, object y)
        {
            return ((BarData)x).DateTime.CompareTo(((BarData)y).DateTime);
        }


      
        #endregion
    }
}
