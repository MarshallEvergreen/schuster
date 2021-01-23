using NLua;

namespace Schuster
{
    public class StageApi : ILuaApi
    {
        public void RegisterTo(Lua lua)
        {
            lua.RegisterFunction("Stage", this,
                typeof(StageApi).GetMethod("CreateStage"));
        }
        
        public LuaStage CreateStage(string name, LuaTable table)
        {
            return new LuaStage(name, table);
        }
    }
}