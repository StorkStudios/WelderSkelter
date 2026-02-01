using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneEnum = StorkStudios.CoreNest.Scene;

public class EndScreenController : Singleton<EndScreenController>
{
    public enum Screen { Win, Lose }

    [SerializeField]
    private SerializedDictionary<Screen, GameObject> screens;

    public void SetScreen(Screen screen)
    {
        foreach (Screen key in screens.Keys)
        {
           screens[key].SetActive(key == screen);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex());
    }
}
