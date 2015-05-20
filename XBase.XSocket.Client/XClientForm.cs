using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;

using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using XBase;
using XBase.Collection;
using XBase.Framework;
using XBase.Thread;
using XBase.Function.Serialize;

namespace XBase.XSocket
{
    public partial class XClientForm : Form,  XBase.IXApp
    {
        private XClient mClient = null;

        public XClientForm()
        {
            InitializeComponent();

            XApp.SetApp(this);

            mClient = new XClient();
            mClient.SocketConnected = new SocketConnectedHandler(OnSocketConnected);
            mClient.SocketError = new SocketErrorEventHandler(OnSocketError);
            mClient.DataReceived = new SocketDataReceivedEventHandler(OnDataReceived);
            mClient.DataSended = new SocketDataSendedEventHandler(OnDataSended);

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if( mClient.IsStop == true )
                {
                    mClient.Start(textServerIP.Text, textServerPort.Text);
                }
                else
                {
                    mClient.Stop();
                }
            }
            catch
            {
            }
        }

        private void buttonSendControl_Click(object sender, EventArgs e)
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

            XMessageAPI.SendCommand(mClient, tMessage, tData);
        }

        private void buttonSendData_Click(object sender, EventArgs e)
        {
            String tData = textSendData.Text;
            XMessageAPI.SendData(mClient, tData);
        }

        private void buttonSendFile_Click(object sender, EventArgs e)
        {
            String tFile = textSendFile.Text;

            if( String.IsNullOrEmpty(tFile) != true )
            {
                XMessageAPI.SendFile(mClient, tFile);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.textLog.Text = "";
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textSendFile.Text = openFileDialog1.FileName;
            }
        }


        private void OnSocketConnected(object sender, SocketConnectedEventArgs e)
        {
            XApp.WriteLog("connected " + ((IPEndPoint) e.Socket.RemoteEndPoint).Address.ToString());
        }       

        /// <summary>
        /// Listener socket event - when sockect error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSocketError(object sender, SocketErrorEventArgs e)
        {
            XApp.WriteLog("error");

            mClient.Stop();
        }

        private void OnDataSended(object sender, SocketDataSendedEventArgs e)
        {
            XApp.WriteLog("sended " + e.Message.ToString());
        }

        private void OnDataReceived(object sender, SocketDataReceivedEventArgs e)
        {
            if (e.Message != null)
            {
                DoMessageProcess((XMessage)e.Message);
            }
        }

        private void DoMessageProcess(XMessage tMessage)
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

        private void mUpdateTimer_Tick(object sender, EventArgs e)
        {
            textServerIP.Enabled = mClient.IsStop;
            textServerPort.Enabled = mClient.IsStop;

            textControlID.Enabled = !mClient.IsStop;
            textControlData.Enabled = !mClient.IsStop;
            buttonSendControl.Enabled = !mClient.IsStop;
            textSendData.Enabled = !mClient.IsStop;
            buttonSendData.Enabled = !mClient.IsStop;
            textSendFile.Enabled = !mClient.IsStop;
            buttonBrowse.Enabled = !mClient.IsStop;
            buttonSendFile.Enabled = !mClient.IsStop;

            buttonConnect.Text = mClient.IsStop ? "Connect" : "Close";
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

                textLog.Text = tLog + "\r\n" + textLog.Text;
            }
        }

        private void XClientForm_Load(object sender, EventArgs e)
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
    }
}
