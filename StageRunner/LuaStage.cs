using System;
using NLua;

namespace Schuster
{
    public class LuaStage
    {
        private readonly string _name;
        private readonly LuaTable _table;
        public LuaStage(string name, LuaTable luaTable)
        {
            _name = name;
            _table = luaTable;
        }

        public void Run()
        {
            var runFunction = (LuaFunction) _table["Run"];
            runFunction.Call();
        }
    }
}