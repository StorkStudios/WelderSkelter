using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private TextMeshProUGUI cost;

    public bool isBought = false;

    private Item item;

    public event System.Action<UIItem, Item> BuyClicked;

    private void Awake()
    {
        buyButton.onClick.AddListener(OnButtonClick);
    }

    private void Update()
    {
        if (item != null)
        {
            buyButton.interactable = !isBought && MoneyManager.Instance.Money >= item.Cost;
        }
    }

    private void OnButtonClick()
    {
        BuyClicked?.Invoke(this, item);
    }

    public void SetItem(Item item)
    {
        this.item = item;
        title.text = item.Title;
        description.text = item.Upgrade.Description;
        cost.text = $"{item.Cost}zł";
    }
}
