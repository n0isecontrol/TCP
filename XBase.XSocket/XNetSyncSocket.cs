using System;
using System.Collections.Generic;

using System.Text;

using System.Net;
using System.Net.Sockets;

namespace XBase.XSocket
{
    /// <summary>
    /// Synchronize Socket Class
    /// </summary>
    public class XNetSyncSocket : IDisposable
    {
        #region Variable ---------------------------------------------------------
        /// <summary>
        /// Listen Socket
        /// </summary>
        private Socket m_Listener;
        /// <summary>
        /// Socket
        /// </summary>
        private Socket m_Socket;

        /// <summary>
        /// Timeout ( unit : second)
        /// </summary>
        private int m_Timeout = 15;

        /// <summary>
        /// Socket buffer size( bytes)
        /// </summary>
        private int m_BufferSize = 4096;

        /// <summary>
        /// Socket buffer
        /// </summary>
        private byte[] m_Buffers = null;

        /// <summary>
        /// the event handlder for disconnection 
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// the remote end point
        /// </summary>
        IPEndPoint m_RemoteEndPoint = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public XNetSyncSocket()
        {
            m_Buffers = new byte[m_BufferSize];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocket">Socket Object</param>
        public XNetSyncSocket(Socket vSocket)
        {
            m_Buffers = new byte[m_BufferSize];

            m_Socket = vSocket;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        public void Dispose()
        {
            if (m_Listener != null)
            {
                m_Listener.Close();
                m_Listener = null;
            }

            if (m_Socket != null)
            {
                m_Socket.Close();
                m_Socket = null;
            }
        }
        #endregion //Variable ---------------------------------------------------------

        #region Properites ----------------------------------------------------------------

        /// <summary>
        /// Get or Set timeout(unit: second)
        /// </summary>
        public int Timeout
        {
            get { return m_Timeout; }
            set { m_Timeout = value; }
        }

        /// <summary>
        /// Check if it is connecting
        /// </summary>
        public bool Connected
        {
            get { return m_Socket.Connected; }
        }
        #endregion //Properites ----------------------------------------------------------------

        #region Listner Process --------------------------------------------------------

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vPortNumber">port number</param>
        /// <returns>Socket Object</returns>
        public Socket StartListen(int vPortNumber)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());            
            if (ipHostInfo.AddressList.GetLength(0) > 0)
            {
                return StartListen(ipHostInfo.AddressList[0], vPortNumber, 1, ProtocolType.Tcp, SocketType.Stream);
            }
            return null;
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <returns>Socket Object</returns>
        public Socket StartListen(int vPortNumber, int vMaxListenCount)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            if (ipHostInfo.AddressList.GetLength(0) > 0)
            {
                return StartListen(ipHostInfo.AddressList[0], vPortNumber, vMaxListenCount, ProtocolType.Tcp, SocketType.Stream);
            }
            return null;
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <returns>Socket Object</returns>
        public Socket StartListen(int vPortNumber, int vMaxListenCount, ProtocolType vProtocolType)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            if (ipHostInfo.AddressList.GetLength(0) > 0)
            {
                return StartListen(ipHostInfo.AddressList[0], vPortNumber, vMaxListenCount, vProtocolType, SocketType.Stream);
            }
            return null;
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <param name="vSocketType">socket type</param>
        /// <returns>Socket Object</returns>
        public Socket StartListen(int vPortNumber, int vMaxListenCount, ProtocolType vProtocolType, SocketType vSocketType)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            if (ipHostInfo.AddressList.GetLength(0) > 0)
            {
                return StartListen(ipHostInfo.AddressList[0], vPortNumber, vMaxListenCount, vProtocolType, vSocketType);
            }
            return null;
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <returns>Socket Object</returns>
        public Socket StartListen(string vIPAddress, int vPortNumber)
        {
            return StartListen(vIPAddress, vPortNumber, 1, ProtocolType.Tcp, SocketType.Stream);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <returns>Socket Object</returns>
        public Socket StartListen(string vIPAddress, int vPortNumber, int vMaxListenCount)
        {
            return StartListen(vIPAddress, vPortNumber, vMaxListenCount, ProtocolType.Tcp, SocketType.Stream);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <returns>Socket Object</returns>
        public Socket StartListen(string vIPAddress, int vPortNumber, int vMaxListenCount, ProtocolType vProtocolType)
        {
            return StartListen(vIPAddress, vPortNumber, vMaxListenCount, vProtocolType, SocketType.Stream);
        }


        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <param name="vSocketType">socket type</param>
        /// <returns>Socket Object</returns>
        private Socket StartListen(string vIPAddress, int vPortNumber, int vMaxListenCount, ProtocolType vProtocolType, SocketType vSocketType)
        {
            IPAddress tIP = IPAddress.Parse(vIPAddress);
            return StartListen(tIP, vPortNumber, vMaxListenCount, vProtocolType, vSocketType);
        }

        /// <summary>
        /// Start Listening
        /// </summary>
        /// <param name="vIPAddress">listen ip address</param>
        /// <param name="vPortNumber">port number</param>
        /// <param name="vMaxListenCount">max listen account</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <param name="vSocketType">socket type</param>
        /// <returns>Socket Object</returns>
        private Socket StartListen(IPAddress vIPAddress, int vPortNumber, int vMaxListenCount, ProtocolType vProtocolType, SocketType vSocketType)
        {
            Socket tSocket = null;
            try
            {
                IPEndPoint tLocalEndPoint = new IPEndPoint(vIPAddress, vPortNumber);
                m_Listener = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);

                m_Listener.Bind(tLocalEndPoint);
                m_Listener.Listen(vMaxListenCount);

                tSocket = m_Listener.Accept();
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
            return tSocket;
        }
        #endregion // Listening Process --------------------------------------------------------

        #region Socket Connect Process -------------------------------------------------------------
        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        /// <returns>Socket Error</returns>
        public XSocketError StartConnecting(string vRemoteIPAddress, int vRemotePortNumber)
        {
            return StartConnect(vRemoteIPAddress, vRemotePortNumber, ProtocolType.Tcp, SocketType.Stream);
        }

        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <returns>Socket Error</returns>
        public XSocketError StartConnecting(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType)
        {
            return StartConnect(vRemoteIPAddress, vRemotePortNumber, vProtocolType, SocketType.Stream);
        }

        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <param name="vSocketType">socket type</param>
        /// <returns>Socket Error</returns>
        public XSocketError StartConnecting(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
        {
            return StartConnect(vRemoteIPAddress, vRemotePortNumber, vProtocolType, vSocketType);
        }

        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <param name="vSocketType">socket type</param>
        /// <returns>Socket Error</returns>
        private XSocketError StartConnect(string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
        {
            XSocketError tResult = new XSocketError();

            try
            {
                IPAddress tRemoteIP = IPAddress.Parse(vRemoteIPAddress);
                m_RemoteEndPoint = new IPEndPoint(tRemoteIP, vRemotePortNumber);
                m_Listener = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);
                if (m_Timeout > 0)
                {
                    m_Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, m_Timeout * 1000);
                    m_Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, m_Timeout * 1000);
                }
                //cklee 2007-12-21 ----------------------------------------------------------------
                m_Listener.Bind(new IPEndPoint(IPAddress.Any, 0));
                //m_Listener.Connect(m_RemoteEndPoint);
                //cklee 2007-12-21 ----------------------------------------------------------------
                m_Socket = m_Listener;
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10060) //Send Timeout발생
                {
                    tResult.m_Error = E_SocketError.ConnectTimeout;
                }
                else
                {
                    tResult.m_Error = E_SocketError.UnknownError;
                    tResult.m_ErrorMessage = sx.ToString();
                }
            }
            catch (System.Exception ex)
            {
                tResult.m_Error = E_SocketError.UnknownError;
                tResult.m_ErrorMessage = ex.ToString();
            }
            return tResult;
        }

        /// <summary>
        /// Start connect 
        /// </summary>
        /// <param name="vRemoteIPAddress">ip address</param>
        /// <param name="vRemotePortNumber">port num</param>
        /// <param name="vProtocolType">protocol type</param>
        /// <param name="vSocketType">socket type</param>
        /// <returns>Socket Error</returns>
        public XSocketError StartConnect(string vLocalIPAddress, int vLocalPortNumber, string vRemoteIPAddress, int vRemotePortNumber, ProtocolType vProtocolType, SocketType vSocketType)
        {
            XSocketError tResult = new XSocketError();

            try
            {
                IPAddress tRemoteIP = IPAddress.Parse(vRemoteIPAddress);
                m_RemoteEndPoint = new IPEndPoint(tRemoteIP, vRemotePortNumber);
                m_Listener = new Socket(AddressFamily.InterNetwork, vSocketType, vProtocolType);
                if (m_Timeout > 0)
                {
                    m_Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, m_Timeout * 1000);
                    m_Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, m_Timeout * 1000);
                }
                IPEndPoint tLocalEndPoint = new IPEndPoint(IPAddress.Parse(vLocalIPAddress), vLocalPortNumber);
                m_Listener.Bind(tLocalEndPoint);
                //cklee 2007-12-21 ----------------------------------------------------------------
                //m_Listener.Connect(m_RemoteEndPoint);
                //cklee 2007-12-21 ----------------------------------------------------------------
                m_Socket = m_Listener;
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10060) //Send Timeout발생
                {
                    tResult.m_Error = E_SocketError.ConnectTimeout;
                }
                else
                {
                    tResult.m_Error = E_SocketError.UnknownError;
                    tResult.m_ErrorMessage = sx.ToString();
                }
            }
            catch (System.Exception ex)
            {
                tResult.m_Error = E_SocketError.UnknownError;
                tResult.m_ErrorMessage = ex.ToString();
            }
            return tResult;
        }
        #endregion //Socket Connection Process -------------------------------------------------------------

        #region Socket receive process ------------------------------------------------------
        /// <summary>
        /// Start receive
        /// </summary>
        /// <param name="vBuffer">Buffer</param>		
        /// <returns>Socket Error</returns>
        public XSocketError Receive(ref byte[] vBuffer)
        {
            XSocketError tResult = new XSocketError();

            vBuffer = null;
            try
            {
                EndPoint tEndPoint = (EndPoint)m_RemoteEndPoint;
                int tReceiveCount = m_Socket.ReceiveFrom(m_Buffers, ref tEndPoint);
                if (tReceiveCount > 0)
                {
                    vBuffer = new byte[tReceiveCount];
                    Buffer.BlockCopy(m_Buffers, 0, vBuffer, 0, tReceiveCount);
                }
                else
                {
                    bool tIsReadable = m_Socket.Poll(1, SelectMode.SelectRead);
                    if (tIsReadable && m_Socket.Available == 0)
                    {
                        if (Disconnected != null) Disconnected(this, EventArgs.Empty);
                    }
                }
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10060) // Receive Timeout
                {
                    tResult.m_Error = E_SocketError.ReceiveTimeout;
                }
                else
                {
                    tResult.m_Error = E_SocketError.UnknownError;
                    tResult.m_ErrorMessage = sx.ToString();
                }
            }
            catch (System.Exception ex)
            {
                tResult.m_Error = E_SocketError.UnknownError;
                tResult.m_ErrorMessage = ex.ToString();
            }
            return tResult;
        }
        #endregion //Socket receive process ------------------------------------------------------

        #region Socket data process ------------------------------------------------------
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send.</param>
        /// <param name="vEncodingType">the encoding type.</param>
        /// <returns>Socket error</returns>
        public XSocketError Send(string vSendData, E_EncodingType vEncodingType)
        {
            XSocketError tResult = new XSocketError();

            byte[] tData = null;
            Encoding tEncoding;

            try
            {
                switch (vEncodingType)
                {
                    case E_EncodingType.ASCII:
                        tEncoding = Encoding.ASCII;
                        tData = tEncoding.GetBytes(vSendData);
                        break;

                    case E_EncodingType.Unicode:
                        tEncoding = Encoding.Unicode;
                        tData = tEncoding.GetBytes(vSendData);
                        break;
                }

                XSocketError tErr = Send(tData);
                tResult.m_Error = tErr.Error;
                tResult.m_ErrorMessage = tErr.ErrorMessage;
            }
            catch (System.Exception ex)
            {
                tResult.m_Error = E_SocketError.UnknownError;
                tResult.m_ErrorMessage = ex.ToString();
            }

            return tResult;
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send.</param>
        /// <returns>Socket Error Result</returns>
        public XSocketError Send(string vSendData)
        {
            char[] tChars = vSendData.ToCharArray();
            byte[] tBytes = new byte[tChars.Length];

            for (int i = 0; i < tChars.Length; i++)
            {
                tBytes[i] = Convert.ToByte(tChars[i]);
            }

            return Send(tBytes);
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send.</param>
        /// <returns>Socket Error Result</returns>
        public XSocketError Send(byte[] vSendData)
        {
            XSocketError tResult = new XSocketError();

            try
            {
                EndPoint tEndPoint = (EndPoint)m_RemoteEndPoint;
                m_Socket.SendTo(vSendData, tEndPoint);
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10060) //Send Timeout
                {
                    tResult.m_Error = E_SocketError.SendTimeout;
                }
                else
                {
                    tResult.m_Error = E_SocketError.UnknownError;
                    tResult.m_ErrorMessage = sx.ToString();
                }
            }
            catch (System.Exception ex)
            {
                tResult.m_Error = E_SocketError.UnknownError;
                tResult.m_ErrorMessage = ex.ToString();
            }
            return tResult;
        }
        #endregion //Socket data process ------------------------------------------------------
    }
    
}
