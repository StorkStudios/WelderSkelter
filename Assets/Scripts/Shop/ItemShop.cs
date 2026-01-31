using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShop : Singleton<ItemShop>
{
    [SerializeField]
    private int maxRandomItems;
    [SerializeField]
    private List<Item> repeatableItems;
    [SerializeField]
    private SerializedSet<Item> uniqueItems;

    private List<Item> taskItems = new List<Item>();

    public void AddTaskItem(Item taskItem)
    {
        taskItems.Add(taskItem);
    }

    public bool HasTaskItem(Item item)
    {
        return taskItems.Contains(item);
    }

    public ShopContents GenerateShopContents()
    {
        List<Item> randomItems = new List<Item>();
        IEnumerable<Item> allItems = repeatableItems.Concat(uniqueItems.Select(e => e.Item));
        for (int i = 0; i < maxRandomItems; i++)
        {
            Item item = allItems.GetRandomElementWeighted(e => e.Rarity.RarityWeight);
            randomItems.Add(item);
            allItems = allItems.Where(e => e != item);
        }
        ShopContents contents = new ShopContents()
        {
            randomItems = randomItems,
            taskItems = taskItems,
        };
        taskItems = new List<Item>();
        return contents;
    }

    private void OnItemBought(Item item)
    {
        uniqueItems.Remove(item);
    }
}
