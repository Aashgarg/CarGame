using UnityEngine;
using System.Collections.Generic;

public class GridOverlay : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    private List<GridCell> gridCells; // gets the children of the grid object
    void Awake()
    {
        gridCells = new List<GridCell>();
        foreach (Transform child in grid.transform)
        {
            GridCell cell = child.GetComponent<GridCell>();
            if (cell != null)
            {
                gridCells.Add(cell);
            }
        }
    }

    
    public void ShowGrid()
    {
        grid.SetActive(true);
    }

    public void HideGrid()
    {
        grid.SetActive(false);
    }

    // called when the player presses the clear button in the UI to restart the planning stage. Clears all occupied cells in the grid so the player can start placing portals again
    public void clearAllOccupiedCells()
    {
        foreach (GridCell cell in gridCells)
        {
            cell.isOccupied = false;
        }
    }
}
