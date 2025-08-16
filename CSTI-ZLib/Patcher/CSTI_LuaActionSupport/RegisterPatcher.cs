using System;
using CSTI_LuaActionSupport.AllPatcher;
using CSTI_LuaActionSupport.LuaBuilder;
using CSTI_LuaActionSupport.LuaCodeHelper;
using CSTI_LuaActionSupport.UIStruct;
using CSTI_ZLib.Utils;
using HarmonyLib;
using NLua;

namespace CSTI_ZLib.Patcher.CSTI_LuaActionSupport;

[HarmonyPatch]
public static class RegisterPatcher
{
    public static void Overwrite_CardActionPatcher_Register()
    {
        CommonLuaRegister.Register(MainRuntime.Lua, typeof(DataAccessTool));
        CommonLuaRegister.Register(MainRuntime.Lua, typeof(CardActionPatcher));
        CommonLuaRegister.Register(MainRuntime.Lua, typeof(LuaTimer), "LuaTimer");
        CommonLuaRegister.Register(MainRuntime.Lua, typeof(LuaInput), "LuaInput");
        CommonLuaRegister.Register(MainRuntime.Lua, typeof(LuaGraphics), "LuaGraphics");
        CommonLuaRegister.Register(MainRuntime.Lua, typeof(LuaSystem), "LuaSystem");
        CommonLuaRegister.Register(MainRuntime.Lua, typeof(LuaAnim), "LuaAnim");
        CommonLuaRegister.Register(MainRuntime.Lua, typeof(MainBuilder), "MainBuilder");
        CommonLuaRegister.Register(MainRuntime.Lua, typeof(UIManagers), "UIManagers");
    }
}