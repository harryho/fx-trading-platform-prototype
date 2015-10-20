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
using System.Threading;

using TicTacTec.TA.Library;


namespace fxpa
{
    /// <summary>
    /// Manages reflection based creation of all the available indicator types (taken from TALIB, those with code etc.)
    /// Singleton thread safe implementation of Indicator Manager class.
    /// </summary>
    public sealed class FxpaIndicatorManager
    {
        #region Singleton Implementation

        static readonly FxpaIndicatorManager instance = new FxpaIndicatorManager();
        public static FxpaIndicatorManager Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Explicit static constructor to tell C# compiler, not to mark type as beforefieldinit
        /// </summary>
        static FxpaIndicatorManager()
        {
        }

        #endregion

     //   Dictionary<IndicatorGroups, Dictionary<string, GenericTALibIndicator>> _indicatorsGroups = new Dictionary<IndicatorGroups, Dictionary<string, GenericTALibIndicator>>();
        Dictionary<IndicatorGroups, Dictionary<string, BasicIndicator>> _indicatorsGroups = new Dictionary<IndicatorGroups, Dictionary<string, BasicIndicator>>();

        public enum IndicatorGroups
        {
            TaLib,
            TaLibCandleStickFormation,
            Custom,
            Fxpa,
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private FxpaIndicatorManager()
        {
            Dictionary<string, MethodInfo> methodsVerified = new Dictionary<string, MethodInfo>();
            lock (this)
            {
                MethodInfo[] methods = typeof(TicTacTec.TA.Library.Core).GetMethods();

                foreach (MethodInfo methodInfo in methods)
                {
                    if (methodInfo.ReturnType == typeof(Core.RetCode))
                    {
                        if (methodsVerified.ContainsKey(methodInfo.Name))
                        {// Establish the double[] input method of the two and use that.

                            ParameterInfo[] parameters = methodsVerified[methodInfo.Name].GetParameters();
                            ParameterInfo[] newParameters = methodInfo.GetParameters();

                            Console.WriteLine(parameters.Length == newParameters.Length);

                            for (int i = 0; i < parameters.Length; i++)
                            {
                                if (newParameters[i].ParameterType == typeof(Single[])
                                    && parameters[i].ParameterType == typeof(Double[]))
                                {// Current is double, new is single, stop parameter check.
                                    break;
                                }

                                if (newParameters[i].ParameterType == typeof(Double[])
                                    && parameters[i].ParameterType == typeof(Single[]))
                                {// Current is single, new is double, swap and stop parameter check.
                                    methodsVerified[methodInfo.Name] = methodInfo;
                                    break;
                                }
                            }                           
                        }
                        else
                        {
                            methodsVerified.Add(methodInfo.Name, methodInfo);
                        }

                    }
                }

                methodsVerified.Add("FxpaRsi", typeof(AppContext).GetMethod("method"));    

            }

//            _indicatorsGroups.Add(IndicatorGroups.TaLib, new Dictionary<string, GenericTALibIndicator>());
         //   _indicatorsGroups.Add(IndicatorGroups.TaLib, new Dictionary<string, BasicIndicator>());
            _indicatorsGroups.Add(IndicatorGroups.Fxpa, new Dictionary<string, BasicIndicator>());
            // List here data for all the indicators that are to be imported from TaLIB.

            #region Group TALin

            //InitializeTaLibIndicator(methodsVerified["Acos"], "Vector Trigonometric ACos", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Ad"], "Chaikin A/D Line", null, null, null, null);

            //InitializeTaLibIndicator(methodsVerified["Add"], "Vector Arithmetic Add", null, null, BarData.DataValueSourceEnum.Open, BarData.DataValueSourceEnum.Close);

            //InitializeTaLibIndicator(methodsVerified["AdOsc"], "Chaikin A/D Oscillator", null, null, null, null, 10, 20);

            //InitializeTaLibIndicator(methodsVerified["Adx"], "Average Directional Movement Index", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Adxr"], "Average Directional Movement Index Rating", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Apo"], "Absolute Price Oscillator", null, null, BarData.DataValueSourceEnum.Close, null, 10, 20, Core.MAType.Ema);

            //InitializeTaLibIndicator(methodsVerified["Aroon"], "Aroon", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["AroonOsc"], "Aroon Oscillator", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Asin"], "Vector Trigonometric ASin", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Atan"], "Vector Trigonometric ATan", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Atr"], "Average True Range", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["AvgPrice"], "Average Price", null, null, null, null);

            //InitializeTaLibIndicator(methodsVerified["Bbands"], "Bollinger Bands", null, null, BarData.DataValueSourceEnum.Close, null, 14, 10d, 10d, Core.MAType.Ema);

            //InitializeTaLibIndicator(methodsVerified["Beta"], "Beta", null, null, BarData.DataValueSourceEnum.Open, BarData.DataValueSourceEnum.Close, 14);

            //InitializeTaLibIndicator(methodsVerified["Bop"], "Balance Of Power", null, null, null, null);

            //InitializeTaLibIndicator(methodsVerified["Cci"], "Commodity Channel Index", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Ceil"], "Vector Ceil", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Cmo"], "Chande Momentum Oscillator", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Correl"], "Pearson's Correlation Coefficient (r)", null, null, BarData.DataValueSourceEnum.Open, BarData.DataValueSourceEnum.Close, 14);

            //InitializeTaLibIndicator(methodsVerified["Cos"], "Vector Trigonometric Cos", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Cosh"], "Vector Trigonometric Cosh", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Dema"], "Double Exponential Moving Average", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Div"], "Vector Arithmetic Div", null, null, BarData.DataValueSourceEnum.Open, BarData.DataValueSourceEnum.Close);

            //InitializeTaLibIndicator(methodsVerified["Dx"], "Directional Movement Index", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Ema"], "Exponential Moving Average", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Exp"], "Vector Arithmetic Exp", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Floor"], "Vector Floor", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["HtDcPeriod"], "Hilbert Transform - Dominant Cycle Period", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["HtDcPhase"], "Hilbert Transform - Dominant Cycle Phase", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["HtPhasor"], "Hilbert Transform - Phasor Components", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["HtSine"], "Hilbert Transform - SineWave", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["HtTrendline"], "Hilbert Transform - Instantaneous Trendline", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["HtTrendMode"], "Hilbert Transform - Trend vs Cycle Mode", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Kama"], "Kaufman Adaptive Moving Average", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["LinearReg"], "Linear Regression", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["LinearRegAngle"], "Linear Regression Angle", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["LinearRegIntercept"], "Linear Regression Intercept", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["LinearRegSlope"], "Linear Regression Slope", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Ln"], "Vector Log Natural", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Log10"], "Vector Log10", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Macd"], "Moving Average Convergence/Divergence", null, null, BarData.DataValueSourceEnum.Close, null, 10, 20, 12);

            //InitializeTaLibIndicator(methodsVerified["MacdExt"], "MACD with controllable MA type", null, null, BarData.DataValueSourceEnum.Close, null, 10, Core.MAType.Ema, 20, Core.MAType.Ema, 12, Core.MAType.Ema);

            //InitializeTaLibIndicator(methodsVerified["MacdFix"], "Moving Average Convergence/Divergence Fix 12/26", null, null, BarData.DataValueSourceEnum.Close, null, 10);

            //InitializeTaLibIndicator(methodsVerified["Mama"], "MESA Adaptive Moving Average", null, null, BarData.DataValueSourceEnum.Close, null, 10d, 20d);

            //InitializeTaLibIndicator(methodsVerified["Max"], "Highest value over a specified period", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MaxIndex"], "Index of highest value over a specified period", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MedPrice"], "Median Price", null, null, null, null);

            //InitializeTaLibIndicator(methodsVerified["Mfi"], "Money Flow Index", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MidPoint"], "MidPoint over period", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MidPrice"], "Midpoint Price over period", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Min"], "Lowest value over a specified period", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MinIndex"], "Index of lowest value over a specified period", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MinMax"], "Lowest and highest values over a specified period", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MinMaxIndex"], "Indexes of lowest and highest values over a specified period", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MinusDI"], "Minus Directional Indicator", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MinusDM"], "Minus Directional Movement", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Mom"], "Momentum", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["MovingAverage"], "Moving Average", null, null, BarData.DataValueSourceEnum.Close, null, 14, Core.MAType.Sma);

            //// Consumes double[] inPeriods, which is to be established programatically only.
            ////InitializeTaLibIndicator(methodsVerified["MovingAverageVariablePeriod"], "Moving Average with variable period", null, null, 2, 20, Core.MAType.Ema);

            //InitializeTaLibIndicator(methodsVerified["Mult"], "Vector Arithmetic Mult", null, null, BarData.DataValueSourceEnum.Open, BarData.DataValueSourceEnum.Close);

            //InitializeTaLibIndicator(methodsVerified["Natr"], "Normalized Average True Range", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Obv"], "On Balance Volume", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["PlusDI"], "Plus Directional Indicator", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["PlusDM"], "Plus Directional Movement", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Ppo"], "Percentage Price Oscillator", null, null, BarData.DataValueSourceEnum.Close, null, 12, 24, Core.MAType.Ema);

            //InitializeTaLibIndicator(methodsVerified["Roc"], "Rate of change : ((price/prevPrice)-1)*100", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["RocP"], "Rate of change Percentage: (price-prevPrice)/prevPrice", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["RocR"], "Rate of change ratio: (price/prevPrice)", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["RocR100"], "Rate of change ratio 100 scale: (price/prevPrice)*100", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            InitializeTaLibIndicator(methodsVerified["Rsi"], "Relative Strength Index", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Sar"], "Parabolic SAR", null, null, null, null, 1d, 2d);

            //InitializeTaLibIndicator(methodsVerified["SarExt"], "Parabolic SAR - Extended", null, null, null, null, 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d);

            //InitializeTaLibIndicator(methodsVerified["Sin"], "Vector Trigonometric Sin", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Sinh"], "Vector Trigonometric Sinh", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Sma"], "Simple Moving Average", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Sqrt"], "Vector Square Root", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["StdDev"], "Standard Deviation", null, null, BarData.DataValueSourceEnum.Close, null, 14, 2d);

            //InitializeTaLibIndicator(methodsVerified["Stoch"], "Stochastic", null, null, null, null, 10, 20, Core.MAType.Ema, 22, Core.MAType.Ema);

            //InitializeTaLibIndicator(methodsVerified["StochF"], "Stochastic Fast", null, null, null, null, 10, 20, Core.MAType.Ema);

            //InitializeTaLibIndicator(methodsVerified["StochRsi"], "Stochastic Relative Strength Index", null, null, BarData.DataValueSourceEnum.Close, null, 14, 10, 20, Core.MAType.Ema);

            //InitializeTaLibIndicator(methodsVerified["Sub"], "Vector Arithmetic Substraction", null, null, BarData.DataValueSourceEnum.Open, BarData.DataValueSourceEnum.Close);

            //InitializeTaLibIndicator(methodsVerified["Sum"], "Summation", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["T3"], "Triple Exponential Moving Average (T3)", null, null, BarData.DataValueSourceEnum.Close, null, 14, 2d);

            //InitializeTaLibIndicator(methodsVerified["Tan"], "Vector Trigonometric Tan", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Tanh"], "Vector Trigonometric Tanh", null, null, BarData.DataValueSourceEnum.Close, null);

            //InitializeTaLibIndicator(methodsVerified["Tema"], "Triple Exponential Moving Average", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Trima"], "Triangular Moving Average", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Trix"], "1-day Rate-Of-Change (ROC) of a Triple Smooth EMA", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["TrueRange"], "True Range", null, null, null, null);

            //InitializeTaLibIndicator(methodsVerified["Tsf"], "Time Series Forecast", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            //InitializeTaLibIndicator(methodsVerified["TypPrice"], "Typical Price", null, null, null, null);

            //InitializeTaLibIndicator(methodsVerified["UltOsc"], "Ultimate Oscillator", null, null, null, null, 12, 14, 16);

            //InitializeTaLibIndicator(methodsVerified["Variance"], "Variance", null, null, BarData.DataValueSourceEnum.Close, null, 14, 2d);

            //InitializeTaLibIndicator(methodsVerified["WclPrice"], "Weighted Close Price", null, null, null, null);

            //InitializeTaLibIndicator(methodsVerified["WillR"], "Williams' %R", null, null, null, null, 14);

            //InitializeTaLibIndicator(methodsVerified["Wma"], "Weighted Moving Average", null, null, BarData.DataValueSourceEnum.Close, null, 14);

            InitializeFxpaIndicator(methodsVerified["FxpaRsi"], "FXPA Relative Strength Index", null, null, BarData.DataValueSourceEnum.Close, null, 14);


            #endregion
        }

        void InitializeExtendedIndicators()
        {
            //List<Type> indicatorTypes =
            //ReflectionHelper.GatherTypeChildrenTypesFromAssembliesWithMatchingConstructor(
            //    typeof(Indicator), true, ReflectionHelper.GetApplicationEntryAssemblyReferencedAssemblies(),
            //    new Type[] { });
            //foreach (Type type in indicatorTypes)
            //{
            //    string name = type.Name;
            //    if (UserFriendlyNameAttribute.GetClassAttributeValue(type, ref name))
            //    {
            //        name = type.Name + ", " + name;
            //    }
            //    this.listViewIndicatorTypes.Items.Add(name).Tag = type;
            //}




            //Type indicatorType = listViewIndicatorTypes.SelectedItems[0].Tag as Type;
            //ConstructorInfo constructor = indicatorType.GetConstructor(new Type[] { });
            //if (constructor != null)
            //{
            //    _pendingIndicator = (Indicator)constructor.Invoke(new object[] { });
            //    propertyGrid1.SelectedObject = _pendingIndicator;
            //}


        }

        //void InitializeTaLibIndicator(MethodInfo methodInfo, string indicatorName, bool? isTradeable, bool? isScaledToQuotes, params object[] inputParameters)
        //{
        //    InitializeTaLibIndicator(methodInfo, indicatorName, isTradeable, isScaledToQuotes, null, null, inputParameters);
        //}

        void InitializeTaLibIndicator(MethodInfo methodInfo, string indicatorName, bool? isTradeable, 
            bool? isScaledToQuotes, BarData.DataValueSourceEnum? inRealSource, BarData.DataValueSourceEnum? inReal1Source, params object[] inputParameters)
        {
            lock (this)
            {
                GenericTALibIndicator indicator = GenericTALibIndicator.CreateInstance(methodInfo, indicatorName, isTradeable, isScaledToQuotes);

                if (indicator == null)
                {
                    Console.WriteLine("Creating indicator [" + indicator.Name + "] failed.");
                    return;
                }

                Console.WriteLine(indicator.SetInputParameters(inputParameters));

                _indicatorsGroups[IndicatorGroups.Fxpa].Add(indicator.Name, indicator);
                indicator.RealInputArraySource = inRealSource;
                indicator.Real1InputArraySource = inReal1Source;
            }
        }

        void InitializeFxpaIndicator(MethodInfo methodInfo, string indicatorName, bool? isTradeable,
          bool? isScaledToQuotes, BarData.DataValueSourceEnum? inRealSource, BarData.DataValueSourceEnum? inReal1Source, params object[] inputParameters)
        {
            lock (this)
            {
                FxpaIndicator indicator = FxpaIndicator.CreateInstance(methodInfo, indicatorName, isTradeable, isScaledToQuotes);

                if (indicator == null)
                {
                    Console.WriteLine("Creating indicator [" + indicator.Name + "] failed.");
                    return;
                }

                Console.WriteLine(indicator.SetInputParameters(inputParameters));

                _indicatorsGroups[IndicatorGroups.Fxpa].Add(indicator.Name, indicator);
                indicator.RealInputArraySource = inRealSource;
                indicator.Real1InputArraySource = inReal1Source;
            }
        }

        public string[] GetIndicatorsDescriptions(IndicatorGroups indicatorsGroup)
        {
            lock (this)
            {
                string[] result = new string[_indicatorsGroups[indicatorsGroup].Count];
                int i = 0;
                foreach(string name in _indicatorsGroups[indicatorsGroup].Keys)
                {
                    result[i] = _indicatorsGroups[indicatorsGroup][name].Description;
                    i++;
                }
                return result;
            }
        }
        
        public string[] GetIndicatorsNames(IndicatorGroups indicatorsGroup)
        {
            lock (this)
            {
                return GeneralHelper.EnumerableToArray<string>(_indicatorsGroups[IndicatorGroups.Fxpa].Keys);
            }
        }
        
        public BasicIndicator GetIndicatorCloneByName(IndicatorGroups indicatorGroup, string name)
        {
            lock (this)
            {
                return ((BasicIndicator)_indicatorsGroups[indicatorGroup][name]).SimpleClone();
            }
        }
    }
}
