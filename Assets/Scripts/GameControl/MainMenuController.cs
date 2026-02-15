using StorkStudios.CoreNest;
using UnityEngine;

public class MainMenuController : Singleton<MainMenuController>
{
    public event System.Action StartGameEvent;
    public event System.Action StartTutorialEvent;

    public void StartGame()
    {
        StartGameEvent?.Invoke();
    }

    public void StartTutorial()
    {
        StartTutorialEvent?.Invoke();
    }
}
