using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathState
{
    public int Index { get; private set; }
    public PathNode pathNode { get; private set; }
    public PathState(int index, PathNode node)
    {
        Index = index;
        pathNode = node;
    }
}

public class MeshPathState
{
    public int PathIndex { get; private set; }
    public MeshPathNode MeshPathNode { get; private set; }
    public float Scale { get; private set; }
    public float LengthScale { get; private set; }
   public int Index { get; private set; }

    public MeshPathState(int pathIndex, MeshPathNode node, float scale, float lengthScale, int index)
    {
        PathIndex = pathIndex;
        MeshPathNode = node;
        Scale = scale;
        LengthScale = lengthScale;
        Index = index;  
    }
}
