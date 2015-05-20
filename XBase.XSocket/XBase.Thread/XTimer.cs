using System;
using System.Threading;

namespace XBase.Thread
{
	public class XTimer
	{
		private bool m_started = false;

		private bool m_disposed = false;

		private System.Threading.Thread m_Thread = null;

		public event XBase.VoidEventHandler  Tick = null;

		private int  m_Interval = 100;

		public XTimer()
		{
			m_Thread = new System.Threading.Thread( new ThreadStart( Run ));			
			m_Thread.Name = "XTIMER_TH";
			m_Thread.Start();
		}

		public bool Enabled
		{
			get
			{
				return m_started;
			}

			set
			{
				m_started = value;
			}
		}

		public int Interval
		{
			get
			{
				return this.m_Interval;
			}

			set
			{
				lock( this )
				{
					m_Interval = value;
				}
			}
		}

		public void Start()
		{
			m_started = true;
		}

		public void Stop()
		{
			m_started = false;
		}

		public void Dispose()
		{
			m_started = false;
			m_disposed = true;

			System.Threading.Thread.Sleep(100);

			if( this.m_Thread.IsAlive )
			{
				this.m_Thread.Abort();
			}
		}

		private void Run()
		{
			try
			{
				while(!m_disposed)
				{
					if( m_started )
					{
						if( Tick != null )	Tick();

						System.DateTime dateTime = DateTime.Now;
						System.TimeSpan diffTime = System.TimeSpan.FromMilliseconds(0);

						while( diffTime.TotalMilliseconds < m_Interval && m_started )
						{
							System.Threading.Thread.Sleep( 10 );
							diffTime = DateTime.Now - dateTime;
						}
					}
					else
					{
						System.Threading.Thread.Sleep(10);
					}
				}
			}
			catch
			{
				;
			}
		}
	}


}
