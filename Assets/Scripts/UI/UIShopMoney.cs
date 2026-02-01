using TMPro;
using UnityEngine;

public class UIShopMoney : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI money;
    [SerializeField]
    private UIMoneyChangeEffect effectPrefab;

    private void Start()
    {
        money.text = $"{MoneyManager.Instance.Money}zł";
        MoneyManager.Instance.MoneyChanged += OnMoneyChanged;
    }

    private void OnMoneyChanged(int oldValue, int newValue)
    {
        money.text = $"{newValue}zł";
        Instantiate(effectPrefab.gameObject, transform)
            .GetComponent<UIMoneyChangeEffect>().StartAnimation(newValue - oldValue);
    }
}
