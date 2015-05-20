using System;
using System.Collections.Generic;

using System.Text;


using System.Threading;
using System.Net;
using System.Net.Sockets;

using XBase;
using XBase.Framework;
using XBase.Thread;
using XBase.Collection;

using XBase.Function.Serialize;

namespace XBase.XSocket
{
    /// <summary>
    /// XClient Class
    /// </summary>
    public class XClient
    {
        #region Members ----------------------------------

        /// <summary>
        /// Socket Object
        /// </summary>
        //private XTcpSocket mSocket = null;
        private Socket mSocket = null;

        /// <summary>
        /// Sender Object
        /// </summary>
        private XPriorityQueue2 mSenderQueue = null;       

        /// <summary>
        /// Socket Receiver Thread
        /// </summary>
        private XThread mReceiverThread = null;

        /// <summary>
        /// Socket Sender Thread
        /// </summary>
        private XThread mSenderThread = null;

        /// <summary>
        /// Receive Event
        /// </summary>
        private ManualResetEvent mReceiveEvent = new ManualResetEvent(false);

        /// <summary>
        /// Stop Flag
        /// </summary>
        private bool mbStop = true;

        /// <summary>
        /// Is Disposed
        /// </summary>
        private bool mIsDisposed = false;

        /// <summary>
        /// Parent Container
        /// </summary>
        private XClientContainer mParent = null;

        /// <summary>
        /// The socket connected event handler
        /// </summary>
        public SocketConnectedHandler SocketConnected;        

        /// <summary>
        /// The socket error event handler
        /// </summary>
        public SocketErrorEventHandler SocketError;

        /// <summary>
        /// The data received event handler
        /// </summary>
        public SocketDataReceivedEventHandler DataReceived;

        /// <summary>
        /// The data send event handler
        /// </summary>
        public SocketDataSendedEventHandler DataSended;

        #endregion // Members ------------------------------

        #region Constructor & Destructor -----------------

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocket"></param>
        public XClient()
        {
            mSocket = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vContainer"> Parent Container</param>
        /// <param name="vSocket">SocketObject</param>
        //public XClient(XClientContainer vContainer, XTcpSocket vSocket)
        public XClient(XClientContainer vContainer, Socket vSocket)
        {
            mParent = vContainer;

            Initialize(vSocket);
        }

        // private void Initialize(XTcpSocket vSocket)        
        private void Initialize( Socket vSocket)        
        {
            if( mSocket != null )
            {
                try
                {
                    mSocket.Close();
                    mSocket = null;
                }
                catch
                {
                }
            }
            mSocket = vSocket;

            // Don't allow another socket to bind to this port.
            //mSocket.ExclusiveAddressUse = true;

            // The socket will linger for 10 seconds after  
            // Socket.Close is called.
            //mSocket.LingerState = new LingerOption(true, 10);

            // Disable the Nagle Algorithm for this tcp socket.
            //mSocket.NoDelay = true;

            // Set the receive buffer size to 8k
            mSocket.ReceiveBufferSize = 8192;

            // Set the timeout for synchronous receive methods to  
            // 1 second (1000 milliseconds.)
            mSocket.ReceiveTimeout = 60000;

            // Set the send buffer size to 8k.
            mSocket.SendBufferSize = 8192;

            // Set the timeout for synchronous send methods 
            // to 1 second (1000 milliseconds.)            
            mSocket.SendTimeout = 60000;

            // Set the Time To Live (TTL) to 42 router hops.
            mSocket.Ttl = 42;


            
            
            mSenderQueue = new XPriorityQueue2(XProtocol.LEVEL_MAX + 1, 4096);
        }

        

        /// <summary>
        /// Destructor
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (mIsDisposed == false)
                {
                    Stop();

                    if (mSocket != null)
                    {
                        mSocket.Close();
                        mSocket = null;
                    }
                        
                }
                mIsDisposed = true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        #endregion Constructor & Destructor --------------

        #region Properties -------------------------------

        /// <summary>
        /// Check if is stopped 
        /// </summary>
        public bool IsStop
        {
            get
            {
                return mbStop;
            }
        }

        #endregion Properties ----------------------------

        #region Methods ----------------------------------

        /// <summary>
        /// Get Socket's IP Address string.
        /// </summary>
        /// <returns></returns>
        public string GetIPAddress()
        {
            IPEndPoint tEndPoint = (IPEndPoint)mSocket.RemoteEndPoint;
            IPAddress tAddress = tEndPoint.Address;
            return tAddress.ToString();
        }


        public bool IsServerSocket()
        {
            return (mParent != null);
        }

        /// <summary>
        /// Socket Receive Start 
        /// </summary>
        /// <param name="vIP">if server socket then null, else remote server ip</param>
        /// <param name="vPort">if server socket then null, else remote server port</param>
        /// <returns></returns>
        public bool Start(string vIP, string vPort)
        {
            if (mbStop == false )
                return true;

            if ( IsServerSocket() == true )
            {
                mbStop = false;
                mReceiverThread = new XThread(new XThreadEventHandler(DoSocketReceiverProcess), null);
                mReceiverThread.Run();
            }
            else
            {
                mbStop = false;
                XSimpleOption tOption = new XSimpleOption();
                tOption.SetOption("ip", vIP);
                tOption.SetOption("port", vPort);

                mReceiverThread = new XThread(new XThreadEventHandler(DoSocketReceiverProcess), tOption);
                mReceiverThread.Run();
            }

            return true;
        }


        /// <summary>
        /// Send XMessage via Socket
        /// </summary>
        /// <param name="vMessage"></param>
        public void SendMessage( XMessage vMessage)
        {
            mSenderQueue.Enqueue(vMessage, vMessage.Level);
        }


        private void InternalSendMessage(XMessage vMessage)
        {
            byte[] tMessageBytes = XBinaryConverter.GetBytes(vMessage);
            int tMessageLength = tMessageBytes.Length;

            byte[] tLengthBytes = BitConverter.GetBytes(tMessageLength);

            mSocket.Send(tLengthBytes, 0, tLengthBytes.Length, SocketFlags.None);

            int totalBytesToSend = tMessageLength;
            int bytesSend = 0;

            while(bytesSend < totalBytesToSend )
                bytesSend += mSocket.Send(tMessageBytes, bytesSend, totalBytesToSend - bytesSend, SocketFlags.None);

        }

        

        /// <summary>
        /// Listner Stop
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            try
            {
                if (mbStop == true)
                    return true;

                mbStop = true;
                System.Threading.Thread.Sleep(100);
                mReceiverThread.Dispose();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

            return false;
        }

        public string GetSessionID()
        {
            String time = DateTime.Now.ToString("yyyyMMddhhmmss");
            Random tRand = new Random();
            int iRand = tRand.Next(100000, 999999);            
            String ipAddress = GetIPAddress().Replace(".", "-");
            String tSessionId = time + "-" + iRand + "-" + ipAddress;
            return tSessionId;
        }

        private void CreateSenderThread()
        {
            mSenderThread = new XThread(new XThreadEventHandler(DoSocketSenderProcess), null);
            mSenderThread.Run();
        }   
      

        #endregion Methods -------------------------------
        
        #region Events -----------------------------------

        private void DoSocketSenderProcess(object vParam)
        {
            try
            {
                int nSleepTime = 50;
                int nPingPeriod = 10000 / nSleepTime;
                int nPollCounter = nPingPeriod;

                XMessage pingMessage = new XMessage(XProtocol.XMSG_PING, XProtocol.TYPE_INTEGER, XProtocol.LEVEL_HIGHEST, 0, String.Empty, null );

                String ipAddress = GetIPAddress();
                while (mbStop == false)
                {
                    nPollCounter--;

                    while( mSenderQueue.Count > 0  && mbStop == false)
                    {
                        XMessage tMessage = mSenderQueue.Dequeue() as XMessage;
                        InternalSendMessage( tMessage );
                        OnDataSended(this, new SocketDataSendedEventArgs(ipAddress, tMessage));
                        nPingPeriod = nPollCounter;
                    }

                    if( nPollCounter <=0 )
                    {
                        InternalSendMessage(pingMessage);
                    }
                    System.Threading.Thread.Sleep(nSleepTime);
                }

                if( mSenderQueue.Count > 0 )
                {
                    int nCount = 10;
                    while (mSenderQueue.Count > 0 && nCount > 0 )
                    {
                        XMessage tMessage = mSenderQueue.Dequeue() as XMessage;
                        InternalSendMessage(tMessage);
                        OnDataSended(this, new SocketDataSendedEventArgs(ipAddress, tMessage));
                    }
                    
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
            finally
            {
                mSenderQueue.Clear();

                /*
                if (IsServerSocket() == false)
                {
                    mSocket.Dispose();
                    mSocket = null;
                } 
                */
            }
        }

        
        private void DoSocketReceiverProcess(object vParam)
        {
            try
            {
                if( this.IsServerSocket() == false )
                {
                    XSimpleOption vOption = (XSimpleOption)vParam;

                    string vIP = vOption.GetOption("ip");
                    string vPort = vOption.GetOption("port");

                    int nPort = 55555;
                    try
                    {
                        nPort = Int32.Parse(vPort);
                    }
                    catch (System.Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.ToString());
                    }

                    Socket tSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    Initialize( tSocket );

                    try
                    {
                        mSocket.Connect(vIP, nPort);
                    }
                    catch(System.Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.ToString());
                        this.OnSocketError(this, new SocketErrorEventArgs( E_SocketError.ConnectionError ) );
                    }
                }

                CreateSenderThread();

                byte[] tHeader = new byte[4];
                byte[] tBuffer = new byte[XProtocol.XMSG_DATA_BLOCKSIZE * 2];

                while (mbStop == false)
                {
                    int iLength = mSocket.Receive(tHeader, tHeader.Length, SocketFlags.None) ;
                    if ( iLength != tHeader.Length)
                    {
                        OnSocketError(this, new SocketErrorEventArgs(E_SocketError.UnknownError));
                        break;
                    }

                    iLength = BitConverter.ToInt32(tHeader, 0);
                    int bytesRead = 0;
                    while (bytesRead < iLength)
                    {
                        bytesRead += mSocket.Receive(tBuffer, bytesRead, iLength - bytesRead, SocketFlags.None);
                        if( bytesRead == 0 )
                        {
                            break;
                        }
                    }

                    if ( bytesRead == 0 )
                    {
                        OnSocketError(this, new SocketErrorEventArgs(E_SocketError.UnknownError));
                        break;
                    }

                    XMessage tObject = XBinaryConverter.GetObject(tBuffer, 0, iLength) as XMessage;
                    OnDataReceived(this, new SocketDataReceivedEventArgs(tObject));
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
            finally
            {
                XMessageAPI.SendStop(this);

                if( mParent != null )
                    mParent.RemoveClient(this);
            }
        }

        private void OnSocketConnected(object sender, SocketConnectedEventArgs e)
        {
            if (SocketConnected != null)
                SocketConnected(sender, e);
        }

        private void OnSocketError(object sender, SocketErrorEventArgs e)
        {
            if (SocketError != null)
                SocketError(sender, e);

            if (mParent != null && mParent.SocketError != null)
            {
                mParent.SocketError(sender, e);
                if (e.SocketError == E_SocketError.Disconnected || 
                    e.SocketError == E_SocketError.ReceiveTimeout )
                {
                    mParent.RemoveClient(this);
                    return;
                }
                
            }
        }

        private void OnDataSended(object sender, SocketDataSendedEventArgs e)
        {
            if (DataSended != null)
                DataSended(sender, e);

            if (mParent != null && mParent.DataSended != null)
            {
                mParent.DataSended(sender, e);
            }
        }

        private void OnDataReceived(object sender, SocketDataReceivedEventArgs e)
        {
            if (DataReceived != null)
                DataReceived(sender, e);

            if( mParent != null && mParent.DataReceived != null )
            {
                mParent.DataReceived(sender, e);
            }
        }


        #endregion Events -----------------------------------
    }
}
