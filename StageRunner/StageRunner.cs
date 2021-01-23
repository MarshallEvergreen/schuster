using NLua;

namespace Schuster
{
    public class StageRunner
    {
        private readonly ApiContainer _apiContainer;

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
            var luaStage = (LuaStage) lua["Stages"];

            luaStage.Run();
        }
        
    }
}