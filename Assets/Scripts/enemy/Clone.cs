using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : Damageable, puppetAttack
{
    public int a = 7;
    bool spot = false;
    public bool spotlight { get => spot; set => spot = value; }
    public GameObject bulletPreFab;
    public GameObject dimPreFab;
    GameObject dim;

    double attackSpeedModifier = 1;
    float globalTime = 0;
    int interval = 6;

    bool phaseShifted = false;
    bool attackReady = false;
    bool lights = false;
    bool rushMode = false;
    bool timeIndicator = true;
    bool spinSet = true;
    bool rush = false;
    GameObject cb;

    Vector3 playerPos;


    int followPattern = 1;
    float rotationSpeed = 3;
    float rotationSize = 8;

    private float bulletTime = 0f;
    private float bulletWait = .7f;
    private Vector3 rushDirection = Vector3.up;


    void Start()
    {
        playerPos = PlayerController.instance.gameObject.transform.position;


    }

    public void reset()
    {
        spinSet = true;
        rush = false;
    }

    public IEnumerator Spotlight()
    {
        dim = Instantiate(dimPreFab, transform);

        yield return null;
    }

    public IEnumerator Rush()
    {
        if (spinSet)
        {
            rotationSpeed = UnityEngine.Random.Range(1.0f, 5.0f);
            rotationSize = UnityEngine.Random.Range(4.0f, 8.0f);
            spinSet = false;
        }
        playerPos = BossController.instance.currentBoss.transform.position;

        float x = Mathf.Cos(Time.time * rotationSpeed) * rotationSize;
        float y = Mathf.Sin(Time.time * rotationSpeed) * rotationSize;
        transform.position = new Vector3(x, y);
        transform.position = transform.position + playerPos;

        yield return null;
    }

    public IEnumerator SpinDance(bool rush)
    {
        float moveSpeed = .025f;
        if (spinSet)
        {
            rotationSpeed = UnityEngine.Random.Range(1.0f, 5.0f);
            rotationSize = UnityEngine.Random.Range(4.0f, 8.0f);
            spinSet = false;
        }
        if (!rush)
        {
            playerPos = PlayerController.instance.transform.position;

            float x = Mathf.Cos(Time.time * rotationSpeed) * rotationSize;
            float y = Mathf.Sin(Time.time * rotationSpeed) * rotationSize;
            transform.position = new Vector3(x, y);
            transform.position = transform.position + playerPos;
        }
        else
        {
            var movementVector = (playerPos - transform.position).normalized * moveSpeed;
            transform.Translate(movementVector);
        }


        yield return null;
    }

    public IEnumerator BladeFlourish(int followPattern)
    {
        playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);


        if (followPattern == 1)
        {
            float speed = UnityEngine.Random.Range(0.0f, 3.0f);
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 2f);
        }
        else if (followPattern == 2)
        {
            float speed = UnityEngine.Random.Range(0.0f, 5.0f);
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 3f);
        }
        else if (followPattern == 3)
        {
            float speed = UnityEngine.Random.Range(0.0f, 3.0f);
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * -2f);
        }

        if (bulletTime <= Time.time && MathF.Floor(globalTime) % 2 == 0)
        {

            for (int i = 0; i < 5; i++)
            {
                var r2 = Quaternion.Euler(0, 0, playerAngle * Mathf.Rad2Deg + 90);

                GameObject bullet = Instantiate(bulletPreFab, new Vector3(
                    transform.position.x + (transform.localScale.x / 5f * Mathf.Cos(playerAngle)),
                    transform.position.y + (transform.localScale.y / 5f * Mathf.Sin(playerAngle)), 1), r2);

                bullet.transform.Rotate(0, 0, 290 - (i * 10));
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bullet.transform.right * 13f, ForceMode2D.Impulse);
            }
            bulletTime = Time.time + bulletWait;
            yield return null;
        }



        yield return null;
    }

    public IEnumerator DetonatePuppets()
    {
        yield return null;
    }


    void Update()
    {
        if (spotlight)
        {
            dim.transform.position = transform.position;
        }
    }




    void PhaseChange()
    {

    }

    IEnumerator StartPhase()
    {
        yield return null;
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}