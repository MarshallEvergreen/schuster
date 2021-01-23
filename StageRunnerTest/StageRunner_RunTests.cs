using System;
using FluentAssertions;
using NUnit.Framework;
using Schuster;
using Moq;
using Schuster.Exceptions;

namespace StageRunnerTest
{
    [TestFixture]
    public class StageRunner_RunTests
    {
        private readonly Mock<IMockCalls> _mockCalls;
        private readonly LuaTestApi _luaTestApi;
        private readonly ApiContainer _apiContainer;
        private readonly StageRunner _stageRunner;

        public StageRunner_RunTests()
        {
            _mockCalls = new Mock<IMockCalls>();
            _luaTestApi = new LuaTestApi(_mockCalls.Object);
            _apiContainer = new ApiContainer();
            _apiContainer.AddLuaApi(_luaTestApi);
            _stageRunner = new StageRunner(_apiContainer);
        }

        [Test]
        public void SingleStage_RunsToSuccess_IfConfiguredCorrectly()
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

                    Stages = Stage('MyStage', SimpleStage())
                ";

            _stageRunner.Run(script);
            _mockCalls.Verify(m => m.Call("TestValue"));
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