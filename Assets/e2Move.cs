using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e2Move : MonoBehaviour
{
    public GameObject target;
    private Vector3 startPosition;
    float _amplitude = 4;
    float _frequency = 2;
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        startPosition = target.transform.position;
        float x = Mathf.Cos(Time.time * _frequency) * _amplitude;
        float y = Mathf.Sin(Time.time * _frequency) * _amplitude;
        transform.position = new Vector3(x, y);
        transform.position = transform.position + startPosition;
    }
}
