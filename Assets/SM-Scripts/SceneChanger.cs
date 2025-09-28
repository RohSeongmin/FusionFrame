using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // This code is for simply changing the scene when button is clicked.
    //
    // public string SceneName : you should put scene name in inspector. 


    public string SceneName;
    public void onClicked()
    {
        if (string.IsNullOrWhiteSpace(SceneName))
        {
            Debug.Log("SceneChanger : Empty Scene Name");
        }
        else SceneManager.LoadScene(SceneName);
    }
}
