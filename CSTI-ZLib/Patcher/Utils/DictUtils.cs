using System.Collections.Generic;

namespace CSTI_ZLib.Patcher.Utils;

public static class DictUtils
{
    public static void Add<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey key, TValue value)
    {
        if(!dict.ContainsKey(key)) dict.Add(key, []);
        
        dict[key].Add(value);
    }
}