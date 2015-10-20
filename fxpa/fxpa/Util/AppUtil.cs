using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace fxpa
{
    public class AppUtil
    {
        public static int PARSE_ERROR = -9999;

        public static DateTime GetLatestIntervalTime(DateTime lastTime, DateTime newTime, Interval interval)
        {
            if (newTime.Subtract(lastTime) < AppConst.AppTimeSpans[interval].Add(AppConst.AppTimeSpans[interval])
                && newTime.Subtract(lastTime) >= AppConst.AppTimeSpans[interval])
            {
                return lastTime;
            }
            else
            {
                while (newTime.Subtract(lastTime) < AppConst.AppTimeSpans[interval].Add(AppConst.AppTimeSpans[interval]))
                {
                    if (lastTime.CompareTo(newTime) >= 0)
                    {
                        lastTime = newTime;
                        break;
                    }
                    lastTime = lastTime.Add(AppConst.AppTimeSpans[interval]);
                }
                return lastTime;
            }
        }


        public static DateTime GetCurrentIntervalTime(DateTime lastTime, DateTime newTime, Interval interval)
        {
            if (newTime.Subtract(lastTime) < AppConst.AppTimeSpans[interval])
            {
                return lastTime;
            }
            else
            {
                while (newTime.Subtract(lastTime) >= AppConst.AppTimeSpans[interval])
                {
                    lastTime = lastTime.Add(AppConst.AppTimeSpans[interval]);
                }
                return lastTime;
            }
        }

        public static List<RealTimeData> ParseRtdMsg(string realTimeMsg)
        {
            List<RealTimeData> list = new List<RealTimeData>();
            try
            {
                string[] rmsgs = Regex.Split((string)realTimeMsg, @"\r\n", RegexOptions.IgnoreCase);

                foreach (string msg in rmsgs)
                {
                    if (msg.Trim().Length > 0)
                    {
                        try
                        {
                            string[] msgs = msg.Split(',');
                            //Symbol symbol = (Symbol)Enum.Parse(typeof(Symbol), msgs[0].Trim());     
                            Symbol symbol = ParseSymbol(msgs[0].Trim());
                            if (symbol == Symbol.UNKNOWN)
                            {
                                Console.WriteLine(" UNKNOWN  symbol " + msgs[0]);
                                continue;
                            }
                            DateTime dateTime = BarDataHelper.ParseCSVDateTime(msgs[1].Trim(), msgs[2].Trim());
                            double[] datas = new double[msgs.Length - 3];
                            for (int i = 3; i < msgs.Length; i++)
                            {
                                if (msgs[i].Trim().Length > 0)
                                    datas[i - 3] = Double.Parse(msgs[i]);
                            }
                            RealTimeData realTimeData = new RealTimeData(symbol, dateTime, datas);
                            list.Add(realTimeData);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                            continue;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return list;
        }


        public static Interval StringToInterval(string s)
        {
            int intv = 0;
      
             int.TryParse(s, out intv);
   
            switch (intv)
            {
                case 0:
                    return Interval.MIN1;
                case 5:
                    return Interval.MIN5;

                case 15:
                    return Interval.MIN15;

                case 30:
                    return Interval.MIN30;

                case 60:
                    return Interval.MIN60;

                case 120:
                    return Interval.MIN120;

                case 1440:
                    return Interval.DAY1;
                default:
                    return Interval.MIN5;
            }
        }

        public static string IntervalToString(Interval intv)
        {
            switch (intv)
            {
                case Interval.MIN5:
                    return "5";

                case Interval.MIN15:
                    return "15";

                case Interval.MIN30:
                    return "30";

                case Interval.MIN60:
                    return "60";

                case Interval.MIN120:
                    return "120";

                case Interval.DAY1:
                    return "1440";
                default:
                    return "5";
            }
        }

        public static bool PwdIsValid(string pwd)
        {
            bool chkRst = false;
            char[] chs = pwd.ToCharArray();

            char tmp = chs[0];

            foreach (char c in chs)
            {
                if (c != tmp)
                {
                    chkRst = true;
                    break;
                }
            }
            return chkRst;
        }

        public static string GetSignalChinese(int s)
        {
            if (s == 1)
                return "Up";
            else if (s == -1)

                return "Dn";
            else
                return "";
        }


        public static string GetSymbolChinese(string s)
        {
            string symbolCn = "";

            try
            {
                Symbol symbol = ParseSymbol(s);
                if (symbol != Symbol.UNKNOWN)
                {
                    symbolCn = AppConst.AppCnSymbols[symbol];
                }
            }
            catch (Exception e)
            {
            }
            return symbolCn;
        }

        public static Symbol ParseSymbol(string s)
        {
            Symbol symbol = Symbol.UNKNOWN;

            try
            {
                symbol = (Symbol)Enum.Parse(typeof(Symbol), s);
            }
            catch (Exception e)
            {
            }
            return symbol;
        }

        public static int ParseSignal(string s)
        {
            int r = PARSE_ERROR;
            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    r = int.Parse(s.Trim());
                }
                catch (Exception e)
                {
                }
                return r;
            }
            else
                return r;
        }

        public static int ParseSignalNum(string s)
        {
            int r = PARSE_ERROR;
            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    r = int.Parse(s.Trim());
                }
                catch (Exception e) { }
                return r;
            }
            else
                return r;
        }

        public static int StringToInt(string s)
        {
            int rst = PARSE_ERROR;
            try
            {
                rst = int.Parse(s.Trim());
            }
            catch (Exception e)
            {
            }
            return rst;
        }

        public static Protocol ParseProtocol(string s)
        {
            Protocol p = Protocol.UNKNOWN;

            try
            {
                if (s.StartsWith("C") || s.StartsWith("S")|| s.StartsWith("M") || s.StartsWith("K")  )
                {
                    p = (Protocol)Enum.Parse(typeof(Protocol), s.Trim());
                }
            }
            catch (Exception e) { }
            return p;
        }



        public static void CheckMarketIsOpen()
        {
            DateTime dateTime = AppSetting.START_TIME;

            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                AppContext.IsOpen = false;
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Monday && dateTime.Hour < 4)
            {
                AppContext.IsOpen = false;
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Saturday && dateTime.Hour > 4)
            {
                AppContext.IsOpen = false;
            }
            else
            {
                AppContext.IsOpen = true;
            }
        }


        public static DateTime GetStartTimeOfWeek(DateTime dateTime)
        {
            DateTime startTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 4, 0, 0);
            DayOfWeek firstDay = DayOfWeek.Monday;
            while (startTime.DayOfWeek != firstDay)
                startTime = startTime.AddDays(-1);
            return startTime;
        }

        public static DateTime GetStartTimeOfBizToday(DateTime dateTime)
        {
            DateTime startTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 4, 0, 0);
            return startTime;
        }

        public static DateTime GetStartTimeOfNormalDay(DateTime dateTime)
        {
            DateTime startTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
            return startTime;
        }

        public static PaymentType ParsePaymentType(string s)
        {
            try
            {
                switch (s)
                {
                    case "1":
                        return PaymentType.Point;
                    case "2":
                        return PaymentType.Month;
                    case "3":
                        return PaymentType.Year;
                    case "4":
                        return PaymentType.Free;
                    default:
                        return PaymentType.Unknow;
                }
            }
            catch (Exception e) { }
     return PaymentType.Unknow; 
        }

        public static void ParseRtd(ref List<BarData> datas, int index, string[] msgs, Protocol p)
        {
            while (index <= msgs.Length - 2)
            {
                try
                {
                    string[] rmsgs = msgs[index].Split(',');
                    if (rmsgs.Length >= 5)
                    {
                        BarData barData = new BarData();
                        barData.DateTime = DateTime.Parse(rmsgs[0]);
                        barData.Open = double.Parse(rmsgs[1], new System.Globalization.NumberFormatInfo());
                        barData.Close = double.Parse(rmsgs[2], new System.Globalization.NumberFormatInfo());
                        barData.Low = double.Parse(rmsgs[3], new System.Globalization.NumberFormatInfo());
                        barData.High = double.Parse(rmsgs[4], new System.Globalization.NumberFormatInfo());
                        barData.ExHigh = barData.High;
                        barData.ExLow = barData.Low;
                        if (p == Protocol.M0003_1)
                        {
                            barData.IsCompleted = true;
                        }
                        else if (rmsgs.Length >= 6)
                        {
                            barData.IsCompleted = (int.Parse(rmsgs[5], new System.Globalization.NumberFormatInfo()) == 0);
                        }
                        //Console.WriteLine(barData);
                        //LogUtil.Info(" candle data :::  " + barData);
                        datas.Add(barData);
                    }
                }
                catch (Exception e)
                {
                }
                index++;
            }
        }
    }
}
