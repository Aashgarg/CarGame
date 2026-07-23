using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "Scriptable Objects/CarData")]
public class CarData : ScriptableObject
{
    public float maxSpeed;
    public float accelerationFactor;
    public float turningFactor;
    public float driftFactor;
    public float dragFactor;
    public float brakeFactor;
    public float activeDriftFactor; 
}
