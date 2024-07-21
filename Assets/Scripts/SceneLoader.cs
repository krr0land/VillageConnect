using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
