using System;
using System.Linq;
using System.Reflection;
using CSTI_LuaActionSupport.LuaCodeHelper;
using HarmonyLib;
using MonoMod.Utils;
using NLua;
using Lua = NLua.Lua;
using LuaFunction = KeraLua.LuaFunction;

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
            var table = (LuaTable)MainRuntime.Lua[luaLibType.Name];
            foreach (var methodInfo in luaLibType.GetMethods(
                         BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static))
            {
                table[methodInfo.Name] = GenerateStaticRuntimeWrap(MainRuntime.Lua, methodInfo);
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

    private static LuaFunction GenerateStaticRuntimeWrap(Lua lua, MethodInfo method)
    {
        var fastReflectionDelegate = method.CreateFastDelegate();
        var isVoidReturnType = method.ReturnType == typeof(void);
        var parameterInfos = method.GetParameters();
        var parameterInfosLength = parameterInfos.Length;
        var paramsTypeCode = new TypeCode[parameterInfosLength];

        var defaultValuesCount = parameterInfos.Count(info => info.HasDefaultValue);
        var defaultValues = defaultValuesCount == 0 ? [] : new (int, object)[defaultValuesCount];

        var iValues = 0;
        for (var i = 0; i < parameterInfos.Length; i++)
        {
            var parameterInfo = parameterInfos[i];
            if (parameterInfo.HasDefaultValue)
            {
                defaultValues[iValues] = (i, parameterInfo.DefaultValue);
                iValues++;
            }

            paramsTypeCode[i] = Type.GetTypeCode(parameterInfo.ParameterType);
        }

        return LuaFunction;

        int LuaFunction(IntPtr luaState)
        {
            var luaPtr = KeraLua.Lua.FromIntPtr(luaState);
            var top = luaPtr.GetTop();
            var args = new object?[parameterInfosLength];
            foreach (var (i, value) in defaultValues)
            {
                args[i] = value;
            }

            for (var i = 0; i < top; i++)
            {
                var o = lua.Translator.GetObject(luaPtr, i + 1);
                if (o != null)
                {
                    var inputTypeCode = Type.GetTypeCode(o.GetType());
                    var paramTypeCode = paramsTypeCode[i];
                    o = o.TransformObject(inputTypeCode, paramTypeCode);
                }

                args[i] = o;
            }

            var result = fastReflectionDelegate(null, args);
            if (isVoidReturnType)
            {
                lua.Push(result);
                return 0;
            }

            return 1;
        }
    }

    public static void Register(Lua luaRuntime, Type typeInfo, string? basePath = null)
    {
        LuaTable? table = null;
        if (basePath != null)
        {
            table = luaRuntime.GetTable(basePath);
            if (table == null)
            {
                luaRuntime.NewTable(basePath);
                table = luaRuntime.GetTable(basePath);
            }
        }

        if (typeInfo.CustomAttributes.FirstOrDefault<CustomAttributeData>(
                (Func<CustomAttributeData, bool>)(data => data.AttributeType == typeof(LuaFuncTodo))) != null) return;
        foreach (var declaredMethod in AccessTools.GetDeclaredMethods(typeInfo))
        {
            if (declaredMethod.IsStatic)
            {
                var customAttributeData = declaredMethod.CustomAttributes.FirstOrDefault<CustomAttributeData>(
                    (Func<CustomAttributeData, bool>)(data => data.AttributeType == typeof(LuaFuncAttribute)));
                if (customAttributeData != null)
                {
                    var namedArguments = customAttributeData.NamedArguments;
                    var funName = namedArguments?.FirstOrDefault(namedArgument => namedArgument.MemberName == "FuncName")
                        .TypedValue.Value ?? declaredMethod.Name;
                    if (table != null)
                    {
                        table[funName.ToString()] = GenerateStaticRuntimeWrap(MainRuntime.Lua, declaredMethod);
                    }
                    else
                    {
                        luaRuntime[funName.ToString()] = GenerateStaticRuntimeWrap(MainRuntime.Lua, declaredMethod);
                    }
                }
            }
        }
    }
}