using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f; // Lifetime of the bullet in seconds
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCllisionEnter2D(Collision2D collision)
    {
        DeactivateBullet();
    }

    void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        // Cancel previous invokes and start the timer when the bullet is activated
        CancelInvoke("DeactivateBullet");
        Invoke("DeactivateBullet", lifetime);
    }
}
