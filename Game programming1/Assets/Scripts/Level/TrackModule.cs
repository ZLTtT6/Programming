using UnityEngine;

public class TrackModule : MonoBehaviour
{
    public Vector2Int gridPos;

    [Header("Rotation (0=0°,1=90°,2=180°,3=270°)")]
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

    void SetWallForWorldDir(Vector2Int worldDir, bool shouldShowWall)
    {
        GameObject wallObject = GetWallObjectForWorldDir(worldDir);
        if (wallObject != null)
        {
            wallObject.SetActive(shouldShowWall);
        }
    }

    GameObject GetWallObjectForWorldDir(Vector2Int worldDir)
    {
        int steps = rotationIndex % 4;
        if (steps < 0) steps += 4;

        // 反向旋转：世界方向 -> 预制体本地方向
        int inverseSteps = (4 - steps) % 4;
        Vector2Int localDir = RotateDirCW(worldDir, inverseSteps);

        if (localDir == Vector2Int.up) return wallNorth;
        if (localDir == Vector2Int.down) return wallSouth;
        if (localDir == Vector2Int.right) return wallEast;
        if (localDir == Vector2Int.left) return wallWest;

        return null;
    }

    Vector2Int RotateDirCW(Vector2Int dir, int stepsCW)
    {
        Vector2Int result = dir;

        for (int i = 0; i < stepsCW; i++)
        {
            // (x,y) -> (y,-x)  顺时针 90°
            result = new Vector2Int(result.y, -result.x);
        }

        return result;
    }
}