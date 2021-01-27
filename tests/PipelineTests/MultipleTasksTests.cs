using FluentAssertions;
using Moq;
using NUnit.Framework;
using Schuster;

namespace PipelineTests
{
	public class MultipleTasksTests : PipelineTestFixture
	{
		private const string MultipleTaskScript = @"
                    local MyTask1 = LuaTask('MyTask1')
                    local MyTask2 = LuaTask('MyTask2')

                    local function GenericMethod(task, value)
                        TestValue(value)
                        task:Succeed()
                    end

                    local function Task1Function()
                        GenericMethod(MyTask1, 'Task1Value')
                    end

					local function Task2Function()
                        GenericMethod(MyTask2, 'Task2Value')
                    end

                    MyTask1.RunFunction = Task1Function;
                    MyTask2.RunFunction = Task2Function;

                    Pipeline = {
						MyTask1,
						MyTask2
					}
                ";

		[Test]
		public void MultipleTasks_RunsToSuccess_IfConfiguredCorrectly()
		{
			Pipeline.Run(MultipleTaskScript);
			_mockCalls.Verify(m => m.Call("Task1Value"), Times.Once);
			_mockCalls.Verify(m => m.Call("Task2Value"), Times.Once);
		}

		[Test]
		public void MultipleTasks_CanBeRunMultipleTimes()
		{
			Pipeline.Run(MultipleTaskScript);
			Pipeline.Run(MultipleTaskScript);
			_mockCalls.Verify(m => m.Call("Task1Value"), Times.Exactly(2));
			_mockCalls.Verify(m => m.Call("Task2Value"), Times.Exactly(2));
		}

		[Test]
		public void MultipleTasks_RunToCompletion_StatusUpdates_IfConfiguredCorrectly()
		{
			Pipeline.Run(MultipleTaskScript);

			_statusUpdates.Should().Equal(
				PipelineStatus.Running,
				PipelineStatus.Success);
		}
	}
}