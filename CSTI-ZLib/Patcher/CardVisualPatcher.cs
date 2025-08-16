using System;
using System.Collections.Generic;
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
}