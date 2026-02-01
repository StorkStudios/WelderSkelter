using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using StorkStudios.CoreNest;

public class Pusher : MonoBehaviour
{
    [SerializeField]
    private Transform SpawnLocationOnPusher;
    [SerializeField]
    private Transform SpawnLocationInLevel;
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

    public class PusherModifier
    {
        public float DelayBetweenItemGroups = 3f;
        public int PushersCount = 1;
    }

    private PusherModifier modifier;
    private Coroutine spawnCoroutine;
    private int selectedSlot = 1;
    private GameObject[] itemsOnSlots = new GameObject[3];

    private void Start()
    {
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += OnBeforeWorkPhaseStart;
        WorkPhaseManager.Instance.WorkPhaseEnded += OnWorkPhaseEnded;

        PlayerInputManager.Instance.PusherMoveEvent += OnPusherMove;

        UpdatePusherPosition();
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
            Debug.Log("Spawn items");
            yield return SpawnItems();
            Debug.Log("Wait for selection");
            yield return new WaitForSeconds(modifier.DelayBetweenItemGroups);
            Debug.Log("PushItem");
            PushItem();
            Debug.Log("Remove items");
            RemoveItems();
        }
    }

    private void PushItem()
    {
        Vector3 spawnPositionForSlot = SpawnLocationInLevel.position;
        spawnPositionForSlot.x = slots[selectedSlot].position.x;
        itemsOnSlots[selectedSlot].transform.position = spawnPositionForSlot;
        itemsOnSlots[selectedSlot].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private void RemoveItems()
    {
        RemoveItem(0);
        RemoveItem(1);
        RemoveItem(2);
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

    private void RemoveItem(int slot)
    {
        //float distance = (slots[slot].position - WasteLocation.position).magnitude;
        //itemsOnSlots[slot].transform.DOMove(WasteLocation.position, modifier.DelayBetweenItems * (distance / baseItemSpeed));
        Destroy(itemsOnSlots[slot]);
        itemsOnSlots[slot] = null;
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
