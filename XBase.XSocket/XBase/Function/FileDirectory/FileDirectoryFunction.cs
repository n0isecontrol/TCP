/******************************************************************
 * Package :	XBase.Function.FileDirectory
 * Class   :    FileDirectoryFunction
 * 
 * Define File & Directory functions
 ******************************************************************/
using System;
using System.IO;

namespace XBase.Function.FileDirectory
{
	/// <summary>
	/// FileDirectory Function
	/// </summary>
	public class FileDirectoryFunction
	{
		/// <summary>
		/// GetDirectoryPath From File Path
		/// </summary>
		/// <param name="vFullFileURL"></param>
		/// <returns></returns>
		public static string GetDirectoryPathFromFilePath( string vFullFileURL )
		{
			int iIndex = vFullFileURL.LastIndexOf("/");

			if( iIndex < 0 )
			{
				iIndex = vFullFileURL.LastIndexOf("\\");
			}
			
			if( iIndex < 0 ) return "./";

			return vFullFileURL.Substring( 0, iIndex );
		}

		/// <summary>
		/// Copy from Src directory to Dst directory.
		/// </summary>
		/// <param name="Src">source directory</param>
        /// <param name="Dst">destination directory</param>
		public static void CopyDirectory(string Src,string Dst)
		{
			if( !Directory.Exists( Src ) ) return;
			
			String[] Files;

			if(Dst[Dst.Length-1]!=Path.DirectorySeparatorChar) Dst+=Path.DirectorySeparatorChar;
			if(!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
			Files=Directory.GetFileSystemEntries(Src);

			string dstFile = "";
			foreach(string Element in Files)
			{
				// Sub directories
				if(Directory.Exists(Element)) 
				{
					CopyDirectory(Element,Dst+Path.GetFileName(Element));
					// Files in directory
				}
				else
				{
					dstFile = Dst+Path.GetFileName(Element);
					if( File.Exists( dstFile )) 
					{
						FileAttributes fAttr = File.GetAttributes(dstFile);
						fAttr &= ~FileAttributes.ReadOnly;
						File.SetAttributes(dstFile, fAttr);
					}
					File.Delete(dstFile);
					File.Copy(Element,dstFile,true);
				}
			}
		}

		/// <summary>
		/// Remove directory and files.
		/// </summary>
		/// <param name="strDirectory"></param>
		public static void RemoveDirectoryFiles(string strDirectory )
		{
			if( !Directory.Exists( strDirectory ) ) return;

			string[] strFiles;

			if(strDirectory[strDirectory.Length-1]!=Path.DirectorySeparatorChar) strDirectory+=Path.DirectorySeparatorChar;
			
			strFiles=Directory.GetFiles(strDirectory);
			string dstFile;
			foreach(string Element in strFiles)
			{
				// Sub directories
				dstFile = strDirectory+Path.GetFileName(Element);
				if( File.Exists( dstFile )) 
				{
					FileAttributes fAttr = File.GetAttributes(dstFile);
					fAttr &= ~FileAttributes.ReadOnly;
					File.SetAttributes(dstFile, fAttr);
					File.Delete( dstFile );	
				}
			}
		}

		/// <summary>
		/// Remove sub directories
		/// </summary>
		public static void RemoveSubDirectories(string strDirectory )
		{
			if( !Directory.Exists( strDirectory ) ) return;

			string[] strFiles = Directory.GetDirectories(strDirectory);
			foreach(string Element in strFiles)
			{
				// Sub directories
				RemoveDirectoryFiles(Element );
				RemoveSubDirectories( Element );
				
				Directory.Delete( Element );
			}
		}

		/// <summary>
		/// Check is empty directory
		/// </summary>
		/// <param name="strDirectory"></param>
		/// <returns></returns>
		public static bool IsEmptyDirectory( string strDirectory )
		{
			if( !Directory.Exists( strDirectory ) ) return true;

			string[] strFiles = Directory.GetDirectories(strDirectory);
			if( strFiles.Length > 0 ) return false;

			strFiles = Directory.GetFiles( strDirectory );
			if( strFiles.Length > 0 ) return false;
			return true;
		}
	}
}
