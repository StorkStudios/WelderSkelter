using UnityEngine;

public class WeldingParticles : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    private GameObject weldingPoint;

    private void Start()
    {
        PauseManager.Instance.IsPaused.ValueChanged += OnPauseChanged;
    }

    private void OnDestroy()
    {
        if (PauseManager.IsInstanced)
        {
            PauseManager.Instance.IsPaused.ValueChanged -= OnPauseChanged;
        }
    }

    private void OnPauseChanged(bool _, bool newValue)
    {
        gameObject.SetActive(!newValue);
    }

    public void SetWelding(bool weldingState)
    {
        if (weldingState)
        {
            StartWelding();
        }
        else
        {
            StopWelding();
        }
    }

    public void StartWelding()
    {
        particles.SetActive(true);
        weldingPoint.SetActive(false);
    }

    public void StopWelding()
    {
        particles.SetActive(false);
        weldingPoint.SetActive(true);
    }
}
