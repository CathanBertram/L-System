using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class SimpleTerrainGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 dimension;
    [SerializeField] private float verticesPerUnit;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private FastNoiseUnity fastNoise;
    [SerializeField] private EasingType terrainHeightEasingType;
    [SerializeField] private Vector2 terrainHeightRange;

    public void GenerateTerrain()
    {
        int xLength = Mathf.RoundToInt(dimension.x * verticesPerUnit);
        int yLength = Mathf.RoundToInt(dimension.y * verticesPerUnit);
        float[,] heightMap = GenerateHeightMap(xLength, yLength);

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int vertexIndex = 0;
        int triangleIndex = 0;

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                vertices.Add(new Vector3(x / verticesPerUnit, heightMap[x,y], y / verticesPerUnit));

                if (x < width - 1 && y < height - 1)
                {
                    AddTriangles(triangleIndex, triangles, vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    AddTriangles(triangleIndex, triangles, vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
    private float[,] GenerateHeightMap(int width, int height)
    {
        fastNoise.SaveSettings();
        float[,] heightMap = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heightMap[x,y] = Easing.Ease(terrainHeightEasingType, terrainHeightRange.x, terrainHeightRange.y, 0.5f * (1 + fastNoise.fastNoise.GetNoise(x, y)));
            }
        }

        return heightMap;
    }
    private void AddTriangles(int triangleIndex, List<int> triangles, int a, int b, int c)
    {
        triangles.Add(c);
        triangles.Add(b);
        triangles.Add(a);
    }
}

[CustomEditor(typeof(SimpleTerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    SimpleTerrainGenerator generator;
    private void OnEnable()
    {
        generator = (SimpleTerrainGenerator)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Terrain"))
        {
            generator.GenerateTerrain();
        }
    }
}