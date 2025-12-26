using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public static TrackManager Instance;

    private Dictionary<Vector2Int, TrackModule> tracks = new();

    void Awake()
    {
        Instance = this;
    }

    public void AddTrack(Vector2Int pos, TrackModule track)
    {
        tracks[pos] = track;
        UpdateNeighbors(pos);
    }

    public bool HasTrack(Vector2Int pos)
    {
        return tracks.ContainsKey(pos);
    }

    public TrackModule GetTrack(Vector2Int pos)
    {
        return tracks.TryGetValue(pos, out var t) ? t : null;
    }

    void UpdateNeighbors(Vector2Int pos)
    {
        Vector2Int[] dirs =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in dirs)
        {
            var neighborPos = pos + dir;
            if (HasTrack(neighborPos))
            {
                GetTrack(neighborPos).UpdateWalls();
            }
        }

        GetTrack(pos).UpdateWalls();
    }
}
