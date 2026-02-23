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
        if ((Input.GetKey(KeyCode.Escape) || LoadMainScene) && !DontLoadMainScene)
        {
            DontLoadMainScene = true;
            SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex(), LoadSceneMode.Single);
        }

        //Now I regret.
    }
}
