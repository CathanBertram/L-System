using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathState
{
    public int Index { get; private set; }
    public Vector3 Direction { get; private set; }
    public Vector3 Position { get; private set;}

    public PathState(int index, Vector3 direction, Vector3 position)
    {
        Index = index;
        Direction = direction;
        Position = position;
    }
}
