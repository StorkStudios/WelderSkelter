using System;
using StorkStudios.CoreNest;
using UnityEngine;

public class Welder : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private Camera weldingCamera;

    [SerializeField]
    [NotNull]
    private SpriteRenderer welderSpriteRenderer;

    private void Start()
    {
        PlayerInputManager.Instance.MouseMoveOnWeldCanvasEvent += OnMouseMoveOnWeldCanvas;
        PlayerInputManager.Instance.WeldStartEvent += OnWeldStart;
        PlayerInputManager.Instance.WeldStopEvent += OnWeldStop;
    }

    private void OnMouseMoveOnWeldCanvas(Vector2 vector)
    {
        transform.position = GetWorldPositionOnWeldCanvas(vector);
    }

    private void OnWeldStart()
    {
        welderSpriteRenderer.color = Color.blue;
    }

    private void OnWeldStop()
    {
        welderSpriteRenderer.color = Color.yellow;
    }

    private Vector3 GetWorldPositionOnWeldCanvas(Vector2 canvasPosition)
    {
        Vector3 worldPosition = weldingCamera.ViewportToWorldPoint(new Vector3(canvasPosition.x, canvasPosition.y, weldingCamera.nearClipPlane));
        return worldPosition;
    }
}
