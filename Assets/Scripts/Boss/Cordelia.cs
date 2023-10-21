using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cordelia : MonoBehaviour, Boss
{
    public GameObject bulletPreFab;
    private string currentDanceType = null;
    private double attackSpeedModifier = 1;
    private bool phaseShifted = false;
    private bool attackReady = false;
    private bool l = true;
    private float globalTime = 0;

    public string[] Attacks { get; } =
    {
        "SpinDance", "KickDance", "StringDance", "SummonPuppets", "DetonatePuppets", "Rush", "Spotlight", "BladeFlourish", "PuppeteersGrasp"
    };
    


    // Sets the dance to Spin.
    void SpinDance()
    {
        currentDanceType = "Spin";
    }

    void KickDance()
    {
        currentDanceType = "Kick";
    }

    void StringDance()
    {
        currentDanceType = "String";
    }

    void SummonPuppets()
    {

    }

    void DetonatePuppets()
    {

    }

    void Rush()
    {
        
    }

    void Spotlight()
    {

    }

    void BladeFlourish()
    {

    }

    void PuppeteersGrasp()
    {

    }
    

    void Boss.PhaseChange()
    {
        phaseShifted = true;
        attackSpeedModifier = 1.5;
    }

    void Boss.BossLogic(float deltaTime, GameObject cb, Vector3 playerPos)
    {
        globalTime += deltaTime;

        float second = MathF.Floor(globalTime) % 4;
        
        if (second == 0 && attackReady)
        {   
            attackReady = false;
            if (l)
            {
                SpinDance();
                l = false;
            }
            else
            {
                StringDance();
                l = true;
            }
        }

        if (second != 0)
        {
            attackReady = true;
        }
        if(currentDanceType == "String")
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
                        rb.AddForce(cb.transform.forward * 20f, ForceMode2D.Impulse);
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
            
        }
        
        if(currentDanceType == "Spin")
        {
            Vector3 startPosition = playerPos;
            float x = Mathf.Cos(Time.time * 1) * 5;
            float y = Mathf.Sin(Time.time * 1) * 5;
            cb.transform.position = new Vector3(x, y);
            cb.transform.position = cb.transform.position + startPosition;
        }
    }

    //Run this so the boss controller wont call bosslogic anymore
    private void OnDestroy()
    {
        BossController.instance.BossDie();
    }

}