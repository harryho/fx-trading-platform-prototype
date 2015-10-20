// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace fxpa
//{
//    public class RVI : TechnicalIndicator
//    {
//        public int TimePeriod = 14;

//        /// <summary>
//        /// 
//        /// </summary>
//        public RVI(DataProvider dataProvider)
//            : base(dataProvider)
//        {
//        }

//        //public enum RSIStates
//        //{
//        //    Default = 0,
//        //    Under30Up = 1,
//        //    Under30Down = 2,
//        //    Over70Up = 3,
//        //    Over70Down = 4
//        //}

//        public override string FullName
//        {
//            get { return "Relative Strength Index"; }
//        }

//        public override Type StateEnumType
//        {
//            get { return typeof(RSIStates); }
//        }

//        protected override void OnCalculate(int startingIndex, int indecesCount)
//        {// Calculate result.
//            double[] closes = _dataProvider.GetDataValues(startingIndex, indecesCount, DataProvider.DataValueSourceEnum.Close);
//            double[] result = new double[closes.Length];

//            int beginIndex, number;

//            TicTacTec.TA.Library.Core.RetCode code =
//            TicTacTec.TA.Library.Core.Rsi(0, indecesCount - 1, closes, TimePeriod,
//                    out beginIndex, out number, result);

//            System.Diagnostics.Debug.Assert(code == TicTacTec.TA.Library.Core.RetCode.Success);

//            AddResultSet(beginIndex, number, result);
//        }


//        protected override double[][] ProvideSignalAnalysisLines()
//        {
//            return new double[][] { ResultSets[0], CreateFixedLineResultLength(30), CreateFixedLineResultLength(70) };
//        }


//        /// <summary>
//        /// Line indeces always come from lower to biger.
//        /// </summary>
//        protected override float OnResultAnalysisCrossingFound(int line1index, double line1value, int line2index, double line2value, bool direction, float currentSignalPositionValue)
//        {// Calculating the result signals.

//            if ((line1index == 0 && line2index == 1))
//            {
//                if (direction)
//                {
//                    return (int)RSIStates.Under30Up;
//                }
//                else
//                {
//                    return (int)RSIStates.Under30Down;
//                }
//            }

//            if ((line1index == 0 && line2index == 2))
//            {
//                if (direction)
//                {
//                    return (int)RSIStates.Over70Up;
//                }
//                else
//                {
//                    return (int)RSIStates.Over70Down;
//                }
//            }

//            return (int)RSIStates.Default;
//        }


//        protected override float OnResultAnalysisExtremumFound(int lineIndex, double lineValue, bool direction, float currentPositionSignalValue)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }

//        protected override void RequiredAnalysis(out bool crossingAnalysis, out bool extremumAnalysis)
//        {
//            crossingAnalysis = true;
//            extremumAnalysis = false;
//        }

//    }
//}
