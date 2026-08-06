using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform firePoint;  // empty child at front of car
    [SerializeField] GunData gunData;
    [SerializeField] Camera mainCamera;

    float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / gunData.fireRate;
            }
        }
    }

    void Shoot()
    {
        if (firePoint == null || gunData == null) return;
        if (ObjectPooler.SharedInstance == null) return;

        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject();
        if (bullet == null) return;

        // Spawn at firePoint, facing same direction as car
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);

        // Make sure bullet has the gunData reference
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.gunData = gunData;

        // Fire in the direction firePoint is facing (forward = transform.up in 2D)
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = firePoint.up * gunData.bulletSpeed;
    }
}
