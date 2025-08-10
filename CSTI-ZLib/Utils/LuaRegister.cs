using System;
using System.Linq;
using System.Reflection;
using CSTI_LuaActionSupport.AllPatcher;
using HarmonyLib;

namespace CSTI_ZLib.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LuaLibAttribute : Attribute
    {
    }

    public static class CommonLuaRegister
    {
        public static void RegisterAll()
        {
            var luaRuntime = CardActionPatcher.LuaRuntime;
            foreach (var luaLibType in from assembly in AccessTools.AllAssemblies()
                     from type in AccessTools.GetTypesFromAssembly(assembly)
                     where type.GetCustomAttribute(typeof(LuaLibAttribute)) != null
                     select type)
            {
                luaRuntime.NewTable(luaLibType.Name);
                foreach (var methodInfo in luaLibType.GetMethods(
                             BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static))
                {
                    luaRuntime.RegisterFunction($"{luaLibType.Name}.{methodInfo.Name}", methodInfo);
                }
            }
        }
    }
}