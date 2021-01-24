using NLua;
using Schuster;

namespace StageRunnerTest.Helpers
{
    public class LuaTestApi : ILuaApi
    {
        private readonly IMockCalls _mockCalls;

        public LuaTestApi(IMockCalls mockCalls)
        {
            _mockCalls = mockCalls;
        }

        public void RegisterTo(Lua lua)
        {
            lua.RegisterFunction("TestValue", this,
                typeof(LuaTestApi).GetMethod("TestValue"));
        }

        public void TestValue(string testValue)
        {
            _mockCalls.Call(testValue);
        }
    }
}