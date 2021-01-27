using Schuster;

namespace StageRunnerTest.Helpers
{
	public interface IMockCalls
	{
		void Call(string value);
		void Call(StageRunnerStatus status);
	}
}