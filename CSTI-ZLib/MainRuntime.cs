using System.Reflection;
using BepInEx;
using CSTI_LuaActionSupport.AllPatcher;
using CSTI_LuaActionSupport.LuaCodeHelper;
using CSTI_ZLib.Patcher.CSTI_LuaActionSupport;
using CSTI_ZLib.UI.Data;
using CSTI_ZLib.Utils;
using HarmonyLib;
using NLua;
using TMPro;

namespace CSTI_ZLib;

[BepInPlugin("zender.CSTI_ZLib.MainRuntime", "CSTI_ZLib", Version)]
[BepInDependency("zender.LuaActionSupport.LuaSupportRuntime")]
public class MainRuntime : BaseUnityPlugin
{
    public const string Version = "1.0.3";

    public static Lua Lua => CardActionPatcher.LuaRuntime;

    private void Awake()
    {
        Harmony.CreateAndPatchAll(typeof(MainRuntime));
        RegisterPatcher.Overwrite_CardActionPatcher_Register();
    }

    private void Start()
    {
        CommonLuaRegister.RegisterAll();
        Lua.Register<TextAlignmentOptions>($"Enums.{nameof(TextAlignmentOptions)}");
    }

    private static UIWindow TestUI()
    {
        var uiWindow = new UIWindow
        {
            Name = "T",
            Sprite = "Wind&Leaves_WolfHideCured"
        };
        var uiScrollPanel = uiWindow.AddScrollRect("S", false, scrollSpeed: 3.6f, mask: true);
        uiScrollPanel.AddText("A0", "Test0", 32);
        uiScrollPanel.AddText("A1", "Test1", 32, y: 100);
        uiWindow.Open();
        return uiWindow;
    }

    public static void SetLua(object o, string key)
    {
        Lua[key] = o;
    }

    // CardVisualize.SetIcon(a123, "Wind&Leaves_Blueberry", 20, 50, 200, 300)
    // CardVisualize.SetText(a123, "测试<size=12>文本</size>", 20, 50, 200, 300)
    public static object[] RunLua(string luaCode)
    {
        return Lua.DoString(luaCode, "ZLib.Debug");
    }
}