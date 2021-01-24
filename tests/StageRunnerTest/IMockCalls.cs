using Schuster;

namespace StageRunnerTest
{
    public interface IMockCalls
    {
        void Call(string value);
        void Call(StageRunnerStatus status);
    }
}