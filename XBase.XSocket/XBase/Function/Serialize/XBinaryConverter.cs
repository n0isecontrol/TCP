/**********************************************************************
 * Package XBase.Function.Serialize
 * Class		XBinaryConverter.cs
 *********************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace XBase.Function.Serialize
{
	/// <summary>
	/// XBinary Converter
	/// </summary>
	public class XBinaryConverter
	{
		/// <summary>
		/// Get Bytes From Object
		/// </summary>
		/// <param name="vObject">Object</param>
		/// <returns>byte array</returns>
		public static byte [] GetBytes(object vObject)
		{
			if(vObject == null) return null;
			
			BinaryFormatter tBF = new BinaryFormatter();
			MemoryStream tMS = new MemoryStream();
			
			tBF.Serialize(tMS, vObject);
			return tMS.ToArray();			
		}

		/// <summary>
		/// Get Object from bytes array
		/// </summary>
		/// <param name="vObject">bytes array</param>
		/// <returns>object</returns>
		public static object GetObject(byte [] vObject)
		{
			if(vObject == null) return null;
			if(vObject.Length == 0) return null;

			BinaryFormatter tBF = new BinaryFormatter();
			MemoryStream tMS = new MemoryStream(vObject);

            return tBF.Deserialize(tMS);
		}


        public static object GetObject(byte[] vObject, int vIndex, int vCapacity)
        {
            if (vObject == null) return null;
            if (vObject.Length == 0) return null;

            BinaryFormatter tBF = new BinaryFormatter();
            MemoryStream tMS = new MemoryStream(vObject, vIndex, vCapacity);

            return tBF.Deserialize(tMS);
        }

		/// <summary>
		/// Get bytes from objects with SurrogateSelector.
		/// </summary>
		/// <param name="vObject">object</param>
		/// <returns>byte array</returns>
		public static byte [] GetBytes(object vObject, ISurrogateSelector vSurrogateSelector )
		{
			if(vObject == null) return null;
			
			BinaryFormatter tBF = new BinaryFormatter();

			tBF.SurrogateSelector = vSurrogateSelector;
			MemoryStream tMS = new MemoryStream();
			
			tBF.Serialize(tMS, vObject);
			return tMS.ToArray();			
		}

        /// <summary>
        /// Get Object from bytes array with Surrogate
        /// </summary>
        /// <param name="vObject">bytes array</param>
        /// <returns>object</returns>
		public static object GetObject(byte [] vObject, ISurrogateSelector vSurrogateSelector)
		{
			if(vObject == null) return null;
			if(vObject.Length == 0) return null;

			BinaryFormatter tBF = new BinaryFormatter();
			tBF.SurrogateSelector = vSurrogateSelector;
			MemoryStream tMS = new MemoryStream(vObject);
			
			return tBF.Deserialize(tMS);
		}
	}
}
