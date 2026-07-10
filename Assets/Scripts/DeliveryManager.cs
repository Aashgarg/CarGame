using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.Events;

public enum DeliveryStatus
{
    Inactive,
    Accepted,
    Active,
    Completed,
}
public class DeliveryManager : MonoBehaviour
{
    //[SerializeField] private NightData[] nights;
    [SerializeField] private GameManager gm;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;

    public GameObject[] PickupZones;
    public GameObject[] DropoffZones;
    private float currentTime;
    public DeliveryData currentDelivery;
    public DeliveryStatus currentStatus;
    //public UnityEvent nightFinished;

    private int deliveryNum;
    private NightData currentNight;
    private GameObject pick;
    private GameObject drop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStatus = DeliveryStatus.Inactive;
        //temp to test delivery system
        setUpNightDelivery();
        currentTime = currentDelivery.timeLimit;
        
        
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

    void setUpNightDelivery()
    {
        currentNight = gm.currentNight;
        deliveryNum = 0;
        currentDelivery = currentNight.deliveryDatas[deliveryNum];
        Debug.Log("Current Night: " + currentNight.nightNumber);
        AcceptDelivery(currentDelivery);
    }

    public void AcceptDelivery(DeliveryData delivery)
    {
        if (currentStatus == DeliveryStatus.Inactive)
        {
            currentDelivery = delivery;
            currentStatus = DeliveryStatus.Accepted;
            Debug.Log("Accepted delivery: " + currentDelivery.deliveryName);
            // Additional logic to accept the delivery, such as updating UI or setting timers
            pick = PickupZones[deliveryNum];
            pick.SetActive(true);
            drop = DropoffZones[deliveryNum];
            drop.SetActive(true);
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
            // Wait 5 seconds and accept next delivery in delivery list
            pick.SetActive(false);
            drop.SetActive(false);
            deliveryNum += 1;
            Debug.Log(deliveryNum);
            if (deliveryNum < currentNight.deliveryDatas.Count)
            {
                //Debug.Log("hi");
                currentDelivery = currentNight.deliveryDatas[deliveryNum];
                Debug.Log("Next: " + currentDelivery.deliveryName);
                currentStatus = DeliveryStatus.Inactive;
                AcceptDelivery(currentDelivery);
            }
            else
            {
                if (!gm.endOfGame)
                {
                    gm.SwitchNight();
                    currentStatus = DeliveryStatus.Inactive;
                    setUpNightDelivery();
                }
            }
            
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
