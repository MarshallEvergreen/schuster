using NLua;
using Schuster;

namespace PipelineTests.Helpers
{
	public class LuaTestExtension : ILuaExtension
	{
		private readonly IMockCalls _mockCalls;

		public LuaTestExtension(IMockCalls mockCalls)
		{
			_mockCalls = mockCalls;
		}

		public void LoadExtension(Lua lua)
		{
			lua.RegisterFunction("TestValue", this,
				typeof(LuaTestExtension).GetMethod("TestValue"));
		}

		public void TestValue(string testValue)
		{
			_mockCalls.Call(testValue);
		}
	}
}