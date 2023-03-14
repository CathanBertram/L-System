using UnityEngine;

[System.Serializable]
public class Rule
{
    [SerializeField] private char character;
    public char Character => character;
    [SerializeField] private string transformation;
    public string Transformation => transformation;
}
