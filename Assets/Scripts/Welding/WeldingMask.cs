using System;
using StorkStudios.CoreNest;
using UnityEngine;

public class WeldingMask : Singleton<WeldingMask>
{
    [SerializeField]
    [ReadOnly]
    private ObservableVariable<bool> maskOn = new ObservableVariable<bool>(false);
    public ObservableVariable<bool> MaskOn => maskOn;

    private void Start()
    {
        PlayerInputManager.Instance.ToggleMaskEvent += OnMaskToggled;
    }

    private void OnMaskToggled()
    {
        maskOn.Value = !maskOn.Value;
    }
}
