using BepInEx;
using CSTI_ZLib.Utils;

namespace CSTI_ZLib
{
    [BepInPlugin("zender.CSTI_ZLib.MainRuntime", "CSTI_ZLib", "1.0")]
    [BepInDependency("zender.LuaActionSupport.LuaSupportRuntime")]
    public class MainRuntime : BaseUnityPlugin
    {
        private void Start()
        {
            CommonLuaRegister.RegisterAll();
        }
    }
}