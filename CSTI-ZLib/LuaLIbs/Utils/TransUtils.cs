using UnityEngine;

namespace CSTI_ZLib.LuaLIbs.Utils
{
    public static class TransUtils
    {
        public static Transform GetChildOrCreate(this Transform t, string name)
        {
            var child = t.Find(name);
            if (child == null)
            {
                var cimg = new GameObject(name, typeof(RectTransform));
                cimg.transform.SetParentAndReset(t);
                child = cimg.GetComponent<RectTransform>();
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