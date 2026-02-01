using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShop : Singleton<ItemShop>
{
    [SerializeField]
    private int maxRandomItems;
    [SerializeField]
    private UIItem itemPrefab;
    [SerializeField]
    private RectTransform randomParent;
    [SerializeField]
    private RectTransform taskParent;

    private List<Item> taskItems = new List<Item>();
    private List<Item> purchaseHistory = new List<Item>();

    public void ShowShop()
    {
        while (randomParent.childCount < maxRandomItems)
        {
            Instantiate(itemPrefab.gameObject, randomParent).GetComponent<UIItem>().BuyClicked += OnBuyClicked;
        }

        while (randomParent.childCount > maxRandomItems)
        {
            Destroy(randomParent.GetChild(randomParent.childCount - 1).gameObject);
        }

        while (taskParent.childCount < taskItems.Count)
        {
            Instantiate(itemPrefab.gameObject, taskParent).GetComponent<UIItem>().BuyClicked += OnBuyClicked;
        }

        while (taskParent.childCount > taskItems.Count)
        {
            Destroy(taskParent.GetChild(taskParent.childCount - 1).gameObject);
        }

        int i = 0;
        foreach (Item item in ItemDatabase.Instance.GenerateRandomItems(maxRandomItems))
        {
            randomParent.GetChild(i).GetComponent<UIItem>().SetItem(item);
            i++;
        }

        i = 0;
        foreach (Item item in taskItems)
        {
            taskParent.GetChild(i).GetComponent<UIItem>().SetItem(item);
            i++;
        }
    }

    private void OnBuyClicked(UIItem uiItem, Item item)
    {
        if (MoneyManager.Instance.Money < item.Cost)
        {
            return;
        }

        uiItem.isBought = true;
        MoneyManager.Instance.DeductMoney(item.Cost);
        PlayerUpgrades.Instance.AddUpgrade(item.Upgrade);
        purchaseHistory.Add(item);
    }

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
}
