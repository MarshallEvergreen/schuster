using NLua;
using Schuster.Stages;

namespace Schuster
{
    public class StageRunner
    {
        private readonly ApiContainer _apiContainer;
        private IStage _rootStage;

        public StageRunner(ApiContainer apiContainer)
        {
            _apiContainer = apiContainer;
            _apiContainer.AddLuaApi(new StageApi());
        }

        public void Run(string luaToRun)
        {
            var lua = new Lua();
            _apiContainer.RegisterApisTo(lua);
            lua.DoString(luaToRun);
            
            var stages = lua["Stages"];
            if (stages.GetType() == typeof(LuaTable))
            {
                _rootStage = new StageCollection((LuaTable)stages);
                _rootStage.Run();
            }
            else if (stages.GetType() == typeof(LuaStage))
            {
                _rootStage = (LuaStage) stages;
                _rootStage.Run();
            }
        }
    }
}