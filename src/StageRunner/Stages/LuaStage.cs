using System;
using NLua;
using Schuster.Exceptions;

namespace Schuster.Stages
{
    public class LuaStage : IStage
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
            if (runFunction is null)
            {
                throw new MissingRunFunctionException($"Run function not defined for stage {_name}");
            }

            try
            {
                runFunction.Call();
            }
            catch(Exception e)
            {
                throw new ErrorInRunFunctionException($"Error calling run function for {_name}: {e.Message}");
            }
        }

        public void Complete()
        {
            OnComplete?.Invoke();
        }

        public event Action OnComplete;
    }
}