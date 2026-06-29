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
    private DeliveryData deliveryData;
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
                    //wait 2 seconds to make sure that the player has stopped on the location
                    // alerts delivery manager to StartDelivery()
                    deliveryManager.StartDelivery(deliveryData);
                }
                else if (zoneType == ZoneType.Dropoff)
                {
                    //wait 2 seconds to make sure that the player has stopped on the location
                    // alerts delivery manager to StartDelivery()
                    deliveryManager.CompleteDelivery();
                }
            }
        }
    }
}
