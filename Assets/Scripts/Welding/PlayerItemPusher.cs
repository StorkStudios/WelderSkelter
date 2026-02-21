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
    private float maxPushForce;

    [SerializeField]
    private float pushForceMultipler;

    [SerializeField]
    private float maxItemVelocity = 10;
    [SerializeField]
    private float maxPushDistance;

    [Header("Push drag")]
    [SerializeField]
    private float pushDragDistance;
    [SerializeField]
    private float pushDragLevel;


    private WeldingCanvasUtils weldingCanvasUtils;

    private WeldingPart currentlyPushedItem;
    private bool stoppedPushingLastFrame;

    public bool IsPushing => currentlyPushedItem != null;

    private void Start()
    {
        weldingCanvasUtils = GetComponent<WeldingCanvasUtils>();

        PlayerInputManager.Instance.PushItemEvent += OnPushItem;
    }

    private void Update()
    {
        if (IsPushing)
        {
            Vector2 mousePosition = PlayerInputManager.Instance.MousePositionToPositionOnWeldViewport(Mouse.current.position.value);
            Vector2 mouseWorldPosition = weldingCanvasUtils.GetWorldPositionOnWeldCanvas(mousePosition);
            Vector2 itemPosition = new(currentlyPushedItem.transform.position.x, currentlyPushedItem.transform.position.y);
            Vector2 pushForce = (mouseWorldPosition - itemPosition).normalized * (mouseWorldPosition - itemPosition).sqrMagnitude * pushForceMultipler / Time.deltaTime;
            if (pushForce.sqrMagnitude > maxPushForce * maxPushForce)
            {
                pushForce = pushForce.normalized * maxPushForce;
            }
            if ((mouseWorldPosition - itemPosition).sqrMagnitude > maxPushDistance * maxPushDistance)
            {
                stoppedPushingLastFrame = true;
            }
            else
            {
                currentlyPushedItem.Rb.AddForce(pushForce, ForceMode2D.Impulse);
                currentlyPushedItem.Rb.linearVelocity = Vector2.ClampMagnitude(currentlyPushedItem.Rb.linearVelocity, maxItemVelocity);
            }
            if ((mouseWorldPosition - itemPosition).sqrMagnitude < pushDragDistance * pushDragDistance)
            {
                currentlyPushedItem.Rb.linearDamping = pushDragLevel;
            }
            else
            {
                currentlyPushedItem.Rb.linearDamping = 0;
            }

            if (stoppedPushingLastFrame)
            {
                stoppedPushingLastFrame = false;
                ClearPushing();
            }
        }
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
            currentlyPushedItem.Rb.linearDamping = 0;
            currentlyPushedItem.ComponentsChangedEvent -= ClearPushing;
        }
        currentlyPushedItem = null;
    }

    private void StartPushing()
    {
        Vector2 mousePosition = PlayerInputManager.Instance.MousePositionToPositionOnWeldViewport(Mouse.current.position.value);
        Vector2 worldPosition = weldingCanvasUtils.GetWorldPositionOnWeldCanvas(mousePosition);
        currentlyPushedItem = Physics2D.OverlapCircleAll(worldPosition, pushRadius)
            .Select(c => c.GetComponentInParent<WeldingPart>())
            .FirstOrDefault(wp => wp != null);
        if (currentlyPushedItem != null)
        {
            currentlyPushedItem.ComponentsChangedEvent += ClearPushing;
        }
    }
}
