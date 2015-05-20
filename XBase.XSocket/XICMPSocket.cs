using System;
using System.Collections.Generic;

using System.Text;

using System.Net;
using System.Net.Sockets;

namespace XBase.XSocket
{
    /// <summary>
    /// XICMPSocket Class
    /// </summary>
    public class XICMPSocket : IDisposable
    {
        #region Variable ---------------------------------------------------------
        /// <summary>
        /// Socket Object
        /// </summary>
        private Socket m_Socket = null;

        /// <summary>
        /// Check the object is disposing 
        /// </summary>
        private bool m_Disposing = false;

        /// <summary>
        /// the data receive event handler
        /// </summary>
        public event XSocketDataReceivedEventHandler DataReceived;

        /// <summary>
        /// the data send event handler
        /// </summary>
        public event EventHandler DataSended;

        /// <summary>
        /// the data socket error event handler
        /// </summary>
        public event XSocketErrorEventHandler SocketError;

        /// <summary>
        /// Constructor
        /// </summary>
        public XICMPSocket() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocket">Socket Object</param>
        public XICMPSocket(Socket vSocket)
        {
            m_Socket = vSocket;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        public void Dispose()
        {
            m_Disposing = true;

            //Close socket
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
        #endregion //Variable ---------------------------------------------------------

        #region Properites ----------------------------------------------------------------
        /// <summary>
        /// Get or Set raw socket
        /// </summary>
        public Socket RawSocket
        {
            get { return m_Socket; }
            set { m_Socket = value; }
        }
        #endregion //Properites ----------------------------------------------------------------

        #region Initialize ---------------------------------------------------
        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            Initialize(IPAddress.Any);
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="vLocalIPAddress">ip Address</param>
        public void Initialize(string vLocalIPAddress)
        {
            try
            {
                IPAddress tIP = IPAddress.Parse(vLocalIPAddress);
                Initialize(tIP);
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "MKICMPSocket Initialize Error - " + ex.ToString()));
            }
        }

        /// <summary>
        /// Initialize 
        /// </summary>
        /// <param name="vLocalIPAddress">the ip Address</param>
        public void Initialize(long vLocalIPAddress)
        {
            try
            {
                IPAddress tIP = new IPAddress(vLocalIPAddress);
                Initialize(tIP);
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "MKICMPSocket Initialize Error - " + ex.ToString()));
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="vLocalIPAddress">ipAddress</param>
        public void Initialize(IPAddress vLocalIPAddress)
        {
            if (m_Socket != null) return;

            try
            {
                IPEndPoint tLocalEndPoint = new IPEndPoint(vLocalIPAddress, 0);
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
                m_Socket.Bind(tLocalEndPoint);
                StartReceive();
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "MKICMPSocket Initialize Error - " + ex.ToString()));
            }
        }
        #endregion //Initialize ---------------------------------------------------

        #region Receive Process ---------------------------------------------------
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

                XSocketData tSocketData = (XSocketData)ar.AsyncState;
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
        #endregion // Recieve Process ---------------------------------------------------

        #region Send data process ---------------------------------------------------
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send.</param>
        public void SendDataTo(byte[] vSendData, string vRemoteIPAddress)
        {
            try
            {
                IPAddress tIP = IPAddress.Parse(vRemoteIPAddress);
                IPEndPoint tIPEndPoint = new IPEndPoint(tIP, 0);
                SendDataTo(vSendData, (EndPoint)tIPEndPoint);
            }
            catch (System.Exception ex)
            {
                if (SocketError != null) SocketError(this, new XSocketErrorEventArgs(E_SocketError.UnknownError, "Send data error : " + ex.Message));
            }
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send.</param>
        public void SendDataTo(byte[] vSendData, long vRemoteIPAddress)
        {
            IPEndPoint tIPEndPoint = new IPEndPoint(vRemoteIPAddress, 0);
            SendDataTo(vSendData, (EndPoint)tIPEndPoint);
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="vSendData">the data to send.</param>
        public void SendDataTo(byte[] vSendData, EndPoint vRemoteEndPoint)
        {
            try
            {
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
            try
            {
                int tSendCount = m_Socket.EndSend(ar);
                if (DataSended != null) DataSended(this, EventArgs.Empty); //the event for sended
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
        #endregion // Send data process ---------------------------------------------------
    }
    
  
}
