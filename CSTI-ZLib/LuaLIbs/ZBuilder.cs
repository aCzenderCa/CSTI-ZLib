using CSTI_ZLib.LuaLIbs.Data;
using CSTI_ZLib.Utils;
using UnityEngine;

namespace CSTI_ZLib.LuaLIbs;

[LuaLib]
public static class ZBuilder
{
    #region Init

    internal static void LuaLibInit()
    {
    }

    #endregion

    public static CardDataBase CreateCard(string id, string name, string description)
    {
        return new CardDataBase(id, name, description);
    }
}