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
