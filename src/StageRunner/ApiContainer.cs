using System.Collections.Generic;
using NLua;

namespace Schuster
{
	public class ApiContainer
	{
		private readonly List<ILuaApi> _luaApis = new();

		public void AddLuaApi(ILuaApi luaApi)
		{
			_luaApis.Add(luaApi);
		}

		public void RegisterApisTo(Lua lua)
		{
			foreach (var luaApi in _luaApis)
			{
				luaApi.RegisterTo(lua);
			}
		}
	}
}