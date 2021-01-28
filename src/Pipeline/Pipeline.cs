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

		public event Action<PipelineUpdate> Update;

		public void Run(string luaToRun)
		{
			_currentEnvironment = new Environment(luaToRun, _extensionCollection);
			_currentEnvironment.Update += update => Update?.Invoke(update);
			_currentEnvironment.Run();
		}
	}
}