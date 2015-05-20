/******************************************************************
 * Namesapce	XBase.Logging
 * Source	    XLogger.cs
 * 
 * Define class for logging.
 * This class is based to Log's queuing and Threadding - technique.
 * This class is tested by XBaseTester.
 * 
 ******************************************************************/
using System;
using System.IO;
using System.Collections;

using XBase;
using XBase.Thread;

namespace XBase.Util.Logging
{
    public class XLogger : System.IDisposable
    {
        /// <summary>
        /// Initialize
        /// </summary>
        private bool m_isInited = false;

        /// <summary>
        /// Logging File Name.
        /// </summary>
        private string m_LogFileName = "";
        private string m_InitLogFileName = "";

        private int m_InitDay = 0;

        public object LogParam = null;
        public XEventHandler Inited = null;
        public XEventHandler Closed = null;
        public XEventHandler Writed = null;


        private int m_Buffers = 16384;
        private int m_Buffers2 = 12288;
        private Queue m_Queue = null;

        private XBase.Thread.XTimer2 m_Timer = null;
        private XBase.Thread.XTimerObject m_TimerObject = null;

        public XLogger()
        {
        }

        public XLogger(string szLogFileName)
        {
            InitLog(szLogFileName, null, null, null, 1000);
        }

        public XLogger(string szLogFileName, XEventHandler hInited, XEventHandler hClosed, XEventHandler hWrited)
        {
            InitLog(szLogFileName, hInited, hClosed, hWrited, 1000);
        }


        public XLogger(string szLogFileName, int nBuffer)
        {
            if (nBuffer > m_Buffers) m_Buffers = nBuffer;

            InitLog(szLogFileName, null, null, null, 1000);
        }

        public XLogger(string szLogFileName, int nBuffer, XEventHandler hInited, XEventHandler hClosed, XEventHandler hWrited)
        {
            if (nBuffer > m_Buffers) m_Buffers = nBuffer;
            InitLog(szLogFileName, hInited, hClosed, hWrited, 1000);
        }

        public XLogger(string szLogFileName, XEventHandler hInited, XEventHandler hClosed, XEventHandler hWrited, int vTick)
        {
            if (vTick < 1000) vTick = 1000;
            InitLog(szLogFileName, hInited, hClosed, hWrited, vTick);
        }

        public XLogger(string szLogFileName, int nBuffer, XEventHandler hInited, XEventHandler hClosed, XEventHandler hWrited, int vTick)
        {
            if (nBuffer > m_Buffers) m_Buffers = nBuffer;

            if (vTick < 1000) vTick = 1000;
            InitLog(szLogFileName, hInited, hClosed, hWrited, vTick);
        }

        public void Dispose()
        {
            if (this.m_Timer != null && this.m_TimerObject != null)
            {
                this.m_Timer.RemoveTimer(this.m_TimerObject);
            }

            while (m_Queue.Count > 0)
            {
                this.WriteQueue();
            }

            if (Closed != null) Closed(null);
        }


        public void Write(string vText)
        {
            lock (m_Queue.SyncRoot)
            {
                this.m_Queue.Enqueue(vText);
            }

            if (Writed != null)
            {
                Hashtable hs = new Hashtable();
                hs.Add("logParam", LogParam);
                hs.Add("sender", this);
                hs.Add("text", vText);
                Writed(hs);
            }

            // Buffer Overflowed
            if (m_Queue.Count > m_Buffers2)
            {
#if DEBUGMARK
				System.Diagnostics.Trace.WriteLine("Log Overflow");
#endif
                WriteQueue();
            }
        }


        public string GetFileName()
        {
            return this.m_LogFileName;
        }

        internal void InitLog(string szLogFileName, XEventHandler hInited, XEventHandler hClosed, XEventHandler hWrited, int vTick)
        {
            if (m_isInited) throw new InvalidOperationException("Logging is Started");

            m_InitLogFileName = szLogFileName;
            m_LogFileName = InternalCreateLogFile(szLogFileName);

            m_Buffers2 = m_Buffers * 3 / 4;
            m_Queue = new Queue(m_Buffers);


            Inited = hInited;
            Closed = hClosed;
            Writed = hWrited;

            this.m_Timer = XTimer2.CreateTimer("XLOGTIMER");
            this.m_TimerObject = this.m_Timer.AddTimer(vTick, new XBase.VoidEventHandler(OnTick), true);

            if (Inited != null) Inited(null);

            m_isInited = true;
        }

        private void OnTick()
        {
#if DEBUGMARK
			System.Diagnostics.Trace.WriteLine("Log Tick");
#endif
            WriteQueue();
        }

        private void WriteQueue()
        {
            if (m_Queue.Count == 0) return;

            lock (this)
            {
                System.IO.StreamWriter wr = new StreamWriter(m_LogFileName, true, System.Text.Encoding.Unicode, 4096);

                try
                {
                    int day = System.DateTime.Now.Day;

                    if (this.m_InitDay != day)
                    {
                        this.m_LogFileName = this.InternalCreateLogFile(this.m_InitLogFileName);
                    }

                    string tLog;
                    while (m_Queue.Count > 0)
                    {
                        lock (m_Queue.SyncRoot)
                        {
                            tLog = (string)m_Queue.Dequeue();
                        }
                        wr.WriteLine(tLog);
                    }

                    wr.Flush();
                }
                finally
                {
                    wr.Close();
                }
            }
        }

        private string InternalCreateLogFile(string szLogFileName)
        {
            int year = System.DateTime.Now.Year;
            int month = System.DateTime.Now.Month;
            int day = System.DateTime.Now.Day;

            string suffix = year.ToString() + "_" + month.ToString() + "_" + day.ToString() + ".log";
            this.m_InitDay = day;

            szLogFileName += suffix;

            System.IO.FileStream tStream = null;

            try
            {
                if (!System.IO.File.Exists(szLogFileName))
                {
                    tStream = System.IO.File.Create(szLogFileName);
                }
            }
            finally
            {
                if (tStream != null) tStream.Close();
            }

            return szLogFileName;
        }
    }
}

