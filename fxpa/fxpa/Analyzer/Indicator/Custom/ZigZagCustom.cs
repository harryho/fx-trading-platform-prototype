// -----
// GNU General Public License
// The Open Forex Platform is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Open Forex Platform is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

//using System;
//using System.Collections.Generic;
//using System.Text;
//using CommonSupport;
//using CommonFinancial;

//namespace ForexPlatform
//{
//    /// <summary>
//    /// This indicator is not tradeable since it changes its position and requres future looking.
//    /// Indicator written manually no external lib used.
//    /// </summary>
//    [UserFriendlyName("ZigZag Custom")]
//    public class ZigZagCustom : Indicator
//    {
//        /// <summary>
//        /// If not using open close, than we use low and high (default).
//        /// </summary>
//        bool _useOpenClose = false;
//        public bool UseOpenClose
//        {
//            get { return _useOpenClose; }
//            set { _useOpenClose = value; }
//        }

//        double _significancePercentage = 3;
//        public double SignificancePercentage
//        {
//            get { return _significancePercentage; }
//            set { _significancePercentage = value; }
//        }

//        //int _minimumLength = 5;
//        //public int MinimumLength
//        //{
//        //    get { return _minimumLength; }
//        //    set { _minimumLength = value; }
//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        public ZigZagCustom()
//            : base(false, true)
//        {
//        }

//        public enum ZigZagStates
//        {
//            Default = 0,
//            PeakHigh = 1,
//            PeakLow = 2
//        }

//        //public override Type StateEnumType
//        //{
//        //    get { return typeof(ZigZagStates); }
//        //}

//        protected override void OnCalculate(int startingIndex, int indecesCount)
//        {
//            double[] values1, values2;

//            if (UseOpenClose)
//            {
//                values1 = DataProvider.GetDataValues(BarData.DataValueSourceEnum.Open, startingIndex, indecesCount);
//                values2 = DataProvider.GetDataValues(BarData.DataValueSourceEnum.Close, startingIndex, indecesCount);
//            }
//            else
//            {
//                values1 = DataProvider.GetDataValues(BarData.DataValueSourceEnum.High, startingIndex, indecesCount);
//                values2 = DataProvider.GetDataValues(BarData.DataValueSourceEnum.Low, startingIndex, indecesCount);
//            }

//            System.Diagnostics.Debug.Assert(values1.Length == values2.Length);

//            if (indecesCount == 0)
//            {
//                Results.SetResultSetValues("ZigZag", startingIndex, indecesCount, new double[] { });
//                return;
//            }

//            // This is simple indicator, it can directly write to the signals array.
//            double[] signals = new double[indecesCount];
//            signals[0] = (double)ZigZagStates.PeakHigh;

//            int lastPeakIndex = 0;
//            ZigZagStates lastPeakState = ZigZagStates.PeakHigh;

//            // Perform actual calculation.
//            for (int i = 0; i < values1.Length; i++)
//            {
//                double requiredDifferenceValue = (SignificancePercentage / 100) * values1[i];

//                double high = Math.Max(values1[i], values2[i]);
//                double low = Math.Min(values1[i], values2[i]);

//                double lastPeakValue = 0;
//                if (lastPeakState == ZigZagStates.PeakHigh)
//                {
//                    lastPeakValue = Math.Max(values1[lastPeakIndex], values2[lastPeakIndex]);
//                }
//                else if (lastPeakState == ZigZagStates.PeakLow)
//                {
//                    lastPeakValue = Math.Min(values1[lastPeakIndex], values2[lastPeakIndex]);
//                }

//                bool newLow = (lastPeakState == ZigZagStates.PeakHigh && (lastPeakValue - low >= requiredDifferenceValue)) || (lastPeakState == ZigZagStates.PeakLow && low < lastPeakValue);
//                bool newHigh = (lastPeakState == ZigZagStates.PeakLow && (high - lastPeakValue >= requiredDifferenceValue)) || (lastPeakState == ZigZagStates.PeakHigh && high > lastPeakValue);

//                //System.Diagnostics.Debug.Assert(newLow == false || newHigh == false);

//                if (newLow && newHigh)
//                {// Favor the extension of existing peak in this case.
//                    if (lastPeakState == ZigZagStates.PeakHigh)
//                    {
//                        newLow = false;
//                    }
//                    else
//                    {
//                        newHigh = false;
//                    }
//                }

//                if (newHigh)
//                {
//                    if (lastPeakState == ZigZagStates.PeakHigh)
//                    {// Update the high.
//                        signals[lastPeakIndex] = (double)ZigZagStates.Default;
//                    }
//                    else if (lastPeakState == ZigZagStates.PeakLow)
//                    {// New high found.
//                        lastPeakState = ZigZagStates.PeakHigh;
//                    } 
//                    else
//                    {
//                        System.Diagnostics.Debug.Fail("Unexpected case.");
//                    }

//                    lastPeakIndex = i;
//                    signals[i] = (double)ZigZagStates.PeakHigh;
//                } 
//                else if (newLow)
//                {
//                    if (lastPeakState == ZigZagStates.PeakLow)
//                    {// Update the low.
//                        signals[lastPeakIndex] = (double)ZigZagStates.Default;
//                    }
//                    else if (lastPeakState == ZigZagStates.PeakHigh)
//                    {// New low found.
//                        lastPeakState = ZigZagStates.PeakLow;
//                    }

//                    lastPeakIndex = i;
//                    signals[lastPeakIndex] = (double)ZigZagStates.PeakLow;
//                }

//            }

//            // Finish with a signal.
//            if (signals[signals.Length - 1] == (double)ZigZagStates.Default)
//            {
//                if (lastPeakState == ZigZagStates.PeakHigh)
//                {
//                    signals[signals.Length - 1] = (double)ZigZagStates.PeakLow;
//                }
//                else
//                {
//                    signals[signals.Length - 1] = (double)ZigZagStates.PeakHigh;
//                }
//            }

//            // Finally create the results, that shows a line moving between the peak values - like a proper ZigZag indicator.
//            lastPeakIndex = 0;
//            lastPeakState = ZigZagStates.PeakHigh;
//            double[] results = new double[indecesCount];

//            for (int i = 1; i < signals.Length; i++)
//            {
//                if (signals[i] != (double)ZigZagStates.Default)
//                {
//                    double lastPeakValue, currentPeakValue;

//                    if (lastPeakState == ZigZagStates.PeakHigh)
//                    {
//                        lastPeakValue = Math.Max(values1[lastPeakIndex], values2[lastPeakIndex]);
//                        currentPeakValue = Math.Min(values1[i], values2[i]);
//                    }
//                    else 
//                    {
//                        lastPeakValue = Math.Min(values1[lastPeakIndex], values2[lastPeakIndex]);
//                        currentPeakValue = Math.Max(values1[i], values2[i]);
//                    }

//                    double[] midValues = MathHelper.CreateConnectionValues(lastPeakValue, currentPeakValue, i - lastPeakIndex);
//                    midValues.CopyTo(results, lastPeakIndex);

//                    lastPeakIndex = i;
//                    lastPeakState = (ZigZagStates)signals[i];
//                }

//            }

//            Results.SetResultSetValues("ZigZag", startingIndex, indecesCount, results);
//            // Only after we add the result set, the signals become available.
//            signals.CopyTo(Results.Signals, 0);
//        }

//    }
//}
