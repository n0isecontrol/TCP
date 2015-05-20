using System;
using System.Collections;

namespace XBase.Function.Collection
{
	/// <summary>
	/// HashtableFunction API
	/// </summary>
	public class HashtableFunction
	{
		public HashtableFunction()
		{
			
		}

		public static Hashtable Clone( Hashtable hs )
		{
			if( hs == null ) return null;

			Hashtable result = new Hashtable();

			IDictionaryEnumerator enumerator = hs.GetEnumerator();

			while( enumerator.MoveNext() )
			{
				object tObj = enumerator.Value;

				if( tObj is ICloneable ) tObj = ((ICloneable)tObj).Clone();
				result.Add( enumerator.Key, tObj );
			}

			return result;
		}

		public static object SyncronizedGet(Hashtable hs, object vKey )
		{
			object tResult = null;
#if DEBUG
			if( !hs.ContainsKey(vKey) ) throw new InvalidOperationException("Unknown Key");
#endif			
			lock( hs.SyncRoot )
			{
				tResult = hs[vKey];
			}
			return tResult;
		}

		public static void SyncronizedSet(Hashtable hs, object vKey, object vValue )
		{
			lock( hs.SyncRoot )
			{
				hs.Remove(vKey);
				hs.Add(vKey, vValue);
			}
		}
	}
}
