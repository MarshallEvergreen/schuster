using System;

namespace Schuster.Stages
{
    public interface IStage
    {
        event Action OnComplete;

        void Run();
        void Complete();
    }
}