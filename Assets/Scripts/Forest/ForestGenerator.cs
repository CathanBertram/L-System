using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public partial class ForestGenerator : MonoBehaviour
{
    [SerializeField] private PlacementType placementType;
    [SerializeField] private float gridCellSize;
    [SerializeField] private Vector2Int gridDimensions;
    private float[,] grid;
    [SerializeField] private bool drawGrid;
    [SerializeField] private bool onlyDrawExterior;
    [SerializeField] private bool useNoiseDensity;
    [SerializeField] private bool useLSystem;
    [SerializeField] private List<LSystem> lSystems;
    [SerializeField] private List<GameObject> treeObjects;
    [SerializeField] private float objectSpacing;
    [SerializeField] private Generator lSystemGenerator;
    [SerializeField] private GameObject lSystemTemplate;
    [SerializeField] private int seed;
    [SerializeField] private bool randomizeInitialSeed;
    public bool DrawGrid => drawGrid;
    public float GridCellSize => gridCellSize;
    public Vector2Int GridDimensions => gridDimensions;
    public float[,] Grid => grid;
    public bool OnlyDrawExterior => onlyDrawExterior;

    private Vector3 right = new Vector3();
    private Vector3 down = new Vector3();
    private Vector3 offset = new Vector3();
    private System.Random random;
    public Vector3 Right => right;
    public Vector3 Down => down;
    public Vector3 Offset => offset;
    private Transform forestParent;
    public void GenerateForest()
    {
        if (randomizeInitialSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        random = new System.Random(seed);

        if (forestParent == null)
        {
            forestParent = new GameObject().transform;
            forestParent.position = transform.position;
            forestParent.rotation = transform.rotation;
            forestParent.name = "Forest";
        }
        for (int i = forestParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(forestParent.GetChild(i).gameObject);
        }

        switch (placementType)
        {
            case PlacementType.GRID:
                GenerateGridForest();
                break;
            case PlacementType.POISSON:
                if (useNoiseDensity)
                    GenerateDensityPoissonForest();
                else
                    GeneratePoissonForest();
                break;
        }
    }

    private void GenerateGridForest() 
    {
        var cellSpacing = objectSpacing / gridCellSize;

        for (float x = 0; x < gridDimensions.x; x += cellSpacing)
        {
            for (float y = 0; y < gridDimensions.y; y += cellSpacing)
            {
                PlaceObject(transform.position + new Vector3(x, 0, y));
            }
        }
    }
    private void GeneratePoissonForest() 
    { 

    }
    private void GenerateDensityPoissonForest() 
    { 
    
    }
    private void PlaceObject(Vector3 position)
    {
        if (useLSystem)
        {
            PlaceObject(lSystems[random.Next(lSystems.Count)], position);
        }
    }
    private void PlaceObject(LSystem lSystem, Vector3 position)
    {
        var mesh = lSystemGenerator.GenerateMesh(random.Next(int.MinValue, int.MaxValue),lSystem, 0.05f);
        var obj = Object.Instantiate(lSystemTemplate, position, Quaternion.identity);
        obj.GetComponent<MeshFilter>().sharedMesh = mesh;
        obj.transform.SetParent(forestParent);
    }
    private void PlaceObject(GameObject prefab, Vector3 position)
    {
        var obj = Object.Instantiate(prefab, position, Quaternion.identity);
        obj.transform.SetParent(forestParent);

    }
    public void CreateGrid()
    {
        if (grid == null)
            grid = new float[gridDimensions.x + 1, gridDimensions.y + 1];
        var zero = transform.position;
        zero.y = 10000;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int z = 0; z < grid.GetLength(1); z++)
            {
                var pos = zero;
                pos.x += x * gridCellSize;
                pos.z += z * gridCellSize;
                if (Physics.Raycast(pos, Vector3.down, out var hit, Mathf.Infinity))
                {
                    grid[x, z] = hit.point.y;
                }
                else
                {
                    grid[x, z] = transform.position.y;
                }
            }    
        }
    }

    private void Update()
    {
        if (transform.hasChanged)
            CreateGrid();
    }

    private void OnValidate()
    {
        grid = new float[gridDimensions.x + 1, gridDimensions.y + 1];
        right.x = gridCellSize;
        down.z = gridCellSize;
        offset.x  = -gridCellSize * 0.5f;
        offset.z  = -gridCellSize * 0.5f;
    }
}

