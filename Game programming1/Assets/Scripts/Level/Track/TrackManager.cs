using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public static TrackManager Instance;

    public Vector2Int startGridPos;
    public Vector2Int finishGridPos;
    public Vector2Int startDir = Vector2Int.up;

    private Dictionary<Vector2Int, TrackModule> tracks = new Dictionary<Vector2Int, TrackModule>();

    void Awake()
    {
        Instance = this;
    }

    // Determine if a cell is "occupied".
    public bool IsOccupied(Vector2Int gridPos)
    {
        return tracks.ContainsKey(gridPos);
    }
    public bool HasTrack(Vector2Int gridPos)
    {
        return tracks.ContainsKey(gridPos);
    }

    // get trackmoudle
    public TrackModule GetTrack(Vector2Int gridPos)
    {
        TrackModule trackModule;
        bool found = tracks.TryGetValue(gridPos, out trackModule);

        if (found)
            return trackModule;

        return null;
    }

    // start and finish
    public void ReserveCell(Vector2Int gridPos)
    {
        tracks[gridPos] = null;
        UpdateNeighbors(gridPos);
    }

    // place or delect
    public void AddTrack(Vector2Int gridPos, TrackModule trackModule)
    {
        tracks[gridPos] = trackModule;
        UpdateNeighbors(gridPos);
    }
    public void RemoveTrack(Vector2Int gridPos)
    {
        if (tracks.ContainsKey(gridPos) == false)
            return;

        TrackModule trackModule = GetTrack(gridPos);
        if (trackModule == null)
            return;

        tracks.Remove(gridPos);
        UpdateNeighbors(gridPos);
    }

    // neighbors walls
    void UpdateNeighbors(Vector2Int gridPos)
    {
        Vector2Int[] directions = new Vector2Int[4];
        directions[0] = Vector2Int.up;
        directions[1] = Vector2Int.down;
        directions[2] = Vector2Int.left;
        directions[3] = Vector2Int.right;

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int neighborGridPos = gridPos + directions[i];
            TrackModule neighborTrack = GetTrack(neighborGridPos);

            if (neighborTrack != null)
            {
                neighborTrack.UpdateWalls();
            }
        }

        TrackModule currentTrack = GetTrack(gridPos);
        if (currentTrack != null)
        {
            currentTrack.UpdateWalls();
        }
    }
}
