using System;
using System.Threading;

namespace XBase.Thread
{
	/// <summary>
	/// XThread Class
	/// </summary>
	public class XThread
	{
		private bool   m_IsDisposed = false;
		private object m_Tag = null;
		private System.Threading.Thread m_Thread  = null;
		private XThreadEventHandler m_ThreadEventHandler = null;
		
		public XThread(XThreadEventHandler vThreadHandler, object vParam)
		{
			if( vThreadHandler == null ) throw new InvalidOperationException("The thread handler is null");

			m_Thread = new System.Threading.Thread( new System.Threading.ThreadStart(RunThread));
			m_Thread.IsBackground = true;
			m_Thread.Name = "XTH";

			m_Tag = vParam;
			m_ThreadEventHandler = vThreadHandler;
		}

		public void Run()
		{
			m_Thread.Start();
		}


		public void Dispose()
		{
			if( !m_IsDisposed )
			{
                //this.m_Thread.Abort();
				m_IsDisposed = true;
			}
		}
		private void RunThread()
		{
			m_ThreadEventHandler( m_Tag );			
			m_IsDisposed = true;
		}
	}

	public delegate void XThreadEventHandler(object vParam);
}
