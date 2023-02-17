using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    float[,] heightMap;
    [SerializeField] [Range(0.01f, 0.9f)] float sharpness;
    [SerializeField] int scale;
    [SerializeField] int xSize;
    [SerializeField] int zSize;
    [SerializeField] Gradient gradient;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject[] obstacles;
    [SerializeField] int spawnIncrements;
    [SerializeField] int objectsIncrements;
    [SerializeField] int numOfSpecialEnemies;
    [SerializeField] int mapScaleMultiplier;
    SpawnManager spawnManager;

    private void Awake()
    {
        spawnManager = GetComponent<SpawnManager>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateShape();
        ColorShape();
        PlaceSpawnPoints();
       
    }
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        heightMap = new float[xSize + 1, zSize + 1];
        int i = 0;
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * sharpness, z * sharpness) * scale;
                vertices[i] = new Vector3(x, y, z);
                heightMap[x, z] = vertices[z * (xSize + 1) + x].y;
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
        GetComponent<MeshCollider>().sharedMesh = mesh;
        transform.localScale *= mapScaleMultiplier;

    }

    void PlaceSpawnPoints()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("No MeshFilter component found on object.");
            return;
        }

        meshFilter.mesh.RecalculateBounds();
        Bounds bounds = meshFilter.mesh.bounds;
        float z = bounds.min.z * mapScaleMultiplier;

        while (z < bounds.max.z * mapScaleMultiplier)
        {

            Vector3 enemySpawnposition = new Vector3(bounds.max.x * mapScaleMultiplier, heightMap[xSize, Mathf.RoundToInt(z / mapScaleMultiplier)] * mapScaleMultiplier, z);
            GameObject enemySpawn = Instantiate(spawnPoints[1], enemySpawnposition, Quaternion.identity);
            spawnManager.enemySpawnPoints.Add(enemySpawn);
            Vector3 playerSpawnPosition = new Vector3(bounds.min.x * mapScaleMultiplier, heightMap[0, Mathf.RoundToInt(z / mapScaleMultiplier)] * mapScaleMultiplier, z);
            GameObject playerSpawn = Instantiate(spawnPoints[0], playerSpawnPosition, Quaternion.identity);
            spawnManager.playerSpawnPoints.Add(playerSpawn);
            PopulateMap(playerSpawn, enemySpawn);
            z += spawnIncrements;
        }
        for (int j = 0; j < numOfSpecialEnemies; j++)
        {

        }


    }
    void PopulateMap(GameObject playerSpawn, GameObject enemySpawn)
    {
        float obstacleXpos = Random.Range(playerSpawn.transform.position.x + objectsIncrements, enemySpawn.transform.position.x - objectsIncrements);
        float obstacleZpos = playerSpawn.transform.position.z;
        float obstacleYpos = heightMap[Mathf.RoundToInt(obstacleXpos), Mathf.RoundToInt(obstacleZpos)];
        Vector3 obstacleSpawnPos = new Vector3(obstacleXpos,obstacleYpos,obstacleZpos);
        Instantiate(obstacles[Random.Range(0, 3)], obstacleSpawnPos, Quaternion.identity); 
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
