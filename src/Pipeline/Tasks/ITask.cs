using System;
using Schuster.Burglars;

namespace Schuster.Tasks
{
	public interface ITask
	{
		event Action OnComplete;

		void Run();
		void Succeed();
		void Allow(TaskBurglar burglar);
	}
}