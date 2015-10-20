// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;


namespace fxpa
{
    [CustomNameAttribute("Moving Average Convergence/Divergence")]
    public class MACD : Indicator
    {
        private int _fastPeriod = 12;
        public int FastPeriod
        {
            get { return _fastPeriod; }
            set { _fastPeriod = value; }
        }

        private int _slowPeriod = 26;
        public int SlowPeriod
        {
            get { return _slowPeriod; }
            set { _slowPeriod = value; }
        }

        private int _signal = 9;
        public int Signal
        {
            get { return _signal; }
            set { _signal = value; }
        }

        public enum MACDStates
        {
            Default = 0,
            Cross0Up = 1,
            Cross0Down = 2
        }

        /// <summary>
        /// 
        /// </summary>
        public MACD()
            : base(true, false, new string[] { "MACD", "MACDHistory", "MACDSignal" })
        {
        }

        //public override Type StateEnumType
        //{
        //    get { return typeof(MACDStates); }
        //}

        protected override void OnCalculate(int startingIndex, int indecesCount)
        {
            double[] averages = DataProvider.GetDataValues(BarData.DataValueSourceEnum.Average, startingIndex, indecesCount);

            double[] macd = new double[averages.Length];
            double[] macdSignal = new double[averages.Length];
            double[] macdHistory = new double[averages.Length];

            int beginIndex, number;
            TicTacTec.TA.Library.Core.RetCode code =
                TicTacTec.TA.Library.Core.Macd(0, indecesCount - 1, averages,
                    FastPeriod, SlowPeriod, Signal,
                    out beginIndex, out number, macd, macdSignal, macdHistory);

            System.Diagnostics.Debug.Assert(code == TicTacTec.TA.Library.Core.RetCode.Success);

            Results.SetResultSetValues("MACD", beginIndex, number, macd);
            Results.SetResultSetValues("MACDHistory", beginIndex, number, macdHistory);
            Results.SetResultSetValues("MACDSignal", beginIndex, number, macdSignal);
        }

        public override float OnResultAnalysisCrossingFound(int line1index, double line1value, int line2index, double line2value, bool direction, double currentSignalPositionValue)
        {
            if (line1index == 0 && line2index == 1)
            {
                if (direction)
                {
                    return (int)MACDStates.Cross0Up;
                }
                else
                {
                    return (int)MACDStates.Cross0Down;
                }
            }

            return (int)MACDStates.Default;
        }

        protected override double[][] ProvideSignalAnalysisLines()
        {
            return new double[][] { Results.ResultSets[2].Values, Results.CreateFixedLineResultLength(0) };
        }

    }
}
