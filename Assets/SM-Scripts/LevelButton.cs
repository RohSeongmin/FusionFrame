using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// **********************************************************************
// ***** YOU SHOULD CHANGE GAME SCENE NAME AFTER MERGING THE BRANCH *****
// **********************************************************************

// This Code is for each Level Buttons.
//
//      public Image image : You should put each button's   image in INSPECTOR
//  public Sprite unlocked : You should put unlocked button image in INSPECTOR
//    public Sprite locked : You should put locked button   image in INSPECTOR
//      private bool allow : If current level is locked,   it would be false. 
//                           If current level is unlocked, it would be true
//
// If max level is lower  than each button's name (level),         it would be locked.
// If max level is higher than each button's name (level) or same, it would be unlocked.
// 
// If button is clicked and allow is true, button level is saved on PlayerPrefs as "CurrentLevel",
//                                         and scene is changed to gameScene.

public class LevelButton : MonoBehaviour
{
    public Image image;
    public Sprite unlocked;
    public Sprite locked;
    private int max_level;
    private bool allow;
    void Start()
    {
        max_level = PlayerPrefs.GetInt("PlayerMaxLevel");
        if (allow = (max_level >= int.Parse(gameObject.name)))
        {
            image.sprite = unlocked; 
        }
        else
        {
            image.sprite = locked;
        }
    }

    public void Onclicked()
    {
        if (allow)
        {
            PlayerPrefs.SetInt("CurrentLevel", int.Parse(gameObject.name));
            PlayerPrefs.GetInt("CurrentLevel");

            if (PlayerPrefs.GetInt("PlayerMaxLevel") < PlayerPrefs.GetInt("CurrentLevel")) 
            { 
                PlayerPrefs.SetInt("PlayerMaxLevel", PlayerPrefs.GetInt("CurrentLevel"));
            }
            SceneManager.LoadScene("RankingScene");//fixit
        }
    }
}
