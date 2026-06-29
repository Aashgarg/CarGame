using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NightData", menuName = "Scriptable Objects/NightData")]
public class NightData : ScriptableObject
{
    public int nightNumber;
    public List<DeliveryData> deliveryDatas;
    
}
