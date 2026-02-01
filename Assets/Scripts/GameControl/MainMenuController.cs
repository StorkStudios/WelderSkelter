using StorkStudios.CoreNest;
using UnityEngine;

public class MainMenuController : Singleton<MainMenuController>
{
    public event System.Action StartGameEvent;

    public void StartGame()
    {
        StartGameEvent?.Invoke();
    }
}
