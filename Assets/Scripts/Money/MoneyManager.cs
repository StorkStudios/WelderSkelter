using UnityEngine;
using StorkStudios.CoreNest;
using System.Collections.Generic;
using System;
using System.Linq;

public class MoneyManager : Singleton<MoneyManager>
{
    public class MoneyManagerModifiers
    {
        // Placeholder for future modifiers
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
        AddMoney(componentsSum * (componentsSum - 1) * 10);
    }

    public void AddMoney(int amount)
    {
        money.Value += amount;
    }
}
