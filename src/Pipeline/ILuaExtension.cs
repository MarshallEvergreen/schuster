using NLua;

namespace Schuster
{
	public interface ILuaExtension
	{
		public void LoadExtension(Lua lua);
	}
}