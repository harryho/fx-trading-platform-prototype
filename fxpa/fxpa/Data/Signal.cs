using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace fxpa
{
    public class Signal : IComparer , IComparable
    {
        Symbol symbol=Symbol.UNKNOWN;
        Interval interval;

        string id;
        string name;

        int arrow = 0;
        int gainTip = 0;
        int stopLoss = 0;
        int stopGain = 0;
        int profit ;

        double actPrice;
        double stopGainPrice;
        double stopLossPrice;
        double stopLossBidPrice;

        double gainTipPrice;
        double profitPrice;       
        double sar = 0;

        DateTime actTime;
        DateTime gainTipTime;
        DateTime stopLossTime;
        DateTime stopGainTime;
        DateTime profitTime;

        public double StopLossBidPrice
        {
            get { return stopLossBidPrice; }
            set { stopLossBidPrice = value; }
        }

        public DateTime ProfitTime
        {
            get { return profitTime; }
            set { profitTime = value; }
        }

        public DateTime StopGainTime
        {
            get { return stopGainTime; }
            set { stopGainTime =value; }
        }

        public DateTime GainTipTime
        {
            get { return gainTipTime; }
            set { gainTipTime = value; }
        }

        public Symbol Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        public Interval Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public int GainTip
        {
            get { return gainTip; }
            set { gainTip = value; }
        }

        public int StopGain
        {
            get { return stopGain; }
            set { stopGain = value; }
        }

        public string Id
        {
            get { return id??symbol.ToString(); }
            set { id = value; }
        }

        public string Name
        {
            get { return name??symbol.ToString(); }
            set { name = value; }
        }

        public double StopGainPrice
        {
            get { return stopGainPrice; }
            set { stopGainPrice = value; }
        }

        public DateTime ActTime
        {
            get { return actTime; }
            set { actTime = value; }
        }

        public double ActPrice
        {
            get { return actPrice; }
            set { actPrice = value; }
        }

        public double StopLossPrice
        {
            get { return stopLossPrice; }
            set { stopLossPrice = value; }
        }

        public int Arrow
        {
            get { return arrow; }
            set { arrow = value; }
        }

        public int StopLoss
        {
            get { return stopLoss; }
            set { stopLoss = value; }
        }

        public double Sar
        {
            get { return sar; }
            set { sar = value; }
        }

        public DateTime StopLossTime
        {
            get { return stopLossTime; }
            set { stopLossTime = value; }
        }

        public int Profit
        {
            get { return profit; }
            set { profit = value; }
        }

        public double GainTipPrice
        {
            get { return gainTipPrice; }
            set { gainTipPrice = value; }
        }


        public double ProfitPrice
        {
            get { return profitPrice; }
            set { profitPrice = value; }
        }

        public override bool Equals(object obj)
        {
            return this.Symbol==(((Signal)obj).Symbol) && this.Interval==(((Signal)obj).Interval) && this.actTime.CompareTo(((Signal)obj).ActTime) == 0;
                //&& this.signalStatus == ((Signal)obj).signalStatus && this.stopLossStatus == ((Signal)obj).stopLossStatus;
        }

        public override string ToString()
        {
            return base.ToString() + "[DateTime: " + actTime.ToString() + " symbol : " + symbol +
                " arrow: " + arrow + " stopLoss: " + stopLoss + " stopGain:  " + stopGain + " actprice : " 
                + actPrice + " stopLossPrice: " + stopLossPrice + " GainTipPrice: " + gainTipPrice + "]";
        }

        #region IComparer 成员

        public int Compare(object x, object y)
        {
            return ((Signal)x).ActTime.CompareTo(((Signal)y).ActTime)*(-1);
        }

        #endregion

        #region IComparable 成员

        public int CompareTo(object obj)
        {
            return this.ActTime.CompareTo(((Signal)obj).ActTime)*(-1);
        }

        #endregion
    }
}
