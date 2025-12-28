using UnityEngine;

public class TrackModule : MonoBehaviour
{
    public Vector2Int gridPos;

    public GameObject wallNorth;
    public GameObject wallSouth;
    public GameObject wallEast;
    public GameObject wallWest;

    void Start()
    {
        TrackManager trackManager = TrackManager.Instance;
        if (trackManager == null)
            return;

        trackManager.AddTrack(gridPos, this);
        UpdateWalls();
    }

    public void UpdateWalls()
    {
        TrackManager trackManager = TrackManager.Instance;
        if (trackManager == null)
            return;

        bool hasNorth = trackManager.HasTrack(gridPos + Vector2Int.up);
        bool hasSouth = trackManager.HasTrack(gridPos + Vector2Int.down);
        bool hasEast = trackManager.HasTrack(gridPos + Vector2Int.right);
        bool hasWest = trackManager.HasTrack(gridPos + Vector2Int.left);

        if (wallNorth != null) wallNorth.SetActive(!hasNorth);
        if (wallSouth != null) wallSouth.SetActive(!hasSouth);
        if (wallEast != null) wallEast.SetActive(!hasEast);
        if (wallWest != null) wallWest.SetActive(!hasWest);
    }
}