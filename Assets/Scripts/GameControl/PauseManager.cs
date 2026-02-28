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
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += () => pauseEnabled = true;
        WorkPhaseManager.Instance.WorkPhaseEnded += (_) => pauseEnabled = false;
        ShopPhaseManager.Instance.ShopPhaseStarted += () => pauseEnabled = true;
        ShopPhaseManager.Instance.ShopPhaseEnded += () => pauseEnabled = false;

        Time.timeScale = 1;
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
