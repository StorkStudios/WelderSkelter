using System;
using StorkStudios.CoreNest;
using UnityEngine;

public class EyesightManager : Singleton<EyesightManager>
{
    [SerializeField]
    float eyesightReductionPerSecond = 0.1f;

    [SerializeField]
    [ReadOnly]
    ObservableVariable<float> eyesight = new ObservableVariable<float>(1);

    private void Update()
    {
        if (Welder.Instance.IsWelding && !WeldingMask.Instance.MaskOn.Value)
        {
            eyesight.Value -= eyesightReductionPerSecond * Time.deltaTime;
            if (eyesight.Value <= 0)
            {
                Debug.Log("Blind sisiphus");
            }
        }
    }
}
