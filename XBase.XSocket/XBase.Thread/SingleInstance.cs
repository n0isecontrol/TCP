using System;
using System.Threading;

namespace XBase.Thread
{
	public class XSingleInstance : System.IDisposable
	{
		private Mutex m_Mutex = null;
		private bool  m_IsMutexCreated = false;

		public XSingleInstance(string vID)
		{
			m_Mutex = new Mutex(true, vID, out m_IsMutexCreated );
		}

		~XSingleInstance()
		{
			Release();
		}

		#region IDisposable

		public void Dispose()
		{
			Release();
		}

		#endregion

		public bool IsSingleInstance
		{
			get
			{
				return m_IsMutexCreated;
			}
		}

		public void Release()
		{
			if( m_Mutex != null )
			{
				if( m_IsMutexCreated )
				{
					m_IsMutexCreated = false;
					m_Mutex.ReleaseMutex();				
				}
				m_Mutex.Close();
				m_Mutex = null;
			}
		}

	}
}
