using System;

namespace Schuster.Exceptions
{
	public class MissingRunFunctionException : Exception
	{
		public MissingRunFunctionException(string message)
			: base(message)
		{
		}
	}
}