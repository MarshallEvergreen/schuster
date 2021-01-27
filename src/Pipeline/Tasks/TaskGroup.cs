using System;
using System.Collections.Generic;
using NLua;

namespace Schuster.Tasks
{
	public class TaskGroup : ITask
	{
		private readonly List<ITask> _tasks;
		private int _index;

		public TaskGroup(LuaTable tasks)
		{
			_tasks = new List<ITask>();
			foreach (var task in tasks.Values)
			{
				if (task.GetType() == typeof(LuaTable))
				{
					_tasks.Add(new TaskGroup((LuaTable) task));
				}
				else
				{
					_tasks.Add((LuaTask) task);
				}
			}
		}

		public event Action OnComplete;

		public void Run()
		{
			var currentTask = _tasks[_index];
			currentTask.OnComplete += () =>
			{
				_index++;
				if (_index < _tasks.Count)
				{
					Run();
				}
				else
				{
					Succeed();
				}
			};
			currentTask.Run();
		}

		public void Succeed()
		{
			OnComplete?.Invoke();
		}
	}
}