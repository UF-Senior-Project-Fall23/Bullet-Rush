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
            FindObjectOfType<MusicManager>().FadeCurrentInto("Onyx Theme", 0.5f);
        }
    }
}
