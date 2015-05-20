using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;

using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;


using XBase;
using XBase.Database;
using XBase.Collection;
using XBase.Framework;
using XBase.Thread;
using XBase.Function.Serialize;

using XBase.App.Model;
using XBase.App.Controller;

namespace XBase.XSocket
{
    public partial class XServerForm : Form, XBase.IXApp
    {
        private XClientContainer mClientContainer = null;
                       
        public XServerForm()
        {
            InitializeComponent();

            XApp.SetApp(this);

            mClientContainer = new XClientContainer();
            mClientContainer.SocketAccepted = new SocketAcceptedEventHandler(OnSocketAccepted);
            mClientContainer.SocketError = new SocketErrorEventHandler(OnSocketError);
            mClientContainer.DataReceived = new SocketDataReceivedEventHandler(OnDataReceived);
            mClientContainer.DataSended = new SocketDataSendedEventHandler(OnDataSended);
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (mClientContainer.IsStop == true)
                {
                    XApp.WriteLog("start server");
                    mClientContainer.Start(textServerIP.Text, textServerPort.Text);
                }
                else
                {
                    XApp.WriteLog("stop server");
                    mClientContainer.Stop();    
                }
            }
            catch
            {
            }
        }

        private void buttonSendControl_Click(object sender, EventArgs e)
        {
            string ipAddress = textClientIP.Text;

            XClient tClient = mClientContainer.GetClient(ipAddress);

            if (tClient != null)
            {

                int tMessage = XProtocol.XMSG_NOP;
                String tData = "";
                try
                {
                    tMessage = Int32.Parse(textControlID.Text);
                }
                catch
                {
                }

                tData = textControlData.Text;

                XMessageAPI.SendCommand(tClient, tMessage, tData);
            }
        }

        private void buttonSendText_Click(object sender, EventArgs e)
        {
            string ipAddress = textClientIP.Text;

            XClient tClient = mClientContainer.GetClient(ipAddress);

            if (tClient != null)
            {
                String tData = textSend.Text;

                XMessageAPI.SendData(tClient, tData);
            }
            
        }

        private void buttonFileBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textSendFile.Text = openFileDialog1.FileName;
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textLog.Text = "";
        }

        private void buttonSendData_Click(object sender, EventArgs e)
        {
            string ipAddress = textClientIP.Text;

            XClient tClient = mClientContainer.GetClient(ipAddress);

            if (tClient != null)
            {
                String tFile = textSendFile.Text;

                if (String.IsNullOrEmpty(tFile) != true)
                {
                    XMessageAPI.SendFile(tClient, tFile);
                }
            }
        }

        private void buttonDBConnect_Click(object sender, EventArgs e)
        {
            string dbHost = textDBServer.Text;
            string dbDatabase = textDBName.Text;
            string dbUser = textDBUser.Text;
            string dbPassword = textDBPassword.Text;

            DataEngine.SetConnectionString(dbHost, dbDatabase, dbUser, dbPassword );

            DataEngine tEngine = new DataEngine();

            bool bLogin = LoginController.Login("test", "test", "");

            if( bLogin == true )
            {
                MessageBox.Show("Connection OK!");
            }
            else
            {
                MessageBox.Show("Connection Fail!");
            }
        }

        private void XServerForm_Load(object sender, EventArgs e)
        {
            IPHostEntry host;
            string localIP = "0.0.0.0";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }

            textServerIP.Text = localIP;

            System.Threading.Thread.CurrentThread.Name = "MTA";
        }        

        private void mUpdateTimer_Tick(object sender, EventArgs e)
        {
            textServerPort.Enabled = mClientContainer.IsStop;
            buttonStartServer.Text = mClientContainer.IsStop ? "Start" : "Stop";

            string tClientList = "";

            
            for (int i = 0; i < mClientContainer.GetCount(); i++)
            {
                XClient tClient = mClientContainer.GetClient(i);
                tClientList += tClient.GetIPAddress() + " \r\n";
            }

            textClientList.Text = tClientList;
        }


        private delegate void WriteLogDelegate(string message);

        public void WriteLog(string message)
        {
            if (this.textLog.InvokeRequired)
            {
                this.textLog.Invoke(new WriteLogDelegate(WriteLog), message);
            }
            else
            {
                DateTime t_dateTime = DateTime.Now;
                System.Threading.Thread tThread = System.Threading.Thread.CurrentThread;
                string tLog = t_dateTime.ToString() + "." + String.Format("{0:000}", t_dateTime.Millisecond) + " [" + tThread.GetHashCode() + " " + tThread.Name + "] " + message;

                this.textLog.Text = tLog + "\r\n" + this.textLog.Text;
            }
        }

        
        /// <summary>
        /// Listener socket event - when sockect accepted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSocketAccepted(object sender, SocketAcceptedEventArgs e)
        {
            XApp.WriteLog("accepted " + ((IPEndPoint)e.Socket.RemoteEndPoint).Address.ToString());
        }

        /// <summary>
        /// Listener socket event - when sockect error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSocketError(object sender, SocketErrorEventArgs e)
        {
            XApp.WriteLog("error");
        }

        private void OnDataSended(object sender, SocketDataSendedEventArgs e)
        {
            XApp.WriteLog("sended " + e.Message.ToString() );
        }

        private void OnDataReceived(object sender, SocketDataReceivedEventArgs e)
        {           
            if (e.Message != null)
            {
                DoMessageProcess(e.SenderIPAddress, e.Message);
            }
        }

        private void DoMessageProcess(String vClientIPAddress, XMessage tMessage)
        {
            if (tMessage != null)
            {
                if (tMessage.MessageId != XProtocol.XMSG_PING)
                {
                    XApp.WriteLog("received " + tMessage.ToString());
                }

                switch (tMessage.MessageId)
                {
                    case XProtocol.XMSG_AUTHENTICATION:                       
                        break;
                    case XProtocol.XMSG_CLIENTINIT:                        
                        break;
                    case XProtocol.XMSG_CLIENTEND:
                        mClientContainer.RemoveClient(vClientIPAddress);
                        break;
                    case XProtocol.XMSG_DATABEGIN:                        
                        break;
                    case XProtocol.XMSG_DATATRANSFER:                        
                        break;
                    case XProtocol.XMSG_DATAEND:                        
                        break;
                    case XProtocol.XMSG_PING:
                        break;
                }
            }
        }

    }
}
