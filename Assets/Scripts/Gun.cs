using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform firePoint;  // empty child at front of car
    [SerializeField] GunData gunData;
    [SerializeField] Camera mainCamera;

    float nextFireTime = 0f;

    void Update()
    {
        bool shootPressed = Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);

        if (shootPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / (gunData != null ? gunData.fireRate : 1f);
        }
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogWarning("Gun firePoint is not assigned.");
            return;
        }

        if (gunData == null)
        {
            Debug.LogWarning("GunData is not assigned on the Gun component.");
            return;
        }

        if (ObjectPooler.SharedInstance == null)
        {
            Debug.LogWarning("ObjectPooler was not found in the scene.");
            return;
        }

        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject();
        if (bullet == null)
        {
            Debug.LogWarning("No bullet is available from the object pool.");
            return;
        }

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
        else
            Debug.LogWarning("The bullet prefab is missing a Rigidbody2D component.");

        Debug.Log("Firing bullet.");
    }
}
