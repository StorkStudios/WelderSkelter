using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item Database")]
public class ItemDatabase : ScriptableObjectSingleton<ItemDatabase>
{
    [SerializeField]
    private List<Item> repeatableItems;
    [SerializeField]
    private List<Item> uniqueItems;

    public List<Item> GenerateRandomItems(int count)
    {
        List<Item> randomItems = new List<Item>(count);
        IEnumerable<Item> allItems = uniqueItems.Where(e => !ItemShop.Instance.IsInPurchaseHistory(e));
        allItems = allItems.Concat(repeatableItems);
        for (int i = 0; i < count; i++)
        {
            Item item = allItems.GetRandomElementWeighted(e => e.Rarity.RarityWeight);
            randomItems.Add(item);
            allItems = allItems.Where(e => e != item);
        }
        return randomItems;
    }
}
