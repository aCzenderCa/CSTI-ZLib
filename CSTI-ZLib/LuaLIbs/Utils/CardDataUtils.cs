using System;
using ModLoader.LoaderUtil;
using UnityEngine;

namespace CSTI_ZLib.LuaLIbs.Utils;

public static class CardDataUtils
{
    public static void LoadImage(this CardData cardData, string iconId, Action<CardData, Sprite> callback)
    {
        if (ModLoader.ModLoader.SpriteDict.TryGetValue(iconId, out var icon))
        {
            callback(cardData, icon);
        }
        else
        {
            cardData.PostSetEnQueue((o, sp) =>
            {
                var card = (CardData)o;
                callback(card, (Sprite)sp);
            }, iconId);
        }
    }
}