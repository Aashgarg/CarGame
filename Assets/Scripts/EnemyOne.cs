using UnityEngine;

public enum EnemyState
{
    Idle,
    Chasing,
    Shooting,
    Ramming,
    Dead
}
public class EnemyOne : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private string enemyName;
    [SerializeField] private EnemyState currentState;
    [SerializeField] private float health;

    [SerializeField] private float speed;
    [SerializeField] private float detectionRange;
    [SerializeField] private float shootingRange;
    [SerializeField] private float rammingRange;

    [SerializeField] private float fireRate;
    [SerializeField] private float fireForce;
    [SerializeField] private int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            currentState = EnemyState.Dead;
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= rammingRange /*&& ramming cooldown is over*/)
            {
                currentState = EnemyState.Ramming;
            }
            else if (distanceToPlayer <= shootingRange /*&& shooting cooldown is over && is on screen*/)
            {
                currentState = EnemyState.Shooting;
            }
            else if (distanceToPlayer <= detectionRange)
            {
                currentState = EnemyState.Chasing;
            }
            else
            {
                currentState = EnemyState.Idle;
            }
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Shooting:
                Shoot();
                break;
            case EnemyState.Ramming:
                Ram();
                break;
            case EnemyState.Dead:
                Die();
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            int bulletDamage = collision.gameObject.GetComponent<Bullet>().gunData.damage;
            TakeDamage(bulletDamage);
        }
    }

    void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle enemy death logic here
        // death animation
        // show number of damage taken
        // destroy enemy
        Destroy(gameObject);
    }

    void Shoot()
    {
        // Implement shooting logic here
        // Instantiate bullets, apply force, etc.
        // Show shooting animation
    }

    void Ram()
    {
        // Implement ramming logic here
        // Move towards the player and apply damage on collision
        // Show ramming animation
    }
    void ChasePlayer()
    {
        // Implement chasing logic here
        // Move towards the player's position
        // Show chasing animation
    }
    void Idle()
    {
        // Implement idle behavior here
        // Patrol, wander, or stay in place
        // Show idle animation
    }
}
