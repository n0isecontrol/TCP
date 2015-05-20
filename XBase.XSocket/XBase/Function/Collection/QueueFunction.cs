using System;
using System.Collections;
using XBase;

namespace XBase.Function.Collection
{
	public class QueueFunction
	{
		/// <summary>
		/// Add Queue
		/// </summary>
		/// <param name="vDstQueue">Destination Que</param>
		/// <param name="vSrcQueue">Source Queue</param>
		public static int AddQueue( Queue vDstQueue, Queue vSrcQueue )
		{
			int tAdds = 0;
			while( vSrcQueue.Count > 0 )
			{
				vDstQueue.Enqueue(vSrcQueue.Dequeue());				
				tAdds ++;
			}

			return tAdds;
		}

        /// <summary>
        /// Add Queue
        /// </summary>
        /// <param name="vDstQueue">Destination Que</param>
        /// <param name="vSrcQueue">Source Queue</param>
        public static int AddQueue(Queue vDstQueue, Queue vSrcQueue, int vMaxCount)
		{
			int tAdds = 0;
			while( vSrcQueue.Count > 0  )
			{
				if( vMaxCount > 0 && vDstQueue.Count >= vMaxCount ) break;
				vDstQueue.Enqueue(vSrcQueue.Dequeue());				
				tAdds ++;
			}

			return tAdds;
		}


        /// <summary>
        /// Add Queue
        /// </summary>
        /// <param name="vDstQueue">Destination Que</param>
        /// <param name="vSrcArray">ArrayList</param>
		public static int AddQueue( Queue vDstQueue, ArrayList vSrcArray )
		{
			int tAdds = 0;
			
			for( int i=0; i < vSrcArray.Count; i++ )
			{
				vDstQueue.Enqueue(vSrcArray[i]);				
				tAdds ++;
			}

			return tAdds;
		}
	}
}



