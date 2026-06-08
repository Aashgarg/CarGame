using UnityEngine;
using System.Collections.Generic;
// This script will handle the logic for placing portals in the game. It will check if the player can place a portal, and if so, it will instantiate a portal prefab at the desired location. This only handles user input.  
public class PortalPlacementHandler : MonoBehaviour
{
    [SerializeField] private PortalManager portalManager;
    [SerializeField] private GameManager gameManager;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            // Update mouse position
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Set z to 0 for 2D

            // Snap to grid
            mousePosition.x = Mathf.Round(mousePosition.x / gridSize) * gridSize;
            mousePosition.y = Mathf.Round(mousePosition.y / gridSize) * gridSize;

            if (isDragging)
            {
                // Check for valid placement using raycast
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, placementLayerMask);
                if (hit.collider == null)
                {
                    // Valid placement, you can add visual feedback here (e.g., change cursor color)
                    if (Input.GetMouseButtonDown(0)) // Left click to place portal
                    {
                        placePortal();
                        isDragging = false; // Stop dragging after placing the portal
                    }
                }
                else
                {
                    // Invalid placement, you can add visual feedback here (e.g., change cursor color)
                }
            }
        }
    }

    //when clicking the button to get a portal.
    void beginPortalPlacement()
    {
        //check if player has reached max portal pairs
        if (currentPortalPairsPlaced >= maxPortalPairs)
        {
            Debug.Log("Max portal pairs reached!");
        }
        else if (gameManager.currentState != GameState.planningStage)
        {
            Debug.Log("You can only place portals during the planning stage!");
        }
        else if (isDragging)
        {
            Debug.Log("Already placing a portal!");
        }
        else
        {
            //attach start portal prefab to mouse cursor, make sure when player drags, it drags with the mouse
            isDragging = true;
            Instantiate(startPortalPrefab, mousePosition, Quaternion.identity);
        }
        
        
        
    }

    void placePortal()
    {
        
    }
}
