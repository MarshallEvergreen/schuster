using NLua;

namespace Schuster
{
	public class StageApi : ILuaApi
	{
		private readonly Lua _lua;

		public StageApi(Lua lua)
		{
			lua.LoadCLRPackage();
			_lua = lua;
			lua.DoString(@" import ('StageRunner', 'Schuster.Stages')");
		}

		public void RegisterTo(Lua lua)
		{
		}
	}
}