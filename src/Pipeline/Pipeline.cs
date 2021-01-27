using System;

namespace Schuster
{
	public class Pipeline
	{
		private readonly ExtensionCollection _extensionCollection;
		private Environment _currentEnvironment;

		public Pipeline(ExtensionCollection extensionCollection)
		{
			_extensionCollection = extensionCollection;
		}

		public event Action<PipelineStatus> StatusUpdate;

		public void Run(string luaToRun)
		{
			StatusUpdate?.Invoke(PipelineStatus.Running);
			_currentEnvironment = new Environment(luaToRun, _extensionCollection);
			_currentEnvironment.OnComplete += status => StatusUpdate?.Invoke(status);
			_currentEnvironment.Run();
		}
	}
}