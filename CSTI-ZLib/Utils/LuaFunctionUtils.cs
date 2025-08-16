using System;
using System.Collections.Generic;
using NLua;

namespace CSTI_ZLib.Utils;

public static class LuaFunctionUtils
{
    private static readonly Dictionary<Type, Dictionary<LuaFunction, MulticastDelegate>> Delegates = new();

    public static Action Map(this LuaFunction function)
    {
        var dtype = typeof(Action);
        if (!Delegates.ContainsKey(dtype)) Delegates[dtype] = new Dictionary<LuaFunction, MulticastDelegate>();
        if (Delegates.TryGetValue(dtype, out var map) && map.TryGetValue(function, out var action))
            return (Action)action;

        Delegates[dtype][function] = Action;
        return Action;
        void Action() => function.Call();
    }

    public static Action<T> Map<T>(this LuaFunction function)
    {
        var dtype = typeof(Action<T>);
        if (!Delegates.ContainsKey(dtype)) Delegates[dtype] = new Dictionary<LuaFunction, MulticastDelegate>();
        if (Delegates.TryGetValue(dtype, out var map) && map.TryGetValue(function, out var action))
            return (Action<T>)action;

        Delegates[dtype][function] = Action;
        return Action;
        void Action(T t) => function.Call(t);
    }
}