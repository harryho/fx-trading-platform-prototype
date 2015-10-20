using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
    public class RSI : Indicator
	{

		private String code;
		private int interval;
		// Initialize necessary candles 
		private int initCandleSize;
		// 是否已经初始化
		private bool isInitialized = false;
		private double initTotalGain;
		private double initTotalLoss;
		private bool isFirst = true;
		private double prevPrice;
		private double prevAvgGain;
		private double prevAvgLoss;
		private int n;


		private List<double> histGain = new List<double>();
		private List<double> histLoss = new List<double>();


		private List<BarData> initCandleList = new List<BarData>();




		public RSI(String code, int interval)
		{
			this.code = code;		
			this.interval = interval;
			this.initCandleSize = this.n = 14;
		}

        public RSI(String code, int interval, int initSize)
        {
            this.code = code;
            this.interval = interval;
            this.initCandleSize = this.n = initSize;
        }

		private void initial(List<BarData> candleList)
		{
			if (candleList == null || candleList.Count == 0)
			{
				return;
			}

			initTotalGain = 0D;
			initTotalLoss = 0D;

			prevPrice = candleList[0].Close;

			// 从第二个蜡烛开始处理差价
			for (int i = 1; i < candleList.Count; i++)
			{
				BarData candle = candleList[i];

				double diff = candle.Close - prevPrice;

				if (diff > 0)
				{
					initTotalGain = initTotalGain + Math.Abs(diff);
				}
				else
				{
					initTotalLoss = initTotalLoss + Math.Abs(diff);
				}
				prevPrice = candle.Close;
			}

			this.isInitialized = true;

		}


		public double handleFullCandle(ref BarData candle)
		{

			if (!isInitialized)
			{
				initCandleList.Add(candle);

				if (initCandleList.Count == this.initCandleSize)
				{
					initial(initCandleList);
					initCandleList = null;
				}

				return 0;
			}

			double diff = candle.Close - prevPrice;

			double avgGain = 0D;
			double avgLoss = 0D;

			double currentGain = 0D;
			double currentLoss = 0D;

			if (diff > 0)
			{
				currentGain = Math.Abs(diff);				
			}
			else
			{
				currentLoss = Math.Abs(diff);				
			}

			if (isFirst)
			{// 第一个RSI
				initTotalGain = initTotalGain + currentGain;
				initTotalLoss = initTotalLoss + currentLoss;

				// First Average Gain = Total of Gains during past 14 periods / 14
				avgGain = initTotalGain/n;
				// First Average Loss = Total of Losses during past 14 periods / 14
				avgLoss = initTotalLoss/n;
				isFirst = false;
			}
			else
			{
				// Average Gain = [(previous Average Gain) x 13 + current Gain] / 14
				//avgGain = prevAvgGain.multiply(new BigDecimal(n - 1)).add(
				//        currentGain).divide(new BigDecimal(n), SCALE, RMODE);
				avgGain = (prevAvgGain * (n - 1) + currentGain)/n;
				// Average Loss = [(previous Average Loss) x 13 + current Loss] / 14
				//avgLoss = prevAvgLoss.multiply(new BigDecimal(n - 1)).add(
				//        currentLoss).divide(new BigDecimal(n), SCALE, RMODE);
				avgLoss = (prevAvgLoss * (n - 1) + currentLoss)/n;
			}

			prevAvgGain = avgGain;
			prevAvgLoss = avgLoss;

			// RS = Average Gain / Average Loss
			double rs = avgGain/avgLoss;
			// RSI = 100 - 100/(1 + RS)
			Double rsi = 100-(100/(1+rs));


			rsi = Double.Parse(String.Format("{0:00.00000}", rsi));

			prevPrice = candle.Close;
			candle.Rsi = rsi;		

			return rsi;
		}


        public double handleFullCandle(ref BarData candle, bool isUpdate, int id)
        {

            if (!isInitialized)
            {
                initCandleList.Add(candle);

                if (initCandleList.Count == this.initCandleSize)
                {
                    initial(initCandleList);
                    initCandleList = null;
                }

                return 0;
            }

            double diff = candle.Close - prevPrice;

            double avgGain = 0D;
            double avgLoss = 0D;

            double currentGain = 0D;
            double currentLoss = 0D;

            if (diff > 0)
            {
                currentGain = Math.Abs(diff);

            }
            else
            {
                currentLoss = Math.Abs(diff);
            }

            if (isFirst)
            {// 第一个RSI
                initTotalGain = initTotalGain + currentGain;
                initTotalLoss = initTotalLoss + currentLoss;

                // First Average Gain = Total of Gains during past 14 periods / 14
                avgGain = initTotalGain / n;
                // First Average Loss = Total of Losses during past 14 periods / 14
                avgLoss = initTotalLoss / n;
                isFirst = false;
            }
            else
            {
                // Average Gain = [(previous Average Gain) x 13 + current Gain] / 14
                //avgGain = prevAvgGain.multiply(new BigDecimal(n - 1)).add(
                //        currentGain).divide(new BigDecimal(n), SCALE, RMODE);
                avgGain = (prevAvgGain * (n - 1) + currentGain) / n;
                // Average Loss = [(previous Average Loss) x 13 + current Loss] / 14
                //avgLoss = prevAvgLoss.multiply(new BigDecimal(n - 1)).add(
                //        currentLoss).divide(new BigDecimal(n), SCALE, RMODE);
                avgLoss = (prevAvgLoss * (n - 1) + currentLoss) / n;
            }
           

            // RS = Average Gain / Average Loss
            double rs = avgGain / avgLoss;
            // RSI = 100 - 100/(1 + RS)
            Double rsi = 100 - (100 / (1 + rs));


            rsi = Double.Parse(String.Format("{0:00.00000}", rsi));

            if (isUpdate)
            {
                prevAvgGain = avgGain;
                prevAvgLoss = avgLoss;
                prevPrice = candle.Close;
            }
           switch(id)
           {
               case 1:
            candle.Rsi = rsi;
            break;
               case 2:
            candle.Rsi2 = rsi;
            break;
               case 3:
            candle.Rsi3 = rsi;
            break;
               case 4:
            candle.Rsi4 = rsi;
            break;
        }
            return rsi;
        }

        protected override void OnCalculate(int startingIndex, int indexCount)
        {
            throw new NotImplementedException();
        }
    }
}
