using System.Collections.Generic;
using System.Linq;
using NLua;

namespace Schuster.Stages
{
    public class StageCollection : IStage
    {
        private readonly List<IStage> _stages;

        public StageCollection(LuaTable stages)
        {
            _stages = new List<IStage>();
            foreach (IStage stage in stages.Values)
            {
                _stages.Add(stage);
            }
        }
        public void Run()
        {
            _stages.FirstOrDefault()?.Run();
        }
    }
}