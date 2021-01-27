using System.Collections.Generic;
using NLua;

namespace Schuster
{
	public class ExtensionCollection
	{
		private readonly List<ILuaExtension> _luaApis = new();

		public void AddLuaApi(ILuaExtension luaExtension)
		{
			_luaApis.Add(luaExtension);
		}

		public void RegisterExtensions(Lua lua)
		{
			foreach (var luaApi in _luaApis)
			{
				luaApi.RegisterExtension(lua);
			}
		}
	}
}