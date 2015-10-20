using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace fxpa
{
  
    
    /// <summary> 
    /// Provide TcpNetwork Connection service的Client Class 
    /// </summary> 
    public class DSConnector :Connector
    {
       #region Event Definition

        //需要订阅事件才能收到事件的通知， If 订阅者退出，必须Cancel订阅 


        /// <summary> 
        /// Connection disconnected event
        /// </summary> 
        public event NetEvent DisConnectedServer;

        /// <summary> 
        /// 接收到TCPServer信息数据Datagram事件 C002
        /// </summary> 
        public event NetEvent ReceivedTCPDatagram;

        #endregion

        #region public method

        /// <summary> 
        /// Default constructor,使用默认的编码格式 
        /// </summary> 
        public DSConnector()
        {
            _coder = new Coder(Coder.EncodingMothord.Default);
        }

        /// <summary> 
        /// Constructor,使用一个特定的编码器来初始化 
        /// </summary> 
        /// <param name="_coder">Datagram编码器</param> 
        public DSConnector(Coder coder)
        {
            _coder = coder;
        }

        #endregion

        #region protected method

        /// <summary> 
        /// Establish Tcp连接后处理过程 
        /// </summary> 
        /// <param name="iar">Async Socket</param> 
       

        /// <summary> 
        /// Data accepter processing method 
        /// </summary> 
        /// <param name="iar">Async Socket</param> 
        protected override void RecvData(IAsyncResult iar)
        {
            Socket remote = (Socket)iar.AsyncState;

            try
            {
                int recv = remote.EndReceive(iar);

                //Normal exit 
                if (recv == 0)
                {
                    _session.TypeOfExit = Session.ExitType.NormalExit;

                    if (DisConnectedServer != null)
                    {
                        DisConnectedServer(this, new NetEventArgs(_session));
                    }

                    return;
                }

                string receivedData = _coder.GetEncodingString(_recvDataBuffer, recv);
                //Console.WriteLine("  original  receivedData  " + receivedData);
              

                //Via event Publish 收到的Datagram 
                if (ReceivedTCPDatagram != null)
                {
                    //Via Datagram Parser 分析出Datagram 
                    // If 定义了Datagram flag of end , needs to process Datagram different situation 
                    if (_resolver != null)
                    {
                        if (_session.Datagram != null &&
                        _session.Datagram.Length != 0)
                        {
                            // Supplement Communication  remain Datagram fragment  
                            receivedData = _session.Datagram + receivedData;

                            //Console.WriteLine("   receivedData  " + receivedData);
                        }
                        //receivedData = receivedData.Substring(0, receivedData.Length - 2);

                        //receivedData = decrypt(receivedData, Key);
                        //Console.WriteLine("  after decrypt  receivedData  " + receivedData);
                        //构造客户端可以识别的字符串
                        //receivedData += "\r\n";

                        string[] recvDatagrams = _resolver.Resolve(ref receivedData);

                        //string[] recvDatagrams = Regex.Split((string)receivedData, @"\r\n", RegexOptions.IgnoreCase);
                        if (recvDatagrams != null)
                        {
                            for (int i = 0; i < recvDatagrams.Length; i++) //string newDatagram in recvDatagrams)
                            {

                                string newDatagram = recvDatagrams[i];
                                if (!string.IsNullOrEmpty(newDatagram))
                                {
                                    //Console.WriteLine("   newDatagram  :::::::::::::::::::: " + newDatagram);
                                    //Need Deep Copy. Because needs to make sure multiple different Datagram can be deemed  as isolated object 
                                    ICloneable copySession = (ICloneable)_session;

                                    Session clientSession = (Session)copySession.Clone();
                                    //Console.WriteLine("  before decrypt  receivedData  " + newDatagram);

                                    newDatagram = newDatagram.Replace("\r\n", "");

                                    //newDatagram = decrypt(newDatagram, Key);

                                    if (string.IsNullOrEmpty(newDatagram))
                                    {
                                        Console.WriteLine(" NULL decrypt  receivedData  " + newDatagram);
                                    }
                                    else
                                    {
                                        //Console.WriteLine("  after decrypt  receivedData  " + newDecrypt);
                                    }
                                    clientSession.Datagram = newDatagram;

                                    ReceivedTCPDatagram(this, new NetEventArgs(clientSession));
                                }
                            }
                        }
                        //Console.WriteLine("   receivedData  " + receivedData);
                        // remain 代码 fragment ,下次接收的时候使用 
                        if(_session!=null)
                         _session.Datagram = receivedData;
                    }


                }//end of if(ReceivedDatagram != null) 

                //继续接收数据 
                _session.ClientSocket.BeginReceive(_recvDataBuffer, 0, DefaultBufferSize, SocketFlags.None,
                new AsyncCallback(RecvData), _session.ClientSocket);
            }
            catch (SocketException ex)
            {
                //客户端退出 
                if (10054 == ex.ErrorCode)
                {
                    //Server强制的Close连接，强制退出 
                    _session.TypeOfExit = Session.ExitType.ExceptionExit;

                    if (DisConnectedServer != null)
                    {
                        DisConnectedServer(this, new NetEventArgs(_session));
                    }
                }
                else
                {
                    throw (ex);
                }
            }
            catch (ObjectDisposedException ex)
            {
                //这里的实现不够优雅 
                //当调用CloseSession()时,会结束Data accepter ,但是Data accepter  
                //处理中会调用int recv = client.EndReceive(iar); 
                //就访问了CloseSession()已经处置的 object  
                //我想这样的实现方法也是无伤大雅的. 
                if (ex != null)
                {
                    ex = null;
                    //DoNothing; 
                }
            }

        }

        #endregion
    }  
}
