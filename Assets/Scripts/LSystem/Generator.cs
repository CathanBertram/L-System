using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;
using UnityEditor.ShaderGraph.Internal;
using System.Runtime.CompilerServices;

public class Generator : MonoBehaviour
{
    [SerializeField] private LSystem lSystem;
    private Dictionary<char, string> ruleDictionary;
    [SerializeField] private int iterations;
    [SerializeField] private string iteratedSystem;
    [SerializeField] private float baseLength;
    private float baseAngle;
    private char[] ignoreCharacters;

    public void InitDictionary()
    {
        ruleDictionary= new Dictionary<char, string>();
        ignoreCharacters = lSystem.ignoreChars;
        if (ignoreCharacters == null) ignoreCharacters = new char[1];

        foreach (var item in lSystem.rules)
        {
            ruleDictionary.Add(item.Character, item.Transformation);
        }
    }
    public void Iterate()
    {
        if (ruleDictionary == null) InitDictionary();

        string temp = "";

        foreach (var item in iteratedSystem)
        {
            if (ruleDictionary.ContainsKey(item))
            {
                var s = ruleDictionary[item];
                if (s.Contains(','))
                {
                    string[] strings = s.Split(',');
                    temp += strings[Random.Range(0, strings.Count())];
                }
                else
                    temp += ruleDictionary[item];
            }
            else
                temp += item;
        }

        iteratedSystem = temp;
    }

    public void GenerateLSystem()
    {
        iteratedSystem = lSystem.axiom;
        baseAngle = lSystem.angle;
        InitDictionary();
        for (int i = 0; i < iterations; i++)
        {
            Iterate();
        }
    }
    [SerializeField] private int meshResolution;
    [SerializeField] private float length;
    [SerializeField] private float scale;
    [SerializeField] private float radius;
    [SerializeField] private float scaleModifier;
    [SerializeField] private float pathScaleModifier;
    [SerializeField] private MeshFilter meshFilter;
    private float curScale;
    public void GenerateMesh()
    {
        GeneratePath();

        var meshPaths = new List<List<Circle>>();
        GetCircles(meshPaths);       

        if (meshFilter.sharedMesh == null) meshFilter.sharedMesh = new Mesh();

        var mesh = meshFilter.sharedMesh;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        //Add all points from all circles
        foreach (var path in meshPaths)
        {
            foreach (var item in path)
            {
                vertices.AddRange(item.points);

                //Give vertex offset to each circle
                item.offset = vertices.Count - item.points.Count; 
            }
        }

        // Add Faces For Everything
        for (int i = 0; i < meshPaths.Count; i++)
        {
            for (int j = 0; j < meshPaths[i].Count; j++)
            {
                if (j == 0)
                {
                    //Temporarily store circle variable
                    var circle = meshPaths[i][j];

                    //Loop through each point in circle to create end face
                    for (int k = circle.offset + 1; k < circle.points.Count + circle.offset; k++)
                    {
                        //Add Triangle For Each Vertex
                        triangles.Add(k - 1);
                        triangles.Add(k);
                        triangles.Add(circle.offset);
                    }
                }
                else
                {
                    var circle = meshPaths[i][j];
                    var prevCircle = meshPaths[i][j - 1];
                    var offset = circle.offset;
                    var prevOffset = prevCircle.offset;
                    var curCount = circle.points.Count;
                    var prevCount = prevCircle.points.Count;

                    //Loop through each circle to add faces
                    for (int k = 0; k < curCount - 1; k++)
                    {
                        if (k == 0)
                        {
                            triangles.Add(offset + curCount - 1);
                            triangles.Add(offset);
                            triangles.Add(prevOffset + prevCount - 1);

                            triangles.Add(prevOffset + prevCount - 1);
                            triangles.Add(offset);
                            triangles.Add(prevOffset);
                        }

                        triangles.Add(k + offset);
                        triangles.Add(k + offset + 1);
                        triangles.Add(k + prevOffset);

                        triangles.Add(k + prevOffset);
                        triangles.Add(k + offset + 1);
                        triangles.Add(k + prevOffset + 1);
                    }

                    //If final ring, add outer face
                    if (j == meshPaths[i].Count - 1)
                    {
                        for (int k = offset + 1; k < curCount+ offset; k++)
                        {

                            //Add Triangle For Each Vertex
                            triangles.Add(offset);
                            triangles.Add(k);
                            triangles.Add(k - 1);
                        }
                    }

                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }
    private void GetCircles(List<List<Circle>> meshPaths)
    {
        curScale = scale;

        for (int k = 0; k < paths.Count; k++)
        {
            meshPaths.Add(new List<Circle>());            
            for (int j = 0; j < paths[k].Count; j++)
            {
                meshPaths[k].Add(new Circle());
                meshPaths[k][j].points = new List<Vector3>();
                for (int i = 0; i < meshResolution; i++)
                {
                    GetPoint(meshPaths[k][j].points, (float)i / ((float)meshResolution), paths[k][j]);
                }
                meshPaths[k][j].direction = paths[k][j].direction;
                meshPaths[k][j].center = paths[k][j].point;
                curScale *= scaleModifier;
            }
        }

    }
    public void GetPoint(List<Vector3> p, float i, PathNode node)
    {
        //Get Angle To Point From Centre
        var angle = i * 360f * Mathf.Deg2Rad;

        //Get Direction From Angle
        var dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);

        //Get Direction With Circle Rotation      
        var point = dir * (radius * curScale) + node.point;
        point = Quaternion.LookRotation(node.direction, Vector3.up) * (point - node.point) + node.point;

        // Scale Points And Add To List
        p.Add(point);
    }

    public bool drawGizmo;
    private List<List<PathNode>> paths;
    //Lastinfirstout collection
    private Stack<PathState> previousPath;
    public void GeneratePath()
    {
        GenerateLSystem();

        paths = new List<List<PathNode>>();
        paths.Add(new List<PathNode>());

        int curPath = 0;
        Vector3 dir = Vector3.up;
        Vector3 curPos = Vector3.zero;

        previousPath = new Stack<PathState>();

        paths[curPath].Add(new PathNode(curPos, dir, curPath));

        foreach (var item in iteratedSystem)
        {
            switch (item)
            {
                case 'F':
                    //Move Forward
                    curPos += dir * baseLength;
                    paths[curPath].Add(new PathNode(curPos, dir, curPath));
                    break;
                case '+':
                    //Turn Left
                    dir = Quaternion.AngleAxis(baseAngle, Vector3.left) * dir;
                    break;
                case '-':
                    //Turn Right
                    dir = Quaternion.AngleAxis(-baseAngle, Vector3.left) * dir;
                    break;
                case '&':
                    //Pitch Down
                    dir = Quaternion.AngleAxis(baseAngle, Vector3.down) * dir;
                    break;
                case '^':
                    //Pitch Up
                    dir = Quaternion.AngleAxis(-baseAngle, Vector3.down) * dir;
                    break;
                case '\\':
                    //Roll Left
                    dir = Quaternion.AngleAxis(-baseAngle, Vector3.forward) * dir;
                    break;
                case '/':
                    //Roll Right
                    dir = Quaternion.AngleAxis(baseAngle, Vector3.forward) * dir;
                    break;
                case '|':
                    // Turn 180
                    dir = Quaternion.AngleAxis(180, Vector3.forward) * dir;
                    break;
                case '[':
                    // Start Path
                    PathState state = new PathState(curPath, new PathNode(curPos, dir, curPath));
                    previousPath.Push(state);
                    paths.Add(new List<PathNode>());
                    curPath = paths.Count - 1;
                    paths[curPath].Add(new PathNode(curPos, dir, curPath));
                    break;
                case ']':
                    // End Path
                    PathState s = previousPath.Pop();
                    curPath = s.Index;
                    dir = s.pathNode.direction;
                    curPos = s.pathNode.point;
                    break;
                default:
                    if (ignoreCharacters.Contains(item)) break;

                    curPos += dir * baseLength;
                    paths[curPath].Add(new PathNode(curPos, dir, curPath));
                    break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        if (paths == null) return;

        foreach (var path in paths)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Debug.DrawLine(path[i - 1].point, path[i].point);
            }
        }
    }
}

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : Editor
{
    Generator generator;
    private void OnEnable()
    {
        generator = (Generator)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Generate LSystem"))
        {
            generator.GenerateLSystem();
        }
        if (GUILayout.Button("Generate Path"))
        {
            generator.GeneratePath();
        }
        if (GUILayout.Button("Generate Mesh"))
        {
            generator.GenerateMesh();
        }
    }
}
