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
    /// Hosts the indicators results and signals.
    /// </summary>
    public class IndicatorResults
    {
        Indicator _indicator;
        protected Indicator Indicator
        {
            get { lock (this) { return _indicator; } }
        }
        
        int _actualResultSetStartingIndex = 0;
        protected int ActualResultsLength
        {
            get { return _signals.Length; }
        }

        List<IndicatorResultSet> _resultSets = new List<IndicatorResultSet>();
        public IndicatorResultSet[] ResultSets
        {
            get
            {
                lock (this)
                {
                    return _resultSets.ToArray();
                }
            }
        }

        double[] _signals;
        public double[] Signals
        {
            get
            {
                lock (this)
                {
                    return _signals;
                }
            }
        }

        /// <summary>
        /// In an extremum search, how many periods before and after the extremum are required to indicate an extremum as one.
        /// </summary>
        int _extremumAnalysisOutlinePeriod = 7;
        public int ExtremumAnalysisOutlinePeriod
        {
            get { return _extremumAnalysisOutlinePeriod; }
            set { _extremumAnalysisOutlinePeriod = value; }
        }

        public Type SignalStatesEnumType
        {
            get { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IndicatorResults(Indicator indicator, string[] resultSetNames)
        {
            _signals = new double[0];
            _actualResultSetStartingIndex = 0;
            foreach (string name in resultSetNames)
            {
                IndicatorResultSet set = new IndicatorResultSet(name, new double[0]);
                _resultSets.Add(set);
            }
            _indicator = indicator;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            lock (this)
            {
                _signals = new double[0];
                _actualResultSetStartingIndex = 0;
                foreach (IndicatorResultSet set in _resultSets)
                {
                    set.Clear();
                }
            }
        }

        IndicatorResultSet GetResultSetByName(string name)
        {
            lock (this)
            {
                foreach (IndicatorResultSet set in _resultSets)
                {
                    if (set.Name == name)
                    {
                        return set;
                    }
                }
                return null;
            }
        }

        ///<summary>
        /// This used to handle results.
        ///</summary>
        public bool SetResultSetValues(string name, int startIndex, int count, double[] inputResult)
        {
            lock (this)
            {

                IndicatorResultSet set = GetResultSetByName(name);
                if (set == null)
                {
                    //SystemMonitor.Error("SetResultSetValues result set [" + name + "] not found.");
                    Console.WriteLine(" SetResultSetValues result set [" + name + "] not found. ");
                    return false;
                }

                System.Diagnostics.Debug.Assert(ActualResultsLength == startIndex + count || ActualResultsLength == 0, "Result size mismatch.");
                if (ActualResultsLength == 0)
                {// First pass - set the Signals array.
                    _signals = new double[startIndex + count];
                }

                // We shall select the larges start index of result set to make sure a signal is placed not sooner, before all result sets are ready.
                _actualResultSetStartingIndex = Math.Max(_actualResultSetStartingIndex, startIndex + 1);

                // Get the data from the result it is provided to us.
                double[] finalResult = new double[startIndex + count];
                for (int i = 0; i < startIndex + count; i++)
                {
                    if (i < startIndex)
                    {
                        finalResult[i] = double.NaN;
                    }
                    else
                    {
                        finalResult[i] = inputResult[i - startIndex];
                    }
                }

                set.SetValues(finalResult);
            }
            return true;
        }

        /// <summary>
        /// Provide the system with a way to know what is the scope of the indicator signals.
        /// </summary>
        public void GetStateValues(out string[] names, out int[] values)
        {
            lock (this)
            {

                if (SignalStatesEnumType == null)
                {
                    names = new string[0];
                    values = new int[0];
                    return;
                }

                names = Enum.GetNames(SignalStatesEnumType);
                Array valuesArray = Enum.GetValues(SignalStatesEnumType);
                values = new int[valuesArray.Length];
                int i = 0;
                foreach (object value in valuesArray)
                {
                    values[i] = (int)value;
                    i++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PerformCrossingResultAnalysis(double[][] inputLines)
        {
            lock (this)
            {

                if (inputLines == null || inputLines.Length == 0 || inputLines[0].Length == 0)
                {
                    return;
                }

                for (int i = 0; i < inputLines.Length; i++)
                {
                    for (int j = i + 1; j < inputLines.Length; j++)
                    {
                        double[] line1 = inputLines[i];
                        double[] line2 = inputLines[j];

                        if (line1.Length == 0 || line2.Length == 0)
                        {
                            continue;
                        }

                        System.Diagnostics.Debug.Assert(line1.Length == line2.Length);

                        // Do not look for signals before the ResultSetStartingIndex, as those signals are invalid.
                        for (int k = _actualResultSetStartingIndex; k < line1.Length; k++)
                        {
                            if (k == 0)
                            {
                                if (line1[k] == line2[k])
                                {
                                    _signals[k] = _indicator.OnResultAnalysisCrossingFound(i, line1[k], j, line2[k], true, _signals[k]);
                                }
                            }
                            else
                            {
                                if ((line1[k - 1] >= line2[k - 1] && line1[k] <= line2[k]))
                                {
                                    _signals[k] = _indicator.OnResultAnalysisCrossingFound(i, line1[k], j, line2[k], false, _signals[k]);
                                }
                                else if (line1[k - 1] <= line2[k - 1] && line1[k] >= line2[k])
                                {
                                    _signals[k] = _indicator.OnResultAnalysisCrossingFound(i, line1[k], j, line2[k], true, _signals[k]);
                                }
                            }
                        }
                    }
                }

#if DEBUG // Verify the signals results agains the limits placed by the enum provided.
                string[] names;
                int[] values;
                GetStateValues(out names, out values);

                for (int i = 0; i < _signals.Length; i++)
                {
                    bool found = false;
                    foreach (int value in values)
                    {
                        if (_signals[i] == value)
                        {
                            found = true;
                            break;
                        }
                    }

                    System.Diagnostics.Debug.Assert(found, "Provided result is out of bounds. Possible calculation error.");
                }
#endif

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double[] CreateFixedLineResultLength(double value)
        {
            return MathHelper.CreateFixedLineResultLength(value, ResultSets[0].Values.Length);
        }

        /// <summary>
        /// Keep this in mind : this will search backwards for an extremum, but will not mark anything if any other signals are found on the way.
        /// The method of searching is going back and looking for a point that is the biggest/smallest in the ExtremumAnalysisOutlinePeriod count ticks
        /// before and after.
        /// </summary>
        private void AnalyseIndexExtremums(int inspectedLineIndex, int index, double[] values)
        {
            lock (this)
            {

                // check in the look back perdio, that there is no signal like us.

                double max = double.MinValue;
                double min = double.MaxValue;
                int lastExtremeMaxIndexFound = 0;
                int lastExtremeMinIndexFound = 0;

                // There is no use to look back before (index - (2 * ExtremumAnalysisOutlinePeriod + 2)).
                for (int i = index; i >= _actualResultSetStartingIndex && i >= index - (2 * ExtremumAnalysisOutlinePeriod + 2); i--)
                {
                    if (_signals[i] != 0 && lastExtremeMaxIndexFound == 0 && lastExtremeMinIndexFound == 0)
                    {// Some signal is already found, stop search.
                        return;
                    }

                    double current = values[i];

                    if (i <= index - ExtremumAnalysisOutlinePeriod)
                    { // OK, we are in the zone to start looking now.
                        if (current < min)
                        {
                            lastExtremeMinIndexFound = i;
                        }
                        if (current > max)
                        {
                            lastExtremeMaxIndexFound = i;
                        }
                    }

                    if (lastExtremeMinIndexFound - i >= ExtremumAnalysisOutlinePeriod)
                    {// Extreme minimum found.
                        _signals[index] = _indicator.OnResultAnalysisExtremumFound(inspectedLineIndex, _resultSets[inspectedLineIndex].Values[lastExtremeMinIndexFound], false, _signals[index]);
                        return;
                    }

                    if (lastExtremeMaxIndexFound - i >= ExtremumAnalysisOutlinePeriod)
                    {// Extreme maximum found.
                        _signals[index] = _indicator.OnResultAnalysisExtremumFound(inspectedLineIndex, _resultSets[inspectedLineIndex].Values[lastExtremeMaxIndexFound], true, _signals[index]);
                        return;
                    }

                    max = Math.Max(current, max);
                    min = Math.Min(current, min);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PerformExtremumResultAnalysis()
        {
            lock (this)
            {

                for (int i = 0; i < _resultSets.Count; i++)
                {
                    System.Diagnostics.Debug.Assert(_resultSets[i].Values.Length == _resultSets[0].Values.Length);
                    for (int k = _actualResultSetStartingIndex; k < _resultSets[0].Values.Length; k++)
                    {
                        AnalyseIndexExtremums(i, k, _resultSets[i].Values);
                    }
                }
            }
        }

    }
}



