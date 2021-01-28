namespace Schuster
{
	public struct PipelineUpdate
	{
		public PipelineUpdate(PipelineStatus status, double progress)
		{
			Status = status;
			Progress = progress;
		}

		public PipelineStatus Status { get; }
		public double Progress { get; }
	}
}