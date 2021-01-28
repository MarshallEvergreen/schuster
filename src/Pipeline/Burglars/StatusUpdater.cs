using System;
using Schuster.Tasks;

namespace Schuster.Burglars
{
	public class StatusUpdater : TaskBurglar
	{
		private double _completedTasks;
		private double _numberOfTasks;

		public event Action<PipelineUpdate> StatusUpdate;

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
				StatusUpdate?.Invoke(
					new PipelineUpdate(status, 0.0));
			}
			else
			{
				var progress = _completedTasks / _numberOfTasks * 100;
				StatusUpdate?.Invoke(
					new PipelineUpdate(status, progress));
			}
		}
	}
}