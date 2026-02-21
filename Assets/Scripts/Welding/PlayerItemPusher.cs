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
    private float maxPushForce = 10;

    [SerializeField]
    private float pushForceMultipler = 10;

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
                //stoppedPushingLastFrame = true;
                pushForce = pushForce.normalized * maxPushForce;
            }
            currentlyPushedItem.Rb.AddForce(pushForce, ForceMode2D.Impulse);

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
        }
    }
}
