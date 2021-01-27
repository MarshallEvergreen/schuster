using System;
using NLua;
using Schuster.Exceptions;
using Schuster.Tasks;

namespace Schuster
{
	public class Environment
	{
		private readonly TaskGroup _rootTaskGroup;

		public Environment(string luaToRun, ExtensionCollection extensionCollection)
		{
			var lua = new Lua();
			var taskExtension = new TaskExtension();
			taskExtension.RegisterExtension(lua);
			extensionCollection.RegisterExtensions(lua);

			lua.DoString(luaToRun);

			var tasks = lua["Pipeline"];

			if (tasks is null)
			{
				throw new PipelineNotFoundException();
			}
			if (tasks.GetType() == typeof(LuaTable))
			{
				_rootTaskGroup = new TaskGroup((LuaTable) tasks);
				_rootTaskGroup.OnComplete += () => { OnComplete?.Invoke(PipelineStatus.Success); };
			}
		}

		public event Action<PipelineStatus> OnComplete;

		public void Run()
		{
			_rootTaskGroup.Run();
		}
	}
}