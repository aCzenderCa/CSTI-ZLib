using BepInEx;
using CSTI_LuaActionSupport.AllPatcher;
using CSTI_ZLib.Utils;
using HarmonyLib;
using NLua;

namespace CSTI_ZLib
{
    [BepInPlugin("zender.CSTI_ZLib.MainRuntime", "CSTI_ZLib", "1.0")]
    [BepInDependency("zender.LuaActionSupport.LuaSupportRuntime")]
    public class MainRuntime : BaseUnityPlugin
    {
        public static Lua Lua => CardActionPatcher.LuaRuntime;

        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(MainRuntime));
        }

        private void Start()
        {
            CommonLuaRegister.RegisterAll();
        }

        public static void SetLua(object o, string key)
        {
            Lua[key] = o;
        }

        // CardVisualize.SetIcon(a123, "Wind&Leaves_Blueberry", 20, 50, 200, 300)
        // CardVisualize.SetText(a123, "测试<size=12>文本</size>", 20, 50, 200, 300)
        public static void RunLua(string luaCode)
        {
            Lua.DoString(luaCode, "ZLib.Debug");
        }
    }
}