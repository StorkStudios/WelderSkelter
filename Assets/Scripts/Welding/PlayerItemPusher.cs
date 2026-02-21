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

    private List<WeldingPart> currentlyPushedItems;

    private Vector2 lastMousePosition;
    private bool stoppedPushingLastFrame;

    public bool IsPushing => currentlyPushedItems != null && currentlyPushedItems.Count > 0;

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
                currentlyPushedItems.ForEach(e => e.transform.position = worldPosition);
            }

            if (stoppedPushingLastFrame)
            {
                stoppedPushingLastFrame = false;
                currentlyPushedItems.ForEach(e =>
                {
                    e.Rb.bodyType = RigidbodyType2D.Dynamic;
                    e.Rb.linearVelocity = pushVelocity;
                });
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
        currentlyPushedItems.ForEach(e =>
        {
            if (e != null)
            {
                e.Rb.bodyType = RigidbodyType2D.Dynamic;
                e.ComponentsChangedEvent -= ClearPushing;
            }
        });
        currentlyPushedItems.Clear();
    }

    private void StartPushing()
    {
        Vector2 mousePosition = PlayerInputManager.Instance.MousePositionToPositionOnWeldViewport(Mouse.current.position.value);
        Vector2 worldPosition = weldingCanvasUtils.GetWorldPositionOnWeldCanvas(mousePosition);
        currentlyPushedItems = Physics2D.OverlapCircleAll(worldPosition, pushRadius)
            .Where(c => !c.CompareTag(Tag.Mike.GetStringValue()))
            .Select(c => c.GetComponentInParent<WeldingPart>())
            .Where(wp => wp != null)
            .ToList();
        currentlyPushedItems.ForEach(e =>
        {
            e.ComponentsChangedEvent += ClearPushing;
            e.Rb.bodyType = RigidbodyType2D.Kinematic;
        });
        lastMousePosition = worldPosition;
    }
}
