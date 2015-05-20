using System;
using System.Collections;
using System.IO;

namespace XBase.Framework
{
	[Serializable]
	public class XStringOption
	{
		protected Hashtable m_OptionTable = new Hashtable();

		public XStringOption()
		{
		}

		public string GetOption( string strKey )
		{
			object _option = m_OptionTable[strKey];
			return ( _option != null ) ? ((string)_option) : "";				
		}

		public void SetOption( string strKey, string strValue )
		{
			lock( this.m_OptionTable.SyncRoot )
			{
				object _option = m_OptionTable[strKey];
			
				if( _option != null ) 
				{
					m_OptionTable[strKey] = strValue;
				}
				else
				{
					m_OptionTable.Add( strKey, strValue );
				}
			}
		}

		public Hashtable GetHashtable()
		{
			return this.m_OptionTable;
		}

		public string GetString()
		{
			string tResult = "";
			lock( m_OptionTable.SyncRoot )
			{
				foreach( string key in m_OptionTable.Keys )
				{
					string tValue = (string)m_OptionTable[key];

					tResult +=  key + "=" + tValue + ";"; 
				}
			}
			
			tResult = tResult.Substring(0, tResult.Length - 1 );

			return tResult;
		}

		char[] delimiter1 = new char[]{';'};
		char[] delimiter2 = new char[]{'='};

		public void SetString(string strValue)
		{
			lock( m_OptionTable.SyncRoot )
			{
				m_OptionTable.Clear();
				string[] tValues = strValue.Split(delimiter1);

				for( int i = 0; i < tValues.Length; i++ )
				{
					string[] tValues1 = tValues[i].Split(delimiter2);
					m_OptionTable.Add( tValues1[0], tValues1[1] );
				}
			}
		}
	}


	[Serializable]
	public class XSimpleOption : XStringOption
	{
		protected SetDefaultHandler m_SetDefaultHandler = null;
		protected string m_OptionFileName = "";
		
		public XSimpleOption()
		{
		}

		public XSimpleOption(string vOptionFileName)
		{
			this.m_OptionFileName = vOptionFileName;
		}
		
		public XSimpleOption(string vOptionFileName, SetDefaultHandler handler ) 
		{
			this.m_OptionFileName = vOptionFileName;
			this.m_SetDefaultHandler = handler;
		}

		public void SetDefault()
		{
			lock( this.m_OptionTable.SyncRoot )
			{
				this.m_OptionTable.Clear();

				if( m_SetDefaultHandler != null )
					m_SetDefaultHandler(this);
			}
		}
		public virtual bool Read()
		{
			string configPath = System.Windows.Forms.Application.StartupPath + "\\" + m_OptionFileName;
			
			SetDefault();

			if( !File.Exists( configPath ) )
			{	
				return false;
			}

			System.IO.StreamReader reader = new StreamReader (configPath);
			try
			{
				string strLine = "";
				while( (strLine = reader.ReadLine()) != null)
				{
					string strKey = "";
					string strValue = "";

					int iPos = strLine.IndexOf("=");

					if( iPos > -1 )
					{
						strKey = strLine.Substring( 0, iPos );
						strValue = strLine.Substring( iPos + 1, strLine.Length - iPos - 1 );

						lock( this.m_OptionTable.SyncRoot )
						{
							if( m_OptionTable[strKey] != null )
							{
								m_OptionTable.Remove( strKey );
							}

							if( strKey.Trim().Length == 0 )
								continue;

							m_OptionTable.Add( strKey, strValue );
						}
					}
				}
			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Trace.WriteLine( ex.ToString () );
				return false;
			}
			finally
			{
				reader.Close();
			}

			return true;
			
		}

		public virtual bool Write()
		{
			string configPath = System.Windows.Forms.Application.StartupPath + "\\" + m_OptionFileName;
			
			System.IO.StreamWriter  wr = new StreamWriter (configPath);
			try
			{
				foreach( string key in m_OptionTable.Keys )
				{
					string tValue = (string)m_OptionTable[key];

					wr.WriteLine( key + "=" + tValue ); 
				}
			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Trace.WriteLine( ex.ToString () );
				return false;
			}
			finally
			{
				wr.Close();
			}
			
			return true;
		}
		
	}

	public delegate void SetDefaultHandler( XSimpleOption option );
}
