using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide_script : MonoBehaviour
{

    public float Speed;
    private Rigidbody2D Rigidbody2D;

    private Vector2 Direction;


    public Transform attackPos;

    public float attackRange;

    public LayerMask whatIsEnemies;

    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = Direction * Speed;
    }

    public void SetDirection(Vector2 direction)
    {

        if (direction.x == 1)
        {
            transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        }
        else
        {
            transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);
        }
        Direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<SkeletonScript>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void DestroyDust()
    {
        Destroy(gameObject);
    }
}
