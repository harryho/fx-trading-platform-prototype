using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

using System.Text.RegularExpressions;


namespace fxpa
{

    public delegate void DealLoginEvent(Form form1);

    public delegate void DealSignalEvent(ListView listview,String[] strData);
    /// <summary> 
    /// TestTcpClient 的摘要说明。 
    /// </summary> 
    public class AppClient:Client
    {
        static AppConnector appConn = new AppConnector(new Coder(Coder.EncodingMothord.UTF8));

        List<IMsgHandler> handlerList = new List<IMsgHandler>();

        private static Client client = new AppClient();

        AppClient()
        {


        }

        public static Client GetInstance
        {
            get { return client; }
        }

        public static bool IsReady
        {
            get { return appConn.IsReady; }
        }

        public static bool IsConnected
        {
            get{ return appConn.IsConnected;     }
        }

       static protected bool hasRecvEvent;

       public static bool IsStopAttempt =false ;

        public static void Connect (string ip, int port )
        {
            try
            {
                tcpServerIp = ip;
                tcpSeverPort= port;
                appConn.TimeOut = false;
                //接收TCPServer信息字符串
                if (!hasRecvEvent)
                {
                    appConn.ReceivedTCPDatagram += new NetEvent(RecvTCPDatagram);
                    hasRecvEvent = true;
                }
                
                appConn.Resovlver = new DatagramResolver("\r\n");
                appConn.Connect( tcpServerIp, tcpSeverPort);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                appConn.Close();
                appConn.TimeOut = true;
            }
        }
        static int ret = 0;
        public static void Start()
        {
            try
            {
                //while (string.IsNullOrEmpty(appConn.Key) || !appConn.IsConnected)
                //{
                //    if (appConn.TimeOut)
                //    {
                //         if (appConn.IsConnected)                                                    
                //        { Console.WriteLine((ret++) + "  close session and reconnect   ");
                //            appConn.Close();
                //        }
                //        Console.WriteLine((ret++) + "  out  time  ");
                //        Connect(tcpServerIp, tcpSeverPort);
                      
                //    }
                //    if (IsStopAttempt )
                //    {
                //       // MessageBox.Show( "Server is busy，Please try again later!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        IsStopAttempt = false;
                //        break;
                //    }
                //    Thread.Sleep(500);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine((ret * 500) + "  ms   ");
                Console.WriteLine(e.StackTrace);
            }
        }

        public override bool Send(object msg)
        {
            return   appConn.Send((string)msg);
        }


        //接收TCPServer信息字符串
        public static void RecvTCPDatagram(object sender, NetEventArgs e)
        {

            String msg1 = e.Client.Datagram;
            //HERRYConsole.WriteLine(msg1);
            string[] sResult = DataSplit(e.Client.Datagram);

            if (sResult != null && sResult.Length > 2)
            {
                Console.WriteLine(" TTTTTTTTT " + sResult[1]);        
                Protocol p = AppUtil.ParseProtocol(sResult[1]);
                if (p != Protocol.UNKNOWN)
                {
                    Console.WriteLine(" Protocol @@@@@@@@@@@@@@@@  " + p);
                    LogUtil.Info("Protocol : " +p+ " Msg content "+ msg1);
                    IMsgHandler handler = GetHandler(p);
                    if (handler != null)
                        handler.Receive(sResult);
                }
            }
        }

        public static string Encrypt(string source)
        {
            return appConn.Encrypt(source, appConn.Key);
        }

        public static void Close()
        {
            appConn.Close();
        }

    }
}
