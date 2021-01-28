using System;
using System.Collections.Generic;
using NLua;
using Schuster.Burglars;

namespace Schuster.Tasks
{
	public class TaskGroup : ITask
	{
		private int _index;

		public TaskGroup(LuaTable tasks)
		{
			Tasks = new List<ITask>();
			foreach (var task in tasks.Values)
			{
				if (task.GetType() == typeof(LuaTable))
				{
					Tasks.Add(new TaskGroup((LuaTable) task));
				}
				else
				{
					Tasks.Add((LuaTask) task);
				}
			}
		}

		public List<ITask> Tasks { get; }

		public event Action OnComplete;

		public void Run()
		{
			var currentTask = Tasks[_index];
			currentTask.OnComplete += () =>
			{
				_index++;
				if (_index < Tasks.Count)
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

		public void Allow(TaskBurglar burglar)
		{
			burglar.BreakIn(this);
		}
	}
}