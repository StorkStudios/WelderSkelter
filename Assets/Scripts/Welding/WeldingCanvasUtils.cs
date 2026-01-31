using System;
using StorkStudios.CoreNest;
using UnityEngine;

public class WeldingCanvasUtils : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private Camera weldingCamera;

    private void Start()
    {
        PlayerInputManager.Instance.MouseMoveOnWeldCanvasEvent += OnMouseMoveOnWeldCanvas;
    }

    private void OnMouseMoveOnWeldCanvas(Vector2 vector)
    {
        Vector3 worldPosition = GetWorldPositionOnWeldCanvas(vector);
    }

    public Vector3 GetWorldPositionOnWeldCanvas(Vector2 canvasPosition)
    {
        Vector3 worldPosition = weldingCamera.ViewportToWorldPoint(new Vector3(canvasPosition.x, canvasPosition.y, weldingCamera.nearClipPlane));
        return worldPosition;
    }
}
