using System;
using NLua;
using Schuster.Burglars;
using Schuster.Exceptions;
using Schuster.Tasks;

namespace Schuster
{
	public class Environment
	{
		private readonly StatusUpdater _statusUpdater;
		private readonly TaskGroup _rootTaskGroup;

		public Environment(string luaToRun, ExtensionCollection extensionCollection)
		{
			var lua = new Lua();
			var taskExtension = new TaskExtension();
			taskExtension.LoadExtension(lua);
			extensionCollection.LoadExtensions(lua);

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

			_statusUpdater = new StatusUpdater();
			_rootTaskGroup = new TaskGroup((LuaTable) tasks);
			_rootTaskGroup.Allow(_statusUpdater);
			_statusUpdater.StatusUpdate += update => Update?.Invoke(update);
		}

		public event Action<PipelineUpdate> Update;

		public void Run()
		{
			_statusUpdater.SendUpdate(PipelineStatus.Running);
			_rootTaskGroup.Run();
		}
	}
}