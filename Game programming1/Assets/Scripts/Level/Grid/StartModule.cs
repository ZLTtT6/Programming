using UnityEngine;

public class StartModule : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int exitDir = Vector2Int.up;

    void Start()
    {
        Vector2Int startGridPos = gridManager.WorldToGrid(transform.position);

        TrackManager trackManager = TrackManager.Instance;

        // Record information from the starting grid in the game.
        trackManager.startGridPos = startGridPos;
        trackManager.startDir = exitDir;
        trackManager.ReserveCell(startGridPos);
    }
}