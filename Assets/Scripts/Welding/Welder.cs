using System;
using System.Collections.Generic;
using System.Linq;
using StorkStudios.CoreNest;
using UnityEngine;

[RequireComponent(typeof(WeldingCanvasUtils))]
public class Welder : Singleton<Welder>
{
    public class WelderModifiers
    {
        public float maskOnRadiusMultiplier = 1;
        public float welderCount = 1;
        public float welderPositionRadius = 0;

        public float GetRadiusMultiplier()
        {
            return WeldingMask.Instance.MaskOn.Value ? maskOnRadiusMultiplier : 1;
        }
    }

    [SerializeField]
    private GameObject welderParticlesPrefab;
    [SerializeField]
    private float radius = 0.1f;

    private WeldingCanvasUtils weldingCanvasUtils;
    private Vector2 lastWeldPosition;
    private bool isWelding;
    public bool IsWelding => isWelding;

    private WelderModifiers modifiers;
    private float weldingSampleDistance = 0.1f;
    private List<GameObject> welderParticles = new List<GameObject>();

    private void Start()
    {
        weldingCanvasUtils = GetComponent<WeldingCanvasUtils>();

        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnBeforeWorkPhaseStart;
    }

    private void OnBeforeWorkPhaseStart()
    {
        modifiers = PlayerUpgrades.Instance.GetModifier<WelderModifiers>();

        welderParticles.Clear();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float radius = modifiers.welderPositionRadius;
        float angleSetp = 2 * Mathf.PI / modifiers.welderCount;
        for (int i = 0; i < modifiers.welderCount; i++)
        {
            GameObject particles = Instantiate(welderParticlesPrefab, transform);
            particles.transform.localPosition = new Vector3(Mathf.Cos(i * angleSetp), Mathf.Sin(i * angleSetp)) * radius + Vector3.forward;
            welderParticles.Add(particles);
        }

        OnWeldStop();

        PlayerInputManager.Instance.MouseMoveOnWeldCanvasEvent -= OnMouseMoveOnWeldCanvas;
        PlayerInputManager.Instance.WeldStartEvent -= OnWeldStart;
        PlayerInputManager.Instance.WeldStopEvent -= OnWeldStop;
        PlayerInputManager.Instance.MouseMoveOnWeldCanvasEvent += OnMouseMoveOnWeldCanvas;
        PlayerInputManager.Instance.WeldStartEvent += OnWeldStart;
        PlayerInputManager.Instance.WeldStopEvent += OnWeldStop;
    }

    private void Update()
    {
        if (isWelding)
        {
            float radius = modifiers.welderPositionRadius;
            float angleSetp = 2 * Mathf.PI / modifiers.welderCount;
            for (int i = 0; i < modifiers.welderCount; i++)
            {
                Vector2 circlePoint = new Vector2(Mathf.Cos(i * angleSetp), Mathf.Sin(i * angleSetp)) * radius;
                Vector2 lastPosition = lastWeldPosition + circlePoint;
                Vector2 currentPosition = (Vector2)transform.position + circlePoint;
                WeldBetweenPoints(lastPosition, currentPosition);
            }
        }
    }

    private void WeldBetweenPoints(Vector2 startPosition, Vector2 endPosition)
    {
        Vector2 position = endPosition;
        do
        {
            position = Vector2.MoveTowards(position, startPosition, weldingSampleDistance);
            WeldOnPoint(position);
        }
        while (Vector2.Distance(startPosition, position) > weldingSampleDistance);
    }

    private void WeldOnPoint(Vector2 point)
    {
        float welderRadius = radius * modifiers.GetRadiusMultiplier();
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

    private void OnMouseMoveOnWeldCanvas(Vector2 vector)
    {
        transform.position = weldingCanvasUtils.GetWorldPositionOnWeldCanvas(vector);
    }

    private void OnWeldStart()
    {
        isWelding = true;
        lastWeldPosition = transform.position;
        WeldOnPoint(lastWeldPosition);
        welderParticles.ForEach(e => e.SetActive(true));
    }

    private void OnWeldStop()
    {
        isWelding = false;
        welderParticles.ForEach(e => e.SetActive(false));
    }
}
