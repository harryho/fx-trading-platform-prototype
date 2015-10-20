using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace fxpa
{
    class FileUtil
    {
        private static string key = "sdwv34go";
        private static string fkey = "Cz9nNT^z(p";


        static public bool SaveToFile(string path, BarData[] bars)
        {
            
            using (StreamWriter writer = new StreamWriter(path))
            {
                lock (writer)
                {
                    foreach (BarData bar in bars)
                    {
                        if (bar.HasDataValues)
                        {
                            DateTime start = System.DateTime.Now;

                            StringBuilder sb = new StringBuilder(bar.DateTime.Day.ToString());
                            sb.Append(".").Append(bar.DateTime.Month.ToString());
                            sb.Append(".").Append(bar.DateTime.Year.ToString());
                            sb.Append(",").Append(bar.DateTime.Hour.ToString("00")).Append(":").Append(bar.DateTime.Minute.ToString("00"));
                            sb.Append(",").Append(bar.Open.ToString());
                            sb.Append(",").Append(bar.Close.ToString());
                            sb.Append(",").Append(bar.Low.ToString());     
                            sb.Append(",").Append(bar.High.ToString());
                       
                            //sb.Append(",").Append(bar.Arrow.ToString());
                             writer.WriteLine(sb.ToString());
                            //}
                            DateTime end = System.DateTime.Now;
                            Console.WriteLine(" start  " + start );

                            Console.WriteLine(" end  " + end);
                            Console.WriteLine(" use @@@@@@@@@@@@@@@@  " + (end.Subtract(start) ) );
                        }
                    }
                    writer.Close();
                }
            }
            return true;
        }

        static public bool EncodeSaveFile(string path, BarData[] bars)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                //ICrypta crypta = SecurityService.GetCrypta();
                lock (writer)
                {
                    foreach (BarData bar in bars)
                    {
                        if (bar.HasDataValues)
                        {
                            DateTime start = System.DateTime.Now;

                            StringBuilder sb = new StringBuilder( bar.DateTime.Day.ToString());
                            sb.Append( ".").Append( bar.DateTime.Month.ToString() );
                            sb.Append(".").Append(bar.DateTime.Year.ToString());
                            sb.Append(",").Append(bar.DateTime.Hour.ToString("00")).Append(":").Append(bar.DateTime.Minute.ToString("00"));
                            
                            sb.Append(",").Append(bar.Open.ToString() );
                            sb.Append(",").Append(bar.Close.ToString());
                            sb.Append(",").Append( bar.Low.ToString() );
                            sb.Append(",").Append(bar.High.ToString());

                            //sb.Append(",").Append(bar.Arrow.ToString());

                         //   string endata =crypta.localEncrypt(sb.ToString());
                        //    writer.WriteLine(endata);
                            //}
                            DateTime end = System.DateTime.Now;
                            Console.WriteLine(" start  " + start);

                            Console.WriteLine(" end  " + end);
                            Console.WriteLine(" use @@@@@@@@@@@@@@@@  " + (end.Subtract(start)));
                        }
                    }
                    writer.Close();
                }
                //crypta = null;
            }
            return true;
        }

        static int c = 0;
        public static bool LoadFromFileCSV(string fileName, int startingRow, int rowsLimit, out List<BarData> datas)
        {
            datas = new List<BarData>();
            int i = 0;
         //   ICrypta crypta = SecurityService.GetCrypta();
            using (System.IO.TextReader tr = new System.IO.StreamReader(fileName))
            {
                try
                {
                    for (i = 0; i < startingRow + rowsLimit || rowsLimit == 0; i++)
                    {
                        string lineStr = tr.ReadLine();
                        if (lineStr == null)
                        {
                            break;
                        }

                        if (i >= startingRow)
                        {// Parse.     
                        //    lineStr = crypta.decrypt(lineStr);
                            // Parse 1 line - if the CSV format changes, modify this here as well
                            string[] lineSplit = Regex.Split(lineStr, ",");

                            DateTime time = BarDataHelper.ParseCSVDateTime(lineSplit[0], lineSplit[1]);
                            int signal = int.Parse(lineSplit[6], new System.Globalization.NumberFormatInfo());
                             datas.Add(new BarData(time, double.Parse(lineSplit[2], GeneralHelper.NumberFormatInfo), double.Parse(lineSplit[3], GeneralHelper.NumberFormatInfo), double.Parse(lineSplit[4], GeneralHelper.NumberFormatInfo), double.Parse(lineSplit[5], GeneralHelper.NumberFormatInfo), 0d, signal));
                        }
                    }
                    Console.WriteLine("ccccccccccccccccccccccccccc  " + (c++));
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                    datas.Clear();
                    return false;
                }
            }
            //crypta = null;

            return true;
        }

        public static bool LoadLatestRtd(string fileName, int startingRow, int rowsLimit, out List<BarData> datas, string key )
        {
            datas = new List<BarData>();
            int i = 0;

            using (System.IO.TextReader tr = new System.IO.StreamReader(fileName))
            {
                //ICrypta crypta = SecurityService.GetCrypta();
                try
                {
                    List<double> prices = new List<double>();
                    DateTime time =new DateTime();
                    for (i = 0; i < startingRow + rowsLimit || rowsLimit == 0; i++)
                    {
                        string lineStr = tr.ReadLine();
                        if (lineStr == null)
                        {
                            break;
                        }

                        if (i >= startingRow)
                        {// Parse.
                            //lineStr = crypta.decrypt(lineStr);
                            // Parse 1 line - if the CSV format changes, modify this here as well
                            string[] lineSplit = Regex.Split(lineStr, ",");

                            time = BarDataHelper.ParseCSVDateTime(lineSplit[1], lineSplit[2]);

                            prices.Add( Double.Parse( lineSplit[3]));

                        }
                    }
                    double max = double.MinValue;
                    double min = double.MaxValue;
                    foreach (double p in prices)
                    {
                        max = Math.Max(max, p);
                        min = Math.Min(min, p);
                    }
                    double open = prices.First();
                    double close = prices.Last();

                    datas.Add(new BarData(time, open, close, min, max, 0 ));
                    Console.WriteLine("ccccccccccccccccccccccccccc  " + (c++));
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                    datas.Clear();
                    return false;
                }
                //crypta = null;
            }
            return true;
        }


        public static bool LoadFromFile(string fileName, int startingRow, int rowsLimit, out List<BarData> datas)
        {
            datas = new List<BarData>();
            int i = 0;

            using (System.IO.TextReader tr = new System.IO.StreamReader(fileName))
            {
                try
                {
                    for (i = 0; i < startingRow + rowsLimit || rowsLimit == 0; i++)
                    {
                        string lineStr = tr.ReadLine();
                        if (lineStr == null)
                        {
                            break;
                        }

                        if (i >= startingRow)
                        {// Parse.
                            
                            // Parse 1 line - if the CSV format changes, modify this here as well
                            string[] lineSplit = Regex.Split(lineStr, ",");

                            DateTime time = BarDataHelper.ParseCSVDateTime(lineSplit[0], lineSplit[1]);

                            int signal = int.Parse(lineSplit[6], new System.Globalization.NumberFormatInfo());

                            datas.Add(new BarData(time, double.Parse(lineSplit[2], GeneralHelper.NumberFormatInfo), double.Parse(lineSplit[5], GeneralHelper.NumberFormatInfo), double.Parse(lineSplit[4], GeneralHelper.NumberFormatInfo), double.Parse(lineSplit[3], GeneralHelper.NumberFormatInfo), 0d, signal));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    datas.Clear();
                    return false;
                }
            }

            return true;
        }

        public static IFileWorker GetFileWorker()
        {
            return new FileWorker();// (SecurityService.GetCrypta());
        }
    }
}
