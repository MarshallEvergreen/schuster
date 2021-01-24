using System;

namespace Schuster
{
    public class StageRunner
    {
        private readonly ApiContainer _apiContainer;
        private RunEnvironment _currentEnvironment;

        public StageRunner(ApiContainer apiContainer)
        {
            _apiContainer = apiContainer;
        }

        public event Action<StageRunnerStatus> StatusUpdate;

        public void Run(string luaToRun)
        {
            StatusUpdate?.Invoke(StageRunnerStatus.Running);
            _currentEnvironment = new RunEnvironment(luaToRun, _apiContainer);
            _currentEnvironment.OnComplete += status => StatusUpdate?.Invoke(status); 
            _currentEnvironment.Run();
        }
    }
}