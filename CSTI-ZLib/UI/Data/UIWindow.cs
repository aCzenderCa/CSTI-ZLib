using UnityEngine;

namespace CSTI_ZLib.UI.Data;

public class UIWindow : UIPanel
{
    public static Transform? CanvasRoot
    {
        get
        {
            if (field == null && GraphicsManager.Instance)
            {
                field = GraphicsManager.Instance.MenuObject.transform.parent;
            }

            return field;
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

    public override void Reset()
    {
        base.Reset();
        foreach (var child in Children)
        {
            child.Reset();
        }
    }

    protected override void Init()
    {
        if (Self != null) Self.gameObject.SetActive(false);
        base.Init();
    }

    protected override void ValidInit()
    {
        base.ValidInit();
        if (Self != null) Self.gameObject.SetActive(true);
    }

    public override void Dispose()
    {
        Destroy();
    }
}