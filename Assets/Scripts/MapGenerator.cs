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
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateShape();
    }
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
        triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = xSize + 1;
        triangles[2] = 1;
    }
    void UpdateShape()
    {

    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        for(int i =0; i<vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

}
