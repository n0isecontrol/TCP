using System;
using System.Threading; 
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using XBase;
using XBase.Collection;

namespace XBase.Thread
{
	public enum  E_EventPoolError
	{
		NOERROR = 0,	
		OVERFLOW_THREACOUNT,	
		ERR_NOSTOP
	}

	public enum E_ThreadState
	{
		STOPPED = 0,
		STARTED,
		EXITED
	}

	public class EventPoolException : System.Exception 
	{
		public E_EventPoolError m_ErrorCode;

		public EventPoolException(E_EventPoolError errCode ) 
		{
			m_ErrorCode = errCode;
		}
	}

	public delegate void PoolEvent( EventPoolArg param );	

	public delegate void WorkCompletedHandler();

	public class EventPoolArg
	{
		private EventPool m_MainThread; 

		public object Tag;

		public object Param;

		internal EventPoolArg( EventPool poolThread, object tag, object param )
		{
			m_MainThread = poolThread;
			Param = param;
			Tag = tag;
		}

		public bool IsThreadStopped
		{
			get
			{
				return !m_MainThread.IsBusy ;
			}
		}
	}

	public class EventPool : IDisposable
	{
		public		const	int			MAX_THREADCOUNT = 32;

		internal	ManualResetEvent	SIGNAL_PROCESS;	
		internal	ManualResetEvent	SIGNAL_PROCESSED;	 
		internal	ManualResetEvent[]	SIGNAL_TABLE;
		
		internal	Mutex				MUTEX_RUNTHREADCOUNT;
		internal	Mutex				MUTEX_THREADCOUNT;
		
		internal	E_ThreadState		m_ThreadState	= E_ThreadState.STOPPED;

		private		PoolEventQueue		m_EventQueue	= null;
		private		ArrayList			m_Threads		= null;

		private		int					m_ThreadCount	= 5;	
		private		int					m_RunningThreadCount = 0;
		

		private		string				m_ThreadName	= "VEP_TH";
		
		public		event	WorkCompletedHandler WorkCompleted	= null;

		public		bool	IsDisposed		= false;

		public EventPool()
		{
			InitConstructure(1, 1024, new int[1]{1});
		}

		public EventPool(string threadName, int nThreadCount, int vMaxQueueCapacity )
		{
			m_ThreadCount		= nThreadCount;
			m_ThreadName		= threadName;

			InitConstructure(1, vMaxQueueCapacity,  new int[1]{1} );
		}

		public EventPool(string threadName, int nThreadCount, int vPriorityMax , int vMaxQueueCapacity, int[] vWeights)
		{
			m_ThreadCount		= nThreadCount;
			m_ThreadName		= threadName;

			InitConstructure(vPriorityMax, vMaxQueueCapacity,  vWeights );
		}

		private void InitConstructure( int vPriorityMax , int vMaxQueueCapacity, int[] vWeights )
		{
			MUTEX_RUNTHREADCOUNT	= new Mutex();
			MUTEX_THREADCOUNT	= new Mutex();
			SIGNAL_PROCESS		= new ManualResetEvent(false);
			SIGNAL_PROCESSED	= new ManualResetEvent(true);
			SIGNAL_TABLE		= new ManualResetEvent[]{ SIGNAL_PROCESS, SIGNAL_PROCESSED };

			m_EventQueue		= new PoolEventQueue(vPriorityMax, vMaxQueueCapacity,  vWeights );
			m_Threads			= new ArrayList();
		
			for( int i = 0; i < m_ThreadCount; i++ )
			{
				string threadName = m_ThreadName + "_C" + i.ToString() + "_TH";
				VoidEventThread thread = new VoidEventThread(this, threadName);
				m_Threads.Add(thread);				
			}
		}

		public void ExitThreads()
		{
			if( m_ThreadState != E_ThreadState.EXITED )
			{
				SIGNAL_PROCESSED.Reset();
				m_ThreadState = E_ThreadState.EXITED; 
				SIGNAL_PROCESS.Set();

				System.Threading.Thread.Sleep(100);

				for( int i = 0; i < m_ThreadCount; i++ )
				{
					((VoidEventThread)m_Threads[i]).Abort();
				}
				m_ThreadState = E_ThreadState.EXITED; 
			}
		}

		public void Dispose()
		{
			if( IsDisposed ) return;
			
			if( m_ThreadState != E_ThreadState.EXITED )
			{
				SIGNAL_PROCESSED.Reset();
				m_ThreadState = E_ThreadState.EXITED; 
				SIGNAL_PROCESS.Set();
		
				SIGNAL_PROCESSED.WaitOne(500, false);

				SIGNAL_PROCESSED.Close() ;
				SIGNAL_PROCESS.Close() ;
			}

			MUTEX_THREADCOUNT.Close();
			MUTEX_RUNTHREADCOUNT.Close();
			
			for( int i = 0; i < m_ThreadCount; i++ )
			{
				((VoidEventThread)m_Threads[i]).Dispose();
				m_Threads[i] = null;					
			}
		
			m_Threads.Clear();
			m_Threads = null;
		
			m_EventQueue.Clear();
			m_EventQueue = null;			
 
			GC.SuppressFinalize( this );

			GC.Collect();
			GC.WaitForPendingFinalizers();

			IsDisposed = true;
		}

		public bool IsBusy
		{
			get
			{
				return (m_RunningThreadCount > 0 );
			}
		}

		public int ReadyCount
		{
			get
			{
				return m_EventQueue.Count;
			}
		}

		internal void IncreamentRunningThreadCount()
		{
			try
			{
				MUTEX_RUNTHREADCOUNT.WaitOne();
				m_RunningThreadCount++;		
				//				Console.WriteLine( "IncrementRunningThreadCount :"+ m_RunningThreadCount.ToString());
			}
			finally
			{
				MUTEX_RUNTHREADCOUNT.ReleaseMutex(); 
			}
		}

		internal void DecreamentRunningThreadCount()
		{
			int iValue = 0;
			
			MUTEX_RUNTHREADCOUNT.WaitOne();
			m_RunningThreadCount--;
			iValue = m_RunningThreadCount;
			MUTEX_RUNTHREADCOUNT.ReleaseMutex(); 
			
			//			Console.WriteLine( "DecrementRunningThreadCount :"+ iValue.ToString());
			if( iValue == 0 )
			{
				SIGNAL_PROCESSED.Set();					
				if( WorkCompleted != null ) WorkCompleted();
			}
		}

		internal void DecreamentThreadCount()
		{
			int iValue = 0;

			MUTEX_THREADCOUNT.WaitOne();
			m_ThreadCount--;
			iValue = m_ThreadCount;		
			MUTEX_THREADCOUNT.ReleaseMutex(); 

			//			Console.WriteLine( "DecrementThreadCount :"+ iValue.ToString());
			
			if( iValue == 0 ) SIGNAL_PROCESSED.Set();									
		}

		public void Start()
		{
			SIGNAL_PROCESSED.Reset();
			m_ThreadState = E_ThreadState.STARTED; 
			SIGNAL_PROCESS.Set();
		}

		public void Stop()
		{
			SIGNAL_PROCESSED.Reset();
			m_ThreadState = E_ThreadState.STOPPED; 
			SIGNAL_PROCESS.Set();
			while( IsBusy )
			{
				System.Windows.Forms.Application.DoEvents();
			}
		}

		public void WaitProcess()
		{
			while( IsBusy ) System.Threading.Thread.Sleep(100);
		}

		public object GetTag( int iIndex )
		{
			return ((VoidEventThread)m_Threads[iIndex]).Tag; 
		}

		public void SetTag( int iIndex, object tag )
		{
			((VoidEventThread)m_Threads[iIndex]).Tag = tag; 
		}

		public void AddEvent(PoolEvent evObj, object param )
		{
			m_EventQueue.Enqueue( new PoolEventItem(evObj, param), 0 );
 			
			if( this.m_ThreadState == E_ThreadState.STARTED )
			{
				Start();
			}
		}

		public void AddEvent(PoolEvent evObj, int vPriority, object param )
		{
			m_EventQueue.Enqueue( new PoolEventItem(evObj, param), vPriority );
 			
			if( this.m_ThreadState == E_ThreadState.STARTED )
			{
				Start();
			}
		}

		public void ClearEvents()
		{
			m_EventQueue.Clear();
		}

		public void ClearEvents( ItemCompareHandler vCompare, object vObject1 )
		{
			m_EventQueue.Clear( vCompare, vObject1 );
		}

		internal PoolEventItem GetEvent()
		{
			return (PoolEventItem)m_EventQueue.Dequeue(); 
		}
		
		public PoolEventQueue Events
		{
			get
			{
				return this.m_EventQueue;
			}
		}
	}


	internal class VoidEventThread : IDisposable
	{
		private		EventPool				m_MainThread = null;
		private		System.Threading.Thread		m_Thread	 = null;

		public		object	Tag			= null;
		
		public		bool				IsBusy	 = false;

		public		bool	IsDisposed	= false;

		public VoidEventThread( EventPool poolObject, string threadname )
		{
			m_MainThread = poolObject;
			m_Thread = new System.Threading.Thread( new ThreadStart( RunThread) );
			m_Thread.Name = threadname;
			m_Thread.Start();
		}

		public void Abort()
		{
			if( m_Thread != null )
			{
				m_Thread.Abort();
				m_Thread = null;
			}
		}

		public void Dispose()
		{
			if( IsDisposed ) return;
			
			m_MainThread = null;

			if( m_Thread != null )
			{
				if( m_Thread.ThreadState == ThreadState.Running )
					throw new EventPoolException( E_EventPoolError.ERR_NOSTOP );
 
				m_Thread = null;
			}

			GC.SuppressFinalize( this );
			IsDisposed = true;
		}

		private void RunThread()
		{
			try
			{
				PoolEventItem eventObject;

				while( true )
				{
					m_MainThread.SIGNAL_PROCESS.WaitOne(); 
					
					if( m_MainThread.m_ThreadState  == E_ThreadState.STARTED  )	
					{
						m_MainThread.IncreamentRunningThreadCount();
						while( m_MainThread.m_ThreadState ==  E_ThreadState.STARTED )
						{
							eventObject = m_MainThread.GetEvent();
							if( eventObject == null ) break;	
						
							eventObject.EventObject( new EventPoolArg( m_MainThread,  Tag, eventObject.ParamObject) );  
						}
						m_MainThread.DecreamentRunningThreadCount();					
						m_MainThread.SIGNAL_PROCESS.Reset(); 
					}
					else if( m_MainThread.m_ThreadState == E_ThreadState.EXITED ) 
					{
						m_MainThread.DecreamentThreadCount();
						break;
					}					
					else if( m_MainThread.m_ThreadState == E_ThreadState.STOPPED ) 
					{
						while( m_MainThread.IsBusy ) System.Threading.Thread.Sleep(100);
						m_MainThread.SIGNAL_PROCESS.Reset();
						m_MainThread.SIGNAL_PROCESSED.Set();
					}
				}
#if DEBUG
			}
			catch(System.Exception ex)
			{
				System.Diagnostics.Trace.WriteLine( ex.ToString());;
			}
#else
			}
			catch
			{
				;
			}
#endif
		}
	}


	public class PoolEventItem
	{
		public PoolEvent EventObject;
		public object	 ParamObject;

		public PoolEventItem( PoolEvent evObj, object param )
		{
			EventObject = evObj;
			ParamObject = param;
		}
	}

	public class PoolEventQueue : XPriorityQueue2, IDisposable
	{
		public PoolEventQueue(int vPriorityMax , int vMaxQueueCapacity, int[] vWeights) : base(vPriorityMax, vMaxQueueCapacity, vWeights)
		{
		}
		
		#region IDisposable
		public void Dispose()
		{
			Clear();
			GC.SuppressFinalize(false);
		}

		#endregion

	}
	
	
}

