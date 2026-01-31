using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : Singleton<PlayerInputManager>
{
    [SerializeField]
    [NotNull]
    private RectTransform weldingCanvasRectTransform;

    private PlayerInput playerInput;

    public event System.Action WeldStartEvent;
    public event System.Action WeldStopEvent;
    public event System.Action<Vector2> MouseMoveOnWeldCanvasEvent;


    protected override void Awake()
    {
        base.Awake();

        playerInput = GetComponent<PlayerInput>();
    }

    public void OnWeld(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            WeldStartEvent?.Invoke();
        }
        else if (context.canceled)
        {
            WeldStopEvent?.Invoke();
        }
    }

    public void OnWelderMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mousePosition = MousePositionToPositionOnWeldViewport(context.ReadValue<Vector2>());
            MouseMoveOnWeldCanvasEvent?.Invoke(mousePosition);
        }
    }

    public Vector2 MousePositionToPositionOnWeldViewport(Vector2 position)
    {
        Vector2 localPoint = weldingCanvasRectTransform.InverseTransformPoint(position);
        localPoint.x /= weldingCanvasRectTransform.rect.size.x;
        localPoint.y /= weldingCanvasRectTransform.rect.size.y;
        localPoint.x += 1; //Anchor
        return localPoint;
    }
}