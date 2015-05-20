using System;
using System.Collections.Generic;

using System.Text;

using System.Net;
using System.Net.Sockets;

namespace XBase.XSocket
{
    /// <summary>
    /// The data when used on socket communication
    /// </summary>
    public class XSocketData
    {
        /// <summary>
        /// Worker Socket
        /// </summary>
        private Socket m_WorkSocket;

        /// <summary>
        /// Socket Buffer Size
        /// </summary>
        private int m_BufferSize;

        /// <summary>
        /// Socket Buffer Variable
        /// </summary>
        private byte[] m_Buffers;

        /// <summary>
        /// Construct
        /// </summary>
        public XSocketData()
        {
            m_WorkSocket = null;
            m_BufferSize = 16384;
            Variable_Init();
        }

        /// <summary>
        /// XSocketData Construct
        /// </summary>
        /// <param name="vWorkSocket">Worker Socket</param>
        public XSocketData(Socket vWorkSocket)
        {
            m_WorkSocket = vWorkSocket;
            m_BufferSize = 65536;
            Variable_Init();
        }

        /// <summary>
        /// Socket Data Structure
        /// </summary>
        /// <param name="vWorkSocket">Worker Socket</param>
        /// <param name="vBufferSize">Socket Buffer Size</param>
        public XSocketData(Socket vWorkSocket, int vBufferSize)
        {
            m_WorkSocket = vWorkSocket;
            m_BufferSize = vBufferSize;
            Variable_Init();
        }

        /// <summary>
        /// Init buffer
        /// </summary>
        private void Variable_Init()
        {
            m_Buffers = new byte[m_BufferSize];
        }

        /// <summary>
        /// Get or Set Worker Socket
        /// </summary>
        public Socket WorkSocket
        {
            get { return m_WorkSocket; }
            set { m_WorkSocket = value; }
        }

        /// <summary>
        /// Setting Buffer Size
        /// </summary>
        public int BufferSize
        {
            get { return m_BufferSize; }
            set
            {
                if (m_BufferSize != value)
                {
                    m_BufferSize = value;
                    m_Buffers = null;
                    m_Buffers = new byte[value];
                }
            }
        }

        /// <summary>
        /// Get Buffer's byte data
        /// </summary>
        public byte[] Buffers
        {
            get { return m_Buffers; }
        }
    }
}
