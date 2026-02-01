using TMPro;
using UnityEngine;

public class UIMoney : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI money;
    [SerializeField]
    private UIMoneyChangeEffect effectPrefab;

    private void Start()
    {
        money.text = $"{MoneyManager.Instance.Money}zł/{WorkPhaseManager.Instance.CurrentData.moneyToCollect}zł";
        MoneyManager.Instance.MoneyChanged += OnMoneyChanged;
    }

    private void OnMoneyChanged(int oldValue, int newValue)
    {
        money.text = $"{newValue}/{WorkPhaseManager.Instance.CurrentData.moneyToCollect}zł";
        Instantiate(effectPrefab.gameObject, transform.position, Quaternion.identity)
            .GetComponent<UIMoneyChangeEffect>().StartAnimation(newValue - oldValue);
    }
}
