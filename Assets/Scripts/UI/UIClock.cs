using TMPro;
using UnityEngine;

public class UIClock : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clock;
    [SerializeField]
    private Color normal;
    [SerializeField]
    private Color accent;
    [SerializeField]
    private float accentStartTime;

    private void Update()
    {
        float timeLeft = WorkPhaseManager.Instance.CurrentData.dayLength - WorkPhaseManager.Instance.DayTimer;
        int minutes = (int)(timeLeft / 60);
        int seconds = (int)(timeLeft % 60);
        if (seconds < 10)
        {
            clock.text = $"{minutes}:0{seconds}";
        }
        else
        {
            clock.text = $"{minutes}:{seconds}";
        }
        if (timeLeft <= accentStartTime && seconds % 2 == 0)
        {
            clock.color = accent;
        }
        else
        {
            clock.color = normal;
        }
    }
}
