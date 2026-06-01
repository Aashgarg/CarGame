using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform destinationTransform;
    [SerializeField] private bool keepVelocity;
    [SerializeField] private ParticleSystem entryTeleportEffect;
    [SerializeField] private ParticleSystem exitTeleportEffect;

    private Rigidbody2D playerRigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        if (entryTeleportEffect != null)
        {
            entryTeleportEffect.Play();
        }

        playerTransform.position = destinationTransform.position;
        playerTransform.rotation = destinationTransform.rotation;

        playerRigidbody = playerTransform.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            if (keepVelocity)
            {
                float angleDifference = destinationTransform.eulerAngles.z - playerTransform.eulerAngles.z;
                playerRigidbody.linearVelocity = Quaternion.Euler(0, 0, angleDifference) * playerRigidbody.linearVelocity;
            }
            else
            {
                playerRigidbody.linearVelocity = Vector2.zero;
                playerRigidbody.angularVelocity = 0f;
            }
        }

        if (exitTeleportEffect != null)
        {
            exitTeleportEffect.Play();
        }
    }
}
