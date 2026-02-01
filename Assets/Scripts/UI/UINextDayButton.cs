using UnityEngine;
using UnityEngine.UI;

public class UINextDayButton : MonoBehaviour
{
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
}
