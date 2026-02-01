using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShop : Singleton<ItemShop>
{
    [SerializeField]
    private int maxRandomItems;

    private List<Item> taskItems = new List<Item>();
    private List<Item> purchaseHistory = new List<Item>();

    public void AddTaskItem(Item taskItem)
    {
        taskItems.Add(taskItem);
    }

    public bool HasTaskItem(Item item)
    {
        return taskItems.Contains(item);
    }

    public bool IsInPurchaseHistory(Item item)
    {
        return purchaseHistory.Contains(item);
    }

    private void OnItemBought(Item item)
    {
        purchaseHistory.Add(item);
    }
}
