using System.Collections.Generic;
using UnityEngine;

public class Prefab3Manager : MonoBehaviour
{
    public static Prefab3Manager Instance;

    private Dictionary<Vector2Int, GameObject> prefab3Map = new Dictionary<Vector2Int, GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public bool HasPrefab3(Vector2Int gridPos)
    {
        return prefab3Map.ContainsKey(gridPos) && prefab3Map[gridPos] != null;
    }

    public GameObject GetPrefab3(Vector2Int gridPos)
    {
        if (prefab3Map.TryGetValue(gridPos, out var go))
            return go;
        return null;
    }

    public void AddPrefab3(Vector2Int gridPos, GameObject go)
    {
        prefab3Map[gridPos] = go;
    }

    public void RemovePrefab3(Vector2Int gridPos)
    {
        if (!prefab3Map.TryGetValue(gridPos, out var go) || go == null) return;

        Destroy(go);
        prefab3Map.Remove(gridPos);
    }
}
