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
                cimg.transform.SetParent(t);
                child = cimg.GetComponent<RectTransform>();
            }
            
            return child;
        }
    }
}