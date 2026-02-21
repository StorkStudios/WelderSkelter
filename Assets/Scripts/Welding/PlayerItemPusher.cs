using System.Collections.Generic;
using System.Linq;
using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(WeldingCanvasUtils))]
public class PlayerItemPusher : Singleton<PlayerItemPusher>
{
    [SerializeField]
    private float pushRadius = 0.5f;

    [SerializeField]
    private float maxPushSpeed = 10;

    private WeldingCanvasUtils weldingCanvasUtils;

    private WeldingPart currentlyPushedItem;

    private Vector2 lastMousePosition;
    private bool stoppedPushingLastFrame;

    public bool IsPushing => currentlyPushedItem != null;

    private void Start()
    {
        weldingCanvasUtils = GetComponent<WeldingCanvasUtils>();

        PlayerInputManager.Instance.PushItemEvent += OnPushItem;
    }

    private void Update()
    {
        Vector2 mousePosition = PlayerInputManager.Instance.MousePositionToPositionOnWeldViewport(Mouse.current.position.value);
        Vector2 worldPosition = weldingCanvasUtils.GetWorldPositionOnWeldCanvas(mousePosition);

        if (IsPushing)
        {
            Vector2 pushVelocity = (worldPosition - lastMousePosition) / Time.deltaTime;
            if (pushVelocity.sqrMagnitude > maxPushSpeed * maxPushSpeed)
            {
                stoppedPushingLastFrame = true;
                pushVelocity = pushVelocity.normalized * maxPushSpeed;
            }
            else
            {
                currentlyPushedItem.transform.position = worldPosition;
            }

            if (stoppedPushingLastFrame)
            {
                stoppedPushingLastFrame = false;
                currentlyPushedItem.Rb.bodyType = RigidbodyType2D.Dynamic;
                currentlyPushedItem.Rb.linearVelocity = pushVelocity;
                ClearPushing();
            }
        }

        lastMousePosition = worldPosition;
    }

    private void OnPushItem(bool pushInput)
    {
        if (Welder.Instance.IsWelding)
        {
            return;
        }

        if (pushInput)
        {
            StartPushing();
        }
        else if (IsPushing)
        {
            stoppedPushingLastFrame = true;
        }
    }

    private void ClearPushing()
    {
        if (currentlyPushedItem != null)
        {
            currentlyPushedItem.Rb.bodyType = RigidbodyType2D.Dynamic;
            currentlyPushedItem.ComponentsChangedEvent -= ClearPushing;
        }
        currentlyPushedItem = null;
    }

    private void StartPushing()
    {
        Vector2 mousePosition = PlayerInputManager.Instance.MousePositionToPositionOnWeldViewport(Mouse.current.position.value);
        Vector2 worldPosition = weldingCanvasUtils.GetWorldPositionOnWeldCanvas(mousePosition);
        currentlyPushedItem = Physics2D.OverlapCircleAll(worldPosition, pushRadius)
            .Where(c => !c.CompareTag(Tag.Mike.GetStringValue()))
            .Select(c => c.GetComponentInParent<WeldingPart>())
            .FirstOrDefault(wp => wp != null);
        if (currentlyPushedItem != null)
        {
            currentlyPushedItem.ComponentsChangedEvent += ClearPushing;
            currentlyPushedItem.Rb.bodyType = RigidbodyType2D.Kinematic;
        }
        lastMousePosition = worldPosition;
    }
}
