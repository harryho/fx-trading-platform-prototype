// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;

namespace fxpa
{
    public class GeneralHelper
    {
        public delegate void DefaultDelegate();
        public delegate void GenericDelegate<TParameterOne>(TParameterOne parameter1);
        public delegate void GenericDelegate<TParameterOne, TParameterTwo>(TParameterOne parameter1, TParameterTwo parameter2);
        public delegate void GenericDelegate<TParameterOne, TParameterTwo, TParameterThree>(TParameterOne parameter1, TParameterTwo parameter2, TParameterThree parameter3);

        public delegate TReturnType GenericReturnDelegate<TReturnType, TParameterOne>(TParameterOne parameter1);
        public delegate TReturnType GenericReturnDelegate<TReturnType, TParameterOne, TParameterTwo>(TParameterOne parameter1, TParameterTwo parameter2);

        #region Async Helper
        /// <summary>
        /// Those help the async helper methods.
        /// </summary>

        public static void SafeEventRaise(Delegate d, params object[] args)
        {
            if (d != null)
            {
                // Synchronous.
                d.DynamicInvoke(args);
            }
        }

        /// <summary>
        /// Could use the default thread pool as well, but that does have a ~0.5sec delay when starting.
        /// </summary>
        static ThreadPoolEx _threadPoolEx = new ThreadPoolEx();
        public static void FireAndForget(Delegate d, params object[] args)
        {
            _threadPoolEx.Queue(d, args);
        }

        #endregion

        #region Time and Number Helpers

        static public NumberFormatInfo NumberFormatInfo = new System.Globalization.CultureInfo("en-US", false).NumberFormat;

        static readonly DateTime Time1970 = new DateTime(1970, 1, 1);
        public static DateTime GenerateDateTimeSecondsFrom1970(Int64 secondsFrom1970)
        {
            return Time1970.AddSeconds(secondsFrom1970);
        }

        public static Int64 GenerateSecondsDateTimeFrom1970(DateTime time)
        {
            TimeSpan span = time - Time1970;
            return (long)span.TotalSeconds;
        }


        /// <summary>
        /// This allows parsing with taking TimeZones into the string in consideration.
        /// </summary>
        public static DateTime ParseDateTimeAdvanced(string dateTime)
        {
            DateTime result;

            // The dateTime parameter is either local (no time
            // zone specified), or it has a time zone.
            // Use the regex to examine the last part of the
            // dateTime string and determine which category it falls into.

            Match m = Regex.Match(dateTime.Trim(), @"(\b\w{3,4}|[+-]?\d{4})$");

            if (m.Length == 0)
            {
                // Local date (no time zone).
                result = DateTime.Parse(dateTime);
            }
            else
            // Date w/ time zone. m.Value holds the time zone info
            // (either a time zone name (e.g. PST), or the
            // numeric offset (e.g. -0800).
            result = ConvertToLocalDateTime(dateTime, m.Value);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        static public string GetShortDateTime(DateTime dateTime)
        {
            return dateTime.Day.ToString("00") + "/" + dateTime.Month + "/" + dateTime.Year + " " + dateTime.Hour.ToString("00") + ":" + dateTime.Minute.ToString("00");
        }

        private static DateTime ConvertToLocalDateTime(string dateTime, string timeZoneId)
        {
            // Strip the time zone ID from the end of the dateTime string.
            dateTime = dateTime.Replace(timeZoneId, "").Trim();

            // Convert the timeZoneId to a TimeSpan.
            // (Leading + signs aren't allowed in the TimeSpan.Parse
            // parameter, although leading - signs are.
            // The purpose of the [+]*? at the beginning of the
            // regex is to account for, and ignore, any leading + sign).

            string ts = Regex.Replace(GetTimeZoneOffset(timeZoneId),
            @"^[+]*?(?<hours>[-]?\d\d)(?<minutes>\d\d)$",
            "${hours}:${minutes}:00");
            TimeSpan timeZoneOffset = TimeSpan.Parse(ts);

            TimeSpan localUtcOffset =
            TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now );

            // Get the absolute time difference between the given
            // datetime's time zone and the local datetime's time zone.

            TimeSpan absoluteOffset = timeZoneOffset - localUtcOffset;
            absoluteOffset = absoluteOffset.Duration();

            // Now that the absolute time difference is known,
            // determine whether to add or subtract it from the
            // given dateTime, and then return the result.

            if (timeZoneOffset < localUtcOffset)
            {
                return DateTime.Parse(dateTime) + absoluteOffset;
            }
            else
            {
                return DateTime.Parse(dateTime) - absoluteOffset;
            }
        }

        /// <summary>
        /// Converts a time zone (e.g. "PST") to an offset string
        /// (e.g. "-0700").
        /// </summary>
        /// <param name="tz">The time zone to convert.</param>
        /// <returns>The offset value (e.g. -0700).</returns>
        private static string GetTimeZoneOffset(string tz)
        {
            // If the time zone is already in number format,
            // just return it.
            if (Regex.IsMatch(tz, @"^[+-]?\d{4}$"))
            return tz;

            string result = string.Empty;

            foreach (string[] sa in TimeZones)
            {
            if (sa[0].ToUpper() == tz)
            {
            result = sa[1];
            break;
            }
            }

            return result;
        }

        /// <summary>
        /// An array of time zones
        /// (e.g. new string[] {"PST", "-0700", "(US) Pacific
        /// Standard"}).
        /// </summary>
        private static string[][] TimeZones = new string[][] {
            new string[] {"ACDT", "+1030", "Australian Central Daylight"},
            new string[] {"ACST", "+0930", "Australian Central Standard"},
            new string[] {"ADT", "-0300", "(US) Atlantic Daylight"},
            new string[] {"AEDT", "+1100", "Australian East Daylight"},
            new string[] {"AEST", "+1000", "Australian East Standard"},
            new string[] {"AHDT", "-0900", ""},
            new string[] {"AHST", "-1000", ""},
            new string[] {"AST", "-0400", "(US) Atlantic Standard"},
            new string[] {"AT", "-0200", "Azores"},
            new string[] {"AWDT", "+0900", "Australian West Daylight"},
            new string[] {"AWST", "+0800", "Australian West Standard"},
            new string[] {"BAT", "+0300", "Bhagdad"},
            new string[] {"BDST", "+0200", "British Double Summer"},
            new string[] {"BET", "-1100", "Bering Standard"},
            new string[] {"BST", "-0300", "Brazil Standard"},
            new string[] {"BT", "+0300", "Baghdad"},
            new string[] {"BZT2", "-0300", "Brazil Zone 2"},
            new string[] {"CADT", "+1030", "Central Australian Daylight"},
            new string[] {"CAST", "+0930", "Central Australian Standard"},
            new string[] {"CAT", "-1000", "Central Alaska"},
            new string[] {"CCT", "+0800", "China Coast"},
            new string[] {"CDT", "-0500", "(US) Central Daylight"},
            new string[] {"CED", "+0200", "Central European Daylight"},
            new string[] {"CET", "+0100", "Central European"},
            new string[] {"CST", "-0600", "(US) Central Standard"},
            new string[] {"CENTRAL", "-0600", "(US) Central Standard"},
            new string[] {"EAST", "+1000", "Eastern Australian Standard"},
            new string[] {"EDT", "-0400", "(US) Eastern Daylight"},
            new string[] {"EED", "+0300", "Eastern European Daylight"},
            new string[] {"EET", "+0200", "Eastern Europe"},
            new string[] {"EEST", "+0300", "Eastern Europe Summer"},
            new string[] {"EST", "-0500", "(US) Eastern Standard"},
            new string[] {"EASTERN", "-0500", "(US) Eastern Standard"},
            new string[] {"FST", "+0200", "French Summer"},
            new string[] {"FWT", "+0100", "French Winter"},
            new string[] {"GMT", "-0000", "Greenwich Mean"},
            new string[] {"GST", "+1000", "Guam Standard"},
            new string[] {"HDT", "-0900", "Hawaii Daylight"},
            new string[] {"HST", "-1000", "Hawaii Standard"},
            new string[] {"IDLE", "+1200", "Internation Date Line East"},
            new string[] {"IDLW", "-1200", "Internation Date Line West"},
            new string[] {"IST", "+0530", "Indian Standard"},
            new string[] {"IT", "+0330", "Iran"},
            new string[] {"JST", "+0900", "Japan Standard"},
            new string[] {"JT", "+0700", "Java"},
            new string[] {"MDT", "-0600", "(US) Mountain Daylight"},
            new string[] {"MED", "+0200", "Middle European Daylight"},
            new string[] {"MET", "+0100", "Middle European"},
            new string[] {"MEST", "+0200", "Middle European Summer"},
            new string[] {"MEWT", "+0100", "Middle European Winter"},
            new string[] {"MST", "-0700", "(US) Mountain Standard"},
            new string[] {"MOUNTAIN", "-0700", "(US) Mountain Standard"},
            new string[] {"MT", "+0800", "Moluccas"},
            new string[] {"NDT", "-0230", "Newfoundland Daylight"},
            new string[] {"NFT", "-0330", "Newfoundland"},
            new string[] {"NT", "-1100", "Nome"},
            new string[] {"NST", "+0630", "North Sumatra"},
            new string[] {"NZ", "+1100", "New Zealand "},
            new string[] {"NZST", "+1200", "New Zealand Standard"},
            new string[] {"NZDT", "+1300", "New Zealand Daylight "},
            new string[] {"NZT", "+1200", "New Zealand"},
            new string[] {"PDT", "-0700", "(US) Pacific Daylight"},
            new string[] {"PST", "-0800", "(US) Pacific Standard"},
            new string[] {"PACIFIC", "-0800", "(US) Pacific Standard"},
            new string[] {"ROK", "+0900", "Republic of Korea"},
            new string[] {"SAD", "+1000", "South Australia Daylight"},
            new string[] {"SAST", "+0900", "South Australia Standard"},
            new string[] {"SAT", "+0900", "South Australia Standard"},
            new string[] {"SDT", "+1000", "South Australia Daylight"},
            new string[] {"SST", "+0200", "Swedish Summer"},
            new string[] {"SWT", "+0100", "Swedish Winter"},
            new string[] {"USZ3", "+0400", "USSR Zone 3"},
            new string[] {"USZ4", "+0500", "USSR Zone 4"},
            new string[] {"USZ5", "+0600", "USSR Zone 5"},
            new string[] {"USZ6", "+0700", "USSR Zone 6"},
            new string[] {"UT", "-0000", "Universal Coordinated"},
            new string[] {"UTC", "-0000", "Universal Coordinated"},
            new string[] {"UZ10", "+1100", "USSR Zone 10"},
            new string[] {"WAT", "-0100", "West Africa"},
            new string[] {"WET", "-0000", "West European"},
            new string[] {"WST", "+0800", "West Australian Standard"},
            new string[] {"YDT", "-0800", "Yukon Daylight"},
            new string[] {"YST", "-0900", "Yukon Standard"},
            new string[] {"ZP4", "+0400", "USSR Zone 3"},
            new string[] {"ZP5", "+0500", "USSR Zone 4"},
            new string[] {"ZP6", "+0600", "USSR Zone 5"}
        };

        #endregion

        public static string ApplicationVersion
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }

        public static string RepairHTMLString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Replace special symbols.
            input = input.Replace("&lt;", "<");
            input = input.Replace("&gt;", ">");
            input = input.Replace("&amp;", "&");
            input = input.Replace("&quot;", "\"");
            input = input.Replace("&reg;", "®");
            input = input.Replace("&copy;", "©");
            input = input.Replace("&nbsp;", " ");

            input = input.Replace("&cent;", "¢");
            input = input.Replace("&pound;", "£");
            input = input.Replace("&yen;", "¥");

            // Remove tags.
            input = Regex.Replace(input, "<(.|\n)*?((>(.|\n)*?</(.|\n)*?>)|(/>))", "");

            return input;
        }

        static public Stack<T> CloneStack<T>(Stack<T> inputStack)
        {
            Stack<T> result = new Stack<T>();

            T[] array = inputStack.ToArray();
            for (int i = array.Length - 1; i >= 0; i--)
            {
                result.Push(array[i]);
            }
            return result;
        }

        static public bool CompareArrays(int[] array1, int[] array2)
        {
            System.Diagnostics.Debug.Assert(array1.Length == array2.Length);
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        static public double[] Sort(IEnumerable<double> inputArray, bool reverseOrder)
        {
            List<double> doubles = new List<double>(inputArray);
            doubles.Sort();
            if (reverseOrder)
            {
                doubles.Reverse();
            }
            return doubles.ToArray();
        }

        static public void GetMinMax(IEnumerable<double> values, out double min, out double max)
        {
            min = double.MaxValue;
            max = double.MinValue;

            foreach(double value in values)
            {
                min = Math.Min(min, value);
                max = Math.Max(max, value);
            }
        }
        
        static public float[] IntsToFloats(int[] values)
        {
            float[] result = new float[values.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = values[i];
            }
            return result;
        }

        static public double[] IntsToDoubles(int[] values)
        {
            double[] result = new double[values.Length];
            for (int i = 0; i<result.Length; i++)
            {
                result[i] = values[i];
            }
            return result;
        }

        static public float[] DoublesToFloats(double[] values)
        {
            float[] result = new float[values.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)values[i];
            }
            return result;
        }

        static public double[] FloatsToDoubles(float[] values)
        {
            double[] result = new double[values.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = values[i];
            }
            return result;
        }

        static public string IntsToString(int[] values, string separator)
        {
            StringBuilder sb = new StringBuilder(values.Length);
            foreach (int value in values)
            {
                sb.Append(value);
                sb.Append(separator);
            }
            // Remove last separator
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - separator.Length, separator.Length);
            }
            return sb.ToString();
        }

        static public TDataType[] EnumerableToArray<TDataType>(IEnumerable<TDataType> enumerable)
        {
            List<TDataType> list = new List<TDataType>();
            foreach (TDataType value in enumerable)
            {
                list.Add(value);
            }
            return list.ToArray();
        }

        /// <summary>
        /// This is used to verify for logical usage errors
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        static public void Verify(bool condition, string message)
        {
            if (condition == false)
            {
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Lock when accessing. Watch out for potential dead locks, use in atomic operations only.
        /// </summary>
        static Random _random = new Random();
        public static int Random(int min, int max)
        {
            lock (_random)
            {
                return _random.Next(min, max);
            }
        }

        public static double Random(double min, double max)
        {
            lock (_random)
            {
                double randomValue = _random.NextDouble();
                double difference = max - min;

                return min + difference * randomValue;
            }
        }

        public static void Swap<TType>(ref TType value1, ref TType value2)
        {
            TType cache = value1;
            value1 = value2;
            value2 = cache;
        }

        /// <summary>
        /// max is Exclusive.
        /// </summary>
        public static int Random(int max)
        {
            lock (_random)
            {
                return _random.Next(max);
            }
        }

        public static string MapFilePathToExecutingDirectory(string path)
        {
            string assemblyLaunch = System.Reflection.Assembly.GetEntryAssembly().Location;
            assemblyLaunch = assemblyLaunch.Remove(assemblyLaunch.LastIndexOf('\\') + 1);
            return assemblyLaunch+path;
        }

        //public static string MapFilePathToResourceDirectory(string fileName)
        //{// TODO : move this to simulation, like done in the TraderSimulator
        //    return MapFilePathToExecutingDirectory(@"..\..\..\Resources\"+fileName);
        //}

        public static string SeparateCapitalLetters(string input)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (c.ToString() == c.ToString().ToUpper())
                {
                    result.Append(" ");
                }
                result.Append(c);
            }

            return result.ToString();
        }

        protected static bool ContainsString(string name, string[] names)
        {
            foreach (string n in names)
            {
                if (n == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GenerateUniqueName(string initialName, string[] existingNames)
        {
            if (ContainsString(initialName, existingNames))
            {// We need to generate a new name.
                string tempName = initialName;
                for (int i = 0; i < 9999; i++)
                {
                    if (ContainsString(tempName + i.ToString(), existingNames) == false)
                    {// Ok, found unique one.
                        tempName += i.ToString();
                        break;
                    }
                }
                initialName = tempName;
            }
            return initialName;
        }
    }
}
