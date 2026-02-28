using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneEnum = StorkStudios.CoreNest.Scene;

public class EndScreenController : Singleton<EndScreenController>
{
    public enum Screen { Win, Lose, TutorialWin, TutorialLose }

    [SerializeField]
    private SerializedDictionary<Screen, GameObject> screens;

    [SerializeField]
    private GameObject moneyTextObject;

    [SerializeField]
    private GameObject dayTextObject;

    public void SetScreen(Screen screen)
    {
        foreach (Screen key in screens.Keys)
        {
           screens[key].SetActive(key == screen);
        }

        if (screen == Screen.Lose || screen == Screen.Win)
        {
            TMPro.TextMeshProUGUI moneyText = moneyTextObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI dayText = dayTextObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            moneyTextObject.SetActive(true);
            dayTextObject.SetActive(true);
            moneyText.text = $"{MoneyManager.Instance.Money} zł";
            dayText.text = $"{GameManager.Instance.CurrentDay}/{GameManager.Instance.TotalDays}";
        }
        else
        {
            moneyTextObject.SetActive(false);
            dayTextObject.SetActive(false);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex());
    }
}
