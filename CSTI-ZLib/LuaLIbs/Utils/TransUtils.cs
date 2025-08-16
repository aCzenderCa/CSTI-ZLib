using UnityEngine;

namespace CSTI_ZLib.LuaLIbs.Utils
{
    public static class TransUtils
    {
        public static RectTransform GetChildOrCreate(this Transform t, string name)
        {
            RectTransform? child = null;
            var childCount = t.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var c = t.GetChild(i);
                if (c == null) continue;
                if (c.name != name) continue;
                child = (RectTransform)c;
            }

            if (child == null)
            {
                var obj = new GameObject(name, typeof(RectTransform));
                obj.transform.SetParentAndReset(t);
                child = obj.GetComponent<RectTransform>();
            }

            return child;
        }

        public static void DestroyAllChildren(this Transform t)
        {
            var childCount = t.childCount;
            for (var i = 0; i < childCount; i++)
            {
                Object.Destroy(t.GetChild(i));
            }
        }

        public static bool TryGetChild(this Transform t, string name, out Transform child)
        {
            child = t.Find(name);
            return child;
        }

        public static void SetParentAndReset(this Transform t, Transform parent)
        {
            t.SetParent(parent);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            t.gameObject.layer = parent.gameObject.layer;
        }
    }
}