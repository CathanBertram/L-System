using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : Editor
{
    Generator generator;

    bool lSystemSettingsFoldout;
    bool meshSettingsFoldout;
    bool debugSettingsFoldout;
    private void OnEnable()
    {
        generator = (Generator)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("seed"), new GUIContent("Seed"));
        if (GUILayout.Button("Randomize Seed"))
        {
            generator.RandomizeSeed();
        }

        lSystemSettingsFoldout = EditorGUILayout.Foldout(lSystemSettingsFoldout, "LSystem Settings");
        if (lSystemSettingsFoldout)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lSystem"), new GUIContent("LSystem"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("iterations"), new GUIContent("Iterations"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("iteratedSystem"), new GUIContent("Iterated LSystem"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("baseLength"), new GUIContent("Base Length"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAngleOffset"), new GUIContent("Max Angle Offset"));         
        }
        meshSettingsFoldout = EditorGUILayout.Foldout(meshSettingsFoldout, "Mesh Settings");
        if (meshSettingsFoldout)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("meshFilter"), new GUIContent("Mesh Filter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("meshResolution"), new GUIContent("Mesh Resolution"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lengthScale"), new GUIContent("Length Scale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("radius"), new GUIContent("Radius"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("endRadiusScale"), new GUIContent("End Radius Scale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleEasingType"), new GUIContent("Radius Scale Easing Type"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pathScaleModifier"), new GUIContent("Path Scale Modifier"));           
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minRotateAngle"), new GUIContent("Min Rotate Angle"));           
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxRotateAngle"), new GUIContent("Max Rotate Angle"));           
            EditorGUILayout.PropertyField(serializedObject.FindProperty("upCorrectAngle"), new GUIContent("Up Correction Angle"));           
        }
        debugSettingsFoldout = EditorGUILayout.Foldout(debugSettingsFoldout, "Debug Settings");
        if (debugSettingsFoldout)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawGizmo"), new GUIContent("Draw Gizmos"));
            if (GUILayout.Button("Generate Path"))
            {
                generator.GeneratePath();
            }
        }

        if (GUILayout.Button("Generate LSystem"))
        {
            generator.GenerateLSystem();
        }
        if (GUILayout.Button("Generate Mesh"))
        {
            generator.GenerateMesh();
        }
        

        serializedObject.ApplyModifiedProperties();
    }
}
