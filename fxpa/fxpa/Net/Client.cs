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
    public class Client
    {
        
        static Dictionary<Protocol, IMsgHandler> handlers = new Dictionary<Protocol, IMsgHandler>();

      public  static bool NetworkIsAvailable()
        {
            try
            {
                //TcpClient client = new TcpClient();
                //client.Connect("www.google.com", 80);
                //bool rst = client.Connected;
                //client.Close();
                //return rst;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

      public virtual bool Send(object msg)
      {
          return true;
      }


      public virtual bool Start(object msg)
      {
          return true;
      }
        //public bool HasRecvEvent
        //{
        //    get { return hasRecvEvent; }
        //    set { hasRecvEvent = value; }
        //}

        public static void RegisterHandler( Protocol p, IMsgHandler h){
            lock( handlers){
                if (handlers.ContainsKey(p))
                {
                    if (h != null)
                    {
                        handlers[p] = h;
                    }
                }
                else
                {
                    handlers.Add(p, h);
                }
            }
        }

        public static void RemoveHandler( Protocol p)
        {
            lock (handlers)
            {
                if (handlers.ContainsKey(p))
                    handlers.Remove(p);
            }
        }

        public static IMsgHandler GetHandler(Protocol p)
        {
            lock (handlers)
            {
                if (handlers.ContainsKey(p))
                {
                    return handlers[p];
                }
                else
                {
                    return null;
                }
            }
        }


        public IMsgHandler GetSpecialHandler(string msg)
        {

            if (string.IsNullOrEmpty(msg))
            {
                string p = msg;
                lock (handlers)
                {
                    return handlers[(Protocol)Enum.Parse(typeof(Protocol), p)];
                }
            }
            else
            {
                return null;
            }
        }

        //LoginTCP server的授权码
        protected String loginToken;
        public String LoginToken
        {
            get { return loginToken; }
            set { loginToken = value; }
        }

        //TCP ServerIP
        protected static String tcpServerIp;
        public String TcpServerIp
        {
            get { return tcpServerIp; }
            set { tcpServerIp = value; }
        }

        //TCP Server Port
        protected static int tcpSeverPort;
        public int TcpSeverPort
        {
            get { return tcpSeverPort; }
            set { tcpSeverPort = value; }
        }

        public Client()
        {
            ////Constructor
            //Thread.Sleep(500);
        }

        //Parse Server的字符串
        public static String[] DataSplit(String str)
        {
            string strLeft = str.Replace("\r\n", "");
            string[] Data = Regex.Split(strLeft, @"\|\-\|", RegexOptions.IgnoreCase);
            return Data;
        }

        public virtual  bool ResetConnector(){
            return true;
        }



        //public static string Encrypt(string source )
        //{
        //    return null;
        //}

        //public static string Encrypt(string source, string key)
        //{
        //    return null;
        //}

        
    }
}
