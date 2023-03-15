using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class test : MonoBehaviour
{
    public MeshFilter meshFilter;
    public int resolution;
    public float scale;
    public Vector3 centre;
    public float length;
    private float radius = 1;
    public Transform t;
    public int count;
    public float scaleModifier;
    private float curScale;
    
    public List<List<Vector3>> circles;
    public void Generate()
    {
        if (t == null) t = transform;
        centre = t.position;

        circles = new List<List<Vector3>>();

        curScale = scale;

        for (int j = 0; j < count; j++)
        {
            circles.Add(new List<Vector3>());

            for (int i = 0; i < resolution; i++)
            {
                GetPoint(circles[j], (float)i / ((float)resolution), centre + (t.forward * length * j));
            }

            curScale *= scaleModifier;
        }

        GenerateMesh();
    }
    public void GenerateMesh()
    {
        if (meshFilter.sharedMesh == null) meshFilter.sharedMesh = new Mesh();

        var mesh = meshFilter.sharedMesh;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = 0; i < circles.Count; i++)
        {
            if (i == 0)
            {
                //Add all points from circle
                vertices.AddRange(circles[i]);

                //Calculate offset from first vertex
                var offset = vertices.Count - circles[i].Count;

                //Loop through each vertex for end faces
                for (int j = offset; j < circles[i].Count + offset; j++)
                {
                    //Add Connecting Triangle
                    if (j == offset)
                    {
                        triangles.Add(j);
                        triangles.Add(circles.Count + offset);
                        triangles.Add(0);
                        continue;
                    }

                    //Add Triangle For Each Vertex
                    triangles.Add(j - 1);
                    triangles.Add(j);
                    triangles.Add(0);
                }              
            }
            else
            {
                //Add each point from circle
                vertices.AddRange(circles[i]);

                //Calculate offsets for each set of vertices
                var offset = vertices.Count - circles[i].Count;
                var prevOffset = offset - circles[i - 1].Count;

                //Loop through each circle to add faces
                for (int j = 0; j < circles[i].Count - 1; j++)
                {
                    if (j == 0)
                    {
                        triangles.Add(offset + circles[i].Count - 1);
                        triangles.Add(offset);
                        triangles.Add(prevOffset + circles[i - 1].Count - 1);

                        triangles.Add(prevOffset + circles[i - 1].Count - 1);
                        triangles.Add(offset);
                        triangles.Add(prevOffset);
                    }

                    triangles.Add(j + offset);
                    triangles.Add(j + offset + 1);
                    triangles.Add(j + prevOffset);

                    triangles.Add(j + prevOffset);
                    triangles.Add(j + offset + 1);
                    triangles.Add(j + prevOffset + 1);
                }

                //If final ring, add outer face
                if (i == circles.Count - 1)
                {                    
                    for (int j = 0; j < circles[i].Count; j++)
                    {
                        //Add Connecting Triangle
                        if (j == 0)
                        {
                            triangles.Add(offset);
                            triangles.Add(circles.Count + offset);
                            triangles.Add(offset + 1);
                            continue;
                        }

                        //Add Triangle For Each Vertex
                        triangles.Add(offset);
                        triangles.Add(offset + j);
                        triangles.Add(offset + j - 1);
                    }
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices.ToArray(); 
        mesh.triangles = triangles.ToArray();
    }

    public void GetPoint(List<Vector3> p, float i, Vector3 c)
    {
        //Get Angle To Point From Centre
        var angle = i * 360f * Mathf.Deg2Rad;

        //Get Direction From Angle
        var dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);

        //Get Direction With Circle Rotation      
        var point = dir * (radius * curScale) + c;
        point = Quaternion.LookRotation(t.forward, t.up) * (point - c) + c;

        // Scale Points And Add To List
        p.Add(point);
    }

    private void OnDrawGizmos()
    {
        if (circles == null) return;

        foreach (var circle in circles)
        {
            for (int i = 1; i < circle.Count; i++)
            { 
                Debug.DrawLine(circle[i - 1], circle[i]);
            }
            //Debug.DrawLine(circle[0], circle[circle.Count - 1]);
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