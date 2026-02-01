using TMPro;
using UnityEngine;

public class UIMoney : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI money;

    private void Update()
    {
        money.text = $"{MoneyManager.Instance.Money}zł/{WorkPhaseManager.Instance.CurrentData.moneyToCollect}zł";
    }
}
