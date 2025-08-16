using System;
using System.Collections.Generic;
using CSTI_LuaActionSupport.LuaCodeHelper;
using HarmonyLib;

namespace CSTI_ZLib.Patcher;

[HarmonyPatch]
public static class CardVisualPatcher
{
    /// <summary>
    /// 返回值为true则阻断事件
    /// </summary>
    public delegate bool CardClickDelegate(InGameCardBase card);

    public static event CardClickDelegate CardClickEvent
    {
        add => OnCardClicks.Add(value);
        remove => OnCardClicks.Remove(value);
    }

    private static readonly List<CardClickDelegate> OnCardClicks = [];
    public static Dictionary<string, Action<CardAccessBridge, TooltipText>?> CardHoverEvents;


    [HarmonyPatch(typeof(InGameCardBase), nameof(InGameCardBase.OnPointerClick)), HarmonyPrefix]
    private static bool OnCardClickPatch(InGameCardBase __instance)
    {
        if (__instance.Destroyed || !__instance.CardModel) return false;
        foreach (var onCardClick in OnCardClicks)
        {
            if (onCardClick(__instance))
            {
                return false;
            }
        }

        return true;
    }


    [HarmonyPatch(typeof(InGameCardBase), nameof(InGameCardBase.OnHoverEnter)), HarmonyPrefix]
    private static void OnHoverEnterPatch(InGameCardBase __instance)
    {
        var uniqueID = __instance.CardModel?.UniqueID;
        if (!string.IsNullOrEmpty(uniqueID) && CardHoverEvents.TryGetValue(uniqueID!, out var onHover))
        {
            onHover?.Invoke(new CardAccessBridge(__instance), __instance.MyTooltip);
        }
    }
}