// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Text.RegularExpressions;

namespace fxpa
{
    public static class BarDataHelper
    {
        public enum FileFormat
        {
            CVSDefault
        }

        /// <summary>
        /// Get the after decimal point digits for those bars.
        /// </summary>
        /// <param name="barDatas"></param>
        /// <returns></returns>
        static public int GetPointDigits(IEnumerable<BarData> barDatas)
        {
            int result = 0;
            foreach (BarData data in barDatas)
            {
                string value = data.Close.ToString(GeneralHelper.NumberFormatInfo).Replace(".", ",");
                if (value.IndexOf(",") >= 0)
                {
                    result = Math.Max(result, value.Length - 1 - value.IndexOf(","));
                }
            }

            return result;
        }

        public static DateTime ParseCSVDateTime(string date, string time)
        {
            // Must convert to "," since "." is part of the syntax of regular expressions and causes wrong split.
            string[] dateParts = Regex.Split(date.Replace(".", ","), "," );
            string[] timeParts = Regex.Split(time, ":");

            if (dateParts.Length < 3 || timeParts.Length < 2)
            {
                throw new Exception("DateTime format error");
            }

            return new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[1]), int.Parse(dateParts[0]),
                int.Parse(timeParts[0]), int.Parse(timeParts[1]), 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">Name of the file to load from</param>
        /// <param name="startingRow">Row at which the loading starts</param>
        /// <param name="rowsLimit">How many rows to load maximum (pass 0 for no limit)</param>
         public   static bool LoadFromFileCSV(string fileName, int startingRow, int rowsLimit, out List<BarData> datas)
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

                            DateTime time = ParseCSVDateTime(lineSplit[0], lineSplit[1]);

                            double volume = double.Parse(lineSplit[6], new System.Globalization.NumberFormatInfo());
                            datas.Add(new BarData(time, double.Parse(lineSplit[2], GeneralHelper.NumberFormatInfo), double.Parse(lineSplit[5], GeneralHelper.NumberFormatInfo), double.Parse(lineSplit[4], GeneralHelper.NumberFormatInfo), double.Parse(lineSplit[3], GeneralHelper.NumberFormatInfo), volume));

                        }
                    }
                }
                catch (Exception e)
                {
                
                    //SystemMonitor.Warning("Failed to parse time in data file [" + fileName + "], line [" + i.ToString() + "], message [" + exception.Message + "].");
                    datas.Clear();
                    return false;
                }
            }

            return true;
        }

        static public bool LoadFromFile(FileFormat fileFormat, string path, int startingRow, int rowsLimit, out TimeSpan period, out List<BarData> datas)
        {
            datas = null;
            period = TimeSpan.Zero;
            if (fileFormat == FileFormat.CVSDefault)
            {
                LoadFromFileCSV(path, startingRow, rowsLimit, out datas);
            }
            else
            {
                return false;
            }


            // Find out what is the most oftenly occuring period - use it (for ex. consider weekends, holidays).
            SortedDictionary<double, int> periodOccurences = new SortedDictionary<double, int>();

            for (int i = 1; i < datas.Count; i++)
            {
                TimeSpan timeInterval = datas[i].DateTime - datas[i - 1].DateTime;
                if (periodOccurences.ContainsKey(timeInterval.TotalMinutes) == false)
                {
                    periodOccurences.Add(timeInterval.TotalMinutes, 1);
                }
                else
                {
                    periodOccurences[timeInterval.TotalMinutes]++;
                }
            }

            int totalOccurences = 0;
            foreach (double periodMinutes in periodOccurences.Keys)
            {// Keep going looking for the period with most occurences.
                if (periodOccurences[periodMinutes] > totalOccurences)
                {
                    period = TimeSpan.FromMinutes(periodMinutes);
                    totalOccurences = periodOccurences[periodMinutes];
                }
            }

            return true;
        }

        static public bool SaveToFile(FileFormat fileFormat, string path, BarData[] bars)
        {
            if (fileFormat == FileFormat.CVSDefault)
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    foreach (BarData bar in bars)
                    {
                        if (bar.HasDataValues)
                        {
                            writer.WriteLine(bar.DateTime.Day.ToString() + "." + bar.DateTime.Month.ToString() + "." + bar.DateTime.Year.ToString()
                                + "," + bar.DateTime.Hour.ToString("00") + ":" + bar.DateTime.Minute.ToString("00")
                                + "," + bar.Open.ToString() + "," + bar.High.ToString()
                                + "," + bar.Low.ToString() + "," + bar.Close.ToString()
                                + "," + bar.Volume.ToString());
                        }
                    }
                }
                return true;
            }

            return false;
        }
    }
}
