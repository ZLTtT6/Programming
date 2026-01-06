using UnityEngine;

public class FinishModule : MonoBehaviour
{
    public GridManager gridManager;

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

        Vector2Int finishGridPos = gridManager.WorldToGrid(transform.position);

        TrackManager trackManager = TrackManager.Instance;
        if (trackManager == null)
        {
            return;
        }

        trackManager.finishGridPos = finishGridPos;
        trackManager.ReserveCell(finishGridPos);
    }
}
