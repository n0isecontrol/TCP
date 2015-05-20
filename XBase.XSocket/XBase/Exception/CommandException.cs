
using System;

namespace XBase.Exception
{
	
	public class CommandException : System.Exception
	{
		public E_COMAND_ERROR ErrorCode; 

		public CommandException(E_COMAND_ERROR error)
		{
			ErrorCode = error;
		}

		public override string ToString()
		{
			return "Error Code :" + ErrorCode.ToString() + "\r\n" + base.ToString();
		}
	}

	public enum E_COMAND_ERROR
	{
		Error = -1,
		NoCommitedEndCommand = 5000,
		OverFlow_HistoryPosition,
	}
}
