using System.Collections.Generic;
using NLua;

namespace Schuster
{
	public class ExtensionCollection
	{
		private readonly List<ILuaExtension> _luaExtensions = new();

		public void AddLuaExtension(ILuaExtension luaExtension)
		{
			_luaExtensions.Add(luaExtension);
		}

		public void LoadExtensions(Lua lua)
		{
			foreach (var extension in _luaExtensions)
			{
				extension.LoadExtension(lua);
			}
		}
	}
}