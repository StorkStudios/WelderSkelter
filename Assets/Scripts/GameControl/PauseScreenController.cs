using UnityEngine;
using StorkStudios.CoreNest;
using UnityEngine.SceneManagement;
using SceneEnum = StorkStudios.CoreNest.Scene;

public class PauseScreenController : MonoBehaviour
{
    private void Start()
    {
        PauseManager.Instance.IsPaused.ValueChanged += OnPauseValueChanged;
        gameObject.SetActive(false);
    }

    private void OnPauseValueChanged(bool _, bool newValue)
    {
        gameObject.SetActive(newValue);
    }

    public void Unpause()
    {
        PauseManager.Instance.IsPaused.Value = false;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex());
    }
}
