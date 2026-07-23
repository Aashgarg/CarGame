using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public enum EnemyState
{
    Idle,
    Chasing,
    Ramming,
    Dead
}
public class EnemyOne : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] private UnityEvent<float, float> onHealthChanged = new UnityEvent<float, float>();

    [Header("Enemy Stats")]
    [SerializeField] private string enemyName;
    [SerializeField] private EnemyState currentState;
    [SerializeField] private float speed;
    [SerializeField] private float health;

    [Header("Detection")]
    [SerializeField] private float detectionRange;
    [SerializeField] float separationRadius = 2f;
    [SerializeField] float separationStrength = 2f;
    [SerializeField] private float attackingRange;
    

    [Header("Ramming")]
    [SerializeField] private float ramSpeed = 15f;          // faster than chase speed
    [SerializeField] private float ramDamage = 20f;         // damage dealt to player on impact
    [SerializeField] private float ramCooldown = 2f;        // seconds between rams
    private float lastRamTime = -999f;
    private bool isRamming = false;
    private bool isCommittedToRam = false;
    private float ramStartTime;

    [Header("Shooting")]
    [SerializeField] private float shootInterval;
    [SerializeField] private float fireRate;
    [SerializeField] private float fireForce;
    [SerializeField] private int damage;

    [Header("Sounds")]
    [SerializeField] private AudioClip ramImpactSound;
    [SerializeField] private AudioClip dodgeWarningSound;
    [SerializeField] private AudioClip shootDamageSound;
    private bool isShooting = false;
    private Rigidbody2D rb;
    float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Idle;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = health;
        onHealthChanged?.Invoke(currentHealth, health);
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

            if (!isCommittedToRam)
            {
                if (distanceToPlayer <= attackingRange)
                {
                    if (Time.time >= lastRamTime + ramCooldown)
                    {
                        currentState = EnemyState.Ramming;
                        isCommittedToRam = true; // lock in the ram
                    }
                    else
                    {
                        currentState = EnemyState.Chasing; // wait for cooldown
                    }
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
            case EnemyState.Ramming:
                Ram();
                break;
            case EnemyState.Dead:
                Die();
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            int bulletDamage = collision.gameObject.GetComponent<Bullet>().gunData.damage;
            //play a sound effect for the bullet impact
            //AudioSource.PlayClipAtPoint(shootDamageSound, transform.position);
            TakeDamage(bulletDamage);
        }

        // Deal damage to player on ram impact
        if (collision.gameObject.CompareTag("Player") && isRamming)
        {
            collision.gameObject.GetComponent<CarController>().TakeDamage((int)ramDamage);
            TakeDamage((int)ramDamage);
            //play a sound effect for the ram impact
            //AudioSource.PlayClipAtPoint(ramImpactSound, transform.position);


            // Knock the player back with an impulse
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * 30f, ForceMode2D.Impulse);
            }

            // Stop the enemy dead after impact so it doesn't keep pushing
            rb.linearVelocity = Vector2.zero;
            lastRamTime = Time.time;
            isRamming = false;
            isCommittedToRam = false; // unlock state machine
        }
        else if (collision.gameObject.CompareTag("Player") && !isRamming)
        {
            //player rammed into enemy deal damage to enemy and knockback enemy
            damage = collision.gameObject.GetComponent<CarController>().RamDamage;
            //play a sound effect for the ram impact
            //AudioSource.PlayClipAtPoint(ramImpactSound, transform.position);
            
            // Knock the enemy back with an impulse
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockbackDirection * 20f, ForceMode2D.Impulse);
            TakeDamage((int)damage);
            

            
        }
    }

    void TakeDamage(int damageAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0f, health);
        onHealthChanged?.Invoke(currentHealth, health);

        if (currentHealth <= 0f)
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

    void Ram()
    {
        // Implement ramming logic here
        // Move and acceleratetowards the player and apply damage on collision
        if (player == null) return;

        // Cooldown — stop and wait before next ram
        if (Time.time < lastRamTime + ramCooldown)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime * 5f);
            isRamming = false;
            isCommittedToRam = false;
            return;
        }

        if (!isRamming)
        {
            isRamming = true;
            ramStartTime = Time.time; // record when ram started
        }
        // Timeout — if ram takes more than 3 seconds, give up
        if (Time.time > ramStartTime + 3f)
        {
            rb.linearVelocity = Vector2.zero;
            isRamming = false;
            isCommittedToRam = false;
            lastRamTime = Time.time;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        //Charge back a little before ramming to give the player a chance to react
        //make this take more time so the player can react
        if (Time.time < ramStartTime + 0.5f)
        {
            //play a sound effect for the dodge warning
            rb.AddForce(-direction * ramSpeed * 1.1f, ForceMode2D.Force);
            //AudioSource.PlayClipAtPoint(dodgeWarningSound, transform.position);
            //Add a visual indicator for the dodge warning 
        }

        // Build up speed with AddForce so it feels like an actual charge
        rb.AddForce(direction * ramSpeed, ForceMode2D.Force);

        // Cap the ram speed so it doesn't accelerate forever
        if (rb.linearVelocity.magnitude > ramSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * ramSpeed;

        // Rotate to face player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
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
        //Debug.Log($"ObstacleLayer value: {obstacleLayer.value} | Hit: {(hitAhead.collider != null ? hitAhead.collider.name : "nothing")}");
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
       
        // make it so that the enemy is a few distance away from the player when chasing so that it doesn't collide with the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < 8f)
        {
            //stop moving towards the player and just face the player
            finalDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;
        }
        else
        {
            transform.position = Vector2.MoveTowards(
            transform.position,
            (Vector2)transform.position + finalDirection, 
            speed * Time.deltaTime);
        }

        float angle = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
            
    }
    void Idle()
    {
        // Implement idle behavior here
        // For this enemy it just stands still.
        // Show idle animation
    }
}
