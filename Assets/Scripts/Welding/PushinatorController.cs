using UnityEngine;

public class PushinatorController : MonoBehaviour
{
    [SerializeField]
    private GameObject keyboardTips;

    public void SetKeyboardTips(bool value)
    {
        keyboardTips.SetActive(value);
    }
}