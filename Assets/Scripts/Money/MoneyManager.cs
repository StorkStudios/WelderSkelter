using UnityEngine;
using StorkStudios.CoreNest;
using System.Collections.Generic;
using System;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class MoneyManager : Singleton<MoneyManager>
{
    public class MoneyManagerModifiers
    {
        public float scrapSellMoneyMultipler = 1;
        public float allIncomeMultipler = 1;
        public float perItemIncomeMultiplier = 1;
    }

    public event ObservableVariable<int>.ValueChangedDelegate MoneyChanged
    {
        add => money.ValueChanged += value;
        remove => money.ValueChanged -= value;
    }

    [SerializeField]
    private AudioClip smallMoneySound;
    [SerializeField]
    private AudioClip bigMoneySound;

    [SerializeField]
    [ReadOnly]
    private ObservableVariable<int> money = new ObservableVariable<int>(0);
    public int Money => money.Value;

    private AudioSource audioSource;

    private void Start()
    {
        ItemSeller.Instance.ItemSold += OnItemSold;

        audioSource = GetComponent<AudioSource>();

        WorkPhaseManager.Instance.WorkPhasePreStartEvent += () => money.Value = 0;
    }

    private void OnItemSold(Dictionary<WeldingPartData, int> dictionary)
    {
        int amount = ItemSeller.Instance.CalculateItemPrice(dictionary);
        AddMoney((int)(amount * PlayerUpgrades.Instance.GetModifier<MoneyManagerModifiers>().scrapSellMoneyMultipler));
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        MoneyManagerModifiers modifier = PlayerUpgrades.Instance.GetModifier<MoneyManagerModifiers>();
        float multiplier = modifier.allIncomeMultipler * Mathf.Pow(modifier.perItemIncomeMultiplier, Pusher.Instance.ItemsCount);
        int modifiedAmount = (int)(amount * multiplier);
        if (modifiedAmount > 200)
        {
            audioSource.PlayOneShot(bigMoneySound);
        }
        else
        {
            audioSource.PlayOneShot(smallMoneySound);
        }
        money.Value += modifiedAmount;
    }

    public void DeductMoney(int amount)
    {
        money.Value -= amount;
    }
}
