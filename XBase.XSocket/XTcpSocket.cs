using System;
using System.Collections.Generic;

using System.Text;

using System.Net;
using System.Net.Sockets;


namespace XBase.XSocket
{
    /// <summary>
    /// The Asyncronous Sockect 
    /// </summary>
    public class XTcpSocket : IDisposable
    {
        #region The Connection---------------------------------------------------------
        /// <summary>
        /// The lisenter socket
        /// </summary>
        private Socket m_Listener;

        /// <summary>
        /// The client socket
        /// </summary>
        private Socket m_Socket;

        /// <summary>
        /// The connection timeout timer
        /// </summary>
        private System.Timers.Timer m_ConnectTimeoutTimer;

        /// <summary>
        /// The response timeout timer
        /// </summary>
        private System.Timers.Timer m_ReceiveTimeoutTimer;

        /// <summary>
        /// The send timeout timer
        /// </summary>
        private System.Timers.Timer m_SendTimeoutTimer;

        /// <summary>
        /// The timeout.( unit: second )
        /// </summary>
        private int m_Timeout = 15;

        /// <summary>
        /// checking if the object is disposing..
        /// </summary>
        private bool m_Disposing = false;

        /// <summary>
        /// The socket accept event handler
        /// </summary>
        public event XSocketAcceptedEventHandler SocketAccepted;
        
        /// <summary>
        /// The socket connected event handler
        /// </summary>
        public event XSocketConnectedHandler SocketConnected;
        
        /// <summary>
        /// The data received event handler
        /// </summary>
        public event XSocketDataReceivedEventHandler DataReceived;
               
        /// <summary>
        /// The socket error event handler
        /// </summary>
        public event XSocketErrorEventHandler SocketError;

        #endregion // The Connection---------------------------------------------------------

        #region Constructor & Destructor ----------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public XTcpSocket()
        {
            m_ConnectTimeoutTimer = new System.Timers.Timer(m_Timeout * 1000);
            m_ConnectTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_ConnectTimeoutTimer_Elapsed);
            m_ReceiveTimeoutTimer = new System.Timers.Timer(m_Timeout * 1000);
            m_ReceiveTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_ReceiveTimeoutTimer_Elapsed);
            m_SendTimeoutTimer = new System.Timers.Timer(m_Timeout * 1000);
            m_SendTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_SendTimeoutTimer_Elapsed);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocket"> Windows Socket Object</param>
        public XTcpSocket(Socket vSocket)
        {
            m_Socket = vSocket;

            //m_Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

            m_ConnectTimeoutTimer = new System.Timers.Timer(m_Timeout * 1000);
            m_ConnectTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_ConnectTimeoutTimer_Elapsed);
            m_ReceiveTimeoutTimer = new System.Timers.Timer(m_Timeout * 1000);
            m_ReceiveTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_ReceiveTimeoutTimer_Elapsed);
            m_SendTimeoutTimer = new System.Timers.Timer(m_Timeout * 1000);
            m_SendTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_SendTimeoutTimer_Elapsed);
        }

        /// <summary>
        /// Dispose class
        /// </summary>
        public void Dispose()
        {
            m_Disposing = true;

            try
            {
                // Dispose connect timeout
                if (m_ConnectTimeoutTimer != null)
                {
                    m_ConnectTimeoutTimer.Elapsed -= new System.Timers.ElapsedEventHandler(m_ConnectTimeoutTimer_Elapsed);
                    m_ConnectTimeoutTimer.Dispose();
                    m_ConnectTimeoutTimer = null;
                }

                // Dispose receive timeout
                if (m_ReceiveTimeoutTimer != null)
                {
                    m_ReceiveTimeoutTimer.Elapsed -= new System.Timers.ElapsedEventHandler(m_ReceiveTimeoutTimer_Elapsed);
                    m_ReceiveTimeoutTimer.Dispose();
                    m_ReceiveTimeoutTimer = null;
                }

                // Dispose send timer
                if (m_SendTimeoutTimer != null)
                {
                    m_SendTimeoutTimer.Elapsed -= new System.Timers.ElapsedEventHandler(m_SendTimeoutTimer_Elapsed);
                    m_SendTimeoutTimer.Dispose();
                    m_SendTimeoutTimer = null;
                }

                // Close listener socket
                if (m_Socket != m_Listener)
                {
                    if (m_Socket != null)
                    {
                        if (m_Socket.Connected)
                        {
                            m_Socket.Shutdown(SocketShutdown.Both);
                            m_Socket.Close();
                        }
                        m_Socket = null;
                    }
                }

                // Close listener
                if (m_Listener != null)
                {
                    if (m_Listener.Connected)
                    {
                        m_Listener.Shutdown(SocketShutdown.Both);                        
                    }
                    m_Listener.Close();
                    m_Listener = null;
                }
            }
            catch
            {
                
            }
        }
       
        #endregion // Constructor & Destructor ---------------------------------------------

        #region Properties -----------------------------------------------------------------
        /// <summary>
        /// Get or Set the timeout
        /// </summary>
        public int Timeout
        {
            get { return m_Timeout; }
            set
            {
                if (m_Timeout != value)
                {
                    m_Timeout = value;
                    if (m_ConnectTimeoutTimer != null) m_ConnectTimeoutTimer.Interval = value * 1000;
                    if (m_ReceiveTimeoutTimer != null) m_ReceiveTimeoutTimer.Interval = value * 1000;
                    if (m_SendTimeoutTimer != null) m_SendTimeoutTimer.Interval = value * 1000;
                }
            }
        }

        /// <summary>
        /// Get connected status.
        /// </summary>
        public bool Connected
        {
            get { return m_Socket.Connected; }
        }

        /// <summary>
        /// Get or Set raw socket.
        /// </summary>
        public Socket RawSocket
        {
            get { return m_Socket; }
            set { m_Socket = value; }
        }
        #endregion // Properties ----------------------------------------------------------------

        #region Listen Process --------------------------------------------------------
        /// <summary>
        /// Start Listenning
        /// </summary>
        /// <param name="vPortNumber">Port num</param>
        public void StartListening(int vPortNumber)
        {
            StartListen(IPAddress.Any, vPortNumber, 1, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Start Listenning
        /// </summary>
        /// <param name="vPortNumber">Port num</param>
        /// <param name="vMaxListenCount">Max Listen count</param>
        public void StartListening(int vPortNumber, int vMaxListenCount)
        {
            StartListen(IPAddress.Any, vPortNumber, vMaxListenCount, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vPortNumber">port num</param>
        /// <param name="vMaxListenCount">Max  listen connection count.</param>
        /// <param name="vProtocolType">Protocol Type.</param>
        public void StartListening(int vPortNumber, int vMaxListenCount, ProtocolType vProtocolType)
        {
            StartListen(IPAddress.Any, vPortNumber, vMaxListenCount, SocketType.Stream, vProtocolType);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vPortNumber">port num</param>
        /// <param name="vMaxListenCount">max listen connection count.</param>
        /// <param name="vSocketType">socket type.</param>
        /// <param name="vProtocolType">protocol type.</param>		
        public void StartListening(int vPortNumber, int vMaxListenCount, SocketType vSocketType, ProtocolType vProtocolType)
        {
            StartListen(IPAddress.Any, vPortNumber, vMaxListenCount, vSocketType, vProtocolType);
        }

        /// <summary>
        /// Start Listen
        /// </summary>
        /// <param name="vIPAddress">ip address.</param>
        /// <param name="vPortNumber">port number.</param>
        public void StartListening(string vIPAddress, int vPortNumber)
        {
            StartListen(vIPAddress, vPortNumber, 1, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        public void StartListening(string vIPAddress, int vPortNumber, int vMaxListenCount)
        {
            StartListen(vIPAddress, vPortNumber, vMaxListenCount, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <param name="vProtocolType">protocol type</param>
        public void StartListening(string vIPAddress, int vPortNumber, int vMaxListenCount, ProtocolType vProtocolType)
        {
            StartListen(vIPAddress, vPortNumber, vMaxListenCount, SocketType.Stream, vProtocolType);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <param name="vSocketType">socket type</param>
        /// <param name="vProtocolType">protocol type</param>		
        public void StartListening(string vIPAddress, int vPortNumber, int vMaxListenCount, SocketType vSocketType, ProtocolType vProtocolType)
        {
            StartListen(vIPAddress, vPortNumber, vMaxListenCount, vSocketType, vProtocolType);
        }

        public void BeginAccept()
        {
            m_Listener.BeginAccept(new System.AsyncCallback(AcceptCallback), m_Listener);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <param name="vSocketType">socket type</param>
        /// <param name="vProtocolType">protocol type</param>		
        private void StartListen(string vIPAddress, int vPortNumber, int vMaxListenCount, SocketType vSocketType, ProtocolType vProtocolType)
        {
            IPAddress tIP = IPAddress.Parse(vIPAddress);
            StartListen(tIP, vPortNumber, vMaxListenCount, vSocketType, vProtocolType);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <param name="vSocketType">socket type</param>
        /// <param name="vProtocolType">protocol type</param>		
        private void StartListen(IPAddress vIPAddress, int vPortNumber, int vMaxListenCount, SocketType vSocketType, ProtocolType vProtocolType)
        {
            try
            {
                IPEndPoint tLocalEndPoint = new IPEndPoint(vIPAddress, vPortNumber);
                m_Listener = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);

                m_Listener.Bind(tLocalEndPoint);
                m_Listener.Listen(vMaxListenCount);
                
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10048)
                {
                    Console.WriteLine("This port is alreading used by others");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Listener socket exception : " + sx.ToString());
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Listener other exception : " + ex.ToString());
            }
        }




        /// <summary>
        /// Accept connection
        /// </summary>
        /// <param name="ar">Accept call back</param>
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket tListener = (Socket)ar.AsyncState;
                Socket tConnectedSocket = tListener.EndAccept(ar);

                if (SocketAccepted != null) SocketAccepted(this, new XSocketAcceptedEventArgs(tConnectedSocket));

                tListener.BeginAccept(new System.AsyncCallback(AcceptCallback), tListener);
            }
            catch (SocketException se)
            {
                System.Diagnostics.Debug.WriteLine("receive socket exception : " + se.Message);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("receive other exception : " + ex.Message);
            }
        }
        #endregion // Listen Process --------------------------------------------------------

        #region Socket conenct process ------------------------------------------------------------
        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        public void StartConnecting(string vRemoteIPAddress, int vRemotePortNumber)
        {
            StartConnect(vRemoteIPAddress, vRemotePortNumber, ProtocolType.Tcp, SocketType.Stream);
        }

        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vLocalIPAddress">local ip address</param>
        /// <param name="vLocalPortNumber">local port num</param>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        public void StartConnecting(string vLocalIPAddress, int vLocalPortNumber, string vRemoteIPAddress, int vRemotePortNumber)
        {
            StartConnect(vRemoteIPAddress, vRemotePortNumber, ProtocolType.Tcp, SocketType.Stream);
        }

        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        /// <param name="vProtocolType">protocol type</param>
        public void StartConnecting(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType)
        {
            StartConnect(vRemoteIPAddress, vRemotePortNumber, vProtocolType, SocketType.Stream);
        }

        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <param name="vSocketType">socket type</param>
        public void StartConnecting(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
        {
            StartConnect(vRemoteIPAddress, vRemotePortNumber, vProtocolType, vSocketType);
        }

        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <param name="vSocketType">socket type</param>
        private void StartConnect(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
        {
            try
            {
                IPAddress tRemoteIP = IPAddress.Parse(vRemoteIPAddress);
                IPEndPoint tRemoteEndPoint = new IPEndPoint(tRemoteIP, vRemotePortNumber);
                
                if( m_ConnectTimeoutTimer != null) m_ConnectTimeoutTimer.Enabled = true;
                m_Socket = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);
                m_Socket.BeginConnect(tRemoteEndPoint, new System.AsyncCallback(ConnectCallback), m_Socket);
            }
            catch (SocketException sx)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "socket connecting error : " + sx.Message + " " + sx.ErrorCode));
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "socket connecting error : " + ex.Message));
            }
        }

        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vLocalIPAddress">local ip address</param>
        /// <param name="vLocalPortNumber">local port num</param>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <param name="vSocketType">socket type</param>
        private void StartConnect(string vLocalIPAddress, int vLocalPortNumber, string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
        {
            try
            {
                IPAddress tRemoteIP = IPAddress.Parse(vRemoteIPAddress);
                IPEndPoint tRemoteEndPoint = new IPEndPoint(tRemoteIP, vRemotePortNumber);
                if( m_ConnectTimeoutTimer != null )m_ConnectTimeoutTimer.Enabled = true;
                m_Socket = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);
                IPEndPoint tLocalEndPoint = new IPEndPoint(IPAddress.Parse(vLocalIPAddress), vLocalPortNumber);
                m_Socket.Bind(tLocalEndPoint);
                m_Socket.BeginConnect(tRemoteEndPoint, new System.AsyncCallback(ConnectCallback), m_Socket);
            }
            catch (SocketException sx)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "socket connecting error : " + sx.Message + " " + sx.ErrorCode));
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "socket connecting error : " + ex.Message));
            }
        }

        /// <summary>
        /// Connect Call back
        /// </summary>
        /// <param name="ar">Async connect result</param>
        private void ConnectCallback(System.IAsyncResult ar)
        {
            try
            {
                if( m_ConnectTimeoutTimer != null ) m_ConnectTimeoutTimer.Enabled = false;

                Socket tClient = (Socket)ar.AsyncState;

                if (tClient.Connected)
                {
                    tClient.EndConnect(ar);

                    m_Socket = tClient;
                    m_Listener = null;

                    if (SocketConnected != null) SocketConnected(this, new XSocketConnectedEventArgs(m_Socket));
                }
                else
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectTimeout));
                }
            }
            catch (SocketException sx)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "socket connect error : " + sx.Message + " " + sx.ErrorCode));
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "socket connect error : " + ex.Message));
            }
        }
        #endregion // Socket conenct process -------------------------------------------------------------

        #region Socket receive process ------------------------------------------------------
        /// <summary>
        /// Start receive
        /// </summary>
        public void StartReceive()
        {
            XSocketData tSocketData = new XSocketData(m_Socket);
            StartReceive(tSocketData);
        }

        /// <summary>
        /// Start receive
        /// </summary>
        /// <param name="vSocketData">Socket data</param>
        public void StartReceive(XSocketData vSocketData)
        {
            try
            {
                m_Socket.BeginReceive(vSocketData.Buffers, 0, vSocketData.BufferSize, SocketFlags.None, new AsyncCallback(ReceivedCallBack), vSocketData);              
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10054)
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
                }
                else
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Receive data error : " + sx.Message + " " + sx.ErrorCode));
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Receive data error : " + ex.Message));
            }
        }

        /// <summary>
        /// Proecess receive data
        /// </summary>
        /// <param name="ar">Async Result</param>
        private void ReceivedCallBack(IAsyncResult ar)
        {
            try
            {
                if (m_Disposing) return;

                if (m_ReceiveTimeoutTimer != null)
                    m_ReceiveTimeoutTimer.Enabled = false;

                XSocketData tSocketData = (XSocketData)ar.AsyncState;

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint tempRemoteEP = (EndPoint)sender;

                int nByteRead = tSocketData.WorkSocket.EndReceive(ar);

                if (nByteRead != 0)
                {
                    if (DataReceived != null) DataReceived(this, new XSocketDataReceivedEventArgs(tSocketData, nByteRead)); // Data Receive Event
                }
                else
                {
                    bool tIsReadable = m_Socket.Poll(1, SelectMode.SelectRead);
                    if (tIsReadable && m_Socket.Available == 0)
                    {
                        if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.Disconnected));
                    }
                    else
                    {
                        m_Socket.BeginReceive(tSocketData.Buffers, 0, tSocketData.BufferSize, SocketFlags.None, new AsyncCallback(ReceivedCallBack), tSocketData);
                    }
                }
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10054)
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
                }
                else
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Receive data process error : " + sx.Message + " " + sx.ErrorCode));
                }
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Receive data process error : " + ex.Message));
            }
        }
        #endregion //Socket receive process ------------------------------------------------------

        #region Socket data process ------------------------------------------------------
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send</param>
        /// <param name="vEncodingType">encoding method</param>
        public void SendData(string vSendData, E_EncodingType vEncodingType)
        {
            byte[] tData = null;
            Encoding tEncoding;

            try
            {
                switch (vEncodingType)
                {
                    case E_EncodingType.ASCII:
                        tEncoding = Encoding.ASCII;
                        tData = tEncoding.GetBytes(vSendData);

                        if (m_SendTimeoutTimer != null) 
                            m_SendTimeoutTimer.Enabled = true;

                        m_Socket.BeginSend(tData, 0, tData.GetLength(0), SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
                        break;

                    case E_EncodingType.Unicode:
                        tEncoding = Encoding.Unicode;
                        tData = tEncoding.GetBytes(vSendData);

                        if (m_SendTimeoutTimer != null)
                            m_SendTimeoutTimer.Enabled = true;

                        m_Socket.BeginSend(tData, 0, tData.GetLength(0), SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
                        break;
                }
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10054)
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
                }
                else
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Start to send data error : " + sx.Message + " " + sx.ErrorCode));
                }
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Start to send data error : " + ex.Message));
            }
        }


        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send</param>
        public void SendData(string vSendData)
        {
            char[] tChars = vSendData.ToCharArray();
            byte[] tBytes = new byte[tChars.Length];

            for (int i = 0; i < tChars.Length; i++)
            {
                tBytes[i] = Convert.ToByte(tChars[i]);
            }

            try
            {
                //Console.WriteLine("SD2 S Timer true");
                if (m_SendTimeoutTimer != null) 
                    m_SendTimeoutTimer.Enabled = true;
                m_Socket.BeginSend(tBytes, 0, tBytes.Length, SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10054)
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
                }
                else
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Start to send data error : " + sx.Message + " " + sx.ErrorCode));
                }
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Start to send data error : " + ex.Message));
            }
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send.</param>
        public void SendData(byte[] vSendData, int vSize)
        {
            try
            {
                m_SendTimeoutTimer.Enabled = true;
                int tSize = (vSize == 0) ? vSendData.Length : vSize;
                m_Socket.BeginSend(vSendData, 0, tSize, SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10054)
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
                }
                else
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Start to send data error : " + sx.Message + " " + sx.ErrorCode));
                }
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Start to send data error : " + ex.Message));
            }
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send.</param>
        public void SendDataTo(byte[] vSendData, EndPoint vRemoteEndPoint)
        {
            try
            {
                //Console.WriteLine("SD3 S Timer true");
                if (m_SendTimeoutTimer != null)
                    m_SendTimeoutTimer.Enabled = true;
                m_Socket.BeginSendTo(vSendData, 0, vSendData.Length, SocketFlags.None, vRemoteEndPoint, new AsyncCallback(SendedCallBack), m_Socket);
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10054)
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
                }
                else
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Start to send data error : " + sx.Message + " " + sx.ErrorCode));
                }
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Start to send data error : " + ex.Message));
            }
        }

        /// <summary>
        /// Finish send data
        /// </summary>
        /// <param name="ar">the send result</param>
        private void SendedCallBack(IAsyncResult ar)
        {
            if (m_SendTimeoutTimer != null )
                m_SendTimeoutTimer.Enabled = false;
            try
            {
                int tSendCount = m_Socket.EndSend(ar);
                if (tSendCount > 0)
                {
                    if (m_ReceiveTimeoutTimer != null)
                        m_ReceiveTimeoutTimer.Enabled = true;
                }
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10054)
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectionResetByPeer));
                }
                else
                {
                    if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Send data error : " + sx.Message + " " + sx.ErrorCode));
                }
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Send data error : " + ex.Message));
            }
        }
        #endregion //Socket data process ------------------------------------------------------

         //Connection Timeout
        private void m_ConnectTimeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if( m_ConnectTimeoutTimer != null ) m_ConnectTimeoutTimer.Enabled = false;

            if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectTimeout));
        }

        //Receive Timeout
        private void m_ReceiveTimeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if( m_ReceiveTimeoutTimer != null )
                m_ReceiveTimeoutTimer.Enabled = false;

            if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ReceiveTimeout));
        }

        //Send Timeout
        private void m_SendTimeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (m_SendTimeoutTimer != null )
                m_SendTimeoutTimer.Enabled = false;

            if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.SendTimeout));
        }
    }
}
