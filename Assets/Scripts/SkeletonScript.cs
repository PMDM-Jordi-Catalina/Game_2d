using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{

    public GameObject heroe;

    public GameObject pointA;
    public GameObject pointB;

    private Transform currentPoint;

    private Rigidbody2D rb;

    private float LastAtack;

    private int m_ataque_actual = 0;

    private float m_delaydeAtaque = 0.0f;

    private Animator Animator;

    public Transform attackPos;

    public float attackRange;

    public LayerMask whatIsEnemies;

    public AudioSource source;

    public AudioClip attack;

    public AudioClip death;

    public AudioClip hit;


    public int damage;

    public int health = 100;


    private bool alive = true;
    private float m_delay_do_damage = 0.0f;

    public float speed;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
        Animator.SetBool("Running", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {

            Vector2 point = currentPoint.position - transform.position;
            if (currentPoint == pointB.transform)
            {
                rb.velocity = new Vector2(speed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-speed, 0);
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
            {
                flip();
                currentPoint = pointA.transform;
            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
            {
                flip();
                currentPoint = pointB.transform;
            }

            if (health <= 0)
            {
                alive = false;
                Animator.SetTrigger("Death");
            }
            m_delaydeAtaque += Time.deltaTime;


            float distance = Mathf.Abs(heroe.transform.position.x - transform.position.x);



            if (distance < 2.0f && m_delaydeAtaque > 0.9f)
            {
                Vector3 direction = heroe.transform.position - transform.position;
                if (direction.x >= 0.0f) transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
                else transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);
                Atack();
                LastAtack = Time.time;
            }
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }


    public void TakeDamage(int damage)
    {
        if (alive)
        {
            health -= damage;
            Animator.SetTrigger("Take_Damage");
            source.PlayOneShot(hit);
        }

    }

    public void Destroy()
    {
        source.PlayOneShot(death);
        Destroy(gameObject);

    }


    private void Atack()
    {
        m_ataque_actual++;

        // Loop back to one after third attack
        if (m_ataque_actual > 2)
            m_ataque_actual = 1;

        // Reset Attack combo if time since last attack is too large
        if (m_delaydeAtaque > 1.0f)
            m_ataque_actual = 1;

        // Call one of three attack animations "Attack1", "Attack2", "Attack3"
        Animator.SetTrigger("Attack" + m_ataque_actual);
        source.PlayOneShot(attack);

        m_delaydeAtaque = 0.0f;
    }


    private void doDamage()
    {

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Heroe_move>().TakeDamage(damage);
            m_delay_do_damage += Time.deltaTime;
        }


    }
}
