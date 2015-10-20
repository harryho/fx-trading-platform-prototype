using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;

namespace fxpa
{
    internal class FileWorker : IFileWorker 
    {
      //  ICrypta crypta=;
        static string NULL = "NULL";
        static string FORMAT = "yyyy-MM-dd HH:mm";
        internal FileWorker()//( ICrypta crypta )
        {
         //   this.crypta = crypta;
        }

       public bool SaveCdlFile(string fpath, BarData [] barDatas , bool isEncrypt ){
           try
           {
               using (StreamWriter writer = new StreamWriter(fpath, false))
               {
           
                        int i = 0;
                       DateTime start = System.DateTime.Now;
                       Console.WriteLine(" start  " + start);
                       foreach (BarData bar in barDatas)
                       {
                           if (bar.HasDataValues)
                           {
                               StringBuilder sb = new StringBuilder(bar.DateTime.ToString("yyyy-MM-dd HH:mm"));
                               sb.Append(",").Append(bar.Open.ToString());
                               sb.Append(",").Append(bar.Close.ToString());
                               sb.Append(",").Append(bar.Low.ToString());
                               sb.Append(",").Append(bar.High.ToString());
                               //string endata = crypta.localEncrypt(sb.ToString());
                               i++;
                               writer.WriteLine(sb.ToString());
                           }
                       }
                       DateTime end = System.DateTime.Now; Console.WriteLine(" end  " + end + "  count   " + i);
                       Console.WriteLine(" use @@@@@@@@@@@@@@@@  " + (end.Subtract(start)));
                       writer.Flush();    
                   writer.Close();                       
                   } 
           }
           catch (Exception e)
           {
               Console.WriteLine(" e.StackTrace    " + e.StackTrace);
               return false;
           }
           return true;
        }

       public bool SaveSigFile(string fpath, Signal[] signals, bool isEncrypt)
       {
           try
           {
               using (StreamWriter writer = new StreamWriter(fpath, false))
               {
                   int i = 0;
                   DateTime start = System.DateTime.Now;
                   Console.WriteLine(" start  " + start);
                   foreach (Signal signal in signals)
                   {
                       StringBuilder sb = new StringBuilder(signal.ActTime.ToString(FORMAT));
                       sb.Append(",").Append(signal.Arrow.ToString());
                       sb.Append(",").Append(signal.ActPrice.ToString());
                       sb.Append(",").Append(signal.GainTip);
                       sb.Append(",").Append(signal.GainTipPrice);
                       if (signal.GainTip != 0 && signal.GainTipPrice > 0) sb.Append(",").Append(signal.GainTipTime.ToString(FORMAT));
                       else sb.Append(",").Append(NULL);
                       sb.Append(",").Append(signal.StopGain);
                       sb.Append(",").Append(signal.StopGainPrice);
                       if (signal.StopGain != 0 && signal.StopGainPrice > 0) sb.Append(",").Append(signal.StopGainTime.ToString(FORMAT));
                       else sb.Append(",").Append(NULL);
                       sb.Append(",").Append(signal.StopLoss);
                       sb.Append(",").Append(signal.StopLossBidPrice);
                       if (signal.StopLoss != 0) sb.Append(",").Append(signal.StopLossTime.ToString(FORMAT));
                       else sb.Append(",").Append(NULL);
                       sb.Append(",").Append(signal.Profit);
                       sb.Append(",").Append(signal.ProfitPrice);
                       //if (signal.ProfitPrice > 0) sb.Append(",").Append(signal.ProfitTime.ToString(FORMAT));
                       //else sb.Append(",").Append(NULL);
                       //string endata = crypta.localEncrypt(sb.ToString());
                       //string endata = sb.ToString();
                       //Console.WriteLine(" endata    " + endata);
                       i++;
                       writer.WriteLine(sb.ToString());
                       //}
                   }
                   DateTime end = System.DateTime.Now; Console.WriteLine(" end  " + end + "  count   " + i);
                   Console.WriteLine(" use @@@@@@@@@@@@@@@@  " + (end.Subtract(start)));
                   writer.Close();
               }
           }
           catch (Exception e)
           {
               Console.WriteLine(" e.StackTrace    " + e.StackTrace);
               return false;
           }
           return true;
       }


       public bool LoadCdlFile(string fileName, int startingRow, int rowsLimit, out List<BarData> datas,  bool isDecrypt, bool isLocal)
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
                            //if (isDecrypt)
                            //{
                            //    if( !isLocal)
                            //        lineStr = crypta.decrypt(lineStr);
                            //    else
                            //        lineStr = crypta.localDecrypt(lineStr);
                            //}

                            try
                            {
                                string[] lineSplit = Regex.Split(lineStr, ",");
                                BarData barData = new BarData();
                                string date = lineSplit[0].Trim();
                                string time = lineSplit[1].Trim();
                                barData.DateTime = new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)),
                int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(2, 2)), int.Parse(time.Substring(4, 2)));    //DateTime.Parse(lineSplit[0]);//  BarDataHelper.ParseCSVDateTime(lineSplit[0], lineSplit[1]);
                                   barData.Open = double.Parse(lineSplit[2], new System.Globalization.NumberFormatInfo());
                                   barData.High = double.Parse(lineSplit[3], new System.Globalization.NumberFormatInfo());
                                   barData.Low = double.Parse(lineSplit[4], new System.Globalization.NumberFormatInfo());
                                   barData.Close = double.Parse(lineSplit[5], new System.Globalization.NumberFormatInfo());
                                   barData.ExLow = barData.Low;
                                   barData.ExHigh = barData.ExHigh;
                                   barData.IsCompleted = true;
                                   LogUtil.Info(" His File Loading :::: " + barData);
                                   datas.Add(barData);
                               
                            }catch(Exception e ){
                                LogUtil.Info(" His File Loading :::: " + e.StackTrace);
                            }                            
                        }
                    }
                    Console.WriteLine("ccccccccccccccccccccccccccc  ");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                    datas.Clear();
                    return false;
                }
            }

            return true;
        }

       public bool LoadSigFile(string fileName, int startingRow, int rowsLimit, out List<Signal> datas, bool isDecrypt, bool isLocal)
       {
           datas = new List<Signal>();

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
                           //if (isDecrypt)
                           //{
                           //    if (!isLocal)
                           //        lineStr = crypta.decrypt(lineStr);
                           //    else
                           //        lineStr = crypta.localDecrypt(lineStr);
                           //}

                           try
                           {
                               string[] lineSplit = Regex.Split(lineStr, ",");
                               Signal signal = new Signal();
                               signal.ActTime = DateTime.Parse(lineSplit[0]);
                               signal.Arrow = int.Parse(lineSplit[1]);
                               signal.ActPrice = double.Parse(lineSplit[2]);
                               signal.GainTip = int.Parse(lineSplit[3]);
                               signal.GainTipPrice = double.Parse(lineSplit[4]);
                               if (lineSplit[5] != NULL) signal.GainTipTime = DateTime.Parse(lineSplit[5]);
                               signal.StopGain = int.Parse(lineSplit[6]);
                               signal.StopGainPrice = double.Parse(lineSplit[7]);
                               if (lineSplit[8] != NULL) signal.StopGainTime = DateTime.Parse(lineSplit[8]);
                               signal.StopLoss = int.Parse(lineSplit[9]);
                               signal.StopLossPrice = double.Parse(lineSplit[10]);
                               if (lineSplit[11] != NULL)
                               {
                                   signal.StopLossTime = DateTime.Parse(lineSplit[11]);
                                   signal.StopLossBidPrice = double.Parse(lineSplit[13]);
                               }
                               signal.Profit = int.Parse(lineSplit[12]);
                               signal.ProfitPrice = double.Parse(lineSplit[13]);
                               if (lineSplit[14] != NULL) signal.ProfitTime = DateTime.Parse(lineSplit[14]);
                               LogUtil.Info(" Sig File Loading :::: " + signal);
                               datas.Add(signal);

                           }
                           catch (Exception e)
                           {
                               LogUtil.Info(" His File Loading :::: " + e.StackTrace);
                           }
                       }
                   }
                   Console.WriteLine("ccccccccccccccccccccccccccc  ");
               }
               catch (Exception exception)
               {
                   Console.WriteLine(exception.ToString());
                   datas.Clear();
                   return false;
               }
           }
           return true;
       }

       public bool LoadFile(string fileName, int startingRow, int rowsLimit, out List<RealTimeData> datas, bool isDecrypt)
        {
            datas = new List<RealTimeData>();

           int i = 0;

            using (System.IO.TextReader tr = new System.IO.StreamReader(fileName))
            {
                try
                {
                    DateTime time = new DateTime();
                    RealTimeData rtd = new RealTimeData();
                    for (i = 0; i < startingRow + rowsLimit || rowsLimit == 0; i++)
                    {
                        string lineStr = tr.ReadLine();
                        if (lineStr == null)
                        {
                            break;
                        }

                        if (i >= startingRow)
                        {// Parse.
                            //if (isDecrypt)
                            //{
                            //        lineStr = crypta.decrypt(lineStr);
                            //}
                            
                            string[] lineSplit = Regex.Split(lineStr, ",");

                            time = BarDataHelper.ParseCSVDateTime(lineSplit[1], lineSplit[2]);

                            double [] prices= new double[]{ Double.Parse(lineSplit[3]) };
                            rtd.symbol = (Symbol)Enum.Parse(typeof(Symbol), lineSplit[0]);
                            rtd.dateTime = time;
                            rtd.datas = prices;
                            datas.Add(rtd);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                    datas.Clear();
                    return false;
                }
            }
            return true;
        }


       public bool ZipFile(string file, string outPath, string password)
       {
           try
           {
               ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath)); // create zip stream
               if (password != null && password != String.Empty)
                   oZipStream.Password = password;
               oZipStream.SetLevel(9); // maximum compression
               ZipEntry oZipEntry;
               FileStream ostream;
               byte[] obuffer;

               string f = file;
               Console.WriteLine(" file " + file);
               Console.WriteLine(" file.LastIndexOf('\\'), file.Length-file.LastIndexOf('\\')-1)  rm " + file.Remove(0, file.LastIndexOf('\\') + 1));
               Console.WriteLine(" f " + file);
               if (!file.EndsWith(@"/")) // if a file ends with '/' its a directory
               {
                   oZipEntry = new ZipEntry(file.Remove(0, file.LastIndexOf('\\') + 1));
                   oZipStream.PutNextEntry(oZipEntry);
                   ostream = File.OpenRead(file);
                   obuffer = new byte[ostream.Length];
                   ostream.Read(obuffer, 0, obuffer.Length);
                   oZipStream.Write(obuffer, 0, obuffer.Length);
                   oZipStream.Flush();
                   ostream.Close();
               }

               oZipStream.Finish();
               oZipStream.Close();
               return true;
           }
           catch (Exception e)
           {
               Console.WriteLine(e.StackTrace);
               return false;
           }
       }

       public bool ZipFiles(string inputFolderPath, string outputPathAndFile, string password)
       {
           try
           {
               ArrayList ar = GenerateFileList(inputFolderPath); // generate file list
               int TrimLength = (Directory.GetParent(inputFolderPath)).ToString().Length;
               // find number of chars to remove 	// from orginal file path
               TrimLength += 1; //remove '\'
               FileStream ostream;
               byte[] obuffer;
               //string outPath = inputFolderPath + @"\" + outputPathAndFile;
               string outPath = outputPathAndFile;
               using (ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath)))
               {
                   // create zip stream
                   if (password != null && password != String.Empty)
                       oZipStream.Password = password;
                   oZipStream.SetLevel(9); // maximum compression
                   ZipEntry oZipEntry;
                   string file1 = "";
                   string file2 = "";
                   foreach (string file in ar) // for each file, generate a zipentry
                   {
                       oZipEntry = new ZipEntry(file.Remove(0, file.LastIndexOf('\\') + 1));
                       oZipStream.PutNextEntry(oZipEntry);
                       if (file1 == "") file1 = file; else file2 = file;
                       if (!file.EndsWith(@"/")) // if a file ends with '/' its a directory
                       {
                           ostream = File.OpenRead(file);
                           obuffer = new byte[ostream.Length];
                           ostream.Read(obuffer, 0, obuffer.Length);
                           oZipStream.Write(obuffer, 0, obuffer.Length);
                           oZipStream.Flush();
                           ostream.Flush();
                           ostream.Close();
                       }
                       oZipEntry = null;
                   }
                   oZipStream.Flush();
                   oZipStream.Finish();
                   oZipStream.Close();
               }
           }
           catch (Exception ex)
           {
               return false;
           }
           return true;
       }

        public bool UnZipFile(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile)
        {
            try
            {
                ZipInputStream s = new ZipInputStream(File.OpenRead(zipPathAndFile));
                if (password != null && password != String.Empty)
                    s.Password = password;
                ZipEntry theEntry;
                string tmpEntry = String.Empty;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = outputFolder;
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != "")
                    {
                        if(!Directory.Exists( directoryName))
                        Directory.CreateDirectory(directoryName);
                    }
                    if (fileName != String.Empty)
                    {
                        if (theEntry.Name.IndexOf(".ini") < 0)
                        {
                            string fullPath = directoryName + "\\" + theEntry.Name;
                            fullPath = fullPath.Replace("\\ ", "\\");
                            string fullDirPath = Path.GetDirectoryName(fullPath);
                            if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                            FileStream streamWriter = File.Create(fullPath);
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                         streamWriter.Close();
                        }
                    }
                }                
                s.Close();
                if (deleteZipFile)
                    File.Delete(zipPathAndFile);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public bool UnZipFiles(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile)
        {
            try
            {
                ZipInputStream s = new ZipInputStream(File.OpenRead(zipPathAndFile));
                if (password != null && password != String.Empty)
                    s.Password = password;
                ZipEntry theEntry;
                string tmpEntry = String.Empty;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = outputFolder;
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != "")
                    {
                        if (!Directory.Exists(directoryName))
                            Directory.CreateDirectory(directoryName);
                    }
                    if (fileName != String.Empty)
                    {
                        if (theEntry.Name.IndexOf(".ini") < 0)
                        {
                            string fullPath = directoryName + "\\" + theEntry.Name;
                            fullPath = fullPath.Replace("\\ ", "\\");
                            string fullDirPath = Path.GetDirectoryName(fullPath);
                            if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                            FileStream streamWriter = File.Create(fullPath);
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            streamWriter.Close();
                        }
                    }
                }
                s.Close();
                if (deleteZipFile)
                    File.Delete(zipPathAndFile);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


        private ArrayList GenerateFileList(string Dir)
        {
            ArrayList fils = new ArrayList();
            bool Empty = true;
            foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
            {
                fils.Add(file);
                Empty = false;
            }

            if (Empty)
            {
                if (Directory.GetDirectories(Dir).Length == 0)
                {
                    fils.Add(Dir + @"/");
                }
            }

            foreach (string dirs in Directory.GetDirectories(Dir)) // recursive
            {
                foreach (object obj in GenerateFileList(dirs))
                {
                    fils.Add(obj);
                }
            }
            return fils; // return file list
        }
    }

    //public class FileService
    //{
    //    public static IFileWorker  GetFileWorker()
    //    {
    //        return new FileWorker( SecurityService.GetCrypta());                   
    //    }
    //}
}
