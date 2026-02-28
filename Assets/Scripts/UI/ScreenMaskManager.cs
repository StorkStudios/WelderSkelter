using DG.Tweening;
using StorkStudios.CoreNest;
using System;
using UnityEngine;

public class ScreenMaskManager : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image image;

    [SerializeField]
    private float fadeDuration;
    
    public void FadeIn(TweenCallback callback)
    {
        image.color = Color.clear;
        image.DOColor(Color.black, fadeDuration).SetEase(Ease.OutQuart).OnComplete(callback);
    }

    public void FadeOut(Action callback)
    {
        image.color = Color.clear;
        callback?.Invoke();
/*        image.DOColor(Color.clear, fadeDuration).SetEase(Ease.InQuart);
        //Called a little bit before
        this.CallDelayed(fadeDuration - 0.3f, callback);*/
    }
}
