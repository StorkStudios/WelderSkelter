using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public bool LoadMainScene = false;

    void Update()
    {
        if (LoadMainScene || Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }
}
