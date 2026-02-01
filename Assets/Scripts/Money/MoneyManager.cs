using UnityEngine;
using StorkStudios.CoreNest;
using System.Collections.Generic;
using System;
using System.Linq;

public class MoneyManager : Singleton<MoneyManager>
{
    public class MoneyManagerModifiers
    {
        public float scrapSellMoneyMultipler = 1;
        public float allIncomeMultipler = 1;
    }

    public event ObservableVariable<int>.ValueChangedDelegate MoneyChanged
    {
        add => money.ValueChanged += value;
        remove => money.ValueChanged -= value;
    }

    [SerializeField]
    [ReadOnly]
    private ObservableVariable<int> money = new ObservableVariable<int>(0);
    public int Money => money.Value;

    private void Start()
    {
        ItemSeller.Instance.ItemSold += OnItemSold;
    }

    private void OnItemSold(Dictionary<WeldingPartData, int> dictionary)
    {
        int componentsSum = dictionary.Aggregate(0, (current, key) => current + key.Value);
        int amount;
        if (dictionary.Values.Count() > 1)
        {
            amount = (componentsSum - 1) * 10;
        }
        else
        {
            amount = componentsSum * (componentsSum - 1) * 10;
        }
        AddMoney((int)(amount * PlayerUpgrades.Instance.GetModifier<MoneyManagerModifiers>().scrapSellMoneyMultipler));
    }

    public void AddMoney(int amount)
    {
        money.Value += (int)(amount * PlayerUpgrades.Instance.GetModifier<MoneyManagerModifiers>().allIncomeMultipler);
    }

    public void DeductMoney(int amount)
    {
        money.Value -= amount;
    }
}
