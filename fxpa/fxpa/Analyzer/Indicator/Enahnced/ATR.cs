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
    /// <summary>
    /// Average True Range (ATR) indicator measures a security's volatility.
    /// </summary>
    [CustomNameAttribute("Average True Range")]
    public class ATR : Indicator
    {
        // The range for the last day or so.
        int _timePeriod = 14;
        public int TimePeriod
        {
            get { return _timePeriod; }
            set { _timePeriod = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ATR()
            : base(true, false, new string[] { "ATR" })
        {
        }

        public enum ATRStates
        {
            Default = 0,
            MaxExtreme = 1,
            MinExtreme = 2
        }

        //public override Type StateEnumType
        //{
        //    get { return typeof(ATRStates); }
        //}

        protected override void OnCalculate(int startingIndex, int indecesCount)
        {
            double[] high = DataProvider.GetDataValues(BarData.DataValueSourceEnum.High, startingIndex, indecesCount);
            double[] low = DataProvider.GetDataValues(BarData.DataValueSourceEnum.Low, startingIndex, indecesCount);
            double[] close = DataProvider.GetDataValues(BarData.DataValueSourceEnum.Close, startingIndex, indecesCount);

            double[] result = new double[high.Length];

            int beginIndex, number;

            TicTacTec.TA.Library.Core.RetCode code = TicTacTec.TA.Library.Core.Atr(0, indecesCount - 1, high, low, close, TimePeriod,
                    out beginIndex, out number, result);

            System.Diagnostics.Debug.Assert(code == TicTacTec.TA.Library.Core.RetCode.Success);

            Results.SetResultSetValues("ATR", beginIndex, number, result);
        }

        public override float OnResultAnalysisExtremumFound(int lineIndex, double lineValue, bool direction, double currentPositionSignalValue)
        {
            System.Diagnostics.Debug.Assert(lineIndex == 0);

            if (direction)
            {
                return (int)ATRStates.MaxExtreme;
            }
            else
            {
                return (int)ATRStates.MinExtreme;
            }
        }

    }
}
