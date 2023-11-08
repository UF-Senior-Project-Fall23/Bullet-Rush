using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInputs : MonoBehaviour
{
    private bool canChangeMusic = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && canChangeMusic)
        {
            canChangeMusic = false;
            FindObjectOfType<MusicManager>().FadeTo("Onyx Theme", 0.6f);
        }
    }
}
