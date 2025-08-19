using System;
using System.Collections.Generic;

namespace CSTI_ZLib.Patcher.Utils;

public static class DictUtils
{
    public static void Add<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey key, TValue value)
    {
        if (!dict.ContainsKey(key)) dict.Add(key, []);

        dict[key].Add(value);
    }

    public static void Add<TKey, TDelegate>(this Dictionary<TKey, TDelegate?> dict, TKey key, TDelegate value)
        where TDelegate : MulticastDelegate
    {
        if (!dict.ContainsKey(key)) dict.Add(key, value);
        else dict[key] = (TDelegate)Delegate.Combine(dict[key], value);
    }
}