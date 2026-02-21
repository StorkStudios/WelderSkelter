using UnityEngine;
using StorkStudios.CoreNest;
using UnityEngine.SceneManagement;

using SceneEnum = StorkStudios.CoreNest.Scene;

public class IntroController : MonoBehaviour
{
    public bool LoadMainScene = false;
    public bool DontLoadMainScene = false;

    private void Update()
    {
        if (LoadMainScene && DontLoadMainScene)
        {
            MainMenuController.Instance.StartTutorial();
            SceneManager.UnloadSceneAsync(SceneEnum.Intro.GetBuildIndex());
        }
        if (LoadMainScene && !DontLoadMainScene)
        {
            DontLoadMainScene = true;
            SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex(), LoadSceneMode.Additive);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex(), LoadSceneMode.Single);
        }

        //I regret nothing.
    }
}
