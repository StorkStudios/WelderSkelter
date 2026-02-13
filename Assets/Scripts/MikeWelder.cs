using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MikeWelder : MonoBehaviour
{
    [SerializeField]
    private GameObject welderParticlesPrefab;
    [SerializeField]
    private float radius = 0.1f;

    private Welder.WelderModifiers modifiers;

    private void Start()
    {
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnBeforeWorkPhaseStart;

        OnBeforeWorkPhaseStart();
    }

    private void OnBeforeWorkPhaseStart()
    {
        modifiers = PlayerUpgrades.Instance.GetModifier<Welder.WelderModifiers>();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float radius = modifiers.welderPositionRadius / transform.lossyScale.x;
        float angleSetp = 2 * Mathf.PI / modifiers.welderCount;
        for (int i = 0; i < modifiers.welderCount; i++)
        {
            GameObject particles = Instantiate(welderParticlesPrefab, transform);
            particles.transform.localPosition = new Vector3(Mathf.Cos(i * angleSetp), Mathf.Sin(i * angleSetp)) * radius + Vector3.back;
        }
    }

    private void Update()
    {
        float radius = modifiers.welderPositionRadius;
        float angleSetp = 2 * Mathf.PI / modifiers.welderCount;
        for (int i = 0; i < modifiers.welderCount; i++)
        {
            Vector2 circlePoint = new Vector2(Mathf.Cos(i * angleSetp), Mathf.Sin(i * angleSetp)) * radius;
            Vector2 currentPosition = (Vector2)transform.position + circlePoint;
            WeldOnPoint(currentPosition);
        }
    }

    private void WeldOnPoint(Vector2 point)
    {
        float welderRadius = radius * modifiers.GetCurrentRadiusMultiplier();
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
