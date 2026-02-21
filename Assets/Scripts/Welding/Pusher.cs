using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StorkStudios.CoreNest;
using UnityEngine;

public class Pusher : Singleton<Pusher>
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
    private float delayBetweenItemGroups = 1.25f;
    [SerializeField]
    private int maxItems = 20;

    [SerializeField]
    private Transform[] slots;

    [SerializeField]
    private PusherManager pusher;

    [SerializeField]
    private Vector2 basePushForce;
    [SerializeField]
    private Vector2 randomPushForceRange;

    [SerializeField]
    private GameObject itemsLimitErrorMessage;

    private readonly HashSet<int> pushedItems = new HashSet<int>();

    private enum PusherSound
    {
        Move,
        Push,
        SpawnStart,
        Spawn1,
        Spawn2,
        Spawn3,
        RemoveStart,
        Remove1,
        Remove2,
    }
    [SerializeField]
    private SoundModule<PusherSound> soundModule;

    public class PusherModifier
    {
        public float maskOnItemDelayMultiplier = 1;
        public float itemDelayMultiplier = 1;
        public int pushersCount = 1;
        public float itemCapacityMultiplier = 1;
        public float initialSpeedMultiplier = 1;
        public int mikesCount = 0;
        public float pushYForceMultiplier = 1;
        public bool disableMovement = false;

        public float GetItemDelayMultiplier()
        {
            return itemDelayMultiplier * (WeldingMask.Instance.MaskOn.Value ? maskOnItemDelayMultiplier : 1);
        }
    }

    public int ItemsCount => itemsCount;

    private PusherModifier modifier;
    private Coroutine spawnCoroutine;
    private int selectedSlot = 1;
    private GameObject[] itemsOnSlots = new GameObject[3];

    private int itemsCount = 0;

    private bool lockMovement = false;

    private Tween movementTween = null;

    private void Start()
    {
        soundModule.Initialize();

        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnBeforeWorkPhaseStart;
        WorkPhaseManager.Instance.WorkPhaseEnded += OnWorkPhaseEnded;

        PlayerInputManager.Instance.PusherMoveEvent += OnPusherMove;

        UpdatePusherPosition(true);

        ItemSeller.Instance.ItemSold += OnItemSold;
        Furnace.Instance.ItemDestroyed += OnItemDestroyed;
    }

    private void OnItemDestroyed(Dictionary<WeldingPartData, int> dictionary)
    {
        itemsCount -= dictionary.Aggregate(0, (current, key) => current + key.Value);
    }

    private void OnItemSold(Dictionary<WeldingPartData, int> dictionary)
    {
        itemsCount -= dictionary.Aggregate(0, (current, key) => current + key.Value);
    }

    private void OnPusherMove(float value)
    {
        if (modifier.disableMovement || lockMovement)
        {
            return;
        }

        //Additional pusher is on the left
        if (value > 0)
        {
            if (selectedSlot < slots.Length - 1)
            {
                selectedSlot++;
                UpdatePusherPosition(false);
                soundModule.PlaySound(PusherSound.Move);
            }
        }
        else
        {
            if (selectedSlot > modifier.pushersCount - 1)
            {
                selectedSlot--;
                UpdatePusherPosition(false);
                soundModule.PlaySound(PusherSound.Move);
            }
        }
    }

    private void UpdatePusherPosition(bool instant)
    {
        if (instant)
        {
            Vector3 newPosition = pusher.transform.position;
            newPosition.x = slots[selectedSlot].position.x;
            pusher.transform.position = newPosition;
            return;
        }

        if (movementTween != null)
        {
            movementTween.Kill();
        }
        movementTween = pusher.transform.DOMoveX(slots[selectedSlot].position.x, 0.15f).OnComplete(() => movementTween = null);
    }

    private void OnWorkPhaseEnded(bool _)
    {
        if (this != null && spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private void SpawnMikes()
    {
        Vector3 spawnPositionOn2DScene = SpawnLocationOn2DScene.position;

        for (int i = 0; i < modifier.mikesCount; i++)
        {
            GameObject mike = WeldingPartsSpawner.Instance.SpawnMike();
            Rigidbody2D rb = mike.GetComponent<Rigidbody2D>();
            rb.position = spawnPositionOn2DScene;
            rb.bodyType = RigidbodyType2D.Dynamic;
            float randX = UnityEngine.Random.Range(-randomPushForceRange.x, randomPushForceRange.x);
            float randY = UnityEngine.Random.Range(-randomPushForceRange.y, randomPushForceRange.y);
            Vector2 pushForce = (basePushForce + new Vector2(randX, randY)) * modifier.initialSpeedMultiplier;
            pushForce.y *= modifier.pushYForceMultiplier;
            rb.AddForce(pushForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator SpawnItemsCoroutine()
    {
        //wait for everything to initialize
        yield return null;

        while(true)
        {
            if (itemsCount < maxItems * modifier.itemCapacityMultiplier)
            {
                if (itemsLimitErrorMessage != null && itemsLimitErrorMessage.activeSelf)
                {
                    itemsLimitErrorMessage.SetActive(false);
                }
                yield return SpawnItems();
                yield return new WaitForSeconds(delayBetweenItemGroups * modifier.GetItemDelayMultiplier());
                yield return PushItem();
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
        pusher.StartPushAnimation();
        soundModule.PlaySound(PusherSound.Push);
        yield return new WaitForSeconds(0.25f);

        lockMovement = true;
        Sequence sequence = DOTween.Sequence();
        for (int i = selectedSlot; i > selectedSlot - modifier.pushersCount; i--)
        {
            sequence.Join(itemsOnSlots[i].transform.DOMoveY(SpawnLocationAfterBelt.position.y, 0.25f));
        }
        yield return sequence.WaitForCompletion();

        for (int i = selectedSlot; i > selectedSlot - modifier.pushersCount; i--)
        {
            Vector3 spawnPositionOn2DScene = SpawnLocationOn2DScene.position;
            spawnPositionOn2DScene.x -= slots[1].position.x - slots[i].position.x;
            itemsOnSlots[i].transform.position = spawnPositionOn2DScene;
            Rigidbody2D rb = itemsOnSlots[i].GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            float randX = UnityEngine.Random.Range(-randomPushForceRange.x, randomPushForceRange.x);
            float randY = UnityEngine.Random.Range(-randomPushForceRange.y, randomPushForceRange.y);
            rb.AddForce((basePushForce + new Vector2(randX, randY)) * modifier.initialSpeedMultiplier, ForceMode2D.Impulse);
            itemsCount++;
            itemsOnSlots[selectedSlot].GetComponent<WeldingPart>().OnPush(modifier.initialSpeedMultiplier);
            pushedItems.Add(i);
        }
        
        lockMovement = false;
    }

    private IEnumerator RemoveItems()
    {
        soundModule.PlaySound(PusherSound.RemoveStart);

        int k = 0;
        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (!pushedItems.Contains(i))
            {
                soundModule.PlaySound(PusherSound.Remove1 + (k++));
                yield return RemoveItem(i);
            }
        }
        pushedItems.Clear();
    }

    private IEnumerator SpawnItems()
    {
        soundModule.PlaySound(PusherSound.SpawnStart);

        int k = 0;
        int taskPartIndex = UnityEngine.Random.Range(0, slots.Length);
        for (int i = slots.Length - 1; i >= 0; i--)
        {
            soundModule.PlaySound(PusherSound.Spawn1 + (k++));
            yield return SpawnItem(i, i == taskPartIndex);
        }
    }

    private IEnumerator RemoveItem(int slot)
    {
        GameObject item = itemsOnSlots[slot];
        float distance = (item.transform.position - WasteLocation.position).magnitude;
        yield return item.transform.DOMove(WasteLocation.position, delayBetweenItemGroups * modifier.GetItemDelayMultiplier() * (distance / baseItemSpeed)).WaitForCompletion();
        item.transform.DOScale(0, 0.25f).OnComplete(() => Destroy(item));
    }

    private IEnumerator SpawnItem(int slot, bool spawnTaskItem)
    {
        GameObject part = spawnTaskItem ? WeldingPartsSpawner.Instance.SpawnRandomTaskPart() : WeldingPartsSpawner.Instance.SpawnRandomPart();
        part.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        part.transform.position = SpawnLocationOnPusher.position;
        float distance = (slots[slot].position - transform.position).magnitude;
        itemsOnSlots[slot] = part;
        yield return part.transform.DOMove(slots[slot].position, delayBetweenItemGroups * modifier.GetItemDelayMultiplier() * (distance / baseItemSpeed)).WaitForCompletion();
    }

    private void OnBeforeWorkPhaseStart()
    {
        modifier = PlayerUpgrades.Instance.GetModifier<PusherModifier>();
        itemsCount = 0;
        lockMovement = false;
        pusher.Restart(modifier.pushersCount, Mathf.Abs(slots[0].position.x - slots[1].position.x));
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnItemsCoroutine());
        }

        SpawnMikes();
    }
}
