using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace fxpa
{
    class ProviderHelper
    {
        static string SEPARATOR = "\\";
        static string LOCAL = "local";
        static string CDL= "cdl";
        static string SIG = "sig";
        static string RTD= "rtd";
        static string TMP = "tmp";
        static string DOT = ".";
        private static bool CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static string GetLocalPath( DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            sb.Append(SEPARATOR).Append(LOCAL);
           CreateDirectory(sb.ToString());
            return sb.ToString();
        }

        public static string GetLocalCdlFilePath(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            sb.Append(SEPARATOR).Append(LOCAL);
            CreateDirectory(sb.ToString());
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString()).Append(DOT).Append(CDL);
            return sb.ToString();
        }

        public static string GetLocalSigFilePath(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            sb.Append(SEPARATOR).Append(LOCAL);
            CreateDirectory(sb.ToString());
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString()).Append(DOT).Append(SIG);
            return sb.ToString();
        }

        public static string GetCdlFilePath(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            sb.Append(SEPARATOR).Append(CDL);
            CreateDirectory(sb.ToString());
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString()).Append(DOT).Append(CDL);
            return sb.ToString();
        }

        public static string GetRtdFilePath(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            sb.Append(SEPARATOR).Append(RTD);
            CreateDirectory(sb.ToString());
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString()).Append(DOT).Append(RTD);
            return sb.ToString();
        }

        public static string GetTmpPath(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            sb.Append(SEPARATOR).Append(TMP);
            CreateDirectory(sb.ToString());
            return sb.ToString();
        }

        public static string GetTmpCdlFilePath(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            sb.Append(SEPARATOR).Append(TMP);
            CreateDirectory(sb.ToString());
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString()).Append(DOT).Append(CDL);
            return sb.ToString();
        }

        public static string GetTmpSigFilePath(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            sb.Append(SEPARATOR).Append(TMP);
            CreateDirectory(sb.ToString());
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString()).Append(DOT).Append(SIG);
            return sb.ToString();
        }

        public static string GetDataFilePath(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            CreateDirectory(sb.ToString());
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            return sb.ToString();
        }

        public static string GetTmpDataFilePath(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            CreateDirectory(sb.ToString());
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString()).Append(DOT).Append(TMP);
            return sb.ToString();
        }

        public static void CleanInitDirs( DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            DeleteDirectory(sb.ToString() + SEPARATOR + CDL);
            DeleteDirectory(sb.ToString() + SEPARATOR + RTD);
            DeleteDirectory(sb.ToString() + SEPARATOR + LOCAL);      
        }

        public static void CleanTmpDirs(DataProvider provider)
        {
            StringBuilder sb = new StringBuilder(FxpaSource.folderPath);
            sb.Append(SEPARATOR).Append(provider.Symbol.ToString());
            sb.Append(SEPARATOR).Append(provider.CurrentTInterval.ToString());
            sb.Append(SEPARATOR).Append(TMP);
            DeleteDirectory(sb.ToString());
        }


        private static bool DeleteDirectory( string path){
            try
            {
                if(Directory.Exists(path) ) Directory.Delete(path, true );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
            return true;
        }
    }
}
