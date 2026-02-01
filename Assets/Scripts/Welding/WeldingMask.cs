using System;
using DG.Tweening;
using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeldingMask : Singleton<WeldingMask>
{
    [SerializeField]
    [ReadOnly]
    private ObservableVariable<bool> maskOn = new ObservableVariable<bool>(false);
    public ObservableVariable<bool> MaskOn => maskOn;

    [SerializeField]
    private Transform maskOnPosition;
    [SerializeField]
    private Transform maskOffPosition;

    [SerializeField]
    private Transform mask;
    [SerializeField]
    private GameObject maskShadow;

    private bool moveMaskShadow = false;

    private void Start()
    {
        PlayerInputManager.Instance.ToggleMaskEvent += OnMaskToggled;
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnPreStart;

        OnPreStart();
    }

    private void Update()
    {
        if (moveMaskShadow)
        {
            maskShadow.transform.position = Mouse.current.position.value;
        }
    }

    private void OnPreStart()
    {
        maskOn.Value = false;
        mask.position = maskOffPosition.position;
        maskShadow.SetActive(false);
    }

    private void OnMaskToggled()
    {
        maskOn.Value = !maskOn.Value;
        if (maskOn.Value)
        {
            maskShadow.SetActive(true);
            maskShadow.transform.position = maskOffPosition.position;
            mask.DOMove(maskOnPosition.position, 0.1f).OnComplete(() =>
            {
                moveMaskShadow = true;
            });
        }
        else
        {
            maskShadow.SetActive(false);
            maskShadow.transform.position = maskOffPosition.position;
            mask.DOMove(maskOffPosition.position, 0.1f).OnComplete(() =>
            {
                moveMaskShadow = false;
            });
        }
    }
}
