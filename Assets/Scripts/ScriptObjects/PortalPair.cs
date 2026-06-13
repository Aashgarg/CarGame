using UnityEngine;

[CreateAssetMenu(fileName = "PortalPair", menuName = "Scriptable Objects/PortalPair")]
public class PortalPair : ScriptableObject
{
    public GameObject startPortalPrefab;
    public GameObject endPortalPrefab;
    public bool startPlaced;
    public bool endPlaced;
    public bool isComplete; //both portals have been placed
}
