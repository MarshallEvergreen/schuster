using System;
using NLua;
using Schuster.Tasks;

namespace Schuster
{
	public class Environment
	{
		private readonly Lua _lua;
		private readonly ITask _rootTask;
		private readonly TaskExtension _taskExtension;

		public Environment(string luaToRun, ExtensionCollection extensionCollection)
		{
			_lua = new Lua();
			_taskExtension = new TaskExtension();

			_taskExtension.RegisterExtension(_lua);
			extensionCollection.RegisterExtensions(_lua);

			_lua.DoString(luaToRun);

			var tasks = _lua["Pipeline"];
			if (tasks.GetType() == typeof(LuaTable))
			{
				_rootTask = new TaskGroup((LuaTable) tasks);
			}
			else if (tasks.GetType() == typeof(LuaTask))
			{
				_rootTask = (LuaTask) tasks;
				_rootTask.OnComplete += () => { OnComplete?.Invoke(PipelineStatus.Success); };
			}
		}

		public event Action<PipelineStatus> OnComplete;

		public void Run()
		{
			_rootTask.Run();
		}
	}
}