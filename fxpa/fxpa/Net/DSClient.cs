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


    /// <summary> 
    /// TestTcpClient 的摘要说明。 
    /// </summary> 
    public class DSClient:Client
    {

        static DSConnector dsConn = new DSConnector(new Coder(Coder.EncodingMothord.UTF8));
        private static DSHandler dsHandler= new DSHandler();
        private static Client client = new DSClient();
        DSClient()
        {
            //Constructor
            //Thread.Sleep(500);
        }
        static protected bool hasRecvEvent;

        public static bool IsReady
        {
            get { return dsConn.IsReady; }
        }

        public static bool IsConnected
        {
            get { return dsConn.IsConnected; }
        }

        public static void Connect(string ip, int port)
        {
            try
            {
                tcpServerIp = ip;
                tcpSeverPort = port;
                dsHandler.Client = client;

                dsConn.Resovlver = new DatagramResolver("\r\n");
                //接收TCPServer信息字符串
                if (!hasRecvEvent)
                {
                    dsConn.ReceivedTCPDatagram += new NetEvent(RecvTCPDatagram);
                    hasRecvEvent = true;
                }

                dsConn.Connect(tcpServerIp, tcpSeverPort);
                //cli.Connect("192.168.1.112", 7788);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        static int ret = 0;
     public   static bool  IsStopAttempt =false;
        public static void Start()
        {
            IsStopAttempt =false;
            while (!dsConn.IsConnected)
            {
                //appConn
                if (dsConn.TimeOut)
                {
                    Console.WriteLine((ret++) + "  time  ");
                    Connect(tcpServerIp, tcpSeverPort);
                    if (IsStopAttempt)
                    {
                        IsStopAttempt =false;
                        break;
                    }
                }
                Thread.Sleep(500);
            }
            
            //string str = "#|-|01|-|D0002|-|1|-|" + AppSetting.LOGIN_TOKEN + "|-|";
            string request = NetHelper.BuildRawMsg(Protocol.D0002_1, new string[] { AppSetting.LOGIN_TOKEN });
            request = AppClient.Encrypt(request);
            Console.WriteLine( " request     " + request );
            dsHandler.Send(request);
            //dsHandler.Send("xxx");
        }


        public override bool Send(object msg)
        {
            return dsConn.Send((string)msg);
        }
  
        //接收TCPServer信息字符串
        public static void RecvTCPDatagram(object sender, NetEventArgs e)
        {
            String msg1 = e.Client.Datagram;
            dsHandler.Receive(msg1);
        }

        public static  void SetKey( string k)
        {
          dsConn.Key = k; 
        }

        public static string GetKey(string f)
        {
            //if (f == typeof(ICrypta).Name)
            //    return dsConn.Key;
            //else 
                return null;
        }


        public static void Close()
        {
            dsConn.Close();
        }
    }
}
