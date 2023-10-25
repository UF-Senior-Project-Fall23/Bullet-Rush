using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: Fix this code :)
public class Cordelia : MonoBehaviour, Boss
{
    public GameObject bulletPreFab;
    double attackSpeedModifier = 1;
    bool phaseShifted = false;
    bool attackReady = false;
    bool l = true;
    float globalTime = 0;
    GameObject cb;
    Vector3 playerPos;

    void Start()
    {
        cb = BossController.instance.currentBoss;
        playerPos = PlayerController.instance.gameObject.transform.position;
    }

    public string[] Attacks { get; } =
    {
        "SpinDance", "KickDance", "StringDance", "SummonPuppets", "DetonatePuppets", "Rush", "Spotlight", "BladeFlourish", "PuppeteersGrasp"
    };

    // Sets the dance to Spin.
    IEnumerator SpinDance()
    {
        float speed = .02f;
        float nextActionTime = 0.0f;
        float period = 0.1f;

        bool straight = false;
        float arc = 1.002f;
        var distVal = 50.0f;
        var dis = Vector3.Distance(cb.transform.position, playerPos);
        if (dis <= distVal)
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                if (UnityEngine.Random.value > 0.5f)
                {
                    straight = true;
                    GameObject bullet = Instantiate(bulletPreFab, (cb.transform.position + playerPos) / 2.0f, cb.transform.rotation);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(cb.transform.right * 20f, ForceMode2D.Impulse);
                }
                else
                {
                    straight = false;
                }
            }
            if (straight)
            {
                cb.transform.position = Vector3.MoveTowards(cb.transform.position, playerPos, speed);
            }
            else
            {
                cb.transform.position = Vector3.Slerp(cb.transform.position, playerPos, speed) * arc;
            }
        }
        yield return null;
    }

    IEnumerator KickDance()
    {
        yield return null;
    }

    IEnumerator StringDance()
    {
        Vector3 startPosition = playerPos;
        float x = Mathf.Cos(Time.time * 1) * 5;
        float y = Mathf.Sin(Time.time * 1) * 5;
        cb.transform.position = new Vector3(x, y);
        cb.transform.position = cb.transform.position + startPosition;
        yield return null;
    }

    IEnumerator SummonPuppets()
    {
        yield return null;
    }

    IEnumerator DetonatePuppets()
    {
        yield return null;
    }

    IEnumerator Rush()
    {
        yield return null;
    }

    IEnumerator Spotlight()
    {
        yield return null;
    }

    IEnumerator BladeFlourish()
    {
        yield return null;
    }

    IEnumerator PuppeteersGrasp()
    {
        yield return null;
    }
    

    void Boss.PhaseChange()
    {
        phaseShifted = true;
        attackSpeedModifier = 1.5;
    }

    IEnumerator Boss.StartPhase()
    {
        yield return null;
    }

    void Update()
    {
        globalTime += Time.deltaTime;

        float second = MathF.Floor(globalTime) % 4;
        
        if (second == 0 && attackReady)
        {   
            attackReady = false;
            if (l)
            {
                StartCoroutine(SpinDance());
                l = false;
            }
            else
            {
                StartCoroutine(StringDance());
                l = true;
            }
        }

        if (second != 0)
        {
            attackReady = true;
        }
    }

    //Run this so the boss controller wont call bosslogic anymore
    private void OnDestroy()
    {
        BossController.instance.BossDie();
    }

}