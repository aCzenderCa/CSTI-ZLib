using System;
using System.Collections;
using ChatTreeLoader.Util;
using HarmonyLib;

namespace CSTI_ZLib.Patcher;

[HarmonyPatch]
public static class CardPatcher
{
    [HarmonyPatch(typeof(InGameCardBase), nameof(InGameCardBase.ResetCard)), HarmonyPostfix]
    private static void OnCardRecyclePatch(this InGameCardBase __instance, ref IEnumerator __result)
    {
        __result = __result.OnStart(() => OnCardRecycle?.Invoke(__instance));
    }

    [HarmonyPatch(typeof(InGameCardBase), nameof(InGameCardBase.Init)), HarmonyPostfix]
    private static void OnCardInitPatch(this InGameCardBase __instance, ref IEnumerator __result)
    {
        __result = __result.OnEnd(() => OnCardInit?.Invoke(__instance));
    }

    internal static event Action<InGameCardBase>? OnCardRecycle;
    internal static event Action<InGameCardBase>? OnCardInit;
}