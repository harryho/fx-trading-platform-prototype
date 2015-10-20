using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace fxpa
{
    public class PublishInfo : IComparable, IComparer
    {
        static string INFO_TYPE=" FXPA Info：";

        DateTime dateTime;

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        string type;

        public string Type
        {
            get { return type??INFO_TYPE; }
            set { type = value; }
        }



        string content;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        Symbol symbol=Symbol.UNKNOWN;

        public Symbol Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        Interval interval;

        public Interval Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public PublishInfo()
        {
        }

        public PublishInfo(DateTime dateTime, string content)
        {
            this.dateTime = dateTime;
            this.content = content;
        }

        public int Compare(object x, object y)
        {
            return ((PublishInfo)x).DateTime.CompareTo(((PublishInfo)y).DateTime) * (-1);
        }


        public int CompareTo(object obj)
        {
            return this.DateTime.CompareTo(((PublishInfo)obj).DateTime) * (-1);
        }
    }
}
