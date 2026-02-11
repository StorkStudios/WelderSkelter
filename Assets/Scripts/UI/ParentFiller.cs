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

    private RectTransform rectTransform => transform as RectTransform;

    protected override void OnEnable()
    {
        base.OnEnable();
        Update();
    }

    protected override void OnDisable()
    {
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
        if (otherChild == null)
        {
            return;
        }

        Rect parentRect = (transform.parent as RectTransform).rect;
        float childHeight = otherChild.rect.height;

        float height = parentRect.height - childHeight;

        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = height - parentRect.size.y * (rectTransform.anchorMax.y - rectTransform.anchorMin.y);
        rectTransform.sizeDelta = sizeDelta;
    }

    public void SetLayoutHorizontal() { }

    public void SetLayoutVertical() { }
}
