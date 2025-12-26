using UnityEngine;

public class RuntimeGrid : MonoBehaviour
{
    public Material lineMaterial;

    public int gridSize = 100;
    public float cellSize = 1f;
    public Color gridColor = new Color(1f, 0.7f, 0.4f, 0.2f);

    void OnRenderObject()
    {
        if (lineMaterial == null) return;

        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(Matrix4x4.identity);
        GL.Begin(GL.LINES);
        GL.Color(gridColor);

        for (int x = -gridSize; x <= gridSize; x++)
        {
            GL.Vertex(new Vector3(x * cellSize, 0, -gridSize * cellSize));
            GL.Vertex(new Vector3(x * cellSize, 0, gridSize * cellSize));
        }

        for (int z = -gridSize; z <= gridSize; z++)
        {
            GL.Vertex(new Vector3(-gridSize * cellSize, 0, z * cellSize));
            GL.Vertex(new Vector3(gridSize * cellSize, 0, z * cellSize));
        }

        GL.End();
        GL.PopMatrix();
    }
}
