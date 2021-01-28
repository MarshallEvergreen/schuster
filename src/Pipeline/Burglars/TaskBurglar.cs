using Schuster.Tasks;

namespace Schuster.Burglars
{
	public abstract class TaskBurglar
	{
		public void BreakIn(TaskGroup taskGroup)
		{
			foreach (var task in taskGroup.Tasks)
			{
				if (task.GetType() == typeof(TaskGroup))
				{
					BreakIn((TaskGroup)task);
				}
				else if (task.GetType() == typeof(LuaTask))
				{
					BreakIn((LuaTask)task);
				}
			}
		}

		public abstract void BreakIn(LuaTask luaTask);
	}
}