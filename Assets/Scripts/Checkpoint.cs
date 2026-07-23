using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private CarController player; // Reference to the CheckpointManager
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
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Checkpoint reached: " + gameObject.name);
            // You can add logic here to notify the CheckpointManager that this checkpoint has been reached.
            if (player != null)
            {
                player.CurrentCheckpoint = transform;
            }
        }
    }
}
