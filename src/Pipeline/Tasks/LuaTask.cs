using System;
using NLua;
using Schuster.Exceptions;

namespace Schuster.Tasks
{
	public class LuaTask : ITask
	{
		private readonly string _name;

		public LuaTask(string name)
		{
			_name = name;
		}

		public LuaFunction RunFunction { get; set; }

		public event Action OnComplete;

		public void Run()
		{
			if (RunFunction is null)
			{
				throw new MissingRunFunctionException($"Run function not defined for task {_name}");
			}

			try
			{
				RunFunction.Call();
			}
			catch (Exception e)
			{
				throw new ErrorInRunFunctionException($"Error calling run function for {_name}: {e.Message}");
			}
		}

		public void Succeed()
		{
			OnComplete?.Invoke();
		}
	}
}