using System;

namespace XBase.Framework
{
	public interface IXIterator
	{
		bool	HasNext();

		object	Next();

		void	GoFirst();		
	}

	public interface IXKeyIterator : IXIterator2
	{
		object GetKey();
		object GetValue();
	}

	public interface IXIterator2 : IXIterator
	{
		bool	HasPrev();

		object	Prev();

		void	GoLast();		
	}

	public interface IXIterable
	{
		IXIterator GetIterator();
	}
}
