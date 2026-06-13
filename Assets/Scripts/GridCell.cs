using UnityEngine;

// represents a single cell in the grid wheret the player can place portals during the planning stage. It can be highlighted when the player is dragging a portal to show where the portal will be placed if they release the mouse button
public class GridCell : MonoBehaviour
{
    public bool isOccupied; // whether there is currently a portal placed on this cell
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setOccupied(bool occupied)
    {
        isOccupied = occupied;
        // Optionally change the color of the cell to indicate whether it's occupied or not
    }

    //if mouse is hovering over this cell and the player is dragging a portal, highlight the cell to show where the portal will be placed if they release the mouse button
    // also tells PortalPairManager which cell the player is currently hovering over while dragging a portal so it can update everything accordingly
    void OnMouseOver()
    {
        /*
        if (Input.GetMouseButton(0) && PortalPairManager.instance.isDraggingPortal)
        {
            PortalPairManager.instance.SetCurrentHoverCell(this);
            HighlightCell();
        }*/
    }
}