using System;
using System.Collections;

namespace XBase.Collection
{
	public class XArrayList : System.ICloneable
	{
		ArrayList m_array = null;
		
		private bool m_isModified = false;

		public XArrayList()
		{
			m_array = new ArrayList();
			m_isModified = true;
		}

		public XArrayList( ArrayList array )
		{
			if( array == null )
			{
				m_array = new ArrayList();
			}
			else
			{
				m_array = (ArrayList)array.Clone();
			}

			m_isModified = true;
		}

		public bool IsModified
		{
			get
			{
				return m_isModified;
			}
		}

		public void CheckModified()
		{
			m_isModified = false;
		}

		public int Count
		{
			get
			{
				return m_array.Count;
			}
		}
		
		public object this[int index]
		{
			get
			{
				return m_array[index];
			}

			set
			{
				m_array[index] = value;
				m_isModified = true;
			}
		}

		public void Clear()
		{
			this.m_array.Clear();
			m_isModified = true;
		}

		public void Add( object obj)
		{
			this.m_array.Add( obj );
			m_isModified = true;
		}

		public void Remove( object obj )
		{
			this.m_array.Remove( obj );
			m_isModified = true;
		}

		public void Insert( int index, object obj )
		{
			this.m_array.Insert( index, obj);
			m_isModified = true;
		}

		public ArrayList GetArrayList()
		{
			return this.m_array;
		}

		#region ICloneable Implement

		public object Clone()
		{
			return new XArrayList(m_array);
		}

		#endregion
	}
}
