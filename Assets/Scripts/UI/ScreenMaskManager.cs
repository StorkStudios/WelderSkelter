using DG.Tweening;
using StorkStudios.CoreNest;
using System;
using UnityEngine;

public class ScreenMaskManager : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image image;

    [SerializeField]
    private float fadeInDuration;

    [SerializeField]
    private float fadeOutDuration;
    
    public void FadeIn(TweenCallback callback)
    {
        image.color = Color.clear;
        image.DOColor(Color.black, fadeInDuration).SetEase(Ease.OutQuart).OnComplete(callback);
    }

    public void FadeOut(TweenCallback callback)
    {
        image.color = Color.black;
        image.DOColor(Color.clear, fadeOutDuration).SetEase(Ease.InQuart).OnComplete(callback);
    }
}
