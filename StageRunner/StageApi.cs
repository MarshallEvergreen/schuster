using NLua;
using Schuster.Stages;

namespace Schuster
{
    public class StageApi : ILuaApi
    {
        private readonly Lua _lua;

        public StageApi(Lua lua)
        {
            _lua = lua;
        }

        public void RegisterTo(Lua lua)
        {
            lua.RegisterFunction("Stage", this,
                GetType().GetMethod("CreateStage"));
        }

        public LuaStage CreateStage(string name, LuaTable table)
        {
            var stage = new LuaStage(name, table);
            table["Complete"] = _lua.RegisterFunction("", stage,
                typeof(LuaStage).GetMethod("Complete"));
            return stage;
        }
    }
}