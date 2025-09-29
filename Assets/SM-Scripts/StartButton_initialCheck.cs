using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton_initialCheck : MonoBehaviour
{
    // This Code is for start button.
    //
    //       public int active : If you set active 0, server mode would be off.
    //                           If you set active 1, server mode would be on.
    //                                                It is not needed anymore because this code is for server.
    // public GameObject Panel : It is not needed anymore because this code is for server.
    // 
    // When the button is clicked and if PlayerPrefs has key "PlayerMaxLevel",          it means player is not new.
    //                                                                                  Scene would be changed.
    //                                if PlayerPrefs doesn't have key "PlayerMaxLevel", it means player is new.
    //                                                                                  PlayerPrefs would have key "PlayerMaxLevel" set 0.
    //                                                                                  And then Scene would be changed.
    public int active;
    public GameObject Panel;
    public void onClicked()
    {
        if (PlayerPrefs.HasKey("PlayerName") || active == 0)
        {
            if (active == 0 && !PlayerPrefs.HasKey("PlayerMaxLevel"))
            {
                PlayerPrefs.GetInt("PlayerMaxLevel", 0);
            }
            SceneManager.LoadScene("LevelScene");
        }
        else
        {
            Panel.SetActive(true);
        }

    }
}
