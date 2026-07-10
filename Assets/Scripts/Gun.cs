using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GunData gunData;
    private float nextFireTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // fire rate
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / gunData.fireRate;
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.up * gunData.bulletSpeed;
            }
            else
            {
                Debug.LogWarning("Rigidbody2D component not found on the bullet prefab.");
            }
            
        }
        else
        {
            Debug.LogWarning("No pooled object available!");
        }
    }
}
