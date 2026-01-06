using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public Material lineMaterial;

    public int gridSize = 100;
    public float cellSize = 1f;
    public Color gridColor = new Color(1f, 0.7f, 0.4f, 0.2f);

    void OnRenderObject()
    {
        if (lineMaterial == null)
            return;

        if (cellSize <= 0f)
            return;

        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(Matrix4x4.identity);

        GL.Begin(GL.LINES);
        GL.Color(gridColor);

        float half = gridSize * cellSize;

        for (int x = -gridSize; x <= gridSize; x++)
        {
            float worldX = x * cellSize;
            GL.Vertex(new Vector3(worldX, 0f, -half));
            GL.Vertex(new Vector3(worldX, 0f, half));
        }

        for (int z = -gridSize; z <= gridSize; z++)
        {
            float worldZ = z * cellSize;
            GL.Vertex(new Vector3(-half, 0f, worldZ));
            GL.Vertex(new Vector3(half, 0f, worldZ));
        }

        GL.End();
        GL.PopMatrix();
    }
}
