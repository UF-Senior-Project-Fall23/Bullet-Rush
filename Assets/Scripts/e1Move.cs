using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e1Move : MonoBehaviour
{
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition + transform.right * Mathf.Sin(Time.time * 2f + 0f) * 2f;
    }
}
