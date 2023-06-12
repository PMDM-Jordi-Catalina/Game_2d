using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Heroe_move : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;



    public AudioSource source;

    public AudioClip jump;

    public AudioClip die;

    public AudioClip takeDamage;

    public AudioClip attack;

    public GameObject dustPrefab;

    public HealthBarHUDTester health;
    public float Speed;
    public float jumpForce;
    private bool Grounded;

    private Animator Animator;

    private float m_delaydeAtaque = 0.0f;

    private int m_ataque_actual = 0;

    public float positionDust = 0.0f;

    private bool alive = true;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -16)
        {
            alive = false;
            Animator.SetTrigger("Die");
        }
        if (alive)
        {
            m_delaydeAtaque += Time.deltaTime;
            Horizontal = Input.GetAxisRaw("Horizontal");

            if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            Animator.SetBool("Running", Horizontal != 0.0f);
            Animator.SetBool("Jumping", !Grounded);


            if (Physics2D.Raycast(transform.position, Vector3.down, 0.9f))
            {
                Grounded = true;

            }
            else
            {
                Grounded = false;

            }
            if (Input.GetKeyDown(KeyCode.W) && Grounded)
            {
                Jump();
            }
            else if (Input.GetMouseButtonDown(0) && m_delaydeAtaque > 0.25f)
            {
                m_ataque_actual++;

                // Loop back to one after third attack
                if (m_ataque_actual > 3)
                    m_ataque_actual = 1;

                // Reset Attack combo if time since last attack is too large
                if (m_delaydeAtaque > 1.0f)
                    m_ataque_actual = 1;

                // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                Animator.SetTrigger("Ataque" + m_ataque_actual);
                Dust();

                // Reset timer
                m_delaydeAtaque = 0.0f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (alive)
        {
            health.Hurt(damage);
            Animator.SetTrigger("Hurt");
            source.PlayOneShot(takeDamage);
            if (health.getHealth() == 0)
            {
                source.PlayOneShot(die);
                alive = false;
                Animator.SetTrigger("Die");
            }
        }

    }

    public void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("FinalGame");
    }

    private void Dust()
    {
        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector3.right;
        else direction = Vector3.left;
        GameObject dust = Instantiate(dustPrefab, transform.position + direction * 1.4f, Quaternion.identity);
        dust.GetComponent<Slide_script>().SetDirection(direction);
        source.PlayOneShot(attack);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * jumpForce);
        source.PlayOneShot(jump);
    }
}
