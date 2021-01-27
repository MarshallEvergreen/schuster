using System;

namespace Schuster.Exceptions
{
	public class ErrorInRunFunctionException : Exception
	{
		public ErrorInRunFunctionException(string taskName, string error)
			: base($"Error calling run function for {taskName}: {error}")
		{
		}
	}
}