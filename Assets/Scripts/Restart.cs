using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Needed if you're attaching this to a UI Button

public class RestartScene : MonoBehaviour
{
    void Update()
    {
        // Check if R key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartCurrentScene();
        }
    }

    // This function can be called from a UI Button
    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}

