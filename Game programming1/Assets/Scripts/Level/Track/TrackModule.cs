using UnityEngine;

public class TrackModule : MonoBehaviour
{

    public Vector2Int gridPos;
    [Header("Track Type")]
    public int trackType;

    [Header("Rotation")]
    public int rotationIndex = 0;

    public GameObject wallNorth;
    public GameObject wallSouth;
    public GameObject wallEast;
    public GameObject wallWest;

    public void UpdateWalls()
    {
        TrackManager trackManager = TrackManager.Instance;
        if (trackManager == null)
            return;

        SetWallForWorldDir(Vector2Int.up, trackManager.HasTrack(gridPos + Vector2Int.up) == false);
        SetWallForWorldDir(Vector2Int.down, trackManager.HasTrack(gridPos + Vector2Int.down) == false);
        SetWallForWorldDir(Vector2Int.right, trackManager.HasTrack(gridPos + Vector2Int.right) == false);
        SetWallForWorldDir(Vector2Int.left, trackManager.HasTrack(gridPos + Vector2Int.left) == false);
    }

    // Use "worldDir" to find the corresponding wall object
    void SetWallForWorldDir(Vector2Int worldDir, bool shouldShowWall)
    {
        GameObject wallObject = GetWallObjectForWorldDir(worldDir);
        if (wallObject != null)
        {
            wallObject.SetActive(shouldShowWall);
        }
    }

    // Determine the position of the wall after rotation
    GameObject GetWallObjectForWorldDir(Vector2Int worldDir)
    {
        int steps = rotationIndex % 4;
        if (steps < 0) steps += 4;

        int inverseSteps = (4 - steps) % 4;
        Vector2Int localDir = RotateDirCW(worldDir, inverseSteps);

        if (localDir == Vector2Int.up) return wallNorth;
        if (localDir == Vector2Int.down) return wallSouth;
        if (localDir == Vector2Int.right) return wallEast;
        if (localDir == Vector2Int.left) return wallWest;

        return null;
    }

    // Rotating
    Vector2Int RotateDirCW(Vector2Int dir, int stepsCW)
    {
        Vector2Int result = dir;

        for (int i = 0; i < stepsCW; i++)
        {
            result = new Vector2Int(result.y, -result.x);
        }

        return result;
    }
}