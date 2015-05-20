using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;


using System.Threading;
using System.Net;
using System.Net.Sockets;

using XBase;
using XBase.Framework;
using XBase.Thread;


namespace XBase.XSocket
{
    /// <summary>
    /// Client Thread Container
    /// </summary>
    public class XClientContainer
    {
        // Members
        #region Members ----------------------------------

        /// <summary>
        /// Listener Socket
        /// </summary>
        //private XTcpSocket        mTcpListenSocket = null;
        private Socket              mTcpListenSocket = null;

        /// <summary>
        /// Listener Thread
        /// </summary>
        private XThread             mListnerThread = null;

        /// <summary>
        /// Listener Event
        /// </summary>
        private ManualResetEvent    mListenEvent = new ManualResetEvent(false);

        /// <summary>
        /// Stop Flag
        /// </summary>
        private bool                mbStop = true;

        /// <summary>
        /// Is Disposed
        /// </summary>
        private bool                mIsDisposed = false;

        /// <summary>
        /// Client Collection
        /// </summary>
        private ArrayList           mClientCollection = null;
                
        /// <summary>
        /// The socket error event handler
        /// </summary>
        public SocketErrorEventHandler SocketError;

        /// <summary>
        /// The socket accept event handler
        /// </summary>
        public SocketAcceptedEventHandler SocketAccepted;

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
        public XClientContainer()
        {
            mClientCollection = new ArrayList();
        }

        /// <summary>
        /// Dispose Method
        /// </summary>
        public void Dispose()
        {
            if (mIsDisposed == false)
            {
                Stop();
            }
            mIsDisposed = true;
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
	    /// Get Client Count
	    /// </summary>
	    /// <returns></returns>
	    public int GetCount()
        {
            return mClientCollection.Count;
        }

        /// <summary>
        /// Get Client
        /// </summary>
        /// <param name="vIndex">Index</param>
        /// <returns>XClient Object</returns>
        public XClient GetClient(int vIndex)
        {
            return (XClient)mClientCollection[vIndex];
        }

        /// <summary>
        /// Get Client
        /// </summary>
        /// <param name="vIndex">Index</param>
        /// <returns>XClient Object</returns>
        public XClient GetClient(string vIPAddress)
        {
            int vIndex = Find(vIPAddress);

            if (vIndex >= 0)
                return GetClient(vIndex);

            return null;
        }

       
	    /// <summary>
	    /// Find
	    /// </summary>
	    /// <param name="vIPAddress"></param>
	    public int Find(string vIPAddress )
        {
            for (int i = 0; i < mClientCollection.Count; i++  )
            {
                XClient tClient = (XClient)mClientCollection[i];

                if (tClient.GetIPAddress().Equals(vIPAddress) == true)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Add client
        /// </summary>
        /// <param name="vSocket"></param>
        /// <returns></returns>
        //public bool AddClient( XTcpSocket vSocket )
        public bool AddClient( Socket vSocket )
        {
            XClient tClient = new XClient(this, vSocket);
            tClient.Start(null, null);

            lock (mClientCollection.SyncRoot)
            {
                mClientCollection.Add(tClient);
            }
            return true;
        }

        /// <summary>
        /// Remove client
        /// </summary>
        /// <param name="vIPAddress"></param>
        /// <returns></returns>
        public bool RemoveClient(XClient tClient)
        {
            return RemoveClient(tClient.GetIPAddress());
        }

        /// <summary>
        /// Remove client
        /// </summary>
        /// <param name="vIPAddress"></param>
        /// <returns></returns>
	    public bool RemoveClient( string vIPAddress )
        {
            XClient tClient = null;            
            
            lock (mClientCollection)
            {
                int tIndex = Find(vIPAddress);
                
                if (tIndex >= 0)
                {
                    tClient = (XClient)mClientCollection[tIndex];
                    mClientCollection.RemoveAt(tIndex);
                }
            }

            if( tClient != null )
            {
                tClient.Dispose();
                tClient = null;
            }

            return true;
        }

        /// <summary>
        /// Listner Start
        /// </summary>
        /// <param name="vIP"></param>
        /// <param name="vPort"></param>
        /// <returns></returns>

        public bool Start(string vIP, string vPort)
        {
            if (mbStop == false)
                return true;

            XSimpleOption tOption = new XSimpleOption();
            tOption.SetOption("ip", vIP);
            tOption.SetOption("port", vPort);

            mbStop = false;

            mListnerThread = new XThread(new XThreadEventHandler(DoListener), tOption);
            mListnerThread.Run();

            return true;
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

                for (int i = 0; i < mClientCollection.Count; i++ )
                {
                    XClient tClient = (XClient)mClientCollection[i];
                    tClient.Dispose();
                }

                mClientCollection.Clear();

                mbStop = true;
                mListnerThread.Dispose();
                mListnerThread = null;

                return true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

            return false;
        }

        #endregion Methods -------------------------------

        #region Events -----------------------------------
        /// <summary>
        /// Do Listner Process
        /// </summary>
        /// <param name="vParam"></param>

        private void DoListener(object vParam)
        {
            XSimpleOption vOption = (XSimpleOption)vParam;

            string vIP = vOption.GetOption("ip");
            string vPort = vOption.GetOption("port");

            int nPort = 55555;
            try
            {
                nPort = Int32.Parse(vPort);
            }
            catch
            {

            }

            IPEndPoint tEndPoint = new IPEndPoint(IPAddress.Any, nPort);

            try
            {
                mTcpListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mTcpListenSocket.Bind(tEndPoint);                
                mTcpListenSocket.Listen(5000);
                   

                while (mbStop == false)
                {
                    try
                    {
                        Socket clientSocket = mTcpListenSocket.Accept();
                        AddClient(clientSocket);
                    }
                    catch
                    {

                    }

                    System.Threading.Thread.Sleep(20);
                }
            }
            catch
            {
            }
            finally
            {
                mTcpListenSocket.Close();
                mTcpListenSocket = null;               
            }
        }

        /// <summary>
        /// Listener socket event - when sockect error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSocketError(object sender, SocketErrorEventArgs e)
        {
            if (SocketError != null)
                SocketError(sender, e);
        }

        #endregion Events -----------------------------------

    }
}



