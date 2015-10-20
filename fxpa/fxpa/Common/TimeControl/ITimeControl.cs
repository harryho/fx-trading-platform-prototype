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
    /// Allows to perform time control on providers, executioners etc. that allow this.
    /// </summary>
    public interface ITimeControl
    {
        int PeriodsCount { get; }
        int CurrentPeriod { get; }
        TimeSpan Interval { get; }

        bool StepForward();
        bool StepBack();

        bool StepTo(int index);
    }
}
