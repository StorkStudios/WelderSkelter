using TMPro;
using UnityEngine;

public class UIShopMoney : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI money;

    private void Update()
    {
        money.text = $"{MoneyManager.Instance.Money}zł";
    }
}
