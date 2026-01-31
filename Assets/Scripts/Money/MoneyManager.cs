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
    private int money = 0;
    public int Money => money;

    [SerializeField]
    [ReadOnly]
    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;

    [SerializeField]
    private List<int> moneyRequiredPerLevel;

    public Action<int> levelFinishedEvent;
    public Action<int> moneyUpdatedEvent;

    private void Start()
    {
        ItemSeller.Instance.ItemSoldEvent += OnItemSold;
    }

    private void OnItemSold(Dictionary<WeldingPartData, int> dictionary)
    {
        int componentsSum = dictionary.Aggregate(0, (current, key) => current + key.Value);
        AddMoney(componentsSum * (componentsSum - 1) * 10);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        moneyUpdatedEvent?.Invoke(money);
        //if (money >= moneyRequiredPerLevel[currentLevel])
        {
            //next level
        }
    }
}
