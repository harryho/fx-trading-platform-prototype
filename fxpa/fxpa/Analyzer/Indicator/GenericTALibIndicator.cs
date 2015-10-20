// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


namespace fxpa
{
    public class GenericTALibIndicator : BasicIndicator
    {
        /// <summary>
        /// Dynamically collected using reflection.
        /// </summary>
        List<ParameterInfo> _inputDefaultArrayParameters = new List<ParameterInfo>();

        /// <summary>
        /// Dynamically collected using reflection.
        /// </summary>
        List<ParameterInfo> _intputParameters = new List<ParameterInfo>();
        public List<ParameterInfo> IntputParameters
        {
            get { return _intputParameters; }
        }

        /// <summary>
        /// Dynamically collected using reflection.
        /// </summary>
        List<ParameterInfo> _outputArraysParameters = new List<ParameterInfo>();

        /// <summary>
        /// Set by the user of the indicator.
        /// </summary>
        object[] _inputParametersValues = new object[] { };
        public object[] InputParametersValues
        {
            get { return _inputParametersValues; }
        }

        MethodInfo _methodInfo;

        /// <summary>
        /// Default null means indicator does not use it.
        /// </summary>
        BarData.DataValueSourceEnum? _realInputArraySource = null;
        /// <summary>
        /// Used to fill the double[] realIn (or real0In) array if the indicator requires it.
        /// </summary>
        public BarData.DataValueSourceEnum? RealInputArraySource
        {
            get { return _realInputArraySource; }
            set { _realInputArraySource = value; }
        }

        /// <summary>
        /// Default null means indicator does not use it.
        /// </summary>
        BarData.DataValueSourceEnum? _real1InputArraySource = null;
        /// <summary>
        /// Used to fill the double[] real1In array if the indicator requires it.
        /// </summary>
        public BarData.DataValueSourceEnum? Real1InputArraySource
        {
            get { return _real1InputArraySource; }
            set { _real1InputArraySource = value; }
        }

        GenericTALibIndicatorUI _ui;

        public override BasicIndicatorUI UI
        {
            get { return _ui; }
        }

        /// <summary>
        /// 
        /// </summary>
        GenericTALibIndicator(string name, string description, bool? isTradeable, bool? isScaledToQuotes, string[] resultSetNames)
            : base(isTradeable, isScaledToQuotes, resultSetNames)
        {
            this.Name = name;
            this.Description = description;

            _ui = new GenericTALibIndicatorUI(this);
            _ui.IndicatorUIUpdatedEvent += new BasicIndicatorUI.IndicatorUIUpdatedDelegate(_ui_IndicatorUIUpdatedEvent);
        }

        void _ui_IndicatorUIUpdatedEvent(BasicIndicatorUI ui)
        {
            this.Calculate();
        }

        public bool SetInputParameters(object[] parameters)
        {
            lock (this)
            {
                if (_intputParameters.Count != parameters.Length)
                {
                    return false;
                }

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (_intputParameters[i].ParameterType != parameters[i].GetType())
                    {
                        return false;
                    }
                }

                _inputParametersValues = parameters;
            }
            return true;
        }


        // Parameters format of TaLibCore functions.
        //int startIdx, - mandatory
        //int endIdx, - mandatory
        //double[] inReal[added 0/1] or/and inOpen or/and inLow or/and inHigh or/and inClose
        //int/double optIn[NAME] or/and another one or none - parameters

        //out int outBegIdx, 
        //out int outNBElement, 
        //double/int[] out[Real/Integer] and or another one

        // Example:
        //TicTacTec.TA.Library.Core.RetCode code = TicTacTec.TA.Library.Core.Sma(0, indecesCount - 1, _closeResultValues, Period, out beginIndex, out number, ma);        }

        public static GenericTALibIndicator CreateInstance(MethodInfo methodInfo, string description, bool? isTradeable, bool? isScaledToQuotes)
        {

            if (methodInfo == null)
            {
                return null;
            }

            Type returnType = methodInfo.ReturnType;

            if (returnType != typeof(TicTacTec.TA.Library.Core.RetCode))
            {
                return null;
            }

            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length < 5)
            {
                return null;
            }

            int index = 0;

            if (parameters[index].ParameterType != typeof(int) ||
                parameters[index].Name != "startIdx")
            {
                return null;
            }
            index++;

            if (parameters[index].ParameterType != typeof(int) ||
                parameters[index].Name != "endIdx")
            {
                return null;
            }
            index++;

            List<ParameterInfo> indicatorParameters = new List<ParameterInfo>();
            while (parameters.Length > index && parameters[index].ParameterType == typeof(double[]))
            {
                if (parameters[index].Name != "inReal" &&
                    parameters[index].Name != "inReal0" &&
                    parameters[index].Name != "inReal1" &&
                    parameters[index].Name != "inHigh" &&
                    parameters[index].Name != "inLow" &&
                    parameters[index].Name != "inOpen" &&
                    parameters[index].Name != "inClose" && 
                    parameters[index].Name != "inVolume"
                    )
                {
                    return null;
                }

                indicatorParameters.Add(parameters[index]);
                index++;
            }

            // optIn parameters
            List<ParameterInfo> indicatorInputParameters = new List<ParameterInfo>();
            while(parameters.Length > index && parameters[index].Name.StartsWith("optIn"))
            {
                if (parameters[index].ParameterType == typeof(int) ||
                    parameters[index].ParameterType != typeof(double) ||
                    parameters[index].ParameterType != typeof(TicTacTec.TA.Library.Core.MAType))
                {
                    indicatorInputParameters.Add(parameters[index]);
                }
                else
                {// Invalid type.
                    return null;
                }
                index++;
            }

            if (parameters.Length <= index || parameters[index].IsOut == false
                || parameters[index].Name != "outBegIdx")
            {
                return null;
            }

            index++;

            if (parameters.Length <= index || parameters[index].IsOut == false
                || parameters[index].Name != "outNBElement")
            {
                return null;
            }

            index++;

            List<ParameterInfo> indicatorOutputArrayParameters = new List<ParameterInfo>();
            List<string> indicatorOutputArrayParametersNames = new List<string>();
            while (parameters.Length > index)
            {
                if (parameters[index].Name.StartsWith("out") == false)
                {
                    return null;
                }

                if (parameters[index].ParameterType == typeof(double[])
                    || parameters[index].ParameterType == typeof(int[]))
                {
                    indicatorOutputArrayParametersNames.Add(parameters[index].Name);
                    indicatorOutputArrayParameters.Add(parameters[index]);
                }
                else 
                {
                    return null;
                }
                
                index++;
            }

            if (parameters.Length != index)
            {// Parameters left unknown.
                return null;
            }

            GenericTALibIndicator indicator = new GenericTALibIndicator(methodInfo.Name, description, isTradeable, isScaledToQuotes, indicatorOutputArrayParametersNames.ToArray());
            indicator._inputDefaultArrayParameters.AddRange(indicatorParameters);
            indicator._outputArraysParameters.AddRange(indicatorOutputArrayParameters);
            indicator._intputParameters.AddRange(indicatorInputParameters);
            indicator._methodInfo = methodInfo;

            return indicator;
        }

        double[] GetInputArrayValues(string valueArrayTypeName, int startingIndex, int indexCount)
        {
            if (valueArrayTypeName == "inLow")
            {
                return DataProvider.GetDataValues(BarData.DataValueSourceEnum.Low, startingIndex, indexCount);
            }
            else
            if (valueArrayTypeName == "inHigh")
            {
                return DataProvider.GetDataValues(BarData.DataValueSourceEnum.High, startingIndex, indexCount);
            }
            else
            if (valueArrayTypeName == "inOpen")
            {
                return DataProvider.GetDataValues(BarData.DataValueSourceEnum.Open, startingIndex, indexCount);
            }
            else
            if (valueArrayTypeName == "inClose")
            {
                return DataProvider.GetDataValues(BarData.DataValueSourceEnum.Close, startingIndex, indexCount);
            }
            else
            if (valueArrayTypeName == "inVolume")
            {
                return DataProvider.GetDataValues(BarData.DataValueSourceEnum.Volume, startingIndex, indexCount);
            }
            else
            if (valueArrayTypeName == "inReal")
            {
                if (_realInputArraySource.HasValue)
                {
                    return DataProvider.GetDataValues(_realInputArraySource.Value, startingIndex, indexCount);
                }
                else
                {
                    Console.WriteLine("inReal parameter not assigned.");
                }
            }
            else
            if (valueArrayTypeName == "inReal0")
            {
                if (_realInputArraySource.HasValue)
                {
                    return DataProvider.GetDataValues(_realInputArraySource.Value, startingIndex, indexCount);
                }
                else
                {
                    Console.WriteLine("inReal parameter not assigned.");
                }
            }
            else
            if (valueArrayTypeName == "inReal1")
            {
                if (_real1InputArraySource.HasValue)
                {
                    return DataProvider.GetDataValues(_real1InputArraySource.Value, startingIndex, indexCount);
                }
                else
                {
                    Console.WriteLine("inReal parameter not assigned.");
                }
            }

            Console.WriteLine("Class operation logic error.");
            return null;
        }

        protected override void OnCalculate(int startingIndex, int indexCount)
        {
            // Format of a TA method.
            //int startIdx, - mandatory
            //int endIdx, - mandatory
            //double[] inReal[added 0/1] or/and inOpen or/and inLow or/and inHigh or/and inClose
            //int/double optIn[NAME] or/and another one or none - parameters

            //out int outBegIdx, 
            //out int outNBElement, 
            //double/int[] out[Real/Integer] and or another one

            // Example:
            //TicTacTec.TA.Library.Core.RetCode code = TicTacTec.TA.Library.Core.Sma(0, indecesCount - 1, _closeResultValues, Period, out beginIndex, out number, ma);        }

            // Consider the result returned.
            List<object> parameters = new List<object>();
            parameters.Add(0);
            parameters.Add(indexCount - 1);

            int outBeginIdxPosition = 0;

            lock (this)
            {
                foreach (ParameterInfo info in _inputDefaultArrayParameters)
                {
                    parameters.Add(GetInputArrayValues(info.Name, startingIndex, indexCount));
                }

                foreach (object parameter in _inputParametersValues)
                {
                    parameters.Add(parameter);
                }

                outBeginIdxPosition = parameters.Count;

                // outBeginIdx
                parameters.Add(0);

                // outNBElemenet
                parameters.Add(0);

                foreach (ParameterInfo info in _outputArraysParameters)
                {
                    if (info.ParameterType == typeof(double[]))
                    {// Passed arrays must be prepared to the proper size.
                        double[] array = new double[indexCount];
                        parameters.Add(array);
                    }
                    else if (info.ParameterType == typeof(int[]))
                    {// Passed arrays must be prepared to the proper size.
                        int[] array = new int[indexCount];
                        parameters.Add(array);
                    }
                    else
                    {
                        Console.WriteLine("Class operation logic error.");
                    }
                }

                // This is how the normal call looks like.
                //TicTacTec.TA.Library.Core.Adx((int)parameters[0], (int)parameters[1], (double[])parameters[2],
                //    (double[])parameters[3], (double[])parameters[4], (int)parameters[5],
                //    out outBeginIdx, out outNBElemenet, (double[])parameters[8]);

            }
            
            object[] parametersArray = parameters.ToArray();

            TicTacTec.TA.Library.Core.RetCode code = (TicTacTec.TA.Library.Core.RetCode)
                _methodInfo.Invoke(null, parametersArray);

            lock (this)
            {
                int outBeginIdx = (int)parametersArray[outBeginIdxPosition];
                int outNBElemenet = (int)parametersArray[outBeginIdxPosition + 1];

                for (int i = 0; i < _outputArraysParameters.Count; i++)
                {
                    int index = outBeginIdxPosition + 2 + i;
                    if (parametersArray[index].GetType() == typeof(double[]))
                    {
                        Results.SetResultSetValues(_outputArraysParameters[i].Name, outBeginIdx, outNBElemenet, (double[])parametersArray[index]);
                    }
                    else if (parametersArray[index].GetType() == typeof(int[]))
                    {// Valid scenario, implement.
                        //SystemMonitor.NotImplementedCritical();
                        Results.SetResultSetValues(_outputArraysParameters[i].Name, outBeginIdx, outNBElemenet, GeneralHelper.IntsToDoubles((int[])parametersArray[index]));
                    }
                }
            }
        }

        /// <summary>
        /// Does not include results.
        /// </summary>
        /// <returns></returns>
        public override BasicIndicator SimpleClone()
        {
            Console.WriteLine((this.DataProvider != null).ToString());

            GenericTALibIndicator newIndicator = CreateInstance(_methodInfo, Description, Tradeable, ScaledToQuotes);
            newIndicator._inputDefaultArrayParameters = new List<ParameterInfo>(_inputDefaultArrayParameters);
            newIndicator._intputParameters = new List<ParameterInfo>(_intputParameters);
            newIndicator._outputArraysParameters = new List<ParameterInfo>(_outputArraysParameters);
            newIndicator._inputParametersValues = (object[])_inputParametersValues.Clone();
            newIndicator._realInputArraySource = _realInputArraySource;
            newIndicator._real1InputArraySource = _real1InputArraySource;

            //GenericTALibIndicator newIndicator = (GenericTALibIndicator)this.MemberwiseClone();
            //// Still pointing to old indicator UI, deatach.
            //newIndicator._ui.IndicatorUIUpdatedEvent -= new PlatformIndicatorUI.IndicatorUIUpdatedDelegate(_ui_IndicatorUIUpdatedEvent);

            // Create and attach.
            //newIndicator._ui = new GenericTALibIndicatorUI(newIndicator);
            //newIndicator._ui.IndicatorUIUpdatedEvent += new PlatformIndicatorUI.IndicatorUIUpdatedDelegate(_ui_IndicatorUIUpdatedEvent);

            //lock (this)
            //{
            //    List<string> resultSetsNames = new List<string>();
            //    foreach (IndicatorResultSet set in _results.ResultSets)
            //    {
            //        resultSetsNames.Add(set.Name);
            //    }

            //    newIndicator._results = new IndicatorResults(newIndicator, resultSetsNames.ToArray());
            //}
            return newIndicator;
        }

    }
}
