using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordeliaBullet : MonoBehaviour
{
    private Animator m_Animator;
    private float m_DifficultyModifier;
    private float m_LevelModifier;
    public int damage;
    bool m_alive = true;

    void Start()
    {
        m_DifficultyModifier = GameManager.instance.getDifficultyModifier();
        m_LevelModifier = GameManager.instance.getLevelModifier();
        m_Animator = GetComponent<Animator>();

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit " + collision.gameObject.name);

        if (!collision.gameObject.CompareTag("Boss") && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Clone"))
        {
            
            m_alive = false;
            if(BossController.instance.currentBoss != null)
            {

                collision.GetComponent<Damageable>()?.takeDamage(damage);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                StartCoroutine(Hit());
            }
            
        }

    }

    public IEnumerator Hit()
    {
        
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        m_Animator.SetTrigger("Break");
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

    public IEnumerator TimeOut()
    {
        
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        m_Animator.SetTrigger("Break");
        yield return new WaitForSeconds(.3f);
        Destroy(gameObject);
    }

    float timeLeft = 0f;
    float timeLast = 3f;
    bool callOnce = true;
    void Update()
    {
        timeLeft += Time.deltaTime;
        
        if(timeLeft > timeLast && callOnce)
        {
            callOnce = false;
            StartCoroutine(TimeOut());
        }
    }

}
