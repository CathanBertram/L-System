using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ForestGenerator))]
public class ForestGeneratorEditor : Editor
{
    ForestGenerator generator;
    private void OnEnable()
    {
        generator = (ForestGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("gridCellSize"), new GUIContent("Grid Cell Size"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gridDimensions"), new GUIContent("Grid Dimensions"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("drawGrid"), new GUIContent("Draw Grid"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onlyDrawExterior"), new GUIContent("Only Draw Outside Edge"));

        if (GUILayout.Button("Create Grid"))
        {
            generator.CreateGrid();
        }

        serializedObject.ApplyModifiedProperties();
    }
}