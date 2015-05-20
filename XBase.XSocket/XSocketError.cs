using System;
using System.Collections.Generic;

using System.Text;

using System.Net;
using System.Net.Sockets;

namespace XBase.XSocket
{
    /// <summary>
    /// The class for Socket Communication Error
    /// </summary>
    public class XSocketError
    {
        /// <summary>
        /// Error Code
        /// </summary>
        internal E_SocketError m_Error = E_SocketError.NoError;

        /// <summary>
        /// Error Message
        /// </summary>
        internal string m_ErrorMessage = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public XSocketError() 
        { 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vError">Error Code</param>
        /// <param name="vErrorMessage">Error Message</param>
        public XSocketError(E_SocketError vError, string vErrorMessage)
        {
            m_Error = vError;
            m_ErrorMessage = vErrorMessage;
        }

        /// <summary>
        /// Get error code.
        /// </summary>
        public E_SocketError Error
        {
            get { return m_Error; }
        }

        /// <summary>
        /// Get error message.
        /// </summary>
        public string ErrorMessage
        {
            get { return m_ErrorMessage; }
        }
    }
    
}
