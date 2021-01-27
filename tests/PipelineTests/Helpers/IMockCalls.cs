using Schuster;

namespace PipelineTests.Helpers
{
	public interface IMockCalls
	{
		void Call(string value);
		void Call(PipelineStatus status);
	}
}