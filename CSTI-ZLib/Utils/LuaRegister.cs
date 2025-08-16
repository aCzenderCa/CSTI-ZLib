using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace CSTI_ZLib.Utils;

[AttributeUsage(AttributeTargets.Class)]
public class LuaLibAttribute : Attribute
{
}

public static class CommonLuaRegister
{
    public static void RegisterAll()
    {
        foreach (var luaLibType in from assembly in AccessTools.AllAssemblies()
                 from type in AccessTools.GetTypesFromAssembly(assembly)
                 where type.GetCustomAttribute(typeof(LuaLibAttribute)) != null
                 select type)
        {
            MainRuntime.Lua.NewTable(luaLibType.Name);
            foreach (var methodInfo in luaLibType.GetMethods(
                         BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static))
            {
                MainRuntime.Lua.RegisterFunction($"{luaLibType.Name}.{methodInfo.Name}", methodInfo);
            }
        }

        foreach (var luaLibType in from assembly in AccessTools.AllAssemblies()
                 from type in AccessTools.GetTypesFromAssembly(assembly)
                 where type.GetCustomAttribute(typeof(LuaLibAttribute)) != null
                 select type)
        {
            var luaLibInitFunc = AccessTools.DeclaredMethod(luaLibType, "LuaLibInit");
            if (luaLibInitFunc != null)
            {
                luaLibInitFunc.Invoke(null, []);
            }
        }
    }
}