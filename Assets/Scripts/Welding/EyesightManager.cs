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

    private EyesightModifier modifier;

    public ObservableVariable<float> Eyesight = new ObservableVariable<float>(1);

    private void Start()
    {
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += PreWorkPhaseStart;
        PreWorkPhaseStart();
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
            volume.weight = 1 - Eyesight.Value;
            Color color = itemsImage.color;
            color.a = Eyesight.Value;
            itemsImage.color = color;
            if (Eyesight.Value <= 0)
            {
                Debug.Log("Blind sisiphus");
            }
        }
    }
}
