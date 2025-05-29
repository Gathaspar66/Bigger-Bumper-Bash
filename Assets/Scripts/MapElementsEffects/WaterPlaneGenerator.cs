using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterPlaneGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float quadSize = 1f;

    private void Start()
    {
        GeneratePlane();
    }

    private void GeneratePlane()
    {
        Mesh mesh = new();
        Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
        int[] triangles = new int[width * height * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int z = 0, i = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                vertices[i] = new Vector3(x * quadSize, 0, z * quadSize);
                uvs[i] = new Vector2((float)x / width, (float)z / height);
            }
        }

        for (int z = 0, i = 0, vert = 0; z < height; z++, vert++)
        {
            for (int x = 0; x < width; x++, i += 6, vert++)
            {
                triangles[i + 0] = vert;
                triangles[i + 1] = vert + width + 1;
                triangles[i + 2] = vert + 1;
                triangles[i + 3] = vert + 1;
                triangles[i + 4] = vert + width + 1;
                triangles[i + 5] = vert + width + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}