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
//    [UserFriendlyName("Bollinger Bands")]
//    public class BBANDS : Indicator
//    {
//        int _timePeriod = 20;
//        public int TimePeriod
//        {
//            get { return _timePeriod; }
//            set { _timePeriod = value; }
//        }

//        double _nbDevUp = 2; // NOTE, this value has not been confirmed.
//        public double NbDevUp
//        {
//            get { return _nbDevUp; }
//            set { _nbDevUp = value; }
//        }

//        double _nbDevDown = 2; // NOTE, this value has not been confirmed.
//        public double NbDevDown
//        {
//            get { return _nbDevDown; }
//            set { _nbDevDown = value; }
//        }

//        public const TicTacTec.TA.Library.Core.MAType MAType = TicTacTec.TA.Library.Core.MAType.Ema;

//        double[] _calculationResultAverages;

//        /// <summary>
//        /// 
//        /// </summary>
//        public BBANDS()
//            : base(true, true)
//        {
//        }

//        public enum BBANDSStates
//        {
//            Default = 0,
//            UpperCrossUp = 1,
//            UpperCrossDown = 2,
//            MiddleCrossUp = 3,
//            MiddleCrossDown = 4,
//            LowerCrossUp = 5,
//            LowerCrossDown = 6
//        }

//        //public override Type StateEnumType
//        //{
//        //    get { return typeof(BBANDSStates); }
//        //}

//        protected override void OnCalculate(int startingIndex, int indecesCount)
//        {
//            _calculationResultAverages = DataProvider.GetDataValues(BarData.DataValueSourceEnum.Average, startingIndex, indecesCount);

//            double[] resultUpper = new double[_calculationResultAverages.Length];
//            double[] resultMiddle = new double[_calculationResultAverages.Length];
//            double[] resultLower = new double[_calculationResultAverages.Length];

//            int beginIndex, number;
//            TicTacTec.TA.Library.Core.Bbands(0, indecesCount - 1, _calculationResultAverages, TimePeriod,
//                NbDevUp, NbDevDown, MAType, out beginIndex, out number, resultUpper, resultMiddle, resultLower);

//            Results.SetResultSetValues("Upper", beginIndex, number, resultUpper);
//            Results.SetResultSetValues("Middle", beginIndex, number, resultMiddle);
//            Results.SetResultSetValues("Lower", beginIndex, number, resultLower);
//        }

//        public override float OnResultAnalysisCrossingFound(int line1index, double line1value, int line2index, double line2value, bool direction, double currentPositionSignalValue)
//        {
//            if (line1index == 0 && line2index == 1 && direction)
//            {
//                return (int)BBANDSStates.UpperCrossUp;
//            }
//            if (line1index == 0 && line2index == 1 && !direction)
//            {
//                return (int)BBANDSStates.UpperCrossDown;
//            }
//            if (line1index == 0 && line2index == 2 && direction)
//            {
//                return (int)BBANDSStates.MiddleCrossUp;
//            }
//            if (line1index == 0 && line2index == 2 && !direction)
//            {
//                return (int)BBANDSStates.MiddleCrossDown;
//            }
//            if (line1index == 0 && line2index == 3 && direction)
//            {
//                return (int)BBANDSStates.LowerCrossUp;
//            }
//            if (line1index == 0 && line2index == 3 && !direction)
//            {
//                return (int)BBANDSStates.LowerCrossDown;
//            }

//            return (int)BBANDSStates.Default;
//        }

//        protected override double[][] ProvideSignalAnalysisLines()
//        {
//            return new double[][] { _calculationResultAverages, Results.ResultSets[0].Values, Results.ResultSets[1].Values, Results.ResultSets[2].Values };
//        }

//    }
//}
