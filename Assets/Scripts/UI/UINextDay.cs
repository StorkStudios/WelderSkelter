using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINextDay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI money;
    [SerializeField]
    private TextMeshProUGUI time;
    [SerializeField]
    private Button button;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        ShopPhaseManager.Instance.EndShopPhase();
    }

    private void Update()
    {
        DayshiftData data = ShopPhaseManager.Instance.NextDayData;
        money.text = $"{data.moneyToCollect}zł";
        if (data.dayLength % 60 < 10)
        {
            time.text = $"{(int)(data.dayLength / 60)}:0{(int)(data.dayLength % 60)}";
        }
        else
        {
            time.text = $"{(int)(data.dayLength / 60)}:{(int)(data.dayLength % 60)}";
        }
    }
}
