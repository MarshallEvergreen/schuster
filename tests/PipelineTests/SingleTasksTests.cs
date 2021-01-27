using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Schuster;
using Schuster.Exceptions;

namespace PipelineTests
{
	public class SingleTaskTests : PipelineTestFixture
	{
		private const string SingleTaskScript = @"
                    local MyTask = LuaTask('MyTask')

                    local function GenericMethod(task)
                        TestValue('TestValue')
                        task:Succeed()
                    end

                    local function RunFunction()
                        GenericMethod(MyTask)
                    end

                    MyTask.RunFunction = RunFunction;

                    Pipeline = {
						MyTask
					}
                ";

		[Test]
		public void SingleTask_RunsToSuccess_IfConfiguredCorrectly()
		{
			Pipeline.Run(SingleTaskScript);
			_mockCalls.Verify(m => m.Call("TestValue"), Times.Once);
		}

		[Test]
		public void SingleTask_CanBeRunMultipleTimes()
		{
			Pipeline.Run(SingleTaskScript);
			Pipeline.Run(SingleTaskScript);
			_mockCalls.Verify(m => m.Call("TestValue"), Times.Exactly(2));
		}

		[Test]
		public void SingleTask_RunToCompletion_StatusUpdates_IfConfiguredCorrectly()
		{
			Pipeline.Run(SingleTaskScript);

			_statusUpdates.Should().Equal(
				PipelineStatus.Running,
				PipelineStatus.Success);
		}

		[Test]
		public void SingleTask_GlobalPipelineTableNotDefined_NotifiesUser()
		{
			const string script = @"
					-- Pipeline deliberately misspelled
                    Pipelinee = {
						LuaTask('MyTask')
					}
                ";

			Action act = () => Pipeline.Run(script);

			act.Should()
				.Throw<PipelineNotFoundException>()
				.WithMessage("Global Pipeline table not found in script");
		}
		
		[Test]
		public void SingleTask_RunFunctionNotSpecified_NotifiesUser()
		{
			const string script = @"
                    Pipeline = {
						LuaTask('MyTask')
					}
                ";

			Action act = () => Pipeline.Run(script);

			act.Should()
				.Throw<MissingRunFunctionException>()
				.WithMessage("Run function not defined for task: MyTask");
		}

		[Test]
		public void SingleTask_ErrorInRunFunction_NotifiesUser()
		{
			const string script = @"
                    local function CallTestValue()
                        -- Non existent function
                        XXX('TestValue')
                    end

                    local MyTask = LuaTask('MyTask')
                    MyTask.RunFunction = CallTestValue;
                    
                    Pipeline = {
						MyTask
					}
                ";

			Action act = () => Pipeline.Run(script);

			act.Should()
				.Throw<ErrorInRunFunctionException>()
				.WithMessage("*Error calling run function for MyTask*");
		}
	}
}