
using System;
using System.Collections;

using XBase.Function.Collection;

namespace XBase.Collection
{
	[Serializable]
	public class XPriorityQueue2 : XPriorityQueue 
	{
		/// <summary>
		/// MaxCounter
		/// </summary>
 		private int m_MaxCounter = 100;

		/// <summary>
		/// Weight
		/// </summary>
		internal int[] m_Weights =  new int[XPriorityQueue.ENABLED_MAXPRIORITY];

		/// <summary>
		/// Queue Max Length
		/// </summary>
		internal int[] m_QueueMaxLengths = new int[XPriorityQueue.ENABLED_MAXPRIORITY];

		/// <summary>
		/// Active Index
		/// </summary>
		internal int m_ActiveIndex = 0;

		/// <summary>
		/// Active Counter
		/// </summary>
		internal int m_ActiveCounter = 0;

		#region Constructor ------------------------------------------------------
		
		public XPriorityQueue2(int vPriorityMax ):base(vPriorityMax )
		{
			InitWeights();
		}

		public XPriorityQueue2(int vPriorityMax , int vMaxQueueCapacity ):base(vPriorityMax, vMaxQueueCapacity)
		{
			InitWeights();
		}

		public XPriorityQueue2(int vPriorityMax , int vMaxQueueCapacity, int[] vWeights ):base(vPriorityMax, vMaxQueueCapacity)
		{
#if DEBUG
			if( vWeights.Length != vPriorityMax ) throw new IndexOutOfRangeException("vWeights Length." );
#endif			
			int tTotalWeight = 0;
			for( int i = 0; i < m_MaxPriority; i++ )
			{
				tTotalWeight += vWeights[i];
			}

			m_MaxCounter = tTotalWeight;

			int tSpec = m_MaxCounter;
			for( int i = 0; i < m_MaxPriority; i++ )
			{
				tSpec -= vWeights[i];
				m_Weights[i] = tSpec;
			}

			m_Weights[ m_MaxPriority - 1 ] = 0;
		}

		public XPriorityQueue2(int vPriorityMax , int vMaxQueueCapacity, int[] vWeights, int[] vMaxLengths ):base(vPriorityMax, vMaxQueueCapacity)
		{
#if DEBUG
			if( vWeights.Length != vPriorityMax ) throw new IndexOutOfRangeException("vWeights Length." );
#endif			
			int tTotalWeight = 0;
			for( int i = 0; i < m_MaxPriority; i++ )
			{
				tTotalWeight += vWeights[i];
			}

			m_MaxCounter = tTotalWeight;

			int tSpec = m_MaxCounter;
			for( int i = 0; i < m_MaxPriority; i++ )
			{
				tSpec -= vWeights[i];
				m_Weights[i] = tSpec;
			}

			m_Weights[ m_MaxPriority - 1 ] = 0;

			for( int i = 0; i < m_MaxPriority; i++ )
			{
				this.m_QueueMaxLengths[i] = vMaxLengths[i];
			}
		}
		
		#endregion --------------------------------------------------------------------

		#region Public Method ---------------------------------------------------------

		public override object Dequeue()
		{
			
			object tResult = null;

			lock ( this.SyncRoot)
			{
				Queue tActiveQueue = null;

				if( this.m_ActiveCounter <= 0 )
				{
					this.m_ActiveCounter = m_MaxCounter;
					this.m_ActiveIndex = 0;
				}

				while( tActiveQueue == null )
				{
					if( this.m_Count <= 0 )
					{
						this.m_ActiveIndex = 0;
						this.m_ActiveCounter = m_MaxCounter;
						return null;
					}

					if( this.m_ActiveCounter > this.m_Weights[this.m_ActiveIndex] )
					{
						Queue tQueue = (Queue)this.m_Queues[this.m_ActiveIndex];

						if( tQueue.Count > 0 )
						{
							tActiveQueue = tQueue;break;
						}
						else
						{
							if( this.m_ActiveIndex < this.m_MaxPriority - 1 )
							{
								this.m_ActiveCounter = this.m_Weights[this.m_ActiveIndex];
								this.m_ActiveIndex++;
							}
							else
							{
								this.m_ActiveIndex = 0; 
								this.m_ActiveCounter = m_MaxCounter;
							}
						}
					}
					else
					{
						if( this.m_ActiveIndex < this.m_MaxPriority - 1 )
						{
							this.m_ActiveCounter = this.m_Weights[this.m_ActiveIndex];
							this.m_ActiveIndex++;
						}
						else
						{
							this.m_ActiveIndex = 0; 
							this.m_ActiveCounter = m_MaxCounter;
						}
					}
				}

				this.m_ActiveCounter --;
				tResult = tActiveQueue.Dequeue();
				this.m_Count --;
			}
			return tResult;
		}

		public override void AddPriorityQueue(XPriorityQueue vQueue)
		{
#if DEBUG
			if( vQueue.m_MaxPriority > this.m_MaxPriority ) throw new IndexOutOfRangeException("XPriorityQueue2 Index Out Of Range. ");
#endif
			if( vQueue.m_MaxPriority <= this.m_MaxPriority )
			{
				for(int i = 0; i < vQueue.m_MaxPriority; i++ )
				{
					Queue tSrcQueue = (Queue)vQueue.m_Queues[i];
					Queue tDstQueue = (Queue)m_Queues[i];
				
					lock( this.SyncRoot )
					{			
						m_Count += QueueFunction.AddQueue( tDstQueue, tSrcQueue, this.m_QueueMaxLengths[i]  );
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
						m_Count += QueueFunction.AddQueue( tDstQueue, tSrcQueue, this.m_QueueMaxLengths[i]  );
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

		public override XPriorityQueue Copy()
		{
			XPriorityQueue2 tObject = new XPriorityQueue2( this.m_MaxPriority);

			lock( this.SyncRoot )
			{	
				for( int i = 0; i < this.m_MaxPriority; i++ )
				{
					Queue tQueue = (Queue)this.m_Queues[i];
					tObject.m_Queues[i] = tQueue.Clone();

					tObject.m_Weights[i] = this.m_Weights[i];
					
				}
				tObject.m_Count = this.m_Count;
				tObject.m_MaxCounter = this.m_MaxCounter;
			}

			return tObject;
		}

		public override XPriorityQueue Move()
		{
			XPriorityQueue2 tObject = new XPriorityQueue2( this.m_MaxPriority);

			lock( this.SyncRoot )
			{	
				object tTempQueue = null;
				for( int i = 0; i < this.m_MaxPriority; i++ )
				{
					tTempQueue = tObject.m_Queues[i];
					tObject.m_Queues[i] = this.m_Queues[i];
					this.m_Queues[i] = tTempQueue;
#if DEBUG
					if( tTempQueue == null || !(tTempQueue is Queue)  ) throw new InvalidCastException( "Queue Invalid cast Exception" );
#endif

				}
				tObject.m_Count = this.m_Count;
				
				this.m_Count = 0;
				this.m_ActiveIndex = 0;
				this.m_ActiveCounter = 0;
			}

			return tObject;
		}

		
		#endregion

		#region Private Method --------------------------------------------------------

		private void InitWeights()
		{
			m_MaxCounter = 0;
			for( int i = 0; i < m_MaxPriority; i++ )
			{
				m_Weights[i]= m_MaxPriority - i;
				this.m_MaxCounter += m_Weights[i];
			}

			int tSpec = m_MaxCounter;
			for( int i = 0; i < m_MaxPriority; i++ )
			{
				tSpec -= m_Weights[i];
				m_Weights[i] = tSpec;
			}
		}

		private void InitQueueMaxLengths()
		{
			for( int i = 0; i < m_MaxPriority; i++ )
			{
				m_QueueMaxLengths[i] = 0;
			}
		}

		#endregion
	}
}
