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

public class MeshPathNode
{
    public Vector3 center { get; private set; }
    public Vector3 direction { get; private set; }
    public int pathIndex { get; private set; }

    public MeshPathNode(Vector3 center, Vector3 direction, int pathIndex)
    {
        this.center = center;
        this.direction = direction;
        this.pathIndex = pathIndex;
    }
}
