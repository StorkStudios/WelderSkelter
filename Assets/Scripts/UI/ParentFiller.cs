using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
public class ParentFiller : UIBehaviour, ILayoutSelfController
{
    [NotNull]
    [SerializeField]
    private RectTransform otherChild;

    private DrivenRectTransformTracker tracker;

    private RectTransform rectTransform => transform as RectTransform;

    protected override void OnEnable()
    {
        base.OnEnable();
        Update();
    }

    protected override void OnDisable()
    {
        tracker.Clear();
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        base.OnDisable();
    }

    protected override void OnTransformParentChanged()
    {
        base.OnTransformParentChanged();
        Update();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        Update();
    }

    private void Update()
    {
        tracker.Clear();

        if (otherChild == null)
        {
            return;
        }

        Rect parentRect = (transform.parent as RectTransform).rect;
        float childHeight = otherChild.rect.height;

        float height = parentRect.height - childHeight;
        tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(0, height - parentRect.size.y * (rectTransform.anchorMax.y - rectTransform.anchorMin.y)));
    }

    public void SetLayoutHorizontal() { }

    public void SetLayoutVertical() { }
}
