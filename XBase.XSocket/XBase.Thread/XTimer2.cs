using System;
using System.Collections;
using System.Threading;

namespace XBase.Thread
{
	public class XTimer2
	{
		private ArrayList m_TimerObjects = null;

		private bool m_disposed = true;

		private System.Threading.Thread m_Thread = null;

		internal static ArrayList Timers = null;
		
		private XTimer2(string vName)
		{
			this.Name = vName;
			this.m_TimerObjects = new ArrayList();		
		}

		public void Dispose()
		{
			lock( this )
			{
				m_disposed = true;
			}

			System.Threading.Thread.Sleep(20);

			if( this.m_Thread.IsAlive )
			{
				this.m_Thread.Abort();
				this.m_Thread = null;
			}
		}

		public string Name = "Timer";

		public bool IsDisposed
		{
			get
			{
				bool retValue = false;
				lock( this )
				{
					retValue = this.m_disposed;
				}

				return retValue;
			}
		}

		static XTimer2()
		{
			Timers = new ArrayList();
		}

		public static XTimer2 CreateTimer( string vName )
		{
			XTimer2 tTimer = GetTimer( vName );

			if( tTimer != null ) return tTimer;
			tTimer = new XTimer2(vName);

			Timers.Add( tTimer );

			return tTimer;
		}

		public static void CloseTimer(string vName)
		{
			int tIndex = GetTimerIndex( vName );
			if( tIndex < 0 ) return ;

			XTimer2 tTimer = (XTimer2)Timers[tIndex];
			tTimer.Dispose();
			Timers.RemoveAt( tIndex );
		}

		public static int GetTimerIndex(string vName)
		{
			for( int i = 0; i < Timers.Count; i++ )
			{
				XTimer2 tTimer = (XTimer2)Timers[i];
				if(tTimer.Name == vName ) return i;
			}
			return -1;
		}

		public static XTimer2 GetTimer(string vName)
		{
			int tIndex = GetTimerIndex( vName );
			if( tIndex < 0 ) return null;
			return (XTimer2)Timers[tIndex];
		}


		public XTimerObject AddTimer( int vInterval, XBase.VoidEventHandler vTick, bool vEnable )
		{
			XTimerObject vTimer = new XTimerObject();
			vTimer.Interval		= vInterval;
			vTimer.TickHandler	= vTick;
			vTimer.Enabled		= vEnable;

			return AddTimer( vTimer );
		}

		public XTimerObject AddTimer( int vInterval, XBase.XEventHandler vTick, bool vEnable )
		{
			return AddTimer(vInterval, vTick, vEnable, false, true );
		}

		public XTimerObject AddTimer( int vInterval, XBase.XEventHandler vTick, bool vEnable, bool vIsStart, bool vBaseStart )
		{
			return AddTimer(vInterval, vTick, vEnable, vIsStart, vBaseStart, null);
		}

		public XTimerObject AddTimer( int vInterval, XBase.XEventHandler vTick, bool vEnable, bool vIsStart, bool vBaseStart, XBase.Thread.EventPool vEventPool )
		{
			XTimerObject vTimer		= new XTimerObject(vEventPool);
			vTimer.Interval			= vInterval;
			vTimer.EventTickHandler	= vTick;
			vTimer.Enabled			= vEnable;
			vTimer.InitActivated	= vIsStart;
			vTimer.IsBasedStart		= vBaseStart;
			
			return AddTimer(vTimer);
		}
		public XTimerObject AddTimer( XTimerObject vTimer )
		{
			lock( m_TimerObjects.SyncRoot )
			{
				m_TimerObjects.Add(vTimer);

				if( this.m_TimerObjects.Count == 1 && IsDisposed )
				{
					if( m_Thread != null && m_Thread.IsAlive ) m_Thread.Abort();
					
					lock( this )
					{
						m_disposed = false;
					}
					m_Thread = new System.Threading.Thread( new ThreadStart( Run ));			
					m_Thread.Name = "XTIMER2_" + this.Name;
					m_Thread.Start();
				}
			}

			return vTimer;
		}


		public void RemoveTimer( XTimerObject vTimer )
		{
			lock( m_TimerObjects.SyncRoot )
			{
				m_TimerObjects.Remove( vTimer );
			}
		}


		public ArrayList GetTimerObjects()
		{
			return (ArrayList)this.m_TimerObjects.Clone();
		}
		
		private void Run()
		{
			try
			{
				bool blDisposed = IsDisposed;
				while(!blDisposed)
				{
					ArrayList tTimers = (ArrayList)m_TimerObjects.Clone();
					
					if( tTimers.Count == 0 )
					{
						lock(this)
						{
							m_disposed = true;
						}
						break;
					}

					int tInterval = 10;
					for( int i = 0; i < tTimers.Count && !blDisposed; i++ )
					{
						XTimerObject tTimer = (XTimerObject)tTimers[i];

						if(  tTimer.BeforeTick() )
						{
							tTimer.Tick();
						}

						if( tTimer.EndTick() )
						{
							this.RemoveTimer( tTimer );
						}

						blDisposed = IsDisposed;
					} // for i

					blDisposed = IsDisposed;
					if( !blDisposed && tInterval >= 10 ) 
					{
						System.Threading.Thread.Sleep( tInterval  );
					}
				}
			}
			
#if DEBUG
			catch(System.Exception ex)
			{
				System.Diagnostics.Trace.WriteLine( ex.ToString());;
			}
#else
			catch
			{
				;
			}
#endif
		}

	}

	public class XTimerObject
	{
		public const int E_NOLIMIT			= 0;

		public const int E_COUNTLIMIT		= 1;

		public const int E_DATETIMEOVERLIMIT = 2; 

		private bool m_enabled = true;

		private int  m_Interval = 100;

		private bool m_initActivated = false;

		private bool m_isBasedStart  = true;

		private bool m_started = false;

		private DateTime m_laststart = DateTime.MinValue;

		private DateTime m_lastend	  = DateTime.MinValue;

		private XBase.Thread.EventPool	m_EventPool = null;

		private int		m_TotalCount			= 0;

		private int		m_EnableCount			= 0;

		private int	m_LimitEnd			= E_NOLIMIT;

		private int		m_EndCount				= 1;

		private DateTime	m_EndDateTime		= DateTime.MinValue;

		private object m_Tag		 = null;
		
		public XTimerObject()
		{	
		}
		
		public XTimerObject( XBase.Thread.EventPool vEventPool )
		{
			m_EventPool = vEventPool;
		}

		public XBase.XEnableHandler BeforeTickHandler = null;
		public XBase.XEnableHandler EndTickHandler = null;

		public XBase.VoidEventHandler  TickHandler = null;
		public XBase.XEventHandler	   EventTickHandler = null;

		public bool Enabled
		{
			get	{ return m_enabled; }
			set	
			{ 
				if( m_enabled != value )
				{
					m_enabled = value;					

					lock( this )
					{
						if( m_enabled ) m_EnableCount = 0;
					}
				}
			}
		}

		public bool Running
		{
			get{ return m_started; }			
		}

		public int Interval
		{
			get	{ return this.m_Interval;}
			set	
			{
				lock( this )
				{
					m_Interval = value;
				}
			}
		}

		public DateTime LastStartedTime
		{
			get{ return this.m_laststart; }
		}

		public DateTime LastEndTime
		{
			get{ return this.m_lastend; }
		}

		public bool InitActivated
		{
			get	{ return this.m_initActivated; }
			set { this.m_initActivated = value; }
		}

		public bool IsBasedStart
		{
			get{ return this.m_isBasedStart; }
			set{ m_isBasedStart = value; }
		}

		public int		TotalCount
		{
			get{return this.m_TotalCount;}
		}

		public int		EndCount
		{
			get{ return this.m_EndCount; }
			set{ this.m_EndCount = value; }
		}

		public DateTime	EndDateTime
		{
			get{ return this.m_EndDateTime; }
			set{ this.m_EndDateTime = value; }
		}

		public int	LimitEnd
		{
			get{ return this.m_LimitEnd; }
			set{ this.m_LimitEnd = value; }
		}

		public object Tag
		{
			get{ return this.m_Tag; }
			set{ this.m_Tag = value; }
		}

		#region Public 함수 정의 --------------------------------------------------------
		
		public void Tick()
		{	
			if( this.m_started ) return;

			this.m_started = true;

			this.m_laststart = DateTime.Now;

			this.m_TotalCount ++;
			this.m_EnableCount ++;
			
			if( this.m_EventPool != null )
			{
				XBase.Thread.PoolEvent eventObj = new XBase.Thread.PoolEvent(OnPoolEvent);
				this.m_EventPool.AddEvent( eventObj, this );
				this.m_EventPool.Start();
			}
			else
			{
				this.InternalTick();
			}
		}
	

		public bool BeforeTick()
		{
			if( this.m_started ) return false;

			if( !this.m_enabled ) return false;


			if( BeforeTickHandler != null )
			{
				Hashtable hs = new Hashtable();
				hs.Add( "TimerObject", this );
				return (BeforeTickHandler(hs) == 0);
			}

			return DefaultBeforeTick();
		}

		public bool DefaultBeforeTick()
		{
			if( this.m_initActivated )
			{
				if( this.LastStartedTime == DateTime.MinValue )
				{
					return true;
				}
			}

			TimeSpan ts;
			if( m_isBasedStart )
			{
				ts = DateTime.Now - this.m_laststart;
			}
			else
			{
				ts = DateTime.Now - this.m_lastend;
			}

			if( ts.TotalMilliseconds >= this.Interval ) return true;
			
			return false;
		}
	

		public bool EndTick()
		{			
			if( EndTickHandler != null)
			{
				Hashtable hs = new Hashtable();
				hs.Add( "TimerObject", this );

				return (EndTickHandler(hs) == 0);
			}

			return DefaultEndTick();
			
		}

		public bool DefaultEndTick()
		{
			switch( this.m_LimitEnd )
			{
				case E_DATETIMEOVERLIMIT:
					return ( DateTime.Now > this.m_EndDateTime);

				case E_COUNTLIMIT:
					return ( this.m_TotalCount >= this.m_EndCount );

				default :
					break;
			}
			return false;
		}
		
		private void OnPoolEvent(EventPoolArg arg)
		{
			XTimerObject tTimerObject = (XTimerObject)arg.Param;			
			tTimerObject.InternalTick();
		}

		private void InternalTick()
		{
			if( this.TickHandler != null )
			{
				this.TickHandler();
			}
			
			if( this.EventTickHandler != null )
			{
				Hashtable hs = new Hashtable();
				hs.Add( "TimerObject", this);
				this.EventTickHandler(hs);
			}

			this.m_started = false;

			this.m_lastend = DateTime.Now;
		}

	}

	
}
