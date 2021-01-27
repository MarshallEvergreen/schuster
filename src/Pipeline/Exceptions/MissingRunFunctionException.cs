using System;

namespace Schuster.Exceptions
{
	public class MissingRunFunctionException : Exception
	{
		public MissingRunFunctionException(string taskName)
			: base($"Run function not defined for task: {taskName}")
		{
		}
	}
}