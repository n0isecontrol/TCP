using System;
using System.Collections.Generic;

using System.Text;


namespace XBase.XSocket
{
    public class Global
    {
        private delegate void WriteLogDelegate(string message);

        internal static XClientForm mApp = null;

        public static void WriteLog(string message)
        {
            if (mApp.textLog.InvokeRequired)
            {
                mApp.textLog.Invoke(new WriteLogDelegate(WriteLog), message);
            }
            else
            {
                DateTime t_dateTime = DateTime.Now;
                System.Threading.Thread tThread = System.Threading.Thread.CurrentThread;
                string tLog = t_dateTime.ToString() + "." + String.Format("{0:000}", t_dateTime.Millisecond) + " [" + tThread.GetHashCode() + " " + tThread.Name + "] " + message;

                mApp.textLog.Text = tLog + "\r\n" + mApp.textLog.Text;
            }
        }
    }
}
