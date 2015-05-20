/****************************************************
 * Pacakge: XBase.XSocket
 * Source:  Define.cs
 * 
 * Define delegation, that is used in xbase.xsocket package
 *
 ****************************************************/
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace XBase.XSocket
{
    #region Enumeration ----------------------------------------------------

    /// <summary>
    /// Encoding Type Enum
    /// </summary>
    public enum E_EncodingType
    {
        /// <summary>
        /// Ascii encoding
        /// </summary>
        ASCII,
        /// <summary>
        /// Unicode encoding
        /// </summary>
        Unicode
    }

    /// <summary>
    /// Socket Status
    /// </summary>
    public enum E_SocketStatus
    {
        /// <summary>
        /// Connecting
        /// </summary>
        Connecting,
        /// <summary>
        /// Connected
        /// </summary>
        Connected,
        /// <summary>
        /// Disconected
        /// </summary>
        DisConnected
    }

    /// <summary>
    /// Socket Error Enumeration
    /// </summary>
    public enum E_SocketError
    {
        /// <summary>
        /// No Error
        /// </summary>
        NoError,
        /// <summary>
        /// Connection Error
        /// </summary>
        ConnectionError,
        /// <summary>
        /// Disconnected
        /// </summary>
        Disconnected,
        /// <summary>
        /// Connection Timeout
        /// </summary>
        ConnectTimeout,
        /// <summary>
        /// ReceiveTimeout
        /// </summary>
        ReceiveTimeout,
        /// <summary>
        /// SendTimeout
        /// </summary>
        SendTimeout,
        /// <summary>
        /// Connection reset by peer
        /// </summary>
        ConnectionResetByPeer,
        /// <summary>
        /// Unknown error
        /// </summary>
        UnknownError

    }

    #endregion Enumeration -------------------------------------------------

    #region Handler --------------------------------------------------------


    /// <summary>
    /// Socket Accept Event Handler
    /// </summary>
    public delegate void SocketConnectedHandler(object sender, SocketConnectedEventArgs e);

    /// <summary>
    /// Socket Accept Event Handler
    /// </summary>
    public delegate void SocketAcceptedEventHandler(object sender, SocketAcceptedEventArgs e);

    /// <summary>
    /// Socket data send Event handler
    /// </summary>
    public delegate void SocketDataSendedEventHandler(object sender, SocketDataSendedEventArgs e);


    /// <summary>
    /// Socket data receive Event handler
    /// </summary>
    public delegate void SocketDataReceivedEventHandler(object sender, SocketDataReceivedEventArgs e);

    /// <summary>
    /// Socket error event handler
    /// </summary>
    public delegate void SocketErrorEventHandler(object sender, SocketErrorEventArgs e);


    /// <summary>
    /// Socket Accept Event Handler
    /// </summary>
    public delegate void XSocketConnectedHandler(object sender, XSocketConnectedEventArgs e);

    /// <summary>
    /// Socket Accept Event Handler
    /// </summary>
    public delegate void XSocketAcceptedEventHandler(object sender, XSocketAcceptedEventArgs e);

    /// <summary>
    /// Socket data send Event handler
    /// </summary>
    public delegate void XSocketDataSendedEventHandler(object sender, XSocketDataSendedEventArgs e);


    /// <summary>
    /// Socket data receive Event handler
    /// </summary>
    public delegate void XSocketDataReceivedEventHandler(object sender, XSocketDataReceivedEventArgs e);

    /// <summary>
    /// Socket error event handler
    /// </summary>
    public delegate void XSocketErrorEventHandler(object sender, XSocketErrorEventArgs e);

    #endregion Handler -----------------------------------------------------


}

