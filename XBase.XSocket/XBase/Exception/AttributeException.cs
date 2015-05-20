using System;

namespace XBase.Exception
{
	public class AttributeException : System.Exception
	{
		public E_ATTRIBUTE_ERROR ErrorCode;

		public AttributeException( E_ATTRIBUTE_ERROR errCode )
		{
			ErrorCode = errCode;
		}

		public override string ToString()
		{
			return "Error Code :" + ErrorCode.ToString() + "\r\n" + base.ToString();
		}
	}

	public enum E_ATTRIBUTE_ERROR
	{
		Error = -1,
		NoFindAttribute = 1000,
		
	}
}
