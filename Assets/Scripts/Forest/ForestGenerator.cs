using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class ForestGenerator : MonoBehaviour
{
    [SerializeField] private float gridCellSize;
    [SerializeField] private Vector2Int gridDimensions;
    private float[,] grid;
    [SerializeField] private bool drawGrid;
    [SerializeField] private bool onlyDrawExterior;

    public bool DrawGrid => drawGrid;
    public float GridCellSize => gridCellSize;
    public Vector2Int GridDimensions => gridDimensions;
    public float[,] Grid => grid;
    public bool OnlyDrawExterior => onlyDrawExterior;

    private Vector3 right = new Vector3();
    private Vector3 down = new Vector3();
    private Vector3 offset = new Vector3();

    public Vector3 Right => right;
    public Vector3 Down => down;
    public Vector3 Offset => offset;
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

