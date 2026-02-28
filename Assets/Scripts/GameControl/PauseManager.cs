using UnityEngine;
using StorkStudios.CoreNest;

public class PauseManager : Singleton<PauseManager>
{
    private bool pauseEnabled = false;

    public readonly ObservableVariable<bool> IsPaused = new ObservableVariable<bool>(false);

    protected override void Awake()
    {
        base.Awake();

        IsPaused.ValueChanged += OnPauseValueChanged;
        PlayerInputManager.Instance.PauseEvent += OnPauseClicked;
        MainMenuController.Instance.StartGameEvent += () => pauseEnabled = true;
        MainMenuController.Instance.StartTutorialEvent += () => pauseEnabled = true;
    }

    private void OnPauseClicked()
    {
        if (pauseEnabled)
        {
            IsPaused.Value = !IsPaused.Value;
        }
    }

    private void OnPauseValueChanged(bool _, bool newValue)
    {
        Time.timeScale = newValue ? 0f : 1f;
    }
}
