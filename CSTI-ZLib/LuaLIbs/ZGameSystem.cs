using System;
using System.Collections.Generic;
using CSTI_LuaActionSupport.LuaCodeHelper;
using CSTI_ZLib.Patcher;
using CSTI_ZLib.Patcher.Utils;
using CSTI_ZLib.Utils;
using NLua;

namespace CSTI_ZLib.LuaLIbs;

[LuaLib]
public static class ZGameSystem
{
    #region Init

    internal static void LuaLibInit()
    {
        GamePatcher.OnPassTp += GamePatcherOnOnPassTp;
        CardVisualPatcher.CardClickEvent += CardVisualPatcherOnCardClickEvent;
    }

    #endregion

    #region Callbacks

    private static void GamePatcherOnOnPassTp()
    {
        foreach (var card in GameManager.Instance.AllCards)
        {
            var uniqueID = card?.CardModel?.UniqueID;
            if (uniqueID != null && TpSystems.TryGetValue(uniqueID, out var tpSystem))
            {
                var cardAccessBridge = new CardAccessBridge(card);
                foreach (var action in tpSystem)
                {
                    action(cardAccessBridge);
                }
            }
        }
    }

    private static bool CardVisualPatcherOnCardClickEvent(InGameCardBase card)
    {
        var uniqueID = card.CardModel.UniqueID;
        if (uniqueID != null && ClickSystems.TryGetValue(uniqueID, out var clickSystem))
        {
            var cardAccessBridge = new CardAccessBridge(card);
            foreach (var action in clickSystem)
            {
                if (action(cardAccessBridge))
                {
                    return true;
                }
            }
        }

        return false;
    }

    #endregion

    private static readonly Dictionary<string, List<Action<CardAccessBridge>>> TpSystems = new();
    private static readonly Dictionary<string, List<Func<CardAccessBridge, bool>>> ClickSystems = new();

    #region Reg

    internal static void RegisterCardTp(string uuid, Action<CardAccessBridge> action)
    {
        TpSystems.Add(uuid, action);
    }

    internal static void RegisterCardOnClick(string uuid, Func<CardAccessBridge, bool> action)
    {
        ClickSystems.Add(uuid, action);
    }

    public static void RegisterCardTp(string uuid, LuaFunction action)
    {
        TpSystems.Add(uuid, action.Map<CardAccessBridge>());
    }

    public static void RegisterCardOnClick(string uuid, LuaFunction action)
    {
        ClickSystems.Add(uuid, bridge => action.Call(bridge) is { Length: > 0 } results && results[0] is true);
    }

    #endregion
}