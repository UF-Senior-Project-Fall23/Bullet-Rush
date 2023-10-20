using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e1Move : MonoBehaviour
{
    private Vector3 startPosition;
    public GameObject target;

    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (PlayerController.instance.checkLife())
        {
            var distVal = 50.0f;
            var dis = Vector3.Distance(startPosition, target.transform.position);
            if (dis <= distVal)
            {
                transform.position = startPosition + transform.right * Mathf.Sin(Time.time * 2f + 0f) * 2f;
            }
        }
        
        
    }
}
