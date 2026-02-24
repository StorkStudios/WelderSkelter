using TMPro;
using UnityEngine;

public class UIMoney : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI money;
    [SerializeField]
    private TextMeshProUGUI target;
    [SerializeField]
    private Color underColor;
    [SerializeField]
    private Color overColor;
    [SerializeField]
    private UIMoneyChangeEffect effectPrefab;

    private bool IsOverTarget => MoneyManager.Instance.Money >= WorkPhaseManager.Instance.CurrentData.moneyToCollect;
    private int moneyThisFrame = 0;

    private void Start()
    {
        money.text = $"{MoneyManager.Instance.Money}zł";
        money.color = IsOverTarget ? underColor : overColor;
        MoneyManager.Instance.MoneyChanged += OnMoneyChanged;
    }

    private void OnMoneyChanged(int oldValue, int newValue)
    {
        money.text = $"{newValue}zł";
        moneyThisFrame += newValue - oldValue;
    }

    private void Update()
    {
        target.text = $"{WorkPhaseManager.Instance.CurrentData.moneyToCollect}zł";
        money.color = IsOverTarget ? overColor : underColor;

        if (moneyThisFrame != 0)
        {
            Instantiate(effectPrefab.gameObject, transform).GetComponent<UIMoneyChangeEffect>().StartAnimation(moneyThisFrame);
            moneyThisFrame = 0;
        }
    }
}
