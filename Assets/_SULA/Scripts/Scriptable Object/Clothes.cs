using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Clothes", menuName = "Scriptable Objects/Clothes")]
public class Clothes : ScriptableObject
{
    public string clotheName;
    public double price;
    public Sprite imagen;
    public bool is3D;
    public Color[] materialColors;
    public GameObject prefab;
    public string description;
    
}
