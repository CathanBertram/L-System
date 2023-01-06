using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    [SerializeField] private string axiom;
    [SerializeField] private int iterations;
    [SerializeField] private int angle;
    [SerializeField] private float length;
    [ReadOnly][SerializeField] string current;
    public string Current => current;

    public void Generate()
    {
        current = axiom;
        for (int i = 0; i < iterations; i++)
        {
            current = Iterate(current);
        }
    }

    private string Iterate(string curr)
    {
        return "";
    }
}
[CustomEditor(typeof(LSystem))]
public class LSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myTarg = (LSystem)target;

        if (GUILayout.Button("Generate"))
        {
            myTarg.Generate();
        }
    }
}