using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField]
    private Transform SpawnLocationOnPusher;
    [SerializeField]
    private Transform SpawnLocationAfterBelt;
    [SerializeField]
    private Transform SpawnLocationOn2DScene;
    [SerializeField]
    private Transform WasteLocation;

    [SerializeField]
    private float baseItemSpeed = 15f;
    [SerializeField]
    private float delayBetweenItems = 0.5f;

    [SerializeField]
    private Transform[] slots;

    [SerializeField]
    private Transform pusher;

    [SerializeField]
    private Animator pusherAnimator;

    [SerializeField]
    private Vector2 basePushForce;
    [SerializeField]
    private Vector2 randomPushForceRange;

    [SerializeField]
    private GameObject itemsLimitErrorMessage;

    public class PusherModifier
    {
        public float DelayBetweenItemGroups = 3f;
        public int PushersCount = 1;
        public int MaxItems = 2;
    }

    private PusherModifier modifier;
    private Coroutine spawnCoroutine;
    private int selectedSlot = 1;
    private GameObject[] itemsOnSlots = new GameObject[3];

    private int itemsCount = 0;

    private void Start()
    {
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnBeforeWorkPhaseStart;
        WorkPhaseManager.Instance.WorkPhaseEnded += OnWorkPhaseEnded;

        PlayerInputManager.Instance.PusherMoveEvent += OnPusherMove;

        UpdatePusherPosition();

        ItemSeller.Instance.ItemSold += OnItemSold;
    }

    private void OnItemSold(Dictionary<WeldingPartData, int> dictionary)
    {
        itemsCount -= dictionary.Aggregate(0, (current, key) => current + key.Value);
    }

    private void OnPusherMove(float value)
    {
        //Additional pusher is on the left
        if (value > 0)
        {
            if (selectedSlot < slots.Length - 1)
            {
                selectedSlot++;
                UpdatePusherPosition();
            }
        }
        else
        {
            if (selectedSlot > modifier.PushersCount - 1)
            {
                selectedSlot--;
                UpdatePusherPosition();
            }
        }
    }

    private void UpdatePusherPosition()
    {
        Vector3 newPosition = pusher.position;
        newPosition.x = slots[selectedSlot].position.x;
        pusher.position = newPosition;
    }

    private void OnWorkPhaseEnded(bool _)
    {
        StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
    }

    private IEnumerator SpawnItemsCoroutine()
    {
        while(true)
        {
            if (itemsCount < modifier.MaxItems)
            {
                if (itemsLimitErrorMessage != null && itemsLimitErrorMessage.activeSelf)
                {
                    itemsLimitErrorMessage.SetActive(false);
                }
                Debug.Log("Spawn items");
                yield return SpawnItems();
                Debug.Log("Wait for selection");
                yield return new WaitForSeconds(modifier.DelayBetweenItemGroups);
                Debug.Log("PushItem");
                yield return PushItem();
                Debug.Log("Remove items");
                yield return RemoveItems();
            }
            else
            {
                if (itemsLimitErrorMessage != null && !itemsLimitErrorMessage.activeSelf)
                {
                    itemsLimitErrorMessage.SetActive(true);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private IEnumerator PushItem()
    {
        pusherAnimator.SetTrigger("PushTrigger");
        Vector3 spawnPositionForSlot = SpawnLocationAfterBelt.position;
        spawnPositionForSlot.x = slots[selectedSlot].position.x;
        yield return new WaitForSeconds(0.25f);
        itemsOnSlots[selectedSlot].transform.DOMove(spawnPositionForSlot, 0.25f);
        yield return new WaitForSeconds(0.25f);
        Vector3 spawnPositionOn2DScene = SpawnLocationOn2DScene.position;
        spawnPositionOn2DScene.x -= slots[1].position.x - slots[selectedSlot].position.x;
        itemsOnSlots[selectedSlot].transform.position = spawnPositionOn2DScene;
        Rigidbody2D rb = itemsOnSlots[selectedSlot].GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        float randX = UnityEngine.Random.Range(-randomPushForceRange.x, randomPushForceRange.x);
        float randY = UnityEngine.Random.Range(-randomPushForceRange.y, randomPushForceRange.y);
        rb.AddForce(basePushForce + new Vector2(randX, randY), ForceMode2D.Impulse);
        itemsCount++;
    }

    private IEnumerator RemoveItems()
    {
        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (i != selectedSlot)
            {
                yield return RemoveItem(i);
            }
        }
    }

    private IEnumerator SpawnItems()
    {
        SpawnItem(2);
        yield return new WaitForSeconds(delayBetweenItems * modifier.DelayBetweenItemGroups);
        SpawnItem(1);
        yield return new WaitForSeconds(delayBetweenItems * modifier.DelayBetweenItemGroups);
        SpawnItem(0);
        yield return new WaitForSeconds(delayBetweenItems * modifier.DelayBetweenItemGroups);
    }

    private IEnumerator RemoveItem(int slot)
    {
        float distance = (slots[slot].position - WasteLocation.position).magnitude;
        yield return itemsOnSlots[slot].transform.DOMove(WasteLocation.position, modifier.DelayBetweenItemGroups * (distance / baseItemSpeed)).WaitForCompletion();
        itemsOnSlots[slot].transform.DOScale(0, 0.25f).OnComplete(() => Destroy(itemsOnSlots[slot]));
    }

    private void SpawnItem(int slot)
    {
        GameObject part = WeldingPartsSpawner.Instance.SpawnRandomPart();
        part.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        part.transform.position = SpawnLocationOnPusher.position;
        float distance = (slots[slot].position - transform.position).magnitude;
        part.transform.DOMove(slots[slot].position, modifier.DelayBetweenItemGroups * (distance / baseItemSpeed));
        itemsOnSlots[slot] = part;
    }

    private void OnBeforeWorkPhaseStart()
    {
        modifier = PlayerUpgrades.Instance.GetModifier<PusherModifier>();
        spawnCoroutine = StartCoroutine(SpawnItemsCoroutine());
    }
}
