using CSTI_ZLib.LuaLIbs.Utils;
using CSTI_ZLib.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CSTI_ZLib.LuaLIbs
{
    [LuaLib]
    public static class CardVisualize
    {
        public static void SetIcon(object self, string icon, float x, float y)
        {
            if (!CardUtils.TryParseCard(self, out var cardBridge)) return;

            var cardBase = cardBridge.CardBase;
            if (ModLoader.ModLoader.SpriteDict.TryGetValue(icon, out var sprite))
            {
                var image = CreateImage(cardBase.transform, icon);
                image.sprite = sprite;
                image.transform.localPosition = new Vector3(x, y, 0f);
            }
        }

        private static Image CreateImage(Transform baseTr, string name = "IMG")
        {
            var cimg = baseTr.GetChildOrCreate("CIMG");
            var o = cimg.GetChildOrCreate(name);
            return o.GetOrAdd<Image>();
        }
    }
}