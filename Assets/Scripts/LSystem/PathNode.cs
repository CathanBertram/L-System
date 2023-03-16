using UnityEngine;

public class PathNode
{
    public Vector3 point { get; private set; }
    public Vector3 direction { get; private set; }
    public int pathIndex { get; private set; }
    
    public PathNode(Vector3 p, Vector3 d, int pI)
    {
        point = p; direction = d; pathIndex = pI;
    }
}
