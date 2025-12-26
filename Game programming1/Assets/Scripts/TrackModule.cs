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
        TrackManager.Instance.AddTrack(gridPos, this);
    }

    public void UpdateWalls()
    {
        wallNorth.SetActive(!TrackManager.Instance.HasTrack(gridPos + Vector2Int.up));
        wallSouth.SetActive(!TrackManager.Instance.HasTrack(gridPos + Vector2Int.down));
        wallEast.SetActive(!TrackManager.Instance.HasTrack(gridPos + Vector2Int.right));
        wallWest.SetActive(!TrackManager.Instance.HasTrack(gridPos + Vector2Int.left));
    }
}