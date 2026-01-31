using System;
using System.Collections.Generic;
using System.Linq;
using StorkStudios.CoreNest;
using UnityEngine;

public class Welder : MonoBehaviour
{
    public class WelderModifiers
    {
        public float radius = 0.1f;
    }

    [SerializeField]
    [NotNull]
    private Camera weldingCamera;

    [SerializeField]
    [NotNull]
    private SpriteRenderer welderSpriteRenderer;

    private Vector2 lastWeldPosition;
    private bool isWelding;

    private WelderModifiers welderModifiers; //TODO: Handle upgrades
    private float weldingSampleDistance = 0.1f;

    private void Start()
    {
        PlayerInputManager.Instance.MouseMoveOnWeldCanvasEvent += OnMouseMoveOnWeldCanvas;
        PlayerInputManager.Instance.WeldStartEvent += OnWeldStart;
        PlayerInputManager.Instance.WeldStopEvent += OnWeldStop;

        welderModifiers = new WelderModifiers();
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

        for (int i = 0; i < weldedParts.Count; i++)
        {
            for (int j = 0; j < weldedParts.Count; j++)
            {
                WeldingPart weldingPart = weldedParts.ElementAt(i);
                if (weldingPart != null)
                {
                    WeldingPart otherWeldingPart = weldedParts.ElementAt(j);
                    if (otherWeldingPart != null && otherWeldingPart != weldingPart)
                    {
                        Debug.Log($"Checking welding between parts: {weldingPart.name} and {otherWeldingPart.name}");
                        if (weldingPart.CollidingParts.Contains(otherWeldingPart))
                        {
                            weldingPart.WeldWith(otherWeldingPart);
                            Debug.Log($"Welded parts: {weldingPart.name} with {otherWeldingPart.name}");
                        }
                    }
                }
            }
        }
    }

    private void OnMouseMoveOnWeldCanvas(Vector2 vector)
    {
        transform.position = GetWorldPositionOnWeldCanvas(vector);
    }

    private void OnWeldStart()
    {
        welderSpriteRenderer.color = Color.blue;
        isWelding = true;
        lastWeldPosition = transform.position;
        WeldOnPoint(lastWeldPosition);
    }

    private void OnWeldStop()
    {
        welderSpriteRenderer.color = Color.yellow;
        isWelding = false;
    }

    private Vector3 GetWorldPositionOnWeldCanvas(Vector2 canvasPosition)
    {
        Vector3 worldPosition = weldingCamera.ViewportToWorldPoint(new Vector3(canvasPosition.x, canvasPosition.y, weldingCamera.nearClipPlane));
        return worldPosition;
    }
}
