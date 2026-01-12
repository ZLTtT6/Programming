using UnityEngine;

public class FinishModule : MonoBehaviour
{
    public GridManager gridManager;

    void Start()
    {
        Vector2Int finishGridPos = gridManager.WorldToGrid(transform.position);

        TrackManager trackManager = TrackManager.Instance;

        trackManager.finishGridPos = finishGridPos;
        trackManager.ReserveCell(finishGridPos);
    }
}
