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
    [Serializable]
    public struct SessionInfo
    {
        Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        string _id;
        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// TODO : this needs to be updated.
        /// </summary>
        public string BaseCurrency
        {
            get { return _symbol.Substring(0, 3); }
        }

        string _symbol;
        public string Symbol
        {
          get { return _symbol; }
        }

        TimeSpan _timeInterval;
        /// <summary>
        /// How long is one period of this session.
        /// If the value is zero, Tick based is accepted (each time the value changes).
        /// </summary>
        public TimeSpan TimeInterval
        {
            get { return _timeInterval; }
        }

        double _lotSize;
        public double LotSize
        {
            get { return _lotSize; }
        }

        double _pointDigits;
        public double PointDigits
        {
            get { return _pointDigits; }
        }


        public override bool Equals(object obj)
        {
            return ((SessionInfo)obj).Symbol == this.Symbol;
        }
        /// <summary>
        /// 
        /// </summary>
        public SessionInfo(string id, string symbol, Guid guid, TimeSpan timeInterval, double lotSize, double pointDigits)
        {
            _pointDigits = pointDigits;
            _lotSize = lotSize;
            _id = id;
            _symbol = symbol;
            _timeInterval = timeInterval;
            _guid = guid;
        }
    }
}
