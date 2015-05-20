using System;
using System.Collections;
using System.Threading;
using XBase.Function.Collection;

namespace XBase.Thread
{
	/// <summary>
	/// XSingleThread Class
	/// </summary>
	public class XSingleThread : System.IDisposable
	{
		private System.Threading.Thread m_RunThread = null;
		private System.Threading.ManualResetEvent m_RunEvent = null;
		private System.Threading.ManualResetEvent m_CloseEvent = null;
		private System.Threading.ManualResetEvent[] m_Events = null;
		private Hashtable	m_Param = new Hashtable();

		public event XBase.XEventHandler RunHandler = null;

		public XSingleThread(string vThreadName)
		{
			// Update Thread Event
			m_RunEvent	= new System.Threading.ManualResetEvent(false);
			m_CloseEvent		= new System.Threading.ManualResetEvent(false);
			m_Events		= new System.Threading.ManualResetEvent[]{ m_CloseEvent, m_RunEvent };

			m_RunThread	= new System.Threading.Thread( new System.Threading.ThreadStart(RunThread));
			m_RunThread.Name = vThreadName;
			m_RunThread.Start();
		}

		public void Run()
		{
			m_RunEvent.Set();
		}

		public void Stop()
		{
			HashtableFunction.SyncronizedSet(m_Param, "IsStop", true );
		}

		public void Dispose()
		{
			HashtableFunction.SyncronizedSet(m_Param, "IsStop", true );
			m_CloseEvent.Set();

			// Disposing
			int i = 0;
			for( ; i < 30; i++ )
			{
				if( !m_RunThread.IsAlive ) break;
				System.Threading.Thread.Sleep(100);
			}

			if( i >= 30 ) m_RunThread.Abort();

			m_CloseEvent.Close();
			m_RunEvent.Close();
		}

		private void RunThread()
		{
			while( true )
			{
				int iIndex = System.Threading.WaitHandle.WaitAny( m_Events );
 
				if( iIndex == 0 ) break; // Stopped

				if( iIndex == 1 )
				{
					HashtableFunction.SyncronizedSet(m_Param, "IsStop", false );
					if( RunHandler != null )
					{
						RunHandler( m_Param );						
					}
					this.m_RunEvent.Reset();
					HashtableFunction.SyncronizedSet(m_Param, "IsStop", true );
				}
#if DEBUG
				else
				{
					throw new InvalidOperationException("Error Generate");

				}
#endif
			}
		}
	}
}
