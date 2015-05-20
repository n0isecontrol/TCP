using System;
using System.Collections.Generic;

using System.Text;

using System.Net;
using System.Net.Sockets;


namespace XBase.XSocket
{
    /// <summary>
    /// XUdpSocket
    /// </summary>
    public class XUdpSocket : IDisposable
    {
        #region Variable ---------------------------------------------------------
        /// <summary>
        /// The Listner Socket
        /// </summary>
        private Socket m_Listener;
        /// <summary>
        /// Socket
        /// </summary>
        private Socket m_Socket;
        /// <summary>
        /// Connection Timeout Timer
        /// </summary>
        private System.Timers.Timer m_ConnectTimeoutTimer;
        /// <summary>
        /// Response Timeout Timer
        /// </summary>
        private System.Timers.Timer m_ReceiveTimeoutTimer;
        /// <summary>
        /// Send Timeout Timer
        /// </summary>
        private System.Timers.Timer m_SendTimeoutTimer;

        /// <summary>
        /// Timeout( unit: second )
        /// </summary>
        private int m_Timeout = 15;

        /// <summary>
        /// Check if the object is disposing
        /// </summary>
        private bool m_Disposing = false;

        /// <summary>
        /// Sockect accept event handler
        /// </summary>
        public event XSocketAcceptedEventHandler SocketAccepted;

        /// <summary>
        /// Socket connected event Handler
        /// </summary>
        public event EventHandler SocketConnected;

        /// <summary>
        /// Socket data received event handler
        /// </summary>
        public event XSocketDataReceivedEventHandler DataReceived;

        /// <summary>
        /// Send data event handler
        /// </summary>
        public event EventHandler DataSended;

        /// <summary>
        /// Socket error event handler
        /// </summary>
        public event XSocketErrorEventHandler SocketError;

        /// <summary>
        /// Constructor
        /// </summary>
        public XUdpSocket()
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
        /// <param name="vSocket">Socket Object</param>
        public XUdpSocket(Socket vSocket)
        {
            m_Socket = vSocket;

            m_ConnectTimeoutTimer = new System.Timers.Timer(m_Timeout * 1000);
            m_ConnectTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_ConnectTimeoutTimer_Elapsed);
            m_ReceiveTimeoutTimer = new System.Timers.Timer(m_Timeout * 1000);
            m_ReceiveTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_ReceiveTimeoutTimer_Elapsed);
            m_SendTimeoutTimer = new System.Timers.Timer(m_Timeout * 1000);
            m_SendTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_SendTimeoutTimer_Elapsed);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        public void Dispose()
        {
            m_Disposing = true;

            if (m_ConnectTimeoutTimer != null)
            {
                m_ConnectTimeoutTimer.Elapsed -= new System.Timers.ElapsedEventHandler(m_ConnectTimeoutTimer_Elapsed);
                m_ConnectTimeoutTimer.Dispose();
            }

            if (m_ReceiveTimeoutTimer != null)
            {
                m_ReceiveTimeoutTimer.Elapsed -= new System.Timers.ElapsedEventHandler(m_ReceiveTimeoutTimer_Elapsed);
                m_ReceiveTimeoutTimer.Dispose();
            }

            if (m_SendTimeoutTimer != null)
            {
                m_SendTimeoutTimer.Elapsed -= new System.Timers.ElapsedEventHandler(m_SendTimeoutTimer_Elapsed);
                m_SendTimeoutTimer.Dispose();
            }

            if (m_Socket != m_Listener)
            {
                if (m_Socket != null)
                {
                    m_Socket.Shutdown(SocketShutdown.Both);
                    m_Socket.Close();
                    m_Socket = null;
                }
            }

            if (m_Listener != null)
            {
                m_Listener.Shutdown(SocketShutdown.Both);
                m_Listener.Close();
                m_Listener = null;
            }
        }

        //Connection Timeout
        private void m_ConnectTimeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_ConnectTimeoutTimer.Enabled = false;

            if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ConnectTimeout));
        }

        //Receiveer Timeout
        private void m_ReceiveTimeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_ReceiveTimeoutTimer.Enabled = false;

            if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.ReceiveTimeout));
        }

        //Send Timeout
        private void m_SendTimeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_SendTimeoutTimer.Enabled = false;

            if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.SendTimeout));
        }
        #endregion //Variable ---------------------------------------------------------

        #region Properites ----------------------------------------------------------------
        /// <summary>
        /// Get or Set timeout ( unit : second )
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
        /// Get conencted status
        /// </summary>
        public bool Connected
        {
            get { return m_Socket.Connected; }
        }

        /// <summary>
        /// Get or Set raw socket
        /// </summary>
        public Socket RawSocket
        {
            get { return m_Socket; }
            set { m_Socket = value; }
        }
        #endregion //Properites ----------------------------------------------------------------

        #region Listening Process --------------------------------------------------------
        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vPortNumber">port number</param>
        public void StartListening(int vPortNumber)
        {
            StartListen(IPAddress.Any, vPortNumber, 1);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        public void StartListening(int vPortNumber, int vMaxListenCount)
        {
            StartListen(IPAddress.Any, vPortNumber, vMaxListenCount);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        public void StartListening(string vIPAddress, int vPortNumber)
        {
            StartListen(vIPAddress, vPortNumber, 1);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        public void StartListening(string vIPAddress, int vPortNumber, int vMaxListenCount)
        {
            StartListen(vIPAddress, vPortNumber, vMaxListenCount);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        private void StartListen(string vIPAddress, int vPortNumber, int vMaxListenCount)
        {
            IPAddress tIP = IPAddress.Parse(vIPAddress);
            StartListen(tIP, vPortNumber, vMaxListenCount);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        private void StartListen(IPAddress vIPAddress, int vPortNumber, int vMaxListenCount)
        {
            try
            {
                IPEndPoint tLocalEndPoint = new IPEndPoint(vIPAddress, vPortNumber);
                m_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                m_Listener.Bind(tLocalEndPoint);
                m_Socket = m_Listener;
                StartReceive();
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10048)
                {
                    Console.WriteLine("This port is alreading used by others");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Listener  socket exception : " + sx.ToString());
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
        /// <param name="ar">the send result</param>
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
        #endregion // Listening Process --------------------------------------------------------

        #region Socket Connect Process -------------------------------------------------------------
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
                m_ConnectTimeoutTimer.Enabled = true;
                m_Listener = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);
                m_Listener.BeginConnect(tRemoteEndPoint, new System.AsyncCallback(ConnectCallback), m_Listener);
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
                m_ConnectTimeoutTimer.Enabled = true;
                m_Listener = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);
                IPEndPoint tLocalEndPoint = new IPEndPoint(IPAddress.Parse(vLocalIPAddress), vLocalPortNumber);
                m_Listener.Bind(tLocalEndPoint);
                m_Listener.BeginConnect(tRemoteEndPoint, new System.AsyncCallback(ConnectCallback), m_Listener);
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
        /// The socket connection process.
        /// </summary>
        /// <param name="ar">the send result</param>
        private void ConnectCallback(System.IAsyncResult ar)
        {
            try
            {
                m_ConnectTimeoutTimer.Enabled = false;

                Socket tClient = (Socket)ar.AsyncState;
                
                if (tClient.Connected)
                {
                    tClient.EndConnect(ar);

                    m_Socket = tClient;
                    m_Listener = null;

                    if (SocketConnected != null) SocketConnected(this, EventArgs.Empty);
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
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "socket connect error : " + ex.Message));
            }
        }
        #endregion // The socket connection process -------------------------------------------------------------

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
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint tempRemoteEP = (EndPoint)sender;

                m_Socket.BeginReceiveFrom(vSocketData.Buffers, 0, vSocketData.BufferSize, SocketFlags.None, ref tempRemoteEP, new AsyncCallback(ReceivedCallBack), vSocketData);
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
                m_ReceiveTimeoutTimer.Enabled = false;

                XSocketData tSocketData = (XSocketData)ar.AsyncState;

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint tempRemoteEP = (EndPoint)sender;

                int nByteRead = tSocketData.WorkSocket.EndReceiveFrom(ar, ref tempRemoteEP);

                if (nByteRead != 0)
                {
                    if (DataReceived != null) DataReceived(this, new XSocketDataReceivedEventArgs(((IPEndPoint)tempRemoteEP).Address.ToString(), tSocketData, nByteRead)); //데이터 받음 이벤트
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
                        m_Socket.BeginReceiveFrom(tSocketData.Buffers, 0, tSocketData.BufferSize, SocketFlags.None, ref tempRemoteEP, new AsyncCallback(ReceivedCallBack), tSocketData);
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
                        m_SendTimeoutTimer.Enabled = true;
                        m_Socket.BeginSend(tData, 0, tData.GetLength(0), SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
                        break;

                    case E_EncodingType.Unicode:
                        tEncoding = Encoding.Unicode;
                        tData = tEncoding.GetBytes(vSendData);
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
        public void SendData(byte[] vSendData)
        {
            try
            {
                //Console.WriteLine("SD3 S Timer true");
                m_SendTimeoutTimer.Enabled = true;
                m_Socket.BeginSend(vSendData, 0, vSendData.GetLength(0), SocketFlags.None, new AsyncCallback(SendedCallBack), m_Socket);
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
            //Console.WriteLine("S Timer false");
            m_SendTimeoutTimer.Enabled = false;
            try
            {
                int tSendCount = m_Socket.EndSend(ar);
                if (DataSended != null) DataSended(this, EventArgs.Empty); //the event for sended
                if (tSendCount > 0)
                {
                    //Console.WriteLine("R Timer true");
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
    }
}
