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
    public enum UpdateType
    {// In order of importance.
        Initial, 
        NewBar,
        BarsUpdated,
        Quote
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="updateType"></param>
    /// <param name="count"></param>
    /// <param name="stepsRemaining">Multi step mode occurs when many steps are done one after the other in fast succession. Usefull for UI not to update itself.</param>
    public delegate void ValuesUpdatedDelegate(IDataProvider provider, UpdateType updateType, int count, int stepsRemaining);

    public interface IDataProvider 
    {
        string Name { get; }

        double Ask { get; }
        double Bid { get; }
        double Spread{ get; }

        int DataUnitCount { get; }

        /// <summary>
        /// This is where time control how be exercised on a provider. 
        /// If provider does not support time control, member is null.
        /// </summary>
        ITimeControl TimeControl { get; }

        DateTime FirstBarDateTime{ get; }
        DateTime LastBarDateTime{ get; }
        IList<BarData> DataUnits{ get; }

        DateTime CurrentSourceDateTime{ get; }
        BarData? CurrentDataUnit { get; }

        TimeSpan TimeInterval { get; }

        /// <summary>
        /// Update type, updated items count.
        /// </summary>
        event ValuesUpdatedDelegate ValuesUpdateEvent;
        //Harry--Modified
        //void RequestValuesUpdate();

        double[] GetDataValues(BarData.DataValueSourceEnum valueEnum);
        double[] GetDataValues(BarData.DataValueSourceEnum valueEnum, int startingIndex, int indexCount);

        int GetBarIndexAtTime(DateTime time);

        void UnInitialize();
    }
}
