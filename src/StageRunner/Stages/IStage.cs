using System;

namespace Schuster.Stages
{
    public interface IStage
    {
        void Run();
        void Complete();
        event Action OnComplete;
    }
}