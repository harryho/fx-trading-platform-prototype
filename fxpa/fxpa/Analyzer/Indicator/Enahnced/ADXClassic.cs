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
//    /// Values are 0-100. Not a directional indicator.
//    /// </summary>
//    [UserFriendlyName("Directional Movement - Average Index")]
//    public class ADXClassic : Indicator
//    {
//        // 14 period ADX.
//        private int _timePeriod = 14;
//        public int TimePeriod
//        {
//            get { return _timePeriod; }
//            set { _timePeriod = value; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public ADXClassic() : base(true, false)
//        {
//        }

//        public enum ADXStates
//        {
//            Default = 0, // <20 indicates non trending market with low volumes;
//            Cross20 = 1, /// [1] cross of 20 is the possible start of a trend;
//            Cross30 = 2, /// [2] >30 indicates a strong trend
//            Cross40 = 3 /// [3] >40 and slow down indicates the slowdown of the current trend;
//        }

//        //public override Type StateEnumType
//        //{
//        //    get { return typeof(ADXStates); }
//        //}

//        protected override void OnCalculate(int startingIndex, int indecesCount)
//        {
//            double[] high = DataProvider.GetDataValues(BarData.DataValueSourceEnum.High, startingIndex, indecesCount);
//            double[] low = DataProvider.GetDataValues(BarData.DataValueSourceEnum.Low, startingIndex, indecesCount);
//            double[] close = DataProvider.GetDataValues(BarData.DataValueSourceEnum.Close, startingIndex, indecesCount);

//            double[] result = new double[high.Length];

//            int beginIndex, number;

//            TicTacTec.TA.Library.Core.RetCode code =
//            TicTacTec.TA.Library.Core.Adx(0, indecesCount - 1, high, low, close, TimePeriod,
//                    out beginIndex, out number, result);

//            System.Diagnostics.Debug.Assert(code == TicTacTec.TA.Library.Core.RetCode.Success);

//            Results.SetResultSetValues("default", beginIndex, number, result);
//        }

//        public override float OnResultAnalysisCrossingFound(int line1index, double line1value, int line2index, double line2value, bool direction, double currentSignalPositionValue)
//        {
//            if (line1index == 0 && line2index == 1 && direction)
//            {
//                return (int)ADXStates.Cross20;
//            }
//            if (line1index == 0 && line2index == 2 && direction)
//            {
//                return (int)ADXStates.Cross30;
//            }
//            if (line1index == 0 && line2index == 3 && direction)
//            {
//                return (int)ADXStates.Cross40;
//            }

//            return (int)ADXStates.Default;
//        }

//        protected override double[][] ProvideSignalAnalysisLines()
//        {
//            return new double[][] { Results.ResultSets[0].Values, Results.CreateFixedLineResultLength(20), Results.CreateFixedLineResultLength(30), Results.CreateFixedLineResultLength(40) };
//        }

//        public override float OnResultAnalysisExtremumFound(int lineIndex, double lineValue, bool direction, double currentPositionSignalValue)
//        {// Extremums not analized in this indicator results.
//            return 0;
//        }

//    }
//}
