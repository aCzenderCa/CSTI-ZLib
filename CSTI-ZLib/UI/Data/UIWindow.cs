using UnityEngine;

namespace CSTI_ZLib.UI.Data
{
    public class UIWindow : UIPanel
    {
        private static Transform? _canvasRoot;

        public static Transform? CanvasRoot
        {
            get
            {
                if (_canvasRoot == null && GraphicsManager.Instance)
                {
                    _canvasRoot = GraphicsManager.Instance.MenuObject.transform.parent;
                }

                return _canvasRoot;
            }
        }

        public void Open()
        {
            if (Self != null)
            {
                Self.gameObject.SetActive(true);
                FullInit();
                return;
            }

            if (CanvasRoot == null) return;

            Build(CanvasRoot);
        }

        public void Close()
        {
            if (Self != null)
            {
                Self.gameObject.SetActive(false);
            }
        }
    }
}