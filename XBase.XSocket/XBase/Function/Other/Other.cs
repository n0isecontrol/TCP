using System;

namespace XBase.Function.Other
{
	/// <summary>
	/// Other Utility functions
	/// </summary>
	public class Other
	{
		/// <summary>
		///  Auto Numbering Function
		/// </summary>
		/// <param name="vFormatString">String Format</param>
		/// <param name="vStringArray">String ¹è¿­</param>
		/// <returns></returns>
		public static string GetAutoNumbering( string vFormatString, string[] vStringArray )
		{
			int i = 1;
			bool blFind = true;
			string tString = "";

			while( blFind )
			{
				blFind = false;
				tString = String.Format( vFormatString,  i );
				for( int j = 0; j < vStringArray.Length; j++ )
				{
					if( tString == vStringArray[j] )
					{
						blFind = true;
						break;
					}
				}
				i++;
			}

			return tString;
		}
	}
}
