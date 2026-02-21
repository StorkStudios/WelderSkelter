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
    public event System.Action ItemSellEvent;
    public event System.Action ToggleMaskEvent;
    public event System.Action<Vector2> MouseMoveOnWeldCanvasEvent;
    public event System.Action<float> PusherMoveEvent;
    public event System.Action<bool> PushItemEvent;

    public bool IsWelding { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        playerInput = GetComponent<PlayerInput>();
    }

    public void OnWeld(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsWelding = true;
            WeldStartEvent?.Invoke();
        }
        else if (context.canceled)
        {
            IsWelding = false;
            WeldStopEvent?.Invoke();
        }
    }

    public void OnSellItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ItemSellEvent?.Invoke();
        }
    }

    public void OnPushItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PushItemEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            PushItemEvent?.Invoke(false);
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

    public void OnMaskToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleMaskEvent?.Invoke();
        }
    }

    public void OnPusherMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PusherMoveEvent?.Invoke(context.ReadValue<float>());
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