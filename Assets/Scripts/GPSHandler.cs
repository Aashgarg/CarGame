using UnityEngine;
using UnityEngine.AI;

public class GPSHandler : MonoBehaviour
{
    [SerializeField] private Transform car;
    public Transform destination;

    private NavMeshAgent carAgent;
    private LineRenderer lr;
    private NavMeshPath path;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carAgent = car.GetComponent<NavMeshAgent>();
        lr = GetComponent<LineRenderer>();
        path = new NavMeshPath();

        lr.sortingOrder = 5; 
        lr.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (destination == null || lr == null) return;

        // 1. Force the Z positions to 0 so the calculation happens on a flat 2D plane
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 targetPos = new Vector3(destination.position.x, destination.position.y, 0f);

        // 2. Calculate the path along your baked roads
        bool pathFound = NavMesh.CalculatePath(startPos, targetPos, NavMesh.AllAreas, path);

        // 3. Handle the rendering based on the result
        if (pathFound && path.corners.Length > 1)
        {
            // Path found successfully! Draw the winding road corners
            lr.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
            {
                // Force Z to a small negative number (like -0.1f) so it sits closer to the 2D camera than sprites
                lr.SetPosition(i, new Vector3(path.corners[i].x, path.corners[i].y, -0.1f));
            }
        }
        else
        {
            // FALLBACK: If NavMesh fails, draw a straight line so you know the script runs
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(startPos.x, startPos.y, -0.1f));
            lr.SetPosition(1, new Vector3(targetPos.x, targetPos.y, -0.1f));
            
            Debug.LogWarning("NavMesh path could not be calculated! Drawing fallback straight line. Is your NavMesh baked?");
        }
    }
}
