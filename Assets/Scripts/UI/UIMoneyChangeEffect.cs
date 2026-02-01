using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIMoneyChangeEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private CanvasGroup group;
    [SerializeField]
    private Color minusColor;
    [SerializeField]
    private Color plusColor;
    [SerializeField]
    private float animationDuration;
    [SerializeField]
    private float fadeDuration;
    [SerializeField]
    private float distance;

    public void StartAnimation(int moneyDelta)
    {
        bool onPlus = moneyDelta >= 0;
        text.text = $"{(onPlus ? '+' : '-')}{Mathf.Abs(moneyDelta)}zł";
        text.color = onPlus ? plusColor : minusColor;

        group.DOFade(0, fadeDuration).SetDelay(animationDuration - fadeDuration);
        transform.DOMoveY(transform.position.y + (onPlus ? 1 : -1) * distance, animationDuration).OnComplete(() => Destroy(this));
    }
}
