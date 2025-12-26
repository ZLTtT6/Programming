using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 20;
    public int gridHeight = 20;
    public float cellSize = 1f;

    public Vector3 originPosition = Vector3.zero;

    // 世界坐标 → 网格坐标
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        int z = Mathf.FloorToInt((worldPosition.z - originPosition.z) / cellSize);
        return new Vector2Int(x, z);
    }

    // 网格坐标 → 世界坐标（格子中心）
    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(
            x * cellSize + cellSize / 2f,
            0f,
            z * cellSize + cellSize / 2f
        ) + originPosition;
    }

    // 判断是否在网格内
    public bool IsValidGridPosition(int x, int z)
    {
        return x >= 0 && z >= 0 && x < gridWidth && z < gridHeight;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = originPosition + new Vector3(x * cellSize, 0, 0);
            Vector3 end = originPosition + new Vector3(x * cellSize, 0, gridHeight * cellSize);
            Gizmos.DrawLine(start, end);
        }

        for (int z = 0; z <= gridHeight; z++)
        {
            Vector3 start = originPosition + new Vector3(0, 0, z * cellSize);
            Vector3 end = originPosition + new Vector3(gridWidth * cellSize, 0, z * cellSize);
            Gizmos.DrawLine(start, end);
        }
    }
}



