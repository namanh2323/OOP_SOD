using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float agroRange;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    int enemyHealth;

    public Megaman player;

    public Rigidbody2D r2;
    public Animator anim;

    public bool faceright = true;
    public float distanceToLeft, distanceToRight;
    public float distance;
    public float wakerange;
    public Transform shootpointL, shootpointR;

    // Start is called before the first frame update
    void Start()
    {
        r2 = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Megaman>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        distanceToLeft = Vector2.Distance(shootpointL.position, player.transform.position);
        distanceToRight = Vector2.Distance(shootpointR.position, player.transform.position);
        if (distance < agroRange)
        {
            //chase Megaman
            ChasingPlayer();
        }

        if (distance > agroRange)
        {
            //stop chase
            StopChasingPlayer();
        }
    }

    public void ChasingPlayer()
    {
        if (transform.position.x < transform.position.x)
        {
            r2.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            r2.velocity = new Vector2(-moveSpeed, 0);
        }

        anim.Play("EnemyRotate");
    }

    public void StopChasingPlayer()
    {
        r2.velocity = new Vector2(0, 0);
        anim.Play("EnemyIdle");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            Destroy(col.gameObject);
            enemyHealth--;
            if (distance > agroRange)
            {
                gameObject.GetComponent<Animation>().Play("EnemyIdleRed");
            }
            else if (distance < agroRange)
            {
                gameObject.GetComponent<Animation>().Play("EnemyRotateRed");
            }
            if (enemyHealth == 0)
            {
                Destroy(gameObject);
            }
        }
        else if (col.CompareTag("Player"))
        {
            player.Damage(2);
            player.Knockback(350f, player.transform.position);
        }
    }

    public void Damage(int damage)
    {
        enemyHealth -= damage;

    }

    public void Knockback(float Knockpow, Vector2 Knockdir)
    {
        r2.velocity = new Vector2(0, 0);
        r2.AddForce(new Vector2(Knockdir.x * -100, Knockdir.y * Knockpow));
    }
}
