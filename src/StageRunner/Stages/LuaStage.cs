using System;
using NLua;
using Schuster.Exceptions;

namespace Schuster.Stages
{
	public class LuaStage : IStage
	{
		private readonly string _name;

		public LuaStage(string name)
		{
			_name = name;
		}

		public LuaFunction RunFunction { get; set; }

		public event Action OnComplete;

		public void Run()
		{
			if (RunFunction is null)
			{
				throw new MissingRunFunctionException($"Run function not defined for stage {_name}");
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