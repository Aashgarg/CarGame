using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private CarController carController;
    [SerializeField] Rigidbody2D carRigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= 10f; // Reduce health by 10 on collision with the bullet
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Reduce car's health by 10 on collision with the wall
            carController.TakeDamage(10f);
            health -= (float)carController.RamDamage; // Reduce wall's health by the car's ram damage on collision with the car
            //car knockback logic
            if (carRigidbody != null)
            {
                Vector2 knockbackDirection = (carRigidbody.position - (Vector2)transform.position).normalized;
                float knockbackForce = 5f; // Adjust the force as needed
                carRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }

        if (health <= 0f)
        {
            Destroy(gameObject); // Destroy the wall if health is zero or less
        }
    }
}
