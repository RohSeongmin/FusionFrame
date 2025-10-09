using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// This Code is for Debugging.
// If you set debug 0, this code would not work.
// If you set debug 1, you can initialize PlayerPrefs Database (local) by key "Z" 
//                     you can add LEVEL by key "X"
// [Warning] I only added this code in StartScene, so Debugger only works in StartScene
//           Also, key "X" works even after you initialize PlayerPrefs Database (local)

public class Debugger : MonoBehaviour
{
    public int debug;
    void Update()
    {
        if (debug == 1)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                PlayerPrefs.DeleteAll();  // PlayerPrefs 전체 삭제
                PlayerPrefs.Save();       // 즉시 저장 반영
                Debug.Log("PlayerPrefs are all initialized.");
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                int lev = PlayerPrefs.GetInt("PlayerMaxLevel");
                if (lev < 8)
                {
                    PlayerPrefs.SetInt("PlayerMaxLevel", lev + 1);
                    PlayerPrefs.Save();
                    Debug.Log($"Current Max Level: {lev+1}");
                }
            }
        }
    }
}
