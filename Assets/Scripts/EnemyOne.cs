using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] private Transform player;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask obstacleLayer;

    [SerializeField] private string enemyName;
    [SerializeField] private EnemyState currentState;
    [SerializeField] private float health;

    [SerializeField] private float speed;
    [SerializeField] private float detectionRange;
    [SerializeField] float separationRadius = 2f;
    [SerializeField] float separationStrength = 2f;

    [SerializeField] private float shootingRange;
    [SerializeField] private float shootInterval;
    [SerializeField] private float rammingRange;

    [SerializeField] private float fireRate;
    [SerializeField] private float fireForce;
    [SerializeField] private int damage;

    private NavMeshAgent agent;
    private bool isShooting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Idle;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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
            Debug.Log($"distanceToPlayer: {distanceToPlayer}, currentState: {currentState}");
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

    Vector2 GetSeparationDirection()
    {
        Vector2 separationForce = Vector2.zero;
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (Collider2D col in nearbyEnemies)
        {
            // Skip if it's this enemy or not an enemy at all
            if (col.gameObject == gameObject) continue;
            if (!col.CompareTag("Enemy")) continue;

            Vector2 pushDirection = (Vector2)(transform.position - col.transform.position);
            float distance = pushDirection.magnitude;

            // Closer enemies push harder
            if (distance > 0)
                separationForce += pushDirection.normalized / distance;
        }

        return separationForce.normalized;
    }
    void ChasePlayer()
    {
        // Implement chasing logic here
        // Move towards the player's position
        // Show chasing animation
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        // Cast multiple rays to detect obstacles in different directions
        RaycastHit2D hitAhead = Physics2D.CircleCast(transform.position, 0.3f, direction, 1.5f, obstacleLayer);
        
        
        Debug.DrawRay(transform.position, direction * 1.5f, hitAhead.collider != null ? Color.red : Color.green);
        Debug.Log($"ObstacleLayer value: {obstacleLayer.value} | Hit: {(hitAhead.collider != null ? hitAhead.collider.name : "nothing")}");
        if (hitAhead.collider != null)
        {
            // Try steering left or right around the obstacle
            Vector2 leftDirection = Quaternion.Euler(0, 0, 45) * direction;
            Vector2 rightDirection = Quaternion.Euler(0, 0, -45) * direction;
            
            RaycastHit2D hitLeft = Physics2D.CircleCast(transform.position, 0.3f, leftDirection, 1.5f, obstacleLayer);
            RaycastHit2D hitRight = Physics2D.CircleCast(transform.position, 0.3f, rightDirection, 1.5f, obstacleLayer);

            if (hitLeft.collider == null)
            {
                direction = leftDirection; // left is clear, go left
            }
            else if (hitRight.collider == null)
            {
                direction = rightDirection; // right is clear, go right
            }
            else
            {
                // Both sides blocked, slide along the obstacle normal
                direction = Vector2.Perpendicular(hitAhead.normal).normalized;
            }
        }
        // Blend chase direction with separation force
        Vector2 separation = GetSeparationDirection();
        Vector2 finalDirection = (direction + separation * separationStrength).normalized;

        transform.position = Vector2.MoveTowards(
            transform.position,
            (Vector2)transform.position + finalDirection,
            speed * Time.deltaTime);

        float angle = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
            
    }
    void Idle()
    {
        // Implement idle behavior here
        // Patrol, wander, or stay in place
        // Show idle animation
    }
}
