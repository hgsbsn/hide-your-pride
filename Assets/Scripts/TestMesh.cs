using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimpleMeshGenerator : MonoBehaviour
{
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.name = "My Triangle";

        // Define 3 vertices (counter-clockwise)
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),  // bottom-left
            new Vector3(1, 0, 0),  // bottom-right
            new Vector3(0.5f, 1, 0) // top-center
        };

        // Define triangle by vertex indices
        int[] triangles = new int[]
        {
            0, 1, 2
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}