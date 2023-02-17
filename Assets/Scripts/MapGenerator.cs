using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    [SerializeField] int xSize;
    [SerializeField] int zSize;
    [SerializeField] Gradient gradient;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateShape();
        ColorShape();
    }
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
        triangles = new int[xSize * zSize * 6];

        int verts = 0;
        int tris = 0;
        for (int y = 0; y < zSize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[tris + 0] = verts + 0;
                triangles[tris + 1] = verts + xSize + 1;
                triangles[tris + 2] = verts + 1;
                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = verts + xSize + 1;
                triangles[tris + 5] = verts + xSize + 2;
                verts++;
                tris += 6;
            }
            verts++;
        }


    }
    void ColorShape()
    {
        // Get the mesh and its vertices
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // Create an array to hold the vertex colors
        Color[] colors = new Color[vertices.Length];

        // Create a gradient

        // Loop through each vertex and set its color based on the gradient
        for (int i = 0; i < vertices.Length; i++)
        {
            //float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);

            float height = vertices[i].y;
            colors[i] = gradient.Evaluate(height);

        }

        // Set the vertex colors for the mesh
        mesh.colors = colors;

        
    }
    void UpdateShape()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
    }

    /*private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        for(int i =0; i<vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }*/

}
