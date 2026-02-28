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
    private TMPro.TextMeshProUGUI moneyText;

    [SerializeField]
    private TMPro.TextMeshProUGUI dayText;

    public void SetScreen(Screen screen)
    {
        foreach (Screen key in screens.Keys)
        {
           screens[key].SetActive(key == screen);
        }

        if (screen == Screen.Lose)
        {
            moneyText.text = $"{MoneyManager.Instance.Money} zł";
            dayText.text = $"{GameManager.Instance.CurrentDay}/{GameManager.Instance.TotalDays}";
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex());
    }
}
