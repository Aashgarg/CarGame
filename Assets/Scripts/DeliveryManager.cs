using UnityEngine;
using System.Collections.Generic;

public enum DeliveryStatus
{
    Inactive,
    Accepted,
    Active,
    Completed,
}
public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private DeliveryData[] deliveries;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    private float currentTime;
    public DeliveryData currentDelivery;
    public DeliveryStatus currentStatus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStatus = DeliveryStatus.Inactive;
        currentTime = deliveries[0].timeLimit;
        //temp to test delivery system
        AcceptDelivery(deliveries[0]);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStatus == DeliveryStatus.Active)
        {
            // Update the time text
            timeText.gameObject.SetActive(true);
            timeText.text = "Time: " + Mathf.Max(0, currentTime).ToString("F1");
            // Decrease the time limit
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                FailDelivery();
            }
        }
        if (currentStatus == DeliveryStatus.Inactive)
        {
            timeText.gameObject.SetActive(false);
        }
        if (currentStatus == DeliveryStatus.Completed)
        {
            timeText.gameObject.SetActive(false);
        }
        if (currentStatus == DeliveryStatus.Accepted)
        {
            timeText.gameObject.SetActive(false);
        }
    }

    public void AcceptDelivery(DeliveryData delivery)
    {
        if (currentStatus == DeliveryStatus.Inactive)
        {
            currentDelivery = delivery;
            currentStatus = DeliveryStatus.Accepted;
            Debug.Log("Accepted delivery: " + currentDelivery.deliveryName);
            // Additional logic to accept the delivery, such as updating UI or setting timers
        }
    }

    public void StartDelivery(DeliveryData delivery)
    {
        currentDelivery = delivery;
        currentStatus = DeliveryStatus.Active;
        Debug.Log("Starting delivery: " + currentDelivery.deliveryName);
        // Additional logic to start the delivery, such as setting timers or updating UI
    }

    public void CompleteDelivery()
    {
        if (currentStatus == DeliveryStatus.Active)
        {
            currentStatus = DeliveryStatus.Completed;
            Debug.Log("Completed delivery: " + currentDelivery.deliveryName);
            // Additional logic to complete the delivery, such as rewarding the player or updating UI
        }
    }

    public void FailDelivery()
    {
        if (currentStatus == DeliveryStatus.Active)
        {
            currentStatus = DeliveryStatus.Inactive;
            Debug.Log("Failed delivery: " + currentDelivery.deliveryName);
            currentTime = currentDelivery.timeLimit;
            // Additional logic to handle delivery failure, such as penalizing the player or updating UI
        }
    }
}
