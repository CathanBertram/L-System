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

    private Vector3 right = new Vector3();
    private Vector3 down = new Vector3();
    private Vector3 offset = new Vector3();
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

    private void OnDrawGizmos()
    {
        if (!drawGrid) return;
        if (grid == null) return;
        var lengthZero = grid.GetLength(0);
        var lengthOne = grid.GetLength(1);
        if (onlyDrawExterior)
        {
            //var pos = transform.position;
            //var p1 = pos;
            //var p2 = pos;
            //p2.x += gridCellSize * grid.GetLength(0);
            //var p3 = pos;
            //p3.z += gridCellSize * grid.GetLength(1);
            //var p4 = pos;
            //p4.x += gridCellSize * grid.GetLength(0);
            //p4.z += gridCellSize * grid.GetLength(1);
            var pos = transform.position;
            var p1 = pos + offset;
            var p2 = pos + right * lengthZero + offset;
            var p3 = pos + down * lengthOne + offset;
            var p4 = pos + down * lengthOne + right * lengthZero + offset;
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p1, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p2, p4);

            return;
        }
        
        for (int x = 0;x < lengthZero; x++)
        {
            for(int z = 0;z < lengthOne; z++)
            {
                var pos = transform.position;
                pos.x += x * gridCellSize;
                pos.y = grid[x, z];
                pos.z += z * gridCellSize;

                if (x + 1 < lengthZero)
                {
                    var right = pos;
                    right.x += gridCellSize;
                    right.y = grid[x + 1, z];
                    Gizmos.DrawLine(pos + offset, right + offset);
                }

                if (z + 1 < lengthOne)
                {
                    var down = pos;
                    down.y = grid[x, z + 1];
                    down.z += gridCellSize;
                    Gizmos.DrawLine(pos + offset, down + offset);
                }

            }
        }
    }
}

