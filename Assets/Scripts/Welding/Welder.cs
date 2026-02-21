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
        public bool weldPermanently = false;
        public float lpmWeldingPointRadiusMultiplier = 1;

        public float GetCurrentRadiusMultiplier()
        {
            float maskMultiplier = WeldingMask.Instance.MaskOn.Value ? maskOnRadiusMultiplier : 1;
            float lpmMultiplier = PlayerInputManager.Instance.IsWelding ? lpmWeldingPointRadiusMultiplier : 1;
            return maskMultiplier * lpmMultiplier;
        }
    }

    [SerializeField]
    private GameObject welderParticlesPrefab;
    [SerializeField]
    private float radius = 0.1f;

    private WeldingCanvasUtils weldingCanvasUtils;
    private bool inputWelding;
    public bool IsWelding => inputWelding || (modifiers?.weldPermanently ?? false);

    private WelderModifiers modifiers;
    private List<WeldingParticles> welderParticles = new List<WeldingParticles>();

    private void Start()
    {
        weldingCanvasUtils = GetComponent<WeldingCanvasUtils>();

        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnBeforeWorkPhaseStart;
        WeldingMask.Instance.MaskOn.ValueChanged += OnMaskStateChanged;
    }
    private void OnBeforeWorkPhaseStart()
    {
        modifiers = PlayerUpgrades.Instance.GetModifier<WelderModifiers>();

        welderParticles.Clear();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        inputWelding = false;
        float positionRadius = modifiers.welderPositionRadius;
        float angleStep = 2 * Mathf.PI / modifiers.welderCount;
        for (int i = 0; i < modifiers.welderCount; i++)
        {
            WeldingParticles particles = Instantiate(welderParticlesPrefab, transform).GetComponent<WeldingParticles>();
            particles.transform.localPosition = new Vector3(Mathf.Cos(i * angleStep), Mathf.Sin(i * angleStep)) * positionRadius + Vector3.forward;
            particles.transform.localScale = modifiers.GetCurrentRadiusMultiplier() * radius * Vector3.one;
            particles.SetWelding(IsWelding);
            welderParticles.Add(particles);
        }

        PlayerInputManager.Instance.MouseMoveOnWeldCanvasEvent -= OnMouseMoveOnWeldCanvas;
        PlayerInputManager.Instance.WeldStartEvent -= OnWeldStart;
        PlayerInputManager.Instance.WeldStopEvent -= OnWeldStop;
        PlayerInputManager.Instance.MouseMoveOnWeldCanvasEvent += OnMouseMoveOnWeldCanvas;
        PlayerInputManager.Instance.WeldStartEvent += OnWeldStart;
        PlayerInputManager.Instance.WeldStopEvent += OnWeldStop;
    }

    private void Update()
    {
        if (IsWelding)
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

    private void OnMouseMoveOnWeldCanvas(Vector2 vector)
    {
        transform.position = weldingCanvasUtils.GetWorldPositionOnWeldCanvas(vector);
    }

    private void OnWeldStart()
    {
        if (PlayerItemPusher.Instance.IsPushing)
        {
            return;
        }

        inputWelding = true;
        welderParticles.ForEach(e => e.SetWelding(IsWelding));
    }

    private void OnWeldStop()
    {
        inputWelding = false;
        welderParticles.ForEach(e => e.SetWelding(IsWelding));
    }

    private void OnMaskStateChanged(bool oldValue, bool newValue)
    {
        welderParticles.ForEach(e => e.transform.localScale = modifiers.GetCurrentRadiusMultiplier() * radius * Vector3.one);
    }

}
