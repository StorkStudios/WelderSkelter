using System;
using DG.Tweening;
using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeldingMask : Singleton<WeldingMask>
{
    public class WeldingMaskModifier
    {
        public float darknessAlpha = 1;
        public float darknessScale = 1;
    }

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
    private CanvasGroup maskShadow;

    private bool moveMaskShadow = false;

    private WeldingMaskModifier modifier;

    public float MaskOnTimestamp { get; private set; } = float.PositiveInfinity;
    public float MaskOffTimestamp { get; private set; } = float.NegativeInfinity;

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
        maskShadow.gameObject.SetActive(false);
        modifier = PlayerUpgrades.Instance.GetModifier<WeldingMaskModifier>();

        maskShadow.alpha = modifier.darknessAlpha;
        maskShadow.transform.localScale = new Vector3(modifier.darknessScale, modifier.darknessScale, 1);

        MaskOnTimestamp = float.PositiveInfinity;
        MaskOffTimestamp = float.NegativeInfinity;
    }

    private void OnMaskToggled()
    {
        maskOn.Value = !maskOn.Value;
        if (maskOn.Value)
        {
            MaskOnTimestamp = Time.time;
            maskShadow.gameObject.SetActive(true);
            maskShadow.transform.position = maskOffPosition.position;
            mask.DOMove(maskOnPosition.position, 0.1f).OnComplete(() =>
            {
                moveMaskShadow = true;
            });
        }
        else
        {
            MaskOffTimestamp = Time.time;
            maskShadow.gameObject.SetActive(false);
            maskShadow.transform.position = maskOffPosition.position;
            mask.DOMove(maskOffPosition.position, 0.1f).OnComplete(() =>
            {
                moveMaskShadow = false;
            });
        }
    }
}
