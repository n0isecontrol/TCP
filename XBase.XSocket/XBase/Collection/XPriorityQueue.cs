 
using System;
using System.Collections;

using XBase.Function.Collection;

namespace XBase.Collection
{
	[Serializable]
	public class XPriorityQueue  
	{
		public const int ENABLED_MAXPRIORITY = 32;

		
		protected int m_Count = 0;

		
		internal ArrayList m_Queues =  new ArrayList(ENABLED_MAXPRIORITY);

		
		internal int m_MaxPriority = 1;

		#region Constructor ------------------------------------------------------
		
		public XPriorityQueue(int vPriorityMax )
		{
#if DEBUG
			if( vPriorityMax <= 0 || vPriorityMax > ENABLED_MAXPRIORITY ) throw new IndexOutOfRangeException("Index Out Of range.");
#endif
			
			for( int i = 0; i < vPriorityMax ; i++ )
			{
				m_Queues.Add( new Queue(4096) );
			}

			m_MaxPriority = vPriorityMax;
		}

		public XPriorityQueue(int vPriorityMax , int vMaxQueueCapacity )
		{
#if DEBUG
            if (vPriorityMax <= 0 || vPriorityMax > ENABLED_MAXPRIORITY) throw new IndexOutOfRangeException("Index Out Of range.");
#endif
			for( int i = 0; i < vPriorityMax ; i++ )
			{
				m_Queues.Add( new Queue(vMaxQueueCapacity) );
			}

			m_MaxPriority = vPriorityMax;
		}
		#endregion --------------------------------------------------------------------

		#region Properties ------------------------------------------------------------------
		public object SyncRoot
		{
			get
			{
				return m_Queues.SyncRoot;
			}
		}

		public int Count
		{
			get
			{
				return m_Count;
			}
		}		
		#endregion --------------------------------------------------------------------

		#region Public Methods ---------------------------------------------------------

		public virtual void Enqueue( object vItem, int vPriority )
		{
#if DEBUG
			if( vPriority < 0 || vPriority >= m_Queues.Count )
				throw new IndexOutOfRangeException("OutOfRange" );
#endif
			Queue tQueue = (Queue)m_Queues[vPriority];

			lock( this.SyncRoot )
			{
				tQueue.Enqueue( vItem );
				m_Count++;
			}
			
		}

		public virtual object Dequeue()
		{
			object tResult = null;
			lock( this.SyncRoot )
			{
				for( int i = 0; i < m_Queues.Count; i++ )
				{
					Queue tQueue = (Queue)m_Queues[i];	

					if( tQueue.Count > 0 )
					{
						tResult =  tQueue.Dequeue();
						m_Count--;
						break;
					}
				}
			}

			return tResult;
		}

		
		public virtual void Clear()
		{
			object tObject = null;
			Queue tQueue = null;
			lock( this.SyncRoot )
			{
				for( int i = 0; i < m_Queues.Count; i ++ )
				{
					tQueue = ((Queue)m_Queues[i]);

					while( tQueue.Count > 0)
					{
						tObject = tQueue.Dequeue();
						if( tObject is IDisposable ) ((IDisposable)tObject).Dispose();
					}
				}				
				m_Count = 0;				
			}
		}

		public  virtual void Clear( ItemCompareHandler vCompare, object vObject1 )
		{
			ArrayList[] t_Arrays = new ArrayList[m_Queues.Count];

			for( int i = 0; i < t_Arrays.Length; i++ )
			{
				t_Arrays[i] = new ArrayList();
			}

			object tObject = null;
			Queue tQueue = null;

			lock( this.SyncRoot )
			{
				for( int i = 0; i < m_Queues.Count; i ++ )
				{
					tQueue = ((Queue)m_Queues[i]);

					while( tQueue.Count > 0)
					{
						tObject = tQueue.Dequeue();
						if( vCompare(vObject1, tObject) == 0 )
						{
							if( tObject is IDisposable ) ((IDisposable)tObject).Dispose();
						}
						else
						{
							t_Arrays[i].Add( tObject );
						}
					}
				}

				this.m_Count = 0;
				for( int i =0;  i < t_Arrays.Length; i++ )
				{
					m_Count += QueueFunction.AddQueue( (Queue)m_Queues[i], (ArrayList)t_Arrays[i]);
				}
			}
		}

		
		
		public virtual void AddPriorityQueue(XPriorityQueue vQueue)
		{
			if( vQueue.m_MaxPriority <= this.m_MaxPriority )
			{
				for(int i = 0; i < vQueue.m_MaxPriority; i++ )
				{
					Queue tSrcQueue = (Queue)vQueue.m_Queues[i];
					Queue tDstQueue = (Queue)m_Queues[i];
				
					lock( this.SyncRoot )
					{			
						m_Count += QueueFunction.AddQueue( tDstQueue, tSrcQueue  );
					}
				}
			}
			else
			{
				for(int i = 0; i < this.m_MaxPriority; i++ )
				{
					Queue tSrcQueue = (Queue)vQueue.m_Queues[i];
					Queue tDstQueue = (Queue)m_Queues[i];
				
					lock( this.SyncRoot )
					{
						m_Count += QueueFunction.AddQueue( tDstQueue, tSrcQueue );
					}
				}

				for( int i = m_MaxPriority; i < vQueue.m_MaxPriority; i++ )
				{
					Queue tQueue = (Queue)vQueue.m_Queues[i];

					lock( this.SyncRoot)
					{
						m_Queues.Add( tQueue );
						m_Count += tQueue.Count;
						m_MaxPriority++;	
					}
				}
			}			
		}

		public virtual XPriorityQueue Copy()
		{
			XPriorityQueue tObject = new XPriorityQueue( this.m_MaxPriority);

			lock( this.SyncRoot )
			{	
				for( int i = 0; i < this.m_MaxPriority; i++ )
				{
					Queue tQueue = (Queue)this.m_Queues[i];
					tObject.m_Queues[i] = tQueue.Clone();
				}
				tObject.m_Count = this.m_Count;
			}

			return tObject;
		}

		public virtual XPriorityQueue Move()
		{
			XPriorityQueue tObject = new XPriorityQueue( this.m_MaxPriority);

            lock( this.SyncRoot )
			{	
				object tTempQueue = null;
				for( int i = 0; i < this.m_MaxPriority; i++ )
				{
					tTempQueue = tObject.m_Queues[i];
                    tObject.m_Queues[i] = this.m_Queues[i];
					this.m_Queues[i] = tTempQueue;
#if DEBUG
					if( tTempQueue == null || !(tTempQueue is Queue)  ) throw new InvalidCastException( "Invalid cast queue exception" );
#endif
				}
				tObject.m_Count = this.m_Count;
				this.m_Count = 0;
			}

			return tObject;
		}


		public Queue getQueue(int vIndex)
		{
			return (Queue)this.m_Queues[vIndex];			
		}
		#endregion

	}
}
