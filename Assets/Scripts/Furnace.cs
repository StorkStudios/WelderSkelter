using StorkStudios.CoreNest;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : Singleton<Furnace>
{
    public class Modifiers
    {
        public bool active = false;
    }

    public event System.Action<Dictionary<WeldingPartData, int>> ItemDestroyed;

    public float LastItemDestructionTimestamp { get; private set; } = float.NegativeInfinity;

    private Modifiers modifiers;

    private void Start()
    {
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnWorkPhasePreStart;
    }

    private void OnWorkPhasePreStart()
    {
        modifiers = PlayerUpgrades.Instance.GetModifier<Modifiers>();
        gameObject.SetActive(modifiers.active);
        LastItemDestructionTimestamp = float.NegativeInfinity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!modifiers.active)
        {
            return;
        }

        WeldingPart part = collision.attachedRigidbody.GetComponent<WeldingPart>();
        ItemDestroyed?.Invoke(part.Components);
        Destroy(part.gameObject);
        LastItemDestructionTimestamp = Time.time;
    }
}
