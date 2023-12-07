using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kickAttack : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Player"))
        {
            damage = 2;
            collision.GetComponent<Damageable>()?.takeDamage(damage);
        }
    }

    float timeLeft = 0f;
    float timeLast = 3f;
    
    void Update()
    {
        timeLeft += Time.deltaTime;

        if (timeLeft > timeLast)
        {
            Destroy(gameObject);
        }
        transform.position = BossController.instance.currentBoss.transform.position;
    }
}
