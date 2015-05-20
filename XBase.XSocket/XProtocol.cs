using System;
using System.Collections.Generic;

using System.Text;

using System.IO;

using XBase.Function.Serialize;

namespace XBase.XSocket
{
    /// <summary>
    /// Header Define
    /// </summary>
    public class XProtocol
    {
        // xMessage Type
        #region xMessage Type
  
        public const int TYPE_INTEGER    = 0;
        public const int TYPE_STRING     = 1;
        public const int TYPE_INTSTRING  = 2;
        public const int TYPE_BINARY     = 3; 

        #endregion // xMessage Type


        // xMessage Level
        #region xMessage Level

        public const int LEVEL_HIGHEST   = 0;
        public const int LEVEL_HIGH      = 1;
        public const int LEVEL_NORMAL    = 2;
        public const int LEVEL_LOW       = 3;

        public const int LEVEL_MAX       = 3;
        #endregion // xMessage Level

        
        // xMessage ID
        #region xMessage ID
        
        // Highest Level Message
        public const int XMSG_NOP             = 0;

        public const int XMSG_PING            = 10;
        public const int XMSG_AUTHENTICATION  = 50;
        public const int XMSG_CLIENTINIT      = 51;
        public const int XMSG_CLIENTEND       = 52;

        // High Level Message

        // Normal Level Message
        public const int XMSG_COMMAND         = 300;
        public const int XMSG_DATA            = 301;

        // Low Level Message
        public const int XMSG_DATABEGIN       = 1001;
        public const int XMSG_DATATRANSFER    = 1002;
        public const int XMSG_DATAEND         = 1003;

        #endregion // xMessage ID

        // xMessage Data Type
        #region // xMessage Data Type

        public const int XMSG_DATA_BLOCKSIZE = 262144;

        public const string XMSG_DATA_HEADER_FORMAT = "type:{0}, name:{1}, sessionid:{2}, size:{3}";
        
        public const string XMSG_DATATYPE_FILE = "file";
        public const string XMSG_DATATYPE_BINARY = "bin";

        #endregion // xMessage Data Type
    }

  
    /// <summary>
    /// Message Class
    /// </summary>
    [Serializable]
    public class XMessage
    {
        /*
         * Message Header 
         */
        private int     mId;
        private int     mLevel;
        private int     mType;

        /*
         * Message Content
         */
        private long    mValue;
        private String  mContent;
        private byte[]  mData;

        // Constructor
        #region Constructor

        public XMessage(int vMessage, long vValue)
        {
            Initialize(vMessage, XProtocol.TYPE_INTEGER, XProtocol.LEVEL_NORMAL, vValue, String.Empty, null);
        }

        public XMessage(int vMessage, string vMessageContent)
        {
            Initialize(vMessage, XProtocol.TYPE_STRING, XProtocol.LEVEL_NORMAL, 0, vMessageContent, null);
        }

        public XMessage(int vMessage, long vValue, int vLevel)
        {
            Initialize(vMessage, XProtocol.TYPE_INTEGER, vLevel, vValue, String.Empty, null);
        }

        public XMessage(int vMessage, string vMessageContent, int vLevel)
        {
            Initialize(vMessage, XProtocol.TYPE_STRING, vLevel, 0, vMessageContent, null);
        }

        public XMessage( int vMessage, byte[] vMessageData)
        {
            Initialize(vMessage, XProtocol.TYPE_BINARY, XProtocol.LEVEL_NORMAL, 0, String.Empty, vMessageData);
        }

        public XMessage(int vMessage, int vType, int vLevel, long vValue, string vContent, byte[] vMessageData)
        {
            Initialize(vMessage, vType, vLevel, vValue, vContent, vMessageData);
        }

        #endregion 


        #region Initialize Function
        private void Initialize(int vMessage, int vType, int vLevel, long vVaule, string vContent, byte[] vData )
        {
            mId = vMessage;
            mType = vType;
            mLevel = vLevel;
            mValue = vVaule;
            mContent = vContent;
            mData = vData;
        }
        #endregion // Initialize Function


        #region Properties

        public int MessageId
        {
            get
            {
                return mId;
            }

            set
            {
                mId = value;
            }
        }

        public int Level
        {
            get
            {
                return mLevel;
            }

            set
            {
                mLevel = value;
            }
        }

        public int Type
        {
            get
            {
                return mType;
            }

            set
            {
                mType = value;
            }
        }

        public long Value
        {
            get
            {
                return mValue;
            }

            set
            {
                mValue = value;
            }
        }


        public String Content
        {
            get
            {
                return mContent;
            }

            set
            {
                mContent = value;
            }
        }


        public byte[] Data
        {
            get
            {
                return mData;
            }

            set
            {
                mData = value;
            }
        }
        #endregion // Properties

        public override string ToString()
        {
            String resultString = "";
            switch (MessageId)
            {
                case XProtocol.XMSG_AUTHENTICATION:
                    resultString = " authentication ";
                    break;
                case XProtocol.XMSG_CLIENTINIT:
                    resultString = " client init ";
                    break;
                case XProtocol.XMSG_CLIENTEND:
                    resultString = " client end ";
                    break;
                case XProtocol.XMSG_COMMAND:
                    resultString = " command id: " + mValue + " data : " + mContent;
                    break;
                case XProtocol.XMSG_DATA:
                    resultString = " data : " + mContent ;
                    break;
                case XProtocol.XMSG_DATABEGIN:
                    resultString = " data begin header : " + mContent;
                    break;
                case XProtocol.XMSG_DATATRANSFER:
                    resultString = " data transfer header : " + mContent + " pos : " + mValue + " size : " + mData.Length ;
                    break;
                case XProtocol.XMSG_DATAEND:
                    resultString = " data end : " + mContent;
                    break;
                case XProtocol.XMSG_PING:
                    resultString = " data ping ";
                    break;
                case XProtocol.XMSG_NOP:
                    resultString = " nop ";
                    break;
            }

            return resultString;
        }
    }

    public class XMessageAPI
    {

        public static void SendData(XClient vClient, String vData)
        {
            XMessage vMessage = new XMessage(XProtocol.XMSG_DATA, XProtocol.TYPE_STRING, XProtocol.LEVEL_NORMAL, 0, vData, null);
            vClient.SendMessage(vMessage);
        }

        public static void SendCommand(XClient vClient, int vCmd, String vCmdData)
        {
            XMessage vMessage = new XMessage(XProtocol.XMSG_COMMAND, XProtocol.TYPE_INTSTRING, XProtocol.LEVEL_NORMAL, vCmd, vCmdData, null);
            vClient.SendMessage(vMessage);
        }

        public static void SendFile(XClient vClient, String vFilePath)
        {

            FileStream tStream = null;
            BinaryReader tReader = null;

            try
            {
                FileInfo tFileInfo = new FileInfo(vFilePath);

                long iSize = tFileInfo.Length;

                String tSesionId = vClient.GetSessionID();

                String tHeader = String.Format(XProtocol.XMSG_DATA_HEADER_FORMAT, XProtocol.XMSG_DATATYPE_BINARY, tFileInfo.Name, tSesionId, iSize);

                XMessage tBegin = new XMessage(XProtocol.XMSG_DATABEGIN, tHeader, XProtocol.LEVEL_LOW);
                XMessage tEnd = new XMessage(XProtocol.XMSG_DATAEND, tHeader, XProtocol.LEVEL_LOW);

                tStream = new FileStream(vFilePath, FileMode.Open);
                tReader = new BinaryReader(tStream);

                vClient.SendMessage(tBegin);

                long iPos1 = 0;
                long iPos2 = 0;

                while (iPos1 < iSize)
                {
                    iPos2 = iPos1 + XProtocol.XMSG_DATA_BLOCKSIZE;

                    if (iPos2 > iSize)
                    {
                        iPos2 = iSize;
                    }

                    byte[] tBuffer = tReader.ReadBytes((int)(iPos2 - iPos1));

                    XMessage tMessage = new XMessage(XProtocol.XMSG_DATATRANSFER, XProtocol.TYPE_BINARY, XProtocol.LEVEL_LOW, iPos1, tHeader, tBuffer);

                    vClient.SendMessage(tMessage);

                    System.Threading.Thread.Sleep(20);
                    iPos1 = iPos2;
                }

                vClient.SendMessage(tEnd);

            }
            catch
            {
            }
            finally
            {
                if (tReader != null)
                {
                    tReader.Close();
                    tReader = null;
                }

                if( tStream != null )
                {
                    tStream.Close();
                    tStream = null;
                }
            }
        }

        public static void SendLargeData(XClient vClient, byte[] vLargeData)
        {            
            String tSesionId = vClient.GetSessionID();
            String tHeader = String.Format( XProtocol.XMSG_DATA_HEADER_FORMAT, XProtocol.XMSG_DATATYPE_BINARY, "-", tSesionId);

            XMessage tBegin = new XMessage(XProtocol.XMSG_DATABEGIN, tHeader, XProtocol.LEVEL_LOW);
            XMessage tEnd = new XMessage(XProtocol.XMSG_DATAEND, tHeader, XProtocol.LEVEL_LOW);

            vClient.SendMessage(tBegin);

            int iSize = vLargeData.Length;
            int iSeq = 0;
            int iPos1 = 0;
            int iPos2 = 0;

            while (iPos1 < iSize)
            {
                iPos2 = iPos1 + XProtocol.XMSG_DATA_BLOCKSIZE;

                if (iPos2 > iSize)
                {
                    iPos2 = iSize;
                }

                byte[] tBuffer = new byte[iPos2 - iPos1];
                Buffer.BlockCopy( vLargeData, (int)iPos1, tBuffer, 0, (int)(iPos2 - iPos1) );

                XMessage tMessage = new XMessage(XProtocol.XMSG_DATATRANSFER, XProtocol.TYPE_BINARY, XProtocol.LEVEL_LOW, iPos1, tHeader, tBuffer);
                
                vClient.SendMessage(tMessage);

                System.Threading.Thread.Sleep(10);

                iSeq++;
                iPos1 = iPos2;
            }

            vClient.SendMessage(tEnd);
        }


        public static void SendStop(XClient vClient)
        {
            XMessage vMessage = new XMessage(XProtocol.XMSG_CLIENTEND, 0, XProtocol.LEVEL_HIGH);
            vClient.SendMessage(vMessage);
        }
    }
}
