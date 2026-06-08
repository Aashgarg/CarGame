using UnityEngine;

public enum ZoneType
{
    Pickup,
    Dropoff
}
//
public class DeliveryZone : MonoBehaviour
{
    public ZoneType zoneType;
    public DeliveryData deliveryData;
    public bool isActive;
    [SerializeField] private DeliveryManager deliveryManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //deliveryManager = FindObjectOfType<DeliveryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (deliveryManager == null)
        {
            Debug.LogWarning("DeliveryManager not found in the scene. Please assign it in the inspector.");
            return;
        }
        deliveryData = deliveryManager.currentDelivery;
        if (deliveryManager.currentStatus == DeliveryStatus.Accepted)
        {
            if (zoneType == ZoneType.Pickup)
            {
                isActive = true;
            }
            else if (zoneType == ZoneType.Dropoff)
            {
                isActive = false;
            }
        }
        else if (deliveryManager.currentStatus == DeliveryStatus.Active)
        {
            if (zoneType == ZoneType.Pickup)
            {
                isActive = false;
            }
            else if (zoneType == ZoneType.Dropoff)
            {
                isActive = true;
            }
        }
        else
        {
            isActive = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered " + zoneType + " zone for delivery: " + deliveryData.deliveryName);
            Debug.Log("isActive: " + isActive);
            if (isActive && deliveryManager != null)
            {
                if (zoneType == ZoneType.Pickup)
                {
                    // alerts delivery manager to StartDelivery()
                    deliveryManager.StartDelivery(deliveryData);
                }
                else if (zoneType == ZoneType.Dropoff)
                {
                    // Logic for dropping off the delivery, such as updating the delivery status, rewarding the player, or updating UI
                    deliveryManager.CompleteDelivery();
                }
            }
        }
    }
}
