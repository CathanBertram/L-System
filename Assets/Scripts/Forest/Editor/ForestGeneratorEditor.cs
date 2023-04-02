using log4net.Util;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

[CustomEditor(typeof(ForestGenerator))]
public class ForestGeneratorEditor : Editor
{
    ForestGenerator generator;

    bool gridSettingsFoldout;

    private void OnEnable()
    {
        generator = (ForestGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        gridSettingsFoldout = EditorGUILayout.Foldout(gridSettingsFoldout, "Grid Settings");
        if (gridSettingsFoldout ) 
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gridCellSize"), new GUIContent("Grid Cell Size"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gridDimensions"), new GUIContent("Grid Dimensions"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawGrid"), new GUIContent("Draw Grid"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onlyDrawExterior"), new GUIContent("Only Draw Outside Edge"));
        }

        if (GUILayout.Button("Create Grid"))
        {
            generator.CreateGrid();
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("seed"), new GUIContent("Seed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("randomizeInitialSeed"), new GUIContent("Randomize Initial Seed?"));
        var useLSystem = serializedObject.FindProperty("useLSystem");
        EditorGUILayout.PropertyField(useLSystem, new GUIContent("Generate Using LSystems?"));

        if (useLSystem.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lSystems"), new GUIContent("LSystems"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lSystemGenerator"), new GUIContent("LSystem Generator"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lSystemTemplate"), new GUIContent("LSystem Template"));
        }
        else
            EditorGUILayout.PropertyField(serializedObject.FindProperty("treeObjects"), new GUIContent("Trees"));

        var placementType = serializedObject.FindProperty("placementType");
        EditorGUILayout.PropertyField(placementType, new GUIContent("Forest Generation Type"));

        switch (placementType.enumValueFlag)
        {
            case 0:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("objectSpacing"), new GUIContent("Object Spacing"));
                break;
            case 1:
                var useNoiseDensity = serializedObject.FindProperty("useNoiseDensity");
                EditorGUILayout.PropertyField(useNoiseDensity, new GUIContent("Use Noise For Density"));

                if (useNoiseDensity.boolValue)
                {
                }
                break;
            default:
                break;
        }

        
        if (GUILayout.Button("Generate Forest"))
        {
            generator.GenerateForest();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        if (!generator.DrawGrid) return;
        if (generator.Grid == null) return;
        var lengthZero = generator.Grid.GetLength(0);
        var lengthOne = generator.Grid.GetLength(1);
        var pos = generator.transform.position;
        if (generator.OnlyDrawExterior)
        {
            var p1 = pos + generator.Offset;
            var p2 = pos + generator.Right * lengthZero + generator.Offset;
            var p3 = pos + generator.Down * lengthOne + generator.Offset;
            var p4 = pos + generator.Down * lengthOne + generator.Right * lengthZero + generator.Offset;
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p1, p3);
            Handles.DrawLine(p3, p4);
            Handles.DrawLine(p2, p4);

            return;
        }

        for (int x = 0; x < lengthZero; x++)
        {
            Vector3[] yPositions = new Vector3[lengthOne];
            for (int z = 0; z < lengthOne; z++)
            {
                var p = pos;
                p.x += x * generator.GridCellSize;
                p.y = generator.Grid[x, z];
                p.z += z * generator.GridCellSize;
                yPositions[z] = p + generator.Offset;
            }
            Handles.DrawPolyLine(yPositions);
        }
        
        for (int z = 0; z < lengthOne; z++)
        {
            Vector3[] xPositions = new Vector3[lengthZero];
            for (int x = 0; x < lengthZero; x++)
            {
                var p = pos;
                p.x += x * generator.GridCellSize;
                p.y = generator.Grid[x, z];
                p.z += z * generator.GridCellSize;
                xPositions[x] = p + generator.Offset;
            }
            Handles.DrawPolyLine(xPositions);
        }

        //for (int x = 0; x < lengthZero; x++)
        //{
        //    for (int z = 0; z < lengthOne; z++)
        //    {
        //        var pos = generator.transform.position;
        //        pos.x += x * generator.GridCellSize;
        //        pos.y = generator.Grid[x, z];
        //        pos.z += z * generator.GridCellSize;

        //        if (x + 1 < lengthZero)
        //        {
        //            var right = pos;
        //            right.x += generator.GridCellSize;
        //            right.y = generator.Grid[x + 1, z];
        //            Gizmos.DrawLine(pos + generator.Offset, right + generator.Offset);
        //        }

        //        if (z + 1 < lengthOne)
        //        {
        //            var down = pos;
        //            down.y = generator.Grid[x, z + 1];
        //            down.z += generator.GridCellSize;
        //            Gizmos.DrawLine(pos + generator.Offset, down + generator.Offset);
        //        }

        //    }
        //}
    }
}