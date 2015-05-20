using System;

namespace XBase.Exception
{
	public class XCollectionException : System.Exception
	{
		public E_COLLECTION_ERROR ErrorCode;

		public XCollectionException( E_COLLECTION_ERROR errCode )
		{
			ErrorCode = errCode;
		}

		public override string ToString()
		{
			return "Error Code :" + ErrorCode.ToString() + "\r\n" + base.ToString();
		}
	}

	public enum E_COLLECTION_ERROR
	{
		Error = -1,
		Error_ExistKey = 1000,		
	}
}
