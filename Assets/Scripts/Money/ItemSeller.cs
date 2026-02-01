using System;
using System.Collections.Generic;
using System.Linq;
using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(WeldingCanvasUtils))]
public class ItemSeller : Singleton<ItemSeller>
{
    public class ItemSellerModifiers
    {
    }

    [SerializeField]
    private float sellRadius = 0.5f;

    private WeldingCanvasUtils weldingCanvasUtils;

    public event Action<Dictionary<WeldingPartData, int>> ItemSold;

    private void Start()
    {
        weldingCanvasUtils = GetComponent<WeldingCanvasUtils>();

        PlayerInputManager.Instance.ItemSellEvent += OnItemSell;
    }

    private void OnItemSell()
    {
        Vector2 mousePosition = PlayerInputManager.Instance.MousePositionToPositionOnWeldViewport(Mouse.current.position.value);
        Vector2 worldPosition = weldingCanvasUtils.GetWorldPositionOnWeldCanvas(mousePosition);
        SellItemAtPosition(worldPosition);
    }

    private void SellItemAtPosition(Vector2 position)
    {
        Collider2D itemToSell = Physics2D.OverlapCircle(position, sellRadius);
        if (itemToSell != null)
        {
            WeldingPart weldingPart = itemToSell.GetComponentInParent<WeldingPart>();
            if (weldingPart != null && (weldingPart.Components.Count > 1 || weldingPart.Components.First().Value > 1))
            {
                ItemSold?.Invoke(weldingPart.Components);
                Destroy(weldingPart.gameObject);
            }
        }
    }
}
