using NLua;

namespace Schuster
{
	public class TaskExtension : ILuaExtension
	{
		public void RegisterExtension(Lua lua)
		{
			lua.LoadCLRPackage();
			lua.DoString(@" import ('Pipeline', 'Schuster.Tasks')");
		}
	}
}