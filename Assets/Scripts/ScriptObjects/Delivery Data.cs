using UnityEngine;

[CreateAssetMenu(fileName = "DeliveryData", menuName = "Scriptable Objects/DeliveryData")]
public class DeliveryData : ScriptableObject
{
    public enum DeliveryType
    {
        Food,
        Package,
        Passenger
    }
    public int deliveryID;
    public string deliveryName;
    public Vector2 destination;
    public Vector2 pickupLocation;
    public float timeLimit; // Time limit in seconds
    public float rewardMoney;
    public float cargoHealth;
}
