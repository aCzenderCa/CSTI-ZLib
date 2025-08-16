using UnityEngine;

namespace CSTI_ZLib.LuaLIbs.Utils
{
    public static class ComUtils
    {
        public static T GetOrAdd<T>(this GameObject go)
            where T : Component
        {
            var component = go.GetComponent<T>();
            if (!component) component = go.AddComponent<T>();
            return component;
        }

        public static T GetOrAdd<T>(this Transform tr)
            where T : Component
        {
            var component = tr.GetComponent<T>();
            if (!component) component = tr.gameObject.AddComponent<T>();
            return component;
        }

        public static void DestroyCom<T>(this Transform tr)
            where T : Component
        {
            var component = tr.GetComponent<T>();
            if (component)
            {
                Object.Destroy(component);
            }
        }
    }
}