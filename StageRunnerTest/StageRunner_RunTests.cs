using FluentAssertions;
using NUnit.Framework;
using Schuster;
using Moq;

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
        public void SingleStage_RunsToSuccess()
        {
            const string script = @"
                    local function RunFunction() 
                        TestValue('TestValue')
                    end

                    local simpleStage = {
                        Run = RunFunction
                    }

                    local someStage = Stage('MyStage', simpleStage)
                    Stages = someStage
                ";

            _stageRunner.Run(script);
            _mockCalls.Verify(m => m.Call("TestValue"));
        }
    }
}