using System;

namespace XBase.Framework
{
	[Serializable]
	public class XAttribute : System.ICloneable
	{
		public string AttributeName = "";
		public object AttributeValue = null;
		public object InitValue = null;

		public XAttribute( string attrName, object initValue)
		{
			AttributeName = attrName;
			AttributeValue = initValue;
			InitValue = initValue;
		}

		public void SetDefaultValue()
		{
			if( InitValue is System.ICloneable )
			{
				AttributeValue = ((System.ICloneable)InitValue).Clone();
			}
			else
			{
				AttributeValue = InitValue;
			}
		}		

		public object Clone()
		{
			XAttribute attribute = new XAttribute(AttributeName, InitValue);
			
			if( AttributeValue is System.ICloneable )
			{
				AttributeValue = ((System.ICloneable)AttributeValue).Clone();
			}
			else
			{
				attribute.AttributeValue = AttributeValue;
			}

			return attribute;
		}		
	}
}
