using UnityEngine;
using System.Collections.Generic;

//
public class PortalManager : MonoBehaviour
{
    [SerializeField] public int maxPortalPairs;
    [SerializeField] public float portalTeleportDistanceThreshold;
    [SerializeField] public GameObject startPortalPrefab;
    [SerializeField] public GameObject endPortalPrefab;

    public float currentPortalPairsPlaced;
    //public bool canPlacePortal;
    public List<GameObject> activePortals;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activePortals = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
