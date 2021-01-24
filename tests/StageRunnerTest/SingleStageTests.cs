using System;
using FluentAssertions;
using NUnit.Framework;
using Schuster;
using Moq;
using Schuster.Exceptions;

namespace StageRunnerTest
{
    public class SingleStageTests : StageRunnerTestContext
    {
        const string SingleStageScript = @"
                    local function SimpleStage()
                        local self = {}

                        local function CallTestValue()
                            TestValue('TestValue')
                            self.Complete()
                        end

                        self.Run = CallTestValue

                        return self
                    end

                    Stages = Stage('MyStage', SimpleStage())
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
                    local function SimpleStage()
                        local self = {}

                        local function CallTestValue()
                            TestValue('TestValue')
                        end

                        self.Run = CallTestValue

                        return self
                    end

                    Stages = {
                        Stage('MyStage', SimpleStage())
                    }
                ";

            _stageRunner.Run(script);
            _mockCalls.Verify(m => m.Call("TestValue"), Times.Once);
        }

        [Test]
        public void SingleStage_RunFunctionNotSpecified_NotifiesUser()
        {
            const string script = @"
                    local function SimpleStage()
                        local self = {}
                        return self
                    end

                    Stages = Stage('MyStage', SimpleStage())
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
                    local function SimpleStage()
                        local self = {}

                        local function CallTestValue()
                            -- Non existent function
                            XXX('TestValue')
                        end

                        self.Run = CallTestValue

                        return self
                    end
                    Stages = Stage('MyStage', SimpleStage())
                ";

            Action act = () => _stageRunner.Run(script);

            act.Should()
                .Throw<ErrorInRunFunctionException>()
                .WithMessage("*Error calling run function for MyStage*");
        }
    }
}