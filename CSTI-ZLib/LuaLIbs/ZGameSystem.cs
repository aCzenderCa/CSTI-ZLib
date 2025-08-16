using System;
using System.Collections.Generic;
using CSTI_LuaActionSupport.LuaCodeHelper;
using CSTI_ZLib.Patcher;
using CSTI_ZLib.Utils;

namespace CSTI_ZLib.LuaLIbs
{
    [LuaLib]
    public static class ZGameSystem
    {
        #region Init

        internal static void LuaLibInit()
        {
            GamePatcher.OnPassTp += GamePatcherOnOnPassTp;
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
                    tpSystem(new CardAccessBridge(card));
                }
            }
        }

        #endregion

        public static Dictionary<string, Action<CardAccessBridge>> TpSystems = new();
    }
}