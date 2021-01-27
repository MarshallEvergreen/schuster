using System;
using System.Collections.Generic;
using System.Linq;
using NLua;

namespace Schuster.Tasks
{
	public class TaskGroup : ITask
	{
		private readonly List<ITask> _tasks;

		public TaskGroup(LuaTable tasks)
		{
			_tasks = new List<ITask>();
			foreach (ITask task in tasks.Values)
			{
				_tasks.Add(task);
			}
		}

		public event Action OnComplete;

		public void Run()
		{
			_tasks.FirstOrDefault()?.Run();
		}

		public void Succeed()
		{
			OnComplete?.Invoke();
		}
	}
}