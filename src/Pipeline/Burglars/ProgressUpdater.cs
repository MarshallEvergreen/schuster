using System;
using Schuster.Tasks;

namespace Schuster.Burglars
{
	public class ProgressUpdater : TaskBurglar
	{
		private double _completedTasks;
		private double _numberOfTasks;

		public event Action<PipelineUpdate> ProgressUpdate;

		public override void BreakIn(LuaTask luaTask)
		{
			_numberOfTasks++;
			luaTask.OnComplete += () =>
			{
				_completedTasks++;
				SendUpdate(
					Math.Abs(_numberOfTasks - _completedTasks) > 0.001
						? PipelineStatus.Running
						: PipelineStatus.Success);
			};
		}

		public void SendUpdate(PipelineStatus status)
		{
			if (_completedTasks == 0)
			{
				ProgressUpdate?.Invoke(
					new PipelineUpdate(status, 0.0));
			}
			else
			{
				var progress = _completedTasks / _numberOfTasks * 100;
				ProgressUpdate?.Invoke(
					new PipelineUpdate(status, progress));
			}
		}
	}
}