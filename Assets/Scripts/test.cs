using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class test : MonoBehaviour
{

    [SerializeField] private string axiom;
    [SerializeField] private int iterations;
    [SerializeField] private int angle;
    [SerializeField] private float length;
    [SerializeField] private GameObject cube;
    [ReadOnly][SerializeField] string current;
    private Vector3 dir = Vector3.forward;
    List<Vector3> path;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate()
    {
        current = axiom;
        path = new List<Vector3>();
        Vector3 dir = Vector3.forward;
        Vector3 curPos = Vector3.zero;

        path.Add(curPos);

        foreach (var item in current)
        {
            switch (item)
            {
                case 'F':
                    //Move Forward
                    curPos += dir * length;
                    path.Add(curPos);
                    break;
                case '+':
                    //Turn Left
                    dir = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                    break; 
                case '-':
                    //Turn Right
                    dir = Quaternion.AngleAxis(-angle, Vector3.up) * dir;
                    break;
                case '&':
                    //Pitch Down
                    dir = Quaternion.AngleAxis(angle, Vector3.right) * dir;
                    break;
                case '^':
                    //Pitch Up
                    dir = Quaternion.AngleAxis(-angle, Vector3.right) * dir;
                    break;
                case '\\':
                    //Roll Left
                    dir = Quaternion.AngleAxis(-angle, Vector3.forward) * dir;
                    break;
                case '/':
                    //Roll Right
                    dir = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
                    break;
                case '|':
                    // Turn 180
                    break;
                default:
                    break;
            }
        }
        Debug.Log(dir);
        cube.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    private void OnDrawGizmos()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Debug.DrawLine(path[i - 1], path[i]);
        }
    }
}

[CustomEditor(typeof(test))]
public class testEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myTarg = (test)target;

        if (GUILayout.Button("Generate"))
        {
            myTarg.Generate();
        }
    }
}