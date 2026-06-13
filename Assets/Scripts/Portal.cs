using UnityEngine;
using System.Collections;
using TMPro; // for TextMeshProUGUI

public class Portal : MonoBehaviour
{
    [SerializeField] private PortalPair portalPair; // reference to the ScriptableObject that holds the paired portal info
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform destinationTransform;
    [SerializeField] private Portal destinationPortal;
    [SerializeField] private bool keepVelocity;
    [SerializeField] private ParticleSystem entryTeleportEffect;
    [SerializeField] private ParticleSystem exitTeleportEffect;

    [SerializeField] private float teleportDelay = 0.1f; // if teleported wait for this time before using the destination portal to teleport again
    [SerializeField] private TextMeshProUGUI distanceText; // text that shows the distance between start and end portals when dragging

    private bool isStartPortal; // whether this portal is the start or end of the pair
    private bool isPlaced; // whether this portal has been placed in the scene
    private bool isDragging; // whether the player is currently dragging this portal to place it
    private LineRenderer distanceLine; // line that shows the distance between start and end portals when dragging
    private bool canTeleport = true;
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
            if (canTeleport)
            {
                TeleportPlayer();
            }
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
        canTeleport = false;

        // prevent the destination portal from immediately sending the player back
        if (destinationPortal != null)
        {
            destinationPortal.canTeleport = false;
            destinationPortal.StartCoroutine(destinationPortal.ResetTeleport());
        }

        // nudge the player slightly forward from the destination so they don't remain inside the trigger
        playerTransform.position = playerTransform.position + (Vector3)(destinationTransform.up * 0.5f);

        StartCoroutine(ResetTeleport());
    }
    
    public IEnumerator ResetTeleport()
    {
        yield return new WaitForSeconds(teleportDelay);
        canTeleport = true;
    }
}
