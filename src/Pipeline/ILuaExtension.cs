using NLua;

namespace Schuster
{
	public interface ILuaExtension
	{
		public void RegisterExtension(Lua lua);
	}
}