using System;

namespace Schuster.Exceptions
{
	public class PipelineNotFoundException : Exception
	{
		public PipelineNotFoundException()
			: base("Global Pipeline table not found in script")
		{
		}
	}
}