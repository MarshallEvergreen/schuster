using System.Collections.Generic;
using NUnit.Framework;
using Schuster;
using Moq;

namespace StageRunnerTest
{
    [TestFixture]
    public class StageRunnerTestContext
    {
        protected Mock<IMockCalls> _mockCalls;
        protected LuaTestApi _luaTestApi;
        protected ApiContainer _apiContainer;
        protected StageRunner _stageRunner;
        
        protected List<StageRunnerStatus> _statusUpdates;

        [SetUp]
        public void Setup()
        {
            _mockCalls = new Mock<IMockCalls>();
            _luaTestApi = new LuaTestApi(_mockCalls.Object);
            _apiContainer = new ApiContainer();
            _apiContainer.AddLuaApi(_luaTestApi);
            _stageRunner = new StageRunner(_apiContainer);

            _statusUpdates = new List<StageRunnerStatus>();

            _mockCalls.Setup(m => m.Call(It.IsAny<StageRunnerStatus>()))
                .Callback<StageRunnerStatus>((update) => { _statusUpdates.Add(update); });

            _stageRunner.StatusUpdate += status => { _mockCalls.Object.Call(status); };
        }
    }
}