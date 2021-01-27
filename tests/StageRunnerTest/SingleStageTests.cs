using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Schuster;
using Schuster.Exceptions;

namespace StageRunnerTest
{
	public class SingleStageTests : StageRunnerTestContext
	{
		private const string SingleStageScript = @"
                    local MyStage = LuaStage('MyStage')

                    local function GenericMethod(task)
                        TestValue('TestValue')
                        task:Succeed()
                    end

                    local function RunFunction()
                        GenericMethod(MyStage)
                    end

                    MyStage.RunFunction = RunFunction;

                    Stages = MyStage
                ";

		[Test]
		public void SingleStage_RunsToSuccess_IfConfiguredCorrectly()
		{
			_stageRunner.Run(SingleStageScript);
			_mockCalls.Verify(m => m.Call("TestValue"), Times.Once);
		}

		[Test]
		public void SingleStage_CanBeRunMultipleTimes()
		{
			_stageRunner.Run(SingleStageScript);
			_stageRunner.Run(SingleStageScript);
			_mockCalls.Verify(m => m.Call("TestValue"), Times.Exactly(2));
		}

		[Test]
		public void SingleStage_RunToCompletion_StatusUpdates_IfConfiguredCorrectly()
		{
			_stageRunner.Run(SingleStageScript);

			_statusUpdates.Should().Equal(
				StageRunnerStatus.Running,
				StageRunnerStatus.Success);
		}

		[Test]
		public void SingleStageInCollection_RunsToSuccess_IfConfiguredCorrectly()
		{
			const string script = @"
                    local function CallTestValue()
                        TestValue('TestValue')
                    end

                    local MyStage = LuaStage('MyStage')
                    MyStage.RunFunction = CallTestValue;

                    Stages = {
                        MyStage
                    }
                ";

			_stageRunner.Run(script);
			_mockCalls.Verify(m => m.Call("TestValue"), Times.Once);
		}

		[Test]
		public void SingleStage_RunFunctionNotSpecified_NotifiesUser()
		{
			const string script = @"
                    Stages = LuaStage('MyStage')
                ";

			Action act = () => _stageRunner.Run(script);

			act.Should()
				.Throw<MissingRunFunctionException>()
				.WithMessage("Run function not defined for stage MyStage");
		}

		[Test]
		public void SingleStage_ErrorInRunFunction_NotifiesUser()
		{
			const string script = @"
                    local function CallTestValue()
                        -- Non existent function
                        XXX('TestValue')
                    end

                    local MyStage = LuaStage('MyStage')
                    MyStage.RunFunction = CallTestValue;
                    
                    Stages = MyStage
                ";

			Action act = () => _stageRunner.Run(script);

			act.Should()
				.Throw<ErrorInRunFunctionException>()
				.WithMessage("*Error calling run function for MyStage*");
		}
	}
}