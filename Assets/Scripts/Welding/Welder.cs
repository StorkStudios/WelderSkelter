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
        public float radius = 0.1f;
    }

    [SerializeField]
    [NotNull]
    private GameObject welderParticles;

    private WeldingCanvasUtils weldingCanvasUtils;
    private Vector2 lastWeldPosition;
    private bool isWelding;
    public bool IsWelding => isWelding;

    private WelderModifiers welderModifiers;
    private float weldingSampleDistance = 0.1f;

    private void Start()
    {
        PlayerInputManager.Instance.MouseMoveOnWeldCanvasEvent += OnMouseMoveOnWeldCanvas;
        PlayerInputManager.Instance.WeldStartEvent += OnWeldStart;
        PlayerInputManager.Instance.WeldStopEvent += OnWeldStop;

        weldingCanvasUtils = GetComponent<WeldingCanvasUtils>();

        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnBeforeWorkPhaseStart;

        welderParticles.SetActive(false);

        OnBeforeWorkPhaseStart();
    }

    private void OnBeforeWorkPhaseStart()
    {
        welderModifiers = PlayerUpgrades.Instance.GetModifier<WelderModifiers>();
    }

    private void Update()
    {
        if (isWelding)
        {
            WeldBetweenPoints(lastWeldPosition, (Vector2)transform.position);
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, welderModifiers.radius);
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
        welderParticles.SetActive(true);
    }

    private void OnWeldStop()
    {
        isWelding = false;
        welderParticles.SetActive(false);
    }
}
