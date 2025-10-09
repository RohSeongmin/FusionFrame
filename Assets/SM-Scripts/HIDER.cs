using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Code is for Server Mode.
// If you set active 0, server mode would be off.
// If you set active 1, server mode would be on.
//                      It is not needed anymore because this code is for server.

public class HIDER : MonoBehaviour
{
    public int active;
    void Start()
    {
        if(active == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
