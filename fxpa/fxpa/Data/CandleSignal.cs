using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
    public class CandleSignal 
    {
        public CandleSignal()
        {
        }

        public CandleSignal(Signal signal, int code)
        {
            Arrow = signal.Arrow;
            ArrowTime = signal.ActTime;
            switch (code)
            {
                case 1:
                    SignalPrice = signal.ActPrice;
                    break;
                case 2:
                    SignalPrice = signal.GainTipPrice;
                    break;
                case 3:
                    SignalPrice = signal.StopLossBidPrice;
                    break;
                case 4:
                    SignalPrice = signal.StopGainPrice;
                    break;
            }
            Code =signal.Arrow * code;
        }



        public override bool Equals(object obj)
        {
            return this.ArrowTime.CompareTo(((CandleSignal)obj).ArrowTime) == 0 &&
                this.arrow == ((CandleSignal)obj).Arrow && this.code == ((CandleSignal)obj).Code && this.signalPrice == ((CandleSignal)obj).SignalPrice;
        }


        int arrow;

        public int Arrow
        {
            get { return arrow; }
            set { arrow = value; }
        }

        DateTime arrowTime;

        public DateTime ArrowTime
        {
            get { return arrowTime; }
            set { arrowTime = value; }
        }

       double signalPrice;

        public double SignalPrice
        {
            get { return signalPrice; }
            set { signalPrice = value; }
        }

        /**
         *  1, -1 : arrow
         *  2, -2  : tip
         *  3, -3  : stoploss
         *  4, -4  : stopgain
         */
        int code; 

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        public string GetCnPriceLabel()
        {
            switch (code)
            {
                case 1:
                case -1:
                    return "入场Price: ";
                case 2:
                case -2:
                    return "赢利Price: ";
                case 3:
                case-3:
                    return "止损Price: ";
                case 4:
                case  -4:
                    return "止赢Price: ";
                default:
                    return null;
            }
        }


        public string GetCnSignalLabel()
        {
            switch (code){
                case 1:
                    return "向上入场: ";            
                case -1:
                    return "向下入场: ";            
                case 2:
                case -2:
                    return "赢利: ";
                case 3:
                case -3:
                    return "止损: ";
                case 4:
                case -4:
                    return "止赢: ";
                default:
                    return null;
            }
        }


        BarData barData;

        public BarData BarData
        {
            get { return barData; }
            set { barData = value; }
        }

        public override string ToString()
        {
            return base.ToString() + " [ Arrow Time: " + arrowTime.ToString() + " Arrow: " + arrow + " Code: " + code 
                + " Label: "+ GetCnSignalLabel()+" Price Label: "+GetCnPriceLabel() +" Signal Price: " + signalPrice + " ] ";
        }

    }
}
