using System;
using NLua;
using Schuster.Stages;

namespace Schuster
{
	public class RunEnvironment
	{
		private readonly Lua _lua;
		private readonly IStage _rootStage;
		private readonly StageApi _stageApi;

		public RunEnvironment(string luaToRun, ApiContainer apiContainer)
		{
			_lua = new Lua();
			_stageApi = new StageApi(_lua);

			_stageApi.RegisterTo(_lua);
			apiContainer.RegisterApisTo(_lua);

			_lua.DoString(luaToRun);

			var stages = _lua["Stages"];
			if (stages.GetType() == typeof(LuaTable))
			{
				_rootStage = new StageCollection((LuaTable) stages);
			}
			else if (stages.GetType() == typeof(LuaStage))
			{
				_rootStage = (LuaStage) stages;
				_rootStage.OnComplete += () => { OnComplete?.Invoke(StageRunnerStatus.Success); };
			}
		}

		public event Action<StageRunnerStatus> OnComplete;

		public void Run()
		{
			_rootStage.Run();
		}
	}
}