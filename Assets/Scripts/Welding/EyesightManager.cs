using System;
using StorkStudios.CoreNest;
using UnityEngine;

public class EyesightManager : Singleton<EyesightManager>
{
    public class EyesightModifier
    {
        public float eyesightDamageReduction = 1;
    }

    [SerializeField]
    float eyesightReductionPerSecond = 0.1f;

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
    }

    private void Update()
    {
        if (Welder.Instance.IsWelding && !WeldingMask.Instance.MaskOn.Value)
        {
            Eyesight.Value -= eyesightReductionPerSecond * Time.deltaTime * modifier.eyesightDamageReduction;
            if (Eyesight.Value <= 0)
            {
                Debug.Log("Blind sisiphus");
            }
        }
    }
}
