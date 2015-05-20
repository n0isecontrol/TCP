using System;
using System.Collections.Generic;

using System.Text;

using System.Net;
using System.Net.Sockets;

namespace XBase.XSocket
{

    #region SocketAcceptedEventArgs class ----------------------------------------
    /// <summary>
    /// Socket Accepted Event Argument Class
    /// </summary>
    public class SocketConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// The socket object
        /// </summary>
        private Socket m_Socket;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocket">Socket event</param>
        public SocketConnectedEventArgs(Socket vSocket)
        {
            m_Socket = vSocket;
        }

        /// <summary>
        /// Get Socket
        /// </summary>
        public Socket Socket
        {
            get { return m_Socket; }
        }
    }

    #endregion // SocketAcceptedEventArgs class -----------------------------------

    #region SocketAcceptedEventArgs class ----------------------------------------
    /// <summary>
    /// Socket Accepted Event Argument Class
    /// </summary>
    public class SocketAcceptedEventArgs : EventArgs
    {
        /// <summary>
        /// The socket object
        /// </summary>
        private Socket m_Socket;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocket">Socket event</param>
        public SocketAcceptedEventArgs(Socket vSocket)
        {
            m_Socket = vSocket;
        }

        /// <summary>
        /// Get Socket
        /// </summary>
        public Socket Socket
        {
            get { return m_Socket; }
        }
    }

    #endregion // SocketAcceptedEventArgs class -----------------------------------

    #region SocketDataReceivedEventArgs -------------------------------------------
    /// <summary>
    /// Socket Data Receive Event Arguments
    /// </summary>
    public class SocketDataReceivedEventArgs : EventArgs
    {
        private String mIPAddress;
        private XMessage mMessage;

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="vSocketData">socket data</param>
        /// <param name="vReadCount">read bytes</param>
        public SocketDataReceivedEventArgs(XMessage vMessage)
        {
            mMessage = vMessage;
        }

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="vSenderIPAddress">ip address</param>
        /// <param name="vSocketData">data</param>
        /// <param name="vReadCount">read bytes</param>
        public SocketDataReceivedEventArgs( String vSenderIPAddress, XMessage vMessage )
        {
            mMessage = vMessage;
            mIPAddress = vSenderIPAddress;
        }

        /// <summary>
        /// Get message
        /// </summary>
        public XMessage Message
        {
            get { return mMessage; }
        }

        /// <summary>
        /// Get sender's ip address
        /// </summary>
        public string SenderIPAddress
        {
            get { return mIPAddress; }
        }
    }

    #endregion SocketDataReceivedEventArgs ----------------------------------------

    #region SocketDataReceivedEventArgs -------------------------------------------
    /// <summary>
    /// Socket Data Receive Event Arguments
    /// </summary>
    public class SocketDataSendedEventArgs : EventArgs
    {
        /// <summary>
        /// Read Bytes
        /// </summary>
        private XMessage m_xMessage;

        /// <summary>
        /// Sender IP Address
        /// </summary>
        private string m_IPAddress = "";

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="vSocketData">socket data</param>
        /// <param name="vReadCount">read bytes</param>
        public SocketDataSendedEventArgs(String vIPAddress, XMessage vMessage)
        {
            m_IPAddress = vIPAddress;
            m_xMessage = vMessage;
        }

        /// <summary>
        /// Get socket data
        /// </summary>
        public XMessage Message
        {
            get { return m_xMessage; }
        }

        /// <summary>
        /// Get sender's ip address
        /// </summary>
        public string SenderIPAddress
        {
            get { return m_IPAddress; }
        }
    }

    #endregion SocketDataReceivedEventArgs ----------------------------------------

    #region SocketErrorEventArgs --------------------------------------------------
    /// <summary>
    /// Socket Error Event Argument
    /// </summary>
    public class SocketErrorEventArgs : EventArgs
    {
        /// <summary>
        /// The socket error enum.
        /// </summary>
        private E_SocketError m_SocketError = E_SocketError.NoError;

        /// <summary>
        /// The error message.
        /// </summary>
        private string m_Message = "";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocketError">socket error</param>
        public SocketErrorEventArgs(E_SocketError vSocketError)
        {
            m_SocketError = vSocketError;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocketError">socket error</param>
        /// <param name="vMessage">socket error message</param>
        public SocketErrorEventArgs(E_SocketError vSocketError, string vMessage)
        {
            m_SocketError = vSocketError;
            m_Message = vMessage;
        }

        /// <summary>
        /// Get socket error
        /// </summary>
        public E_SocketError SocketError
        {
            get { return m_SocketError; }
        }

        /// <summary>
        /// Get socket error message
        /// </summary>
        public string Message
        {
            get { return m_Message; }
        }
    }

    #endregion SocketErrorEventArgs -----------------------------------------------

    #region XSocketAcceptedEventArgs class ----------------------------------------
    /// <summary>
    /// Socket Accepted Event Argument Class
    /// </summary>
    public class XSocketConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// The socket object
        /// </summary>
        private XTcpSocket m_Socket;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocket">Socket event</param>
        public XSocketConnectedEventArgs(Socket vSocket)
        {
            m_Socket = new XTcpSocket(vSocket);
        }

        /// <summary>
        /// Get Socket
        /// </summary>
        public XTcpSocket Socket
        {
            get { return m_Socket; }
        }
    }

    #endregion // XSocketAcceptedEventArgs class -----------------------------------

    #region XSocketAcceptedEventArgs class ----------------------------------------
    /// <summary>
    /// Socket Accepted Event Argument Class
    /// </summary>
    public class XSocketAcceptedEventArgs : EventArgs
    {
        /// <summary>
        /// The socket object
        /// </summary>
        private XTcpSocket m_Socket;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocket">Socket event</param>
        public XSocketAcceptedEventArgs(Socket vSocket)
        {
            m_Socket = new XTcpSocket(vSocket);
        }

        /// <summary>
        /// Get Socket
        /// </summary>
        public XTcpSocket Socket
        {
            get { return m_Socket; }
        }
    }

    #endregion // XSocketAcceptedEventArgs class -----------------------------------
    
    #region XSocketDataReceivedEventArgs -------------------------------------------
    /// <summary>
    /// Socket Data Receive Event Arguments
    /// </summary>
    public class XSocketDataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// XSocket Data
        /// </summary>
        private XSocketData m_SocketData;

        /// <summary>
        /// Read Bytes
        /// </summary>
        private int m_ReadCount;

        /// <summary>
        /// Sender IP Address
        /// </summary>
        private string m_SenderIPAddress = "";

        private object m_DataObject = null;

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="vSocketData">socket data</param>
        /// <param name="vReadCount">read bytes</param>
        public XSocketDataReceivedEventArgs(XSocketData vSocketData, int vReadCount)
        {
            m_SocketData = vSocketData;
            m_ReadCount = vReadCount;
        }

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="vSenderIPAddress">ip address</param>
        /// <param name="vSocketData">data</param>
        /// <param name="vReadCount">read bytes</param>
        public XSocketDataReceivedEventArgs(string vSenderIPAddress, XSocketData vSocketData, int vReadCount)
        {
            m_SocketData = vSocketData;
            m_ReadCount = vReadCount;
            m_SenderIPAddress = vSenderIPAddress;
        }

        /// <summary>
        /// Get socket data
        /// </summary>
        public XSocketData SocketData
        {
            get { return m_SocketData; }
        }

        /// <summary>
        /// Get read bytes
        /// </summary>
        public int ReadCount
        {
            get { return m_ReadCount; }
        }


        public object ReceiveObject
        {
            get { return m_DataObject; }
            set { m_DataObject = value; }
        }
       
        /// <summary>
        /// Get sender's ip address
        /// </summary>
        public string SenderIPAddress
        {
            get { return m_SenderIPAddress; }
        }
    }

    #endregion XSocketDataReceivedEventArgs ----------------------------------------

    #region XSocketDataReceivedEventArgs -------------------------------------------
    /// <summary>
    /// Socket Data Receive Event Arguments
    /// </summary>
    public class XSocketDataSendedEventArgs : EventArgs
    {
        /// <summary>
        /// Read Bytes
        /// </summary>
        private XMessage m_xMessage;

        /// <summary>
        /// Sender IP Address
        /// </summary>
        private string m_IPAddress = "";

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="vSocketData">socket data</param>
        /// <param name="vReadCount">read bytes</param>
        public XSocketDataSendedEventArgs(String vIPAddress, XMessage vMessage)
        {
            m_IPAddress = vIPAddress;
            m_xMessage = vMessage;
        }

        /// <summary>
        /// Get socket data
        /// </summary>
        public XMessage Message
        {
            get { return m_xMessage; }
        }

        /// <summary>
        /// Get sender's ip address
        /// </summary>
        public string SenderIPAddress
        {
            get { return m_IPAddress; }
        }
    }

    #endregion XSocketDataReceivedEventArgs ----------------------------------------

    #region XSocketErrorEventArgs --------------------------------------------------
    /// <summary>
    /// Socket Error Event Argument
    /// </summary>
    public class XSocketErrorEventArgs : EventArgs
    {
        /// <summary>
        /// The socket error enum.
        /// </summary>
        private E_SocketError m_SocketError = E_SocketError.NoError;

        /// <summary>
        /// The error message.
        /// </summary>
        private string m_Message = "";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocketError">socket error</param>
        public XSocketErrorEventArgs(E_SocketError vSocketError)
        {
            m_SocketError = vSocketError;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vSocketError">socket error</param>
        /// <param name="vMessage">socket error message</param>
        public XSocketErrorEventArgs(E_SocketError vSocketError, string vMessage)
        {
            m_SocketError = vSocketError;
            m_Message = vMessage;
        }

        /// <summary>
        /// Get socket error
        /// </summary>
        public E_SocketError SocketError
        {
            get { return m_SocketError; }
        }

        /// <summary>
        /// Get socket error message
        /// </summary>
        public string Message
        {
            get { return m_Message; }
        }
    }

    #endregion XSocketErrorEventArgs -----------------------------------------------
}
