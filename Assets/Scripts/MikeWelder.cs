using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MikeWelder : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private GameObject welderParticles;
    [SerializeField]
    private float radius = 0.1f;

    private Welder.WelderModifiers welderModifiers;

    private void Start()
    {
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnBeforeWorkPhaseStart;

        OnBeforeWorkPhaseStart();

        welderParticles.SetActive(true);
    }

    private void OnBeforeWorkPhaseStart()
    {
        welderModifiers = PlayerUpgrades.Instance.GetModifier<Welder.WelderModifiers>();
    }

    private void Update()
    {
        WeldOnPoint((Vector2)transform.position);
    }

    private void WeldOnPoint(Vector2 point)
    {
        float welderRadius = radius * welderModifiers.GetRadiusMultiplier();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, welderRadius);
        HashSet<WeldingPart> weldedParts = colliders.Select(c => c.GetComponentInParent<WeldingPart>()).Where(wp => wp != null).ToHashSet();

        for (int i = 0; i < weldedParts.Count - 1; i++)
        {
            for (int j = i + 1; j < weldedParts.Count; j++)
            {
                WeldingPart weldingPart = weldedParts.ElementAt(i);
                WeldingPart otherWeldingPart = weldedParts.ElementAt(j);
                if (weldingPart != null && otherWeldingPart != null && otherWeldingPart != weldingPart)
                {
                    if (weldingPart.IsCollidingWith(otherWeldingPart))
                    {
                        weldingPart.WeldWith(otherWeldingPart);
                    }
                }
            }
        }
    }
}
