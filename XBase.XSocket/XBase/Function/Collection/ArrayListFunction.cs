
using System;
using System.Collections;
using XBase;

namespace XBase.Function.Collection
{
	public class ArrayListFunction
	{
		public static E_RESULT Move( ArrayList array, int step )
		{
			if( step == 0 ) return E_RESULT.NoError;
			if( array.Count < step ) return E_RESULT.ERROR_MOVE_BIGSTEP;

			int nCount = array.Count;
			if( step < 0 )
			{
				step = -step;
				for( int i = step; i < nCount; i++ )
				{
					array[i-step] = array[i];
				}
				for( int i = nCount - step ; i < nCount; i++ )
				{
					array[i] = null;
				}
			}
			else 
			{
				for( int i = nCount - 1; i >= step; i-- )
				{
					array[i] = array[i-step];
				}

				for( int i = 0; i < step; i++ )
				{
					array[i] = null;
				}
			}

			return E_RESULT.NoError;
		}

	
		public static E_RESULT MoveBefore( ArrayList array )
		{
			return Move( array, -1);
		}

		public static E_RESULT MoveNext( ArrayList array )
		{
			return Move( array, 1);
		}

		public static E_RESULT InsertByBinarySorted(ArrayList array, IComparer compare, object item )
		{
			int iInsert = array.BinarySearch(item, compare );

			if( iInsert >= 0 ) return E_RESULT.Error;

			iInsert = ~iInsert;

			if( iInsert >= array.Count )
			{
				array.Add( item );
			}
			else
			{
				array.Insert( iInsert, item );
			}

			return E_RESULT.NoError;
		}

		public static E_RESULT InsertByBinarySorted(ArrayList array, IComparer compare, object item, ref int vInserted )
		{
			int iInsert = array.BinarySearch(item, compare );

			if( iInsert >= 0 ) return E_RESULT.Error;

			iInsert = ~iInsert;

			if( iInsert >= array.Count )
			{
				vInserted = array.Count;
				array.Add( item );
				
			}
			else
			{
				vInserted = iInsert;
				array.Insert( iInsert, item );
			}

			return E_RESULT.NoError;
		}

		
		public static E_RESULT InsertByBinarySortedCollision(ArrayList array, IComparer compare, object item, ref int vInserted )
		{
			int iInsert = array.BinarySearch(item, compare );

			if( iInsert < 0 ) iInsert = ~iInsert;

			if( iInsert >= array.Count )
			{
				vInserted = array.Count;
				array.Add( item );				
			}
			else
			{
				vInserted = iInsert;
				array.Insert( iInsert, item );
			}

			return E_RESULT.NoError;
		}

		
		public static E_RESULT DeleteByBinarySorted(ArrayList array, IComparer compare, object item )
		{
			int iInsert = array.BinarySearch(item, compare );

			if( iInsert < 0 ) return E_RESULT.Error;

			array.RemoveAt( iInsert );
			
			return E_RESULT.NoError;
		}
	}


	
	public class BinaryArrayItem
	{
		public double m_Index = 0.0;
		public object m_Item = null;
	}


	public class  BinaryArrayItemCompare : System.Collections.IComparer
	{
		#region IComparer Implement

		public int Compare(object x, object y)
		{
			double tX = 0;
			double tY = 0;

			if( x is double || x is float || x is Int16 || x is Int32 || x is Int64 ) 
				tX = (double)x; 
			else if( x is BinaryArrayItem ) 
				tX = ((BinaryArrayItem)x).m_Index;
			else
				throw new InvalidCastException("Unknown BinaryArrayItem Index");

			if( y is double || y is float || y is Int16 || y is Int32 || y is Int64 ) 
				tY = (double)y;
			else if( y is BinaryArrayItem ) 
				tY = ((BinaryArrayItem)y).m_Index;
			else
				throw new InvalidCastException("Unknown BinaryArrayItem Index");

			if( tX > tY ) return 1;
			if( tX < tY ) return -1;

			return 0;
		}

		#endregion
	}


	public class StringArrayItemCompare : System.Collections.IComparer
    {
        #region IComparer Implement

        public int Compare(object x, object y)
		{
			string tX = x.ToString();
			string tY = y.ToString();

			return tX.CompareTo(tY);
		}

		#endregion

	}

}
