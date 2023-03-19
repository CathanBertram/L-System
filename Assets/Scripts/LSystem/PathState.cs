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
    public int Index { get; private set; }
    public MeshPathNode MeshPathNode { get; private set; }
    public float Scale { get; private set; }
    public float LengthScale { get; private set; }

    public MeshPathState(int index, MeshPathNode node, float scale, float lengthScale)
    {
        Index = index;
        MeshPathNode = node;
        Scale = scale;
        LengthScale = lengthScale;
    }
}
