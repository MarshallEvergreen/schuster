using System;
using NLua;
using Schuster.Stages;

namespace Schuster
{
    public class StageRunner
    {
        private readonly ApiContainer _apiContainer;
        private IStage _rootStage;
        private Lua _lua;
        private StageApi _stageApi;

        public StageRunner(ApiContainer apiContainer)
        {
            _apiContainer = apiContainer;
        }

        public event Action<StageRunnerStatus> StatusUpdate;

        public void Run(string luaToRun)
        {
            _lua = new Lua();
            _stageApi = new StageApi(_lua);
            _apiContainer.RegisterApisTo(_lua);
            _stageApi.RegisterTo(_lua);

            StatusUpdate?.Invoke(StageRunnerStatus.Running);

            _lua.DoString(luaToRun);

            var stages = _lua["Stages"];
            if (stages.GetType() == typeof(LuaTable))
            {
                _rootStage = new StageCollection((LuaTable) stages);
                _rootStage.Run();
            }
            else if (stages.GetType() == typeof(LuaStage))
            {
                _rootStage = (LuaStage) stages;
                _rootStage.OnComplete += () => { StatusUpdate?.Invoke(StageRunnerStatus.Success); };
                _rootStage.Run();
            }
        }
    }
}