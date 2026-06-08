using UnityEngine;
using System.Collections.Generic;
// This script will handle the logic for placing portals in the game. It will check if the player can place a portal, and if so, it will instantiate a portal prefab at the desired location. It will also handle the logic for dragging the portal around before placing it, and snapping it to a grid for easier placement. Additionally, it will check for valid placement locations using raycasting to ensure that portals cannot be placed inside walls or other obstacles. 
public class PortalPlacementHandler : MonoBehaviour
{
    [SerializeField] private PortalManager portalManager;
    //[SerializeField] private GameManager gameManager;
    [SerializeField] float gridSize;
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] public int maxPortalPairs;
    [SerializeField] public float portalTeleportDistanceThreshold;
    [SerializeField] public GameObject startPortalPrefab;
    [SerializeField] public GameObject endPortalPrefab;

    private float currentPortalPairsPlaced;
    //public bool canPlacePortal;
    [SerializeField] List<GameObject> portalsToPlace; // List to hold the portals being placed (start and end)
    private bool isDragging;
    private bool isStartPortal; // To track whether we're placing the start or end portal
    private Vector3 mousePosition;
    private void HandlePortalPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }

    private void tryToPlacePortal()
    {
        
    }
}
