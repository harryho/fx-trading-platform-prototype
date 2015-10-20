using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
	public class CR
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

		public CR(String code, int interval)
		{
			this.code = code;
			this.interval = interval;		
			// 26 candles
			this.initCandleSize = 26;
		}

	
		/**
		 * 
		 * @param list
		 */
		private void initial(List<BarData> list)
		{
			
			BarData prevCandle = list[0];
			prevMiddlePrice = this.getMiddlePrice(prevCandle);

			for (int i = 1, k = list.Count; i < k; i++)
			{
				BarData c = list[i];
				double p1 = c.High - prevMiddlePrice;
				if (p1 > 0)
				{
					histP1.Add(p1);
				}
				else
				{
					histP1.Add(0);
				}
				

				double p2 = prevMiddlePrice - c.Low;
				if (p2 > 0)
				{
					histP2.Add(p2);
				}
				else
				{
					histP2.Add(0);
				}
				
			
				// Calculate当前的median price，为下一根做准备
				prevMiddlePrice = this.getMiddlePrice(c);
			}
			this.isInitialized = true;
		}

		/**
		 * 
		 */

		public double handleFullCandle(ref BarData candle)
		{

			if (!isInitialized)
			{
				initCandleList.Add(candle);		
				if (initCandleList.Count == this.initCandleSize)
				{
					this.initial(initCandleList);
					//initCandleList = null;
				}
				return 0;
			}


			double p1 = candle.High - prevMiddlePrice;
			if(histP1.Count >= this.initCandleSize){
				histP1.RemoveAt(0);
			}

			if (p1 > 0)
			{
				histP1.Add(p1);
			}
			else
			{
				histP1.Add(0);
			}
			
			double sumP1 = this.getSum(histP1);

			double p2 = prevMiddlePrice - candle.Low;
			if (histP2.Count >= this.initCandleSize)
			{
				histP2.RemoveAt(0);
			}
				
			if (p2 > 0)
			{
				histP2.Add(p2);
			}
			else
			{
				histP2.Add(0);
			}
			double sumP2 = this.getSum(histP2);

			double cR = 0D;
			if (sumP2 != 0)
			{
				cR = sumP1 * 100 / sumP2;
			}

			cR = Double.Parse(String.Format("{0:00.00000}", cR));

			// 保存最近的20个值
			if (histCR.Count >= 20)
			{
				histCR.Remove(0);
			}
			histCR.Add(cR);

			// Calculate其平均值
			crMA5 = getAvg(histCR, 5);
			crMA10 = getAvg(histCR, 10);
			crMA20 = getAvg(histCR, 20);

			// 为下根做准备
			prevMiddlePrice = this.getMiddlePrice(candle);

			candle.Cr = crMA5;

			return cR;
		}



		/**
		 * 拿最近的count根的平均值
		 * 
		 * @param cRList
		 * @param count
		 * @return
		 */
		private double getAvg(List<Double> cRList, int count)
		{
			double sum = 0D;
			if (cRList.Count ==0)
			{
				return sum;
			}
			//
			int c = 1;
			for (int k = cRList.Count, i = k - 1; i >= 0; i--)
			{
				sum = sum + cRList[i];
				if (c == count)
				{
					break;
				}
				c++;
			}
			return sum / count;
		}

		/**
		 * 
		 * @param hist
		 * @return
		 */
		private double getSum(List<Double> hist)
		{
			double sum = 0D;
			if (hist != null)
			{
				for (int i = 0, k = hist.Count; i < k; i++)
				{
					sum = sum + hist[i];
				}
			}
			return sum;
		}

		/**
		 * get the middle price,there are 4 kind of arithmetic.
		 * 
		 * 1、M=(2C+H+L)÷4 2、M=(C+H+L+O)÷4 3、M=(C+H+L)÷3 4、M=(H+L)÷2 we are now using
		 * the second one : M=(C+H+L+O)÷4
		 * 
		 * @param candle
		 * @return
		 */
		private double getMiddlePrice(BarData candle)
		{
			double middle = candle.Close;
			middle = middle + candle.High + candle.Low + candle.Open;
			middle = middle / 4;
			return middle;
		}

		public double getCrMA5()
		{
			return crMA5;
		}

		public double getCrMA10()
		{
			return crMA10;
		}

		public double getCrMA20()
		{
			return crMA20;
		}
	}

}
