using System;
using CSTI_ZLib.LuaLIbs.Utils;
using CSTI_ZLib.UI.Com;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CSTI_ZLib.UI.Data
{
    public class CommonUIBase : IDisposable
    {
        public RectTransform? Self;
        public string Name = "";
        public Vector2 Size = new(100, 100);
        public Vector2 LocalPosition;
        public float Rotation;
        public Vector2 LocalScale = Vector2.one;
        protected Action? OnClick;

        public void AddOnClick(Action action)
        {
            OnClick += action;
        }

        public virtual void Build(Transform parent)
        {
            Self = parent.GetChildOrCreate(Name);
            FullInit();
        }

        public void FullInit()
        {
            Init();
            ValidInit();
        }

        protected virtual void Init()
        {
            if (Self == null) return;
            Self.sizeDelta = Size;
            Self.anchoredPosition = LocalPosition;
            Self.localRotation = Quaternion.Euler(0, 0, Rotation);
            Self.localScale = LocalScale;

            if (OnClick != null)
            {
                Self.GetOrAdd<EventOnClick>().OnClick += OnClick;
            }
        }

        protected virtual void ValidInit()
        {
        }

        public void Destroy()
        {
            if (Self != null)
            {
                Object.Destroy(Self.gameObject);
            }
        }

        public virtual void Dispose()
        {
            if (Self == null) return;
            if (OnClick != null)
            {
                Self.DestroyCom<EventOnClick>();
            }
        }

        ~CommonUIBase()
        {
            Dispose();
        }
    }
}