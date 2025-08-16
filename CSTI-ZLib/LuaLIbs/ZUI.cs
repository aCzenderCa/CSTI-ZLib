using CSTI_ZLib.UI.Data;
using CSTI_ZLib.Utils;
using UnityEngine;

namespace CSTI_ZLib.LuaLIbs;

[LuaLib]
public class ZGameUI
{
    public static UIWindow CreateWindow(string name, string backgroundSprite = "", float x = 0, float y = 0, float width = 100,
        float height = 100)
    {
        return new UIWindow
        {
            Name = name,
            LocalPosition = new Vector2(x, y),
            Size = new Vector2(width, height),
            Sprite = backgroundSprite,
        };
    }
}