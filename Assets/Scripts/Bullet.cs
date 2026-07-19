using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f; // Lifetime of the bullet in seconds

    private Rigidbody2D rb;
    private Vector2 launchDirection;
    private float launchSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float speed)
    {
        launchDirection = direction.normalized;
        launchSpeed = speed;

        if (rb != null)
        {
            rb.linearVelocity = launchDirection * launchSpeed;
            rb.angularVelocity = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            /*
            // Handle enemy hit logic here
            // For example, you can call a method on the enemy to apply damage
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // Assuming the bullet deals 1 damage
            }*/
        }
        DeactivateBullet();
    }

    void DeactivateBullet()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        // Cancel previous invokes and start the timer when the bullet is activated
        CancelInvoke("DeactivateBullet");
        Invoke("DeactivateBullet", lifetime);
    }
}
