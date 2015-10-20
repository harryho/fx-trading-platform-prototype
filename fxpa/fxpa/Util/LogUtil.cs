using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using fxpa.Properties;

namespace fxpa
{
    class LogUtil
    {
        static string ERROR = "ERROR";
        static string INFO = "INFO";
        static string WARN = "WARN";
        static bool isValid;
        static bool isLogInfo;
        static bool isLogWarn;
        static bool isLogError;

        static string logPath = "";
        static System.IO.StreamWriter sw = null;
        public static int Mon = Convert.ToInt16(DateTime.Now.Month);
        public static int DAY = Convert.ToInt16(DateTime.Now.Day);
        public static int HOUR = Convert.ToInt16(DateTime.Now.Hour);
        public static int Min = Convert.ToInt16(DateTime.Now.Minute);
        public static int Sec = Convert.ToInt16(DateTime.Now.Second);
        public static int Millis = Convert.ToInt16(DateTime.Now.Millisecond);

        public static void  init( bool valid, bool info, bool warn,bool error){
            isValid = valid;
            isLogInfo = info;
            isLogWarn = warn;
            isLogError = error;

            if (valid)
            {
                string logFolder = GetLogFolder();
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }
                else
                {
                //    DateTime dt = Directory.GetCreationTime(FxSource.folderPath);
                //    if (dt.CompareTo(AppSetting.RELEASE_DATE) < 0)
                //    {
                //        Directory.Delete(logFolder, true);
                //        Directory.CreateDirectory(logFolder);
                //    }
                }
                logPath = logFolder + "\\client_log" + Mon + "." + DAY + "." + HOUR + "." + Min + "." + Sec + "."+Millis +".txt";
                if (File.Exists(logPath))
                    File.Delete(logPath);
                //File.Create(logPath);
                sw = System.IO.File.AppendText(logPath);
                sw.AutoFlush = true;
            }
            else
            {
                if (Directory.Exists(AppContext.APP_PATH + "\\logs"))
                    Directory.Delete(AppContext.APP_PATH + "\\logs", true);
            }
        }

        public static void Refresh()
        {
            Close();
            init(isValid, isLogInfo, isLogWarn, isLogError);
        }

        static string GetLogFolder()
        {
            //return (string)Settings.Default["Logs"];
            return AppContext.APP_PATH+"\\logs";
        }

        public static void Info(string message)
        {
            if (isValid && isLogInfo)
            {
                try
                {
                    string logLine =
                       System.String.Format(
                          "{0:G}: {1}, {2}.", System.DateTime.Now, INFO, message);
                    sw.WriteLine(logLine);
                }catch(Exception e )  
                {
                }
            }
        }

        public static void Error(string message)
        {
            if (isValid && isLogError)
            {
                try
                {
                    string logLine =
                       System.String.Format(
                      "{0:G}: {1}, {2}.", System.DateTime.Now, ERROR, message);
                    sw.WriteLine(logLine);
                }
                catch (Exception e)
                {
                }
            }
        }

        public static void Warn(string message)
        {
            if (isValid && isLogWarn)
            {
                try
                {
                    string logLine =
                       System.String.Format("{0:G}: {1}, {2}.", System.DateTime.Now, WARN, message);
                    sw.WriteLine(logLine);
                }
                catch (Exception e)
                {
                }
            }                
        }

        public static void Close()
        {
            try
            {
                if(sw!=null)
                sw.Close();
            }
            catch (Exception e) { }
        }
    }
}
