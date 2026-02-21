using UnityEngine;
using StorkStudios.CoreNest;
using UnityEngine.SceneManagement;

using SceneEnum = StorkStudios.CoreNest.Scene;

public class IntroController : MonoBehaviour
{
    public bool LoadMainScene = false;

    private void Update()
    {
        if (LoadMainScene || Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex(), LoadSceneMode.Single);
        }
    }
}
