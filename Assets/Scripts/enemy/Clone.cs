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
    private Animator m_Animator;

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
    private float bulletWait = 1.5f;
    private Vector3 rushDirection = Vector3.up;


    void Start()
    {
        //m_DifficultyModifier = GameManager.instance.getDifficultyModifier();
        //m_LevelModifier = GameManager.instance.getLevelModifier();
        playerPos = PlayerController.instance.gameObject.transform.position;
        m_Animator = GetComponent<Animator>();
        
    }

    public void reset()
    {
        spinSet = true;
        rush = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Clone")
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerController.instance.GetComponent<Damageable>()?.takeDamage(1);
            }
        }


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
        //if(m)
        playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);


        if (followPattern == 1)
        {
            float speed = UnityEngine.Random.Range(1.0f, 5.0f);
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 2f);
        }
        else if (followPattern == 2)
        {
            float speed = UnityEngine.Random.Range(1.0f, 8.0f);
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 3f);
        }
        else if (followPattern == 3)
        {
            float speed = UnityEngine.Random.Range(1.0f, 5.0f);
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * -2f);
        }

        if (bulletTime <= Time.time && MathF.Floor(globalTime) % 2 == 0)
        {
            int numKnives = GameManager.instance.getCurrentDifficultyInt() + 3;
            float angleShift = 50f / numKnives;
            for (int i = 0; i < numKnives; i++)
            {
                var r2 = Quaternion.Euler(0, 0, playerAngle * Mathf.Rad2Deg + 90);

                var localScale = transform.localScale;
                var position = transform.position; 
                GameObject bullet = Instantiate(bulletPreFab, new Vector3(
                    position.x + (localScale.x / 5f * Mathf.Cos(playerAngle)),
                    position.y + (localScale.y / 5f * Mathf.Sin(playerAngle)),
                    1), r2);

                bullet.transform.Rotate(0, 0, 290 - (i * angleShift));
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bullet.transform.right * .0008f, ForceMode2D.Impulse);
            }
            bulletTime = Time.time + bulletWait;
            yield return null;
        }



        yield return null;
    }

    public IEnumerator DetonatePuppets()
    {
        //yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        m_Animator.SetTrigger("Explode");
        yield return new WaitForSeconds(1.5f);
        Die();
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