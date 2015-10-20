using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO.Compression;
using System.IO;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace fxpa
{
    //--------------------------------------------------------------
    /// <summary> 
    /// Network Communication 事件模型委托 
    /// </summary> 
    public delegate void NetEvent(object sender, NetEventArgs e);
    
    /// <summary> 
    /// Provide TcpNetwork Connection service的Client Class 
    /// </summary> 
    public class Connector
    {
        #region 字段

        /// <summary> 
        /// 命令字列表 
        /// </summary>         
        private ArrayList datagrams = new ArrayList();

        /// <summary> 
        /// 客户端与Server之间的会话类 
        /// </summary> 
        protected Session _session;

        /// <summary> 
        /// 客户端是否已经连接Server 
        /// </summary> 
        protected bool _isConnected = false;

        /// <summary> 
        /// 接收数据缓冲区大小2K 
        /// </summary> 
        protected const int DefaultBufferSize = 1460*2;
        //protected const int DefaultBufferSize = 2 * 1024;
        private string key = null;

        public  string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary> 
        /// Datagram Parser  
        /// </summary> 
        protected DatagramResolver _resolver;

        /// <summary> 
        /// Communication 格式编码解码器 
        /// </summary> 
        protected Coder _coder;

        /// <summary> 
        /// 接收数据缓冲区 
        /// </summary> 
        protected byte[] _recvDataBuffer = new byte[DefaultBufferSize];

        #endregion


        #region Event Definition

        //需要订阅事件才能收到事件的通知， If 订阅者退出，必须Cancel订阅 

        /// <summary> 
        /// 已经连接Server事件 
        /// </summary> 
        public event NetEvent ConnectedServer;



        #endregion

        #region Properties

        /// <summary> 
        /// 返回客户端与Server之间的会话 object  
        /// </summary> 
        public Session ClientSession
        {
            get
            {
                return _session;
            }
        }

        /// <summary> 
        /// 返回客户端与Server之间的连接状态 
        /// </summary> 
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        public bool IsReady
        {
            get
            {
                return _isConnected && !string.IsNullOrEmpty(Key);
            }
        }

        /// <summary> 
        /// 数据Datagram分析器 
        /// </summary> 
        public DatagramResolver Resovlver
        {
            get
            {
                return _resolver;
            }
            set
            {
                _resolver = value;
            }
        }

        /// <summary> 
        /// 编码解码器 
        /// </summary> 
        public Coder ServerCoder
        {
            get
            {
                return _coder;
            }
        }

        #endregion

        #region public method

        /// <summary> 
        /// Default constructor,使用默认的编码格式 
        /// </summary> 
        public Connector()
        {
            _coder = new Coder(Coder.EncodingMothord.Default);
        }

        /// <summary> 
        /// Constructor,使用一个特定的编码器来初始化 
        /// </summary> 
        /// <param name="_coder">Datagram编码器</param> 
        public Connector(Coder coder)
        {
            _coder = coder;
        }

        /// <summary> 
        /// 数据编码 
        /// </summary> 
        /// <param name="datagram">需要编码的Datagram</param> 
        /// <returns>编码后的数据</returns> 
        public virtual String GetKey(string datagram)
        {
            //java add unzip
            //datagram = Decompress(datagram);

            string strLocalKey = datagram.Substring(5, 8);
            string strEncrypt = datagram.Substring(23, datagram.Length - 25);
            string strDecrypt = Decrypt(strEncrypt, strLocalKey);
            return decrypt(strDecrypt, strLocalKey);
        }

        public virtual String decrypt(String source, String key)
        {
            if (!string.IsNullOrEmpty(key))
            {
              //  BlowfishEasy bfes = new BlowfishEasy(key);
              //  return bfes.DecryptString(source);
                return source;
            }
            else
            {
                return source;
            }
        }


        public virtual String encrypt(String source, String key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                //BlowfishEasy bfes = new BlowfishEasy(key);
                //return bfes.EncryptString(source);
                return source;
            }
            else
            {
                return source;
            }
        }

        public virtual String Encrypt(string pToEncrypt, string sKey)
        {
            if (!string.IsNullOrEmpty(sKey))
            {
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                    des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                    des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    string str = Convert.ToBase64String(ms.ToArray());
                    ms.Close();
                    return str;
                }
            }
            else
            {
                return pToEncrypt;
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public virtual String Decrypt(string pToDecrypt, string sKey)
        {
            //Console.WriteLine("  pToDecrypt  :::  " + pToDecrypt);
            if (!string.IsNullOrEmpty(sKey))
            {
                byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                    des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    string str = Encoding.UTF8.GetString(ms.ToArray());
                    ms.Close();
                    return str;
                }
            }
            else
            {
                return pToDecrypt;
            }
        }

        public static string Decompress(String str)
        {
            // Initialize decompressor.
            byte[] compressedBytes = Convert.FromBase64String(str);
            Inflater decompressor = new Inflater();
            decompressor.SetInput(compressedBytes); // Give the decompressor the
            // data to decompress.

            byte[] ret = null;

            using (MemoryStream memStream = new MemoryStream(compressedBytes.Length))
            {
                // Decompress the data
                byte[] buf = new byte[compressedBytes.Length + 100];
                while (!decompressor.IsFinished)
                {
                    memStream.Write(buf, 0, decompressor.Inflate(buf));
                }

                memStream.Close();
                ret = memStream.ToArray();
            }


            return ASCIIEncoding.UTF8.GetString(ret);
        }

        public static String Compress(string str)
        {
            byte[] bytesToCompress = ASCIIEncoding.UTF8.GetBytes(str);
            // Compressor with highest level of compression.
            Deflater compressor = new Deflater(Deflater.BEST_COMPRESSION);
            compressor.SetInput(bytesToCompress); // Give the compressor the data to
            // compress.
            compressor.Finish();

            // Create an expandable byte array to hold the compressed data.
            // It is not necessary that the compressed data will be smaller than
            // the uncompressed data.
            byte[] ret = null;


            using (MemoryStream memStream = new MemoryStream(bytesToCompress.Length))
            {
                // Compress the data
                byte[] buf = new byte[bytesToCompress.Length + 100];
                while (!compressor.IsFinished)
                {
                    memStream.Write(buf, 0, compressor.Deflate(buf));
                }
                memStream.Close();
                ret = memStream.ToArray();
            }

            // Get the compressed data
            return Convert.ToBase64String(ret);
        }

        private bool timeOut = false;

        public bool TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }

        /// <summary> 
        /// 连接Server 
        /// </summary> 
        /// <param name="ip">ServerIP地址</param> 
        /// <param name="port">ServerPort</param> 
        public virtual void Connect(string ip, int port)
        {
            Socket newsock = null;

            if (IsConnected)
            {
                //Re-连接 
                Debug.Assert(_session != null);

                Close();
            }

            newsock = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), port);
            newsock.BeginConnect(iep, new AsyncCallback(Connected), newsock);
        }

        /// <summary> 
        /// 发送数据Datagram 
        /// </summary> 
        /// <param name="datagram"></param> 
        public virtual bool Send(string datagram)
        {
            Console.WriteLine(" Send ================= " + datagram +"  " + (cc++));
            if (datagram.Length == 0)
            {
                return true;
            }

            try
            {
                if (!_isConnected)
                {
                    _isConnected = false;
                    key = null;
                    throw (new ApplicationException("没有连接Server，不能发送数据"));
                }

                datagram += Environment.NewLine;
                //获得Datagram的编码字节 
                byte[] data = _coder.GetEncodingBytes(datagram);

                _session.ClientSocket.SendTimeout = 5000;

                _session.ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None,
                new AsyncCallback(SendDataEnd), _session.ClientSocket);
                Console.WriteLine(" Send ================= " + datagram + "  " );
            }
            catch (Exception e)
            {
                DateTime now = DateTime.Now;
                LogUtil.Error(" Send === Complete  " + e.StackTrace + "  " + now.ToLongTimeString() + " m  " + now.Minute + " s " + now.Second + "  ms " + now.Millisecond);
                _isConnected = false;
                key = null;
                Console.WriteLine(e.StackTrace);
                return false;
            }
            return true;
        }

        /// <summary> 
        /// Close连接 
        /// </summary> 
        public virtual void Close()
        {
            if (!_isConnected)
            {
                return;
            }

            _session.Close();

            _session = null;

            _isConnected = false;

            timeOut = true;
        }

        #endregion

        #region protected method

        /// <summary> 
        /// 数据发送完成processing method 
        /// </summary> 
        /// <param name="iar"></param> 
        protected virtual void SendDataEnd(IAsyncResult iar)
        {
            Socket remote = (Socket)iar.AsyncState;
            int sent = remote.EndSend(iar);            
            Debug.Assert(sent != 0);
            DateTime now =DateTime.Now;
            Console.WriteLine(" Send === Complete  " + sent + "  " + now.ToLongTimeString()+ " m  " + now.Minute + " s " + now.Second + "  ms " + now.Millisecond);
            LogUtil.Info(" Send === Complete  " + sent + "  " + now.ToLongTimeString() + " m  " + now.Minute + " s " + now.Second + "  ms " + now.Millisecond);
        }

        /// <summary> 
        /// Establish Tcp连接后处理过程 
        /// </summary> 
        /// <param name="iar">Async Socket</param> 
        protected virtual void Connected(IAsyncResult iar)
        {
            try
            {
                Socket socket = null;

                socket = (Socket)iar.AsyncState;

                socket.EndConnect(iar);


                //创建新的会话 
                _session = new Session(socket);

                _isConnected = true;

                //触发连接Establish 事件 
                if (ConnectedServer != null)
                {
                    ConnectedServer(this, new NetEventArgs(_session));
                }

                //Establish 连接后应该立即接收数据 
                _session.ClientSocket.BeginReceive(_recvDataBuffer, 0,
                DefaultBufferSize, SocketFlags.None,
                new AsyncCallback(RecvData), socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                DateTime now = DateTime.Now;
                LogUtil.Error(" Send === Exception  " +  "  " + now.ToLongTimeString() + " H "+now.Hour + " m  " + now.Minute + " s " + now.Second + "  ms " + now.Millisecond);
                LogUtil.Error("  "+e.StackTrace);
                timeOut = true;
            }


        }

        static int cc = 0;

        /// <summary> 
        /// Data accepter processing method 
        /// </summary> 
        /// <param name="iar">Async Socket</param> 
        protected virtual void RecvData(IAsyncResult iar)
        {
            

        }

        #endregion
    }

    /// <summary> 
    /// Communication 编码格式Provide 者,为Communication 服务Provide 编码 and 解码服务 
    /// 你可以在继承类中定制自己的encode method如:数据加密传输等 
    /// </summary> 
    public class Coder
    {
        /// <summary> 
        /// encode method 
        /// </summary> 
        private EncodingMothord _encodingMothord;

        protected Coder()
        {

        }

        public Coder(EncodingMothord encodingMothord)
        {
            _encodingMothord = encodingMothord;
        }

        public enum EncodingMothord
        {
            Default = 0,
            Unicode,
            UTF8,
            ASCII,
        }

        /// <summary> 
        /// Communication 数据解码 
        /// </summary> 
        /// <param name="dataBytes">需要解码的数据</param> 
        /// <returns>编码后的数据</returns> 
        public virtual string GetEncodingString(byte[] dataBytes, int size)
        {
            switch (_encodingMothord)
            {
                case EncodingMothord.Default:
                    {
                        return Encoding.Default.GetString(dataBytes, 0, size);
                    }
                case EncodingMothord.Unicode:
                    {
                        return Encoding.Unicode.GetString(dataBytes, 0, size);
                    }
                case EncodingMothord.UTF8:
                    {
                        return Encoding.UTF8.GetString(dataBytes, 0, size);
                    }
                case EncodingMothord.ASCII:
                    {
                        return Encoding.ASCII.GetString(dataBytes, 0, size);
                    }
                default:
                    {
                        throw (new Exception("未定义的编码格式"));
                    }
            }

        }

        /// <summary> 
        /// 数据编码 
        /// </summary> 
        /// <param name="datagram">需要编码的Datagram</param> 
        /// <returns>编码后的数据</returns> 
        public virtual byte[] GetEncodingBytes(string datagram)
        {
            switch (_encodingMothord)
            {
                case EncodingMothord.Default:
                    {
                        return Encoding.Default.GetBytes(datagram);
                    }
                case EncodingMothord.Unicode:
                    {
                        return Encoding.Unicode.GetBytes(datagram);
                    }
                case EncodingMothord.UTF8:
                    {
                        return Encoding.UTF8.GetBytes(datagram);
                    }
                case EncodingMothord.ASCII:
                    {
                        return Encoding.ASCII.GetBytes(datagram);
                    }
                default:
                    {
                        throw (new Exception("未定义的编码格式"));
                    }
            }
        }

    }


    /// <summary> 
    /// 数据Datagram分析器,Via 分析接收到的原始数据,得到完整的数据Datagram. 
    /// 继承该类可以实现自己的Datagram解析方法. 
    /// 通常的Datagram识别方法包括:固定长度,长度标记,标记符等方法 
    /// 本类的现实的是标记符的方法,你可以在继承类中实现其他的方法 
    /// </summary> 
    public class DatagramResolver
    {
        /// <summary> 
        /// Datagram结束标记 
        /// </summary> 
        private string endTag;

        /// <summary> 
        /// 返回结束标记 
        /// </summary> 
        string EndTag
        {
            get
            {
                return endTag;
            }
        }

        /// <summary> 
        /// 受保护的Default constructor,Provide 给继承类使用 
        /// </summary> 
        protected DatagramResolver()
        {

        }

        /// <summary> 
        /// Constructor 
        /// </summary> 
        /// <param name="endTag">Datagram结束标记</param> 
        public DatagramResolver(string endTag)
        {
            if (endTag == null)
            {
                throw (new ArgumentNullException("结束标记不能为null"));
            }

            if (endTag == "")
            {
                throw (new ArgumentException("结束标记符号不能为空字符串"));
            }

            this.endTag = endTag;
        }

        /// <summary> 
        /// 解析Datagram 
        /// </summary> 
        /// <param name="rawDatagram">原始数据,返回未使用的Datagram fragment , 
        /// 该 fragment 会保存在Session的Datagram object 中</param> 
        /// <returns>Datagram数组,原始数据可能包含多个Datagram</returns> 
        public virtual string[] Resolve(ref string rawDatagram)
        {
   
            string[] results = null;
            if (rawDatagram.EndsWith(endTag))
            {
                results = Regex.Split(rawDatagram, @endTag, RegexOptions.IgnoreCase);
                rawDatagram = "";
            }
            else
            {
                if (rawDatagram.Contains(endTag))
                {
                    string forePart = rawDatagram.Remove(rawDatagram.LastIndexOf(endTag));
                    string tail = rawDatagram.Remove(0, forePart.Length);
                    rawDatagram = tail.Replace(endTag, "");
                    results = Regex.Split(forePart, @endTag, RegexOptions.IgnoreCase);
                }
            }
            return results;
        }

    }


    /// <summary> 
    /// 客户端与Server之间的会话类 
    /// 说明: 
    /// 会话类包含远程Communication 端的状态,这些状态包括Socket,Datagram内容, 
    /// 客户端退出的类型(正常Close,强制退出两种类型) 
    /// </summary> 
    public class Session : ICloneable
    {
        #region 字段

        /// <summary> 
        /// 会话ID 
        /// </summary> 
        private SessionId _id;

        /// <summary> 
        /// 客户端发送到Server的Datagram 
        /// 注意:在有些情况下Datagram可能只是Datagram的 fragment 而不完整 
        /// </summary> 
        private string _datagram;

        /// <summary> 
        /// 客户端的Socket 
        /// </summary> 
        private Socket _cliSock;

        /// <summary> 
        /// 客户端的退出类型 
        /// </summary> 
        private ExitType _exitType;

        /// <summary> 
        /// 退出类型枚举 
        /// </summary> 
        public enum ExitType
        {
            NormalExit,
            ExceptionExit
        };

        #endregion

        #region Properties

        /// <summary> 
        /// 返回会话的ID 
        /// </summary> 
        public SessionId ID
        {
            get
            {
                return _id;
            }
        }

        /// <summary> 
        /// 存取会话的Datagram 
        /// </summary> 
        public string Datagram
        {
            get
            {
                return _datagram;
            }
            set
            {
                _datagram = value;
            }
        }

        /// <summary> 
        /// 获得与客户端会话关联的Socket object  
        /// </summary> 
        public Socket ClientSocket
        {
            get
            {
                return _cliSock;
            }
        }

        /// <summary> 
        /// 存取客户端的退出方式 
        /// </summary> 
        public ExitType TypeOfExit
        {
            get
            {
                return _exitType;
            }

            set
            {
                _exitType = value;
            }
        }

        #endregion

        #region 方法

        /// <summary> 
        /// 使用Socket object 的Handle值作为HashCode,它具有良好的线性特征. 
        /// </summary> 
        /// <returns></returns> 
        public override int GetHashCode()
        {
            return (int)_cliSock.Handle;
        }

        /// <summary> 
        /// 返回两个Session是否代表同一个客户端 
        /// </summary> 
        /// <param name="obj"></param> 
        /// <returns></returns> 
        public override bool Equals(object obj)
        {
            Session rightObj = (Session)obj;

            return (int)_cliSock.Handle == (int)rightObj.ClientSocket.Handle;

        }

        /// <summary> 
        /// 重载ToString()方法,返回Session object 的特征 
        /// </summary> 
        /// <returns></returns> 
        public override string ToString()
        {
            string result = string.Format("Session:{0},IP:{1}",
            _id, _cliSock.RemoteEndPoint.ToString());

            //result.C 
            return result;
        }

        /// <summary> 
        /// Constructor 
        /// </summary> 
        /// <param name="cliSock">会话使用的Socket连接</param> 
        public Session(Socket cliSock)
        {
            Debug.Assert(cliSock != null);

            _cliSock = cliSock;

            _id = new SessionId((int)cliSock.Handle);
        }

        /// <summary> 
        /// Close会话 
        /// </summary> 
        public void Close()
        {
            Debug.Assert(_cliSock != null);
            try
            {
                //Close数据的接受和发送 
                _cliSock.Shutdown(SocketShutdown.Both);

                //清理资源 
                _cliSock.Close();
            }
            catch (Exception e)
            {
            }
        }

        #endregion

        #region ICloneable 成员

        object System.ICloneable.Clone()
        {
            Session newSession = new Session(_cliSock);
            newSession.Datagram = _datagram;
            newSession.TypeOfExit = _exitType;

            return newSession;
        }

        #endregion
    }


    /// <summary> 
    /// 唯一的标志一个Session,辅助Session object 在Hash表中完成特定功能 
    /// </summary> 
    public class SessionId
    {
        /// <summary> 
        /// 与Session object 的Socket object 的Handle值相同,必须用这个值来初始化它 
        /// </summary> 
        private int _id;

        /// <summary> 
        /// 返回ID值 
        /// </summary> 
        public int ID
        {
            get
            {
                return _id;
            }
        }

        /// <summary> 
        /// Constructor 
        /// </summary> 
        /// <param name="id">Socket的Handle值</param> 
        public SessionId(int id)
        {
            _id = id;
        }

        /// <summary> 
        /// 重载.为了符合Hashtable键值特征 
        /// </summary> 
        /// <param name="obj"></param> 
        /// <returns></returns> 
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                SessionId right = (SessionId)obj;

                return _id == right._id;
            }
            else if (this == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary> 
        /// 重载.为了符合Hashtable键值特征 
        /// </summary> 
        /// <returns></returns> 
        public override int GetHashCode()
        {
            return _id;
        }

        /// <summary> 
        /// 重载,为了方便显示输出 
        /// </summary> 
        /// <returns></returns> 
        public override string ToString()
        {
            return _id.ToString();
        }

    }


    /// <summary> 
    /// Server程序的事件 parameter ,包含了激发该事件的会话 object  
    /// </summary> 
    public class NetEventArgs : EventArgs
    {

        #region 字段

        /// <summary> 
        /// 客户端与Server之间的会话 
        /// </summary> 
        private Session _client;

        #endregion

        #region Constructor
        /// <summary> 
        /// Constructor 
        /// </summary> 
        /// <param name="client">客户端会话</param> 
        public NetEventArgs(Session client)
        {
            if (null == client)
            {
                throw (new ArgumentNullException());
            }

            _client = client;
        }
        #endregion

        #region Properties

        /// <summary> 
        /// 获得激发该事件的会话 object  
        /// </summary> 
        public Session Client
        {
            get
            {
                return _client;
            }
        }

        #endregion
    }
}
