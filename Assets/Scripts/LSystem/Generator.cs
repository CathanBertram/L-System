using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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
    public void GenerateMesh()
    {

    }


    public bool drawGizmo;
    private List<List<Vector3>> paths;
    //Lastinfirstout collection
    private Stack<PathState> previousPath;
    public void GeneratePath()
    {
        GenerateLSystem();

        paths = new List<List<Vector3>>();
        paths.Add(new List<Vector3>());

        int curPath = 0;
        Vector3 dir = Vector3.forward;
        Vector3 curPos = Vector3.zero;

        previousPath= new Stack<PathState>();

        paths[curPath].Add(curPos);;

        foreach (var item in iteratedSystem)
        {
            switch (item)
            {
                case 'F':
                    //Move Forward
                    curPos += dir * baseLength;
                    paths[curPath].Add(curPos);
                    break;
                case '+':
                    //Turn Left
                    dir = Quaternion.AngleAxis(baseAngle, Vector3.up) * dir;
                    break;
                case '-':
                    //Turn Right
                    dir = Quaternion.AngleAxis(-baseAngle, Vector3.up) * dir;
                    break;
                case '&':
                    //Pitch Down
                    dir = Quaternion.AngleAxis(baseAngle, Vector3.right) * dir;
                    break;
                case '^':
                    //Pitch Up
                    dir = Quaternion.AngleAxis(-baseAngle, Vector3.right) * dir;
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
                    PathState state = new PathState(curPath, dir, curPos);
                    previousPath.Push(state);
                    paths.Add(new List<Vector3>());
                    curPath = paths.Count - 1;
                    paths[curPath].Add(curPos);
                    break;
                case ']':
                    // End Path
                    PathState s = previousPath.Pop();
                    curPath = s.Index;
                    dir = s.Direction;
                    curPos = s.Position;
                    break;
                default:
                    if (ignoreCharacters.Contains(item)) break;

                    curPos += dir * baseLength;
                    paths[curPath].Add(curPos);
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
                Debug.DrawLine(path[i - 1], path[i]);
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
    }
}
