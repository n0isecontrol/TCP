using System;
using System.Collections;

namespace XBase.Collection
{
	
	public class XCollection : XBase.Framework.IXIterable
	{
		protected ArrayList array_collection = null;
		protected Hashtable hash_collection = null;

		public XCollection()
		{
			array_collection = new ArrayList();
			hash_collection = new Hashtable();
		}

		public XCollection( int nCount )
		{
			array_collection = new ArrayList( nCount );
			hash_collection = new Hashtable( nCount );
		}

		public int Count
		{
			get
			{
				if( array_collection == null ) return 0;
				return array_collection.Count;
			}
		}


		
		public void Add(object key, object valueObject )
		{
			if( hash_collection[key] != null ) 
				throw new XBase.Exception.XCollectionException( XBase.Exception.E_COLLECTION_ERROR.Error_ExistKey );

			XCollectionItem item = new XCollectionItem(key, valueObject);

			int iIndex = array_collection.Count;
			array_collection.Add( item );
			hash_collection.Add( item.Key, item );
		}

		
		
		public void Add(XCollectionItem vItem)
		{
			if( hash_collection[vItem.Key] != null ) 
				throw new XBase.Exception.XCollectionException( XBase.Exception.E_COLLECTION_ERROR.Error_ExistKey );

			array_collection.Add( vItem );
			hash_collection.Add( vItem.Key, vItem );
		}
		

		public void Clear()
		{
			hash_collection.Clear();
			array_collection.Clear();
		}

		public void Remove( object key )
		{
			if( hash_collection[key] != null )
			{
				XCollectionItem item = (XCollectionItem)hash_collection[key];
				hash_collection.Remove( key );
				array_collection.Remove( item );
			}
		}

		public object this[object key]
		{
			get
			{
				object tObj = hash_collection[key];
				if( tObj == null) return null;
				return ((XCollectionItem) tObj).Value;
			}
		}

		public XCollectionItem GetCollectionItem(int vIndex )
		{
			return (XCollectionItem)array_collection[vIndex];
		}

		public XCollectionItem GetCollectionItem( object key )
		{
			object tObj = hash_collection[key];
			if( tObj == null) return null;
			return (XCollectionItem) tObj;
		}

		

		public XBase.Framework.IXIterator GetIterator()
		{
			return new XCollectionIterator( array_collection );
		}
		
	}

	public class XCollectionItem
	{
		public object Key = null;
		public object Value = null;

		public XCollectionItem(object key, object valueObject )
		{
			Key = key;
			Value = valueObject;
		}
	}

	public class XCollectionIterator : XBase.Framework.IXKeyIterator
	{
		ArrayList m_collection = null;
		private int  curIndex = -1;
		
		internal XCollectionIterator( ArrayList array_templates )
		{
			m_collection = array_templates;
			curIndex = 0;
		}

		

		public bool HasNext()
		{
			return (curIndex < m_collection.Count - 1);
		}

		public object Next()
		{
			if( curIndex >= 0 && curIndex < m_collection.Count )
			{
				object tObj = ((XCollectionItem) m_collection[curIndex]).Value; 
				curIndex++;

				return tObj;
			}
			return null;			
		}

		public void GoFirst()
		{
			curIndex = 0;		
		}

		

		

		public bool HasPrev()
		{
			return (curIndex > -1);
		}

		public object Prev()
		{
			if( curIndex >= 0 && curIndex < m_collection.Count )
			{
				object tObj = ((XCollectionItem) m_collection[curIndex]).Value; 
				curIndex--;

				return tObj;
			}
			return null;			
		}

		public void GoLast()
		{
			curIndex = m_collection.Count - 1;		
		}

		

		

		public object GetKey()
		{
			if( curIndex >= 0 && curIndex < m_collection.Count )
			{
				return ((XCollectionItem) m_collection[curIndex]).Key; 
			}
			return null;
		}

		public object GetValue()
		{
			if( curIndex >= 0 && curIndex < m_collection.Count )
			{
				return ((XCollectionItem) m_collection[curIndex]).Value; 
			}
			return null;
		}

		
	}
}
