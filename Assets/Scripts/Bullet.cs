using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GunData gunData;
    [SerializeField] float lifetime = 3f; // auto-return to pool after this time
    float timer;

    void OnEnable()
    {
        // Reset timer every time bullet is pulled from pool
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
            ReturnToPool();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore the player car
        if (other.CompareTag("Player")) return;

        // Hit an enemy
        if (other.CompareTag("Enemy"))
        {
            EnemyOne enemy = other.GetComponent<EnemyOne>();
            if (enemy != null)
                enemy.TakeDamage(gunData.damage);
        }

        // Hit a wall or anything else — just return to pool
        ReturnToPool();
    }

    void ReturnToPool()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
