using System;
using NLua;
using Schuster.Burglars;
using Schuster.Exceptions;
using Schuster.Tasks;

namespace Schuster
{
	public class Environment
	{
		private readonly TaskGroup _rootTaskGroup;
		private readonly ProgressUpdater _progressUpdater;
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

			if (tasks.GetType() != typeof(LuaTable))
			{
				return;
			}

			_progressUpdater = new ProgressUpdater();
			_rootTaskGroup = new TaskGroup((LuaTable) tasks);
			_rootTaskGroup.Allow(_progressUpdater);
			_progressUpdater.ProgressUpdate += update => Update?.Invoke(update);
		}

		public event Action<PipelineUpdate> Update;

		public void Run()
		{
			_progressUpdater.SendUpdate(PipelineStatus.Running);
			_rootTaskGroup.Run();
		}
	}
}