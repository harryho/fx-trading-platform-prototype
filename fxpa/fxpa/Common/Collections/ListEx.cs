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
    public class ListEx<TClass> : List<TClass>
    {
        bool _singleEntryMode = true;
        /// <summary>
        /// An item is allowed to enter only once.
        /// </summary>
        public bool SingleEntryMode
        {
            get { return _singleEntryMode; }
            set { _singleEntryMode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ListEx()
        {
        }

        public void UpdateItem(TClass item, bool isAdded)
        {
            if (isAdded)
            {
                Add(item);
            }
            else
            {
                Remove(item);
            }
        }

        public new bool Add(TClass item)
        {
            if (SingleEntryMode && this.Contains(item))
            {
                return false;
            }
            base.Add(item);
            return true;
        }

        public new void AddRange(IEnumerable<TClass> collection)
        {
            if (SingleEntryMode)
            {
                List<TClass> items = new List<TClass>();
                foreach (TClass item in collection)
                {
                    if (this.Contains(item) == false)
                    {
                        items.Add(item);
                    }
                }
                base.AddRange(items);
            }
            else
            {
                base.AddRange(collection);
            }
        }

        public new void Insert(int index, TClass item)
        {
            if (SingleEntryMode && this.Contains(item))
            {
                return;
            }

            base.Insert(index, item);
        }

        public new void InsertRange(int index, IEnumerable<TClass> collection)
        {
            if (SingleEntryMode)
            {
                List<TClass> items = new List<TClass>();
                foreach (TClass item in collection)
                {
                    if (this.Contains(item) == false)
                    {
                        items.Add(item);
                    }
                }
                base.InsertRange(index, items);
            }
            else
            {
                base.InsertRange(index, collection);
            }
            
        }

    }
}
