using UnityEngine;

public class StartModule : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int exitDir = Vector2Int.up;

    void Start()
    {
        if (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
        }

        if (gridManager == null)
        {
            return;
        }

        Vector2Int startGridPos = gridManager.WorldToGrid(transform.position);

        TrackManager trackManager = TrackManager.Instance;
        if (trackManager == null)
        {
            return;
        }

        trackManager.startGridPos = startGridPos;
        trackManager.startExitDir = exitDir;
        trackManager.ReserveCell(startGridPos);
    }
}
