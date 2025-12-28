using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 5000;
    public int gridHeight = 5000;
    public float cellSize = 1f;

    public Vector3 originPosition = Vector3.zero;

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        Vector3 offset = worldPosition - originPosition;

        int x = Mathf.FloorToInt(offset.x / cellSize);
        int z = Mathf.FloorToInt(offset.z / cellSize);

        return new Vector2Int(x, z);
    }

    public Vector3 GridToWorld(int x, int z)
    {
        float worldX = (x + 0.5f) * cellSize;
        float worldZ = (z + 0.5f) * cellSize;

        return originPosition + new Vector3(worldX, 0f, worldZ);
    }

    public bool IsValidGridPosition(int x, int z)
    {
        if (x < 0 || z < 0)
            return false;

        if (x >= gridWidth || z >= gridHeight)
            return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        if (cellSize <= 0f)
            return;

        Gizmos.color = Color.white;

        float widthWorld = gridWidth * cellSize;
        float heightWorld = gridHeight * cellSize;

        for (int x = 0; x <= gridWidth; x++)
        {
            float worldX = x * cellSize;
            Vector3 start = originPosition + new Vector3(worldX, 0f, 0f);
            Vector3 end = originPosition + new Vector3(worldX, 0f, heightWorld);
            Gizmos.DrawLine(start, end);
        }

        for (int z = 0; z <= gridHeight; z++)
        {
            float worldZ = z * cellSize;
            Vector3 start = originPosition + new Vector3(0f, 0f, worldZ);
            Vector3 end = originPosition + new Vector3(widthWorld, 0f, worldZ);
            Gizmos.DrawLine(start, end);
        }
    }
}



