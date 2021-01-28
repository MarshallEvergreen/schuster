using System;
using NLua;
using Schuster.Burglars;
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
				throw new MissingRunFunctionException(_name);
			}

			try
			{
				RunFunction.Call();
			}
			catch (Exception e)
			{
				throw new ErrorInRunFunctionException(_name, e.Message);
			}
		}

		public void Succeed()
		{
			OnComplete?.Invoke();
		}

		public void Allow(TaskBurglar burglar)
		{
			burglar.BreakIn(this);
		}
	}
}