using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e3Comb : MonoBehaviour
{
    public GameObject target;
    public float speed = 2.0f;
    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    public bool straight = false;
    public float arc = 1.002f;

    void Start()
    {

    }
    private void Update()
    {
        var distVal = 50.0f;
        var dis = Vector3.Distance(transform.position, target.transform.position);
        if (dis <= distVal)
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                if (Random.value > 0.5f)
                {
                    straight = true;
                }
                else
                {
                    straight = false;
                }
            }
            if (straight)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
            }
            else
            {
                transform.position = Vector3.Slerp(transform.position, target.transform.position, speed) * arc;
            }
        }
        
    }
}
