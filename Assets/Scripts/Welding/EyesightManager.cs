using System;
using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EyesightManager : Singleton<EyesightManager>
{
    public class EyesightModifier
    {
        public float eyesightDamageReduction = 1;
    }

    [SerializeField]
    float eyesightReductionPerSecond = 0.1f;

    [SerializeField]
    private RawImage itemsImage;

    [SerializeField]
    private Volume volume;

    [SerializeField]
    private Gradient eyesightGradient;

    private EyesightModifier modifier;

    public ObservableVariable<float> Eyesight = new ObservableVariable<float>(1);

    protected override void Awake()
    {
        Eyesight.ValueChanged += OnEyesightChanged;
        base.Awake();
    }

    private void Start()
    {
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += PreWorkPhaseStart;
        PreWorkPhaseStart();
    }

    private void OnEyesightChanged(float oldValue, float newValue)
    {
        volume.weight = 1 - newValue;
        /*Color color = Color.white * newValue;
        color.a = 1;*/
        Color color = eyesightGradient.Evaluate(1 - newValue);
        itemsImage.color = color;
    }

    private void PreWorkPhaseStart()
    {
        modifier = PlayerUpgrades.Instance.GetModifier<EyesightModifier>();
        Eyesight.Value = 1;
    }

    private void Update()
    {
        if (Welder.Instance.IsWelding && !WeldingMask.Instance.MaskOn.Value)
        {
            Eyesight.Value -= eyesightReductionPerSecond * Time.deltaTime * modifier.eyesightDamageReduction;
        }
    }
}
