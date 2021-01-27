using System;

namespace Schuster.Tasks
{
	public interface ITask
	{
		event Action OnComplete;

		void Run();
		void Succeed();
	}
}