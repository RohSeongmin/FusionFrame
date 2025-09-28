using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This Code is for EnterButton
// It is not needed anymore because this code is for server.

public class EnterButton : MonoBehaviour
{
    //public GameObject inputField;
    //public GameObject warning;
    //public SceneChanger sceneChanger;

    //private TMP_InputField inputMessage;
    //private TextMeshProUGUI warningMessage;
    //public async void Onclicked()
    //{

    //    inputMessage = inputField.GetComponent<TMP_InputField>();
    //    warningMessage = warning.GetComponent<TextMeshProUGUI>();

    //    string inputText = inputMessage.text;
    //    if (string.IsNullOrWhiteSpace(inputText))
    //    {
    //        warningMessage.text = "Inproper Name";
    //    }
    //    else
    //    {
    //        if (await GetComponent<FirestoreManager>().AddPlayer(inputText, 0))
    //        {
    //            PlayerPrefs.SetString("PlayerName", inputText);
    //            PlayerPrefs.SetInt("PlayerMaxLevel", 0);
    //            PlayerPrefs.Save();
    //            SceneManager.LoadScene("LevelScene");
    //        }
    //        else
    //        {
    //            warningMessage.text = "Name already exists";
    //        }
            
    //    }

    //}

}
