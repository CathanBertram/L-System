using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LSystem", menuName = "LSystem/Create LSystem", order = 1)]
public class LSystem : ScriptableObject
{
    public string axiom;
    public float angle;
    public List<Rule> rules;
    public char[] ignoreChars;
}
