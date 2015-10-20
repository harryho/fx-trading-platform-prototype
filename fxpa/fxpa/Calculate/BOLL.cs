using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *
 * 
 * @author herry
 */
namespace fxpa
{
    public class BOLL
    {
        public static string UPPER = "UPPER";
        public static string MID = "MID";
        public static string LOWER = "LOWER";

        // Initialize necessary candles 
        private int initCandleSize;
        private int n = 26;

        string name = "";
        string forexCode = "";
        private int interval;
        List<BarData> initCandleList = new List<BarData>();
        bool isInitialized = false;

	public BOLL(String code, int interval) {
		this.forexCode = code;
		this.name = "BOLL";
		this.interval = interval;
		n = 20;
		this.initCandleSize = this.n - 1;
	}

	public BOLL(String code, int interval, int n) {
		this.forexCode = code;
        this.name = "BOLL";
		this.interval = interval;
		this.n = n;
		this.initCandleSize = this.n - 1;
	}

        public   Dictionary<string, double> handleFullCandle(ref BarData candle, int field) {
  Dictionary<string, double> boll = new Dictionary<string, double>();
            boll.Add(UPPER, 0);
               boll.Add(MID, 0);
            boll.Add(LOWER, 0);


	  if (!this.isInitialized)
            {
                this.initCandleList.Add(candle);
                if (initCandleList.Count == this.n)
                {
                    isInitialized = true;
                }
                return boll;
            }

            if (initCandleList.Count >= this.n)
            {
                initCandleList.RemoveAt(0);
            }
		this.initCandleList.Add(candle);
		
		// the sum of close price
        double sum = 0;
		foreach (BarData c in initCandleList) {
			sum = sum+c.Close;
		}

		// 中线
		double midBoll = sum/this.n;

		// Formula : 
		// MID : MA(CLOSE,N);
		// UPPER: MID + P*STD(CLOSE,N);
		// LOWER: MID - P*STD(CLOSE,N);

		// STD -- Standard Deviation
		

		// Calculate上线和下线
        sum = 0;
         foreach (BarData c in initCandleList)
         {
             double close = c.Close;
			double tmp = close-midBoll;
            sum = sum + tmp * tmp;
            //Console.WriteLine(tmp + "  " + sum );
		}
         double p = 2;
         double tmp1 = sum / n;
		double d = p*  Math.Sqrt(tmp1);
		double upBoll = midBoll+(d);
		double lowBoll = midBoll-(d);


        boll[UPPER] = Double.Parse(String.Format("{0:00.000000}", upBoll)); ;
        boll[LOWER] = Double.Parse(String.Format("{0:00.000000}", lowBoll)); ; ;
        boll[MID] = Double.Parse(String.Format("{0:00.000000}", midBoll));

        ////Console.WriteLine( upBoll + "  " +midBoll+ "  " + lowBoll+ "  "+ d +  "   " +sum + "  "+ tmp1);

        candle.Boll = boll;

		return boll;
	}

    }

}
