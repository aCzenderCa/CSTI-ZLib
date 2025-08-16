using System;
using System.Collections;
using System.Collections.Generic;
using ChatTreeLoader.Util;
using CSTI_LuaActionSupport.Helper;
using HarmonyLib;

namespace CSTI_ZLib.Patcher
{
    [HarmonyPatch]
    public static class GamePatcher
    {
        public static event Action? OnPassTp;
        public static List<CoroutineController> OnPassTpEnumerators = new();

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.ProgressCurrentResearch)), HarmonyPostfix]
        public static void OnPassTpPatch(ref IEnumerator __result)
        {
            __result = __result.OnEnd(OnEnd());
            return;

            IEnumerator OnEnd()
            {
                OnPassTpEnumerators.Clear();
                OnPassTp?.Invoke();
                var coroutineQueue = GameManager.Instance.ProcessCache();
                while (coroutineQueue.Dequeue() is { } coroutine)
                {
                    while (coroutine.state == CoroutineState.Running)
                    {
                        yield return null;
                    }
                }

                foreach (var coroutine in OnPassTpEnumerators)
                {
                    while (coroutine.state == CoroutineState.Running)
                    {
                        yield return null;
                    }
                }
            }
        }
    }
}