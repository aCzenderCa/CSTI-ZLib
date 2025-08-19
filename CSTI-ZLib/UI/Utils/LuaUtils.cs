using System.Collections.Generic;
using CSTI_LuaActionSupport.LuaCodeHelper;
using NLua;

namespace CSTI_ZLib.UI.Utils;

public static class LuaUtils
{
    public static LuaTable ToLuaList<T>(this IEnumerable<T> enumerable)
    {
        var tempTable = MainRuntime.Lua.TempTable();
        var i = 0;
        foreach (var val in enumerable)
        {
            i++;
            tempTable[i] = val;
        }

        return tempTable;
    }
}