using System;
using System.Collections.Generic;
using System.Linq;
using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EyesightManager : Singleton<EyesightManager>
{
    public class EyesightModifier
    {
        public float eyesightDamageMultiplier = 1;
        public float missingHealthPercentageHeal = 0;
        public float maxHealthPercentageHeal = 0;
        public List<(float duration, float damageMultiplier)> maskOffTemporaryDamageMultipliers = new List<(float duration, float damageMultiplier)>();
        public float lpmEyesigthDamageMultiplier = 1;

        public float GetCurrentDamageMultiplier()
        {
            float maskOffTimestamp = WeldingMask.Instance.MaskOffTimestamp;
            float temporaryMultiplier = maskOffTemporaryDamageMultipliers.Where(e => maskOffTimestamp + e.duration > Time.time).Aggregate(1f, (current, e) => current * e.damageMultiplier);
            float lpmMultiplier = PlayerInputManager.Instance.IsWelding ? lpmEyesigthDamageMultiplier : 1;
            return eyesightDamageMultiplier * temporaryMultiplier * lpmMultiplier;
        }
    }

    [SerializeField]
    private float maxEyesight = 1;
    [SerializeField]
    float eyesightReductionPerSecond = 0.1f;

    [SerializeField]
    private RawImage itemsImage;

    [SerializeField]
    private Volume volume;

    [SerializeField]
    private Gradient eyesightGradient;

    public float MaxEyesight => maxEyesight;

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
        TaskManager.Instance.TaskCompleted += OnTaskCompleted;
        PreWorkPhaseStart();
    }

    private void OnTaskCompleted(Task _)
    {
        Eyesight.Value = Mathf.Clamp01(Eyesight.Value + (MaxEyesight - Eyesight.Value) * modifier.missingHealthPercentageHeal);
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
        Eyesight.Value = MaxEyesight;
    }

    private void Update()
    {
        if (Welder.Instance.IsWelding && !WeldingMask.Instance.MaskOn.Value)
        {
            Eyesight.Value -= eyesightReductionPerSecond * Time.deltaTime * modifier.GetCurrentDamageMultiplier();
        }
        if (WeldingMask.Instance.MaskOn.Value)
        {
            Eyesight.Value = Mathf.Clamp01(Eyesight.Value + MaxEyesight * modifier.maxHealthPercentageHeal * Time.deltaTime);
        }
    }
}
