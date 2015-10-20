using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * N day AR=Within N day(s)（H－C） accumulation  is divided byWithin N day(s)（O－L） accumulation <br/>
 * 其中，H is  day Highest Price，L is  day Lowest price，C is  day Close，N is customized Time parameter ， default parameter  day  setting is 26 day <br/>
 * N day BR=Within N day(s)（H－CY） accumulation  is divided byWithin N day(s)（CY－L） accumulation <br/>
 * 其中，H is  day Highest Price，L is  day Lowest price，CY is previous trade day 的Close，N is customized Time parameter ， default parameter  day  setting is 26 day 。<br/>
 * 
 * @author Harry
 * @mover herry
 * 
 */

namespace fxpa
{
	public class ARBR
	{

		private String code;

		private int interval;

		// Initialize necessary candles 
		private int initCandleSize;

		// 是否已经初始化
		private bool isInitialized = false;

		// 5 day Moving Average
		private double crMA5;

		private double crMA10;

		private double crMA20;

		// Last median price
		private double prevMiddlePrice;

		// historic CR indicator index，用来Calculate其MA值
		private List<Double> histCR = new List<Double>();

		// 前26 candles的P1
		List<Double> histP1 = new List<Double>();

		// 前26 candles的P2
		List<Double> histP2 = new List<Double>();

		List<BarData> initCandleList = new List<BarData>();

        string name = "";
        string forexCode="";
        int n = 26;

        string AR = "AR";
        string BR = "BR";
        public ARBR(String code, int interval)
        {
            this.forexCode = code;
            this.name ="AR";
            this.interval = interval;
            this.initCandleSize = this.n = 26;
        }

        public ARBR(String code, int interval, int initSize )
        {
            this.forexCode = code;
            this.name = "AR";
            this.interval = interval;
            this.initCandleSize = this.n = initSize;
        }
	


	public double handleFullCandle(ref  BarData candle , int field) {

        //IndValue contrainer = new IndValue(candle);

        double arValue = 0;

		if (initCandleList.Count < this.n) {
			initCandleList.Add(candle);
            return arValue;
		}
		initCandleList.RemoveAt(0);
		initCandleList.Add(candle);

		// Within N day(s)（H－C） accumulation
		double sumOfHC = 0;
		// Within N day(s)（O－L） accumulation
		double sumOfOL = 0;

		// Within N day(s)（H－CY） accumulation
		double sumOfHCY = 0;
		// Within N day(s)（CY－L） accumulation
		double sumOfCYL = 0;
        //Console.WriteLine("  ARBR initCandleList  count  " + initCandleList.Count );
		BarData prevCandle = new BarData();
		foreach (BarData c in initCandleList) {

			// calculate AR
			double tmp =c.High-c.Open;
            sumOfHC = sumOfHC + tmp;

			tmp = c.Open-c.Low;
			sumOfOL = sumOfOL+tmp;

			// calculate BR
			tmp = c.High-prevCandle.Close;
			sumOfHCY = sumOfHCY+(tmp);

			tmp = prevCandle.Close-c.Low;
			sumOfCYL = sumOfCYL+tmp;

			prevCandle = c;
            //Console.WriteLine("  ARBR sumOfHO " + sumOfHO + "  sumOfOL " + sumOfOL);
		}


        double ar = sumOfHC / sumOfOL * 100;
        double br = sumOfHCY / sumOfCYL * 100;

        double ardouble = Double.Parse(String.Format("{0:00.00000}", ar));

        double brdouble = Double.Parse(String.Format("{0:00.00000}", br));
        Dictionary<string, double> additionalIndValue = new Dictionary<string, double>();
        additionalIndValue.Add(AR, ardouble);
        additionalIndValue.Add(BR, brdouble);

        arValue = ardouble;
  

        switch (field)
        {
            case 1:
                candle.Ar = ardouble;
                candle.Br = brdouble;
                break;
            case 2:
                candle.Ar2 = ardouble;
                break;
               }


		return arValue;
	}

	public double handleFragCandle(BarData candle) {

        double arValue = 0;

		// cp the previous candle list
		List<BarData> cpList = new List<BarData>();
		cpList.AddRange(initCandleList);
		cpList.RemoveAt(0);
		cpList.Add(candle);

		// Within N day(s)（H－C） accumulation
        double sumOfHC = 0;
		// Within N day(s)（O－L） accumulation
		double sumOfOL = 0;

		// Within N day(s)（H－CY） accumulation
		double sumOfHCY = 0;
		// Within N day(s)（CY－L） accumulation
		double sumOfCYL = 0;

		BarData prevCandle = new BarData();
		foreach (BarData c in cpList) {

			// calculate AR
            double tmp = c.High - c.Open;
            sumOfHC = sumOfHC + (tmp);

			tmp = c.Open-c
					.Low;
			sumOfOL = sumOfOL+(tmp);

			// calculate BR
			tmp = c.High-prevCandle.Close;
			sumOfHCY = sumOfHCY+(tmp);

			tmp = prevCandle.Close-c.Low;
			sumOfCYL = sumOfCYL+(tmp);

			prevCandle = c;
		}

        double ar = sumOfHC / sumOfOL * 100;
		double br = sumOfHCY/sumOfCYL*100;

        double ardouble = Double.Parse(String.Format("{0:00.00000}", ar));

        double brdouble = Double.Parse(String.Format("{0:00.00000}", br));

		

		Dictionary<string,double> additionalIndValue =new Dictionary<string,double>();
		additionalIndValue.Add(AR, ardouble);
		additionalIndValue.Add(BR, brdouble);
        candle.Ar = ardouble;
        candle.Br = brdouble;
		
        return arValue;
	}
	}

}
