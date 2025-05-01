using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Clothes", menuName = "Scriptable Objects/Clothes")]
public class Clothes : ScriptableObject
{
    [Header("")]
    public string clotheName;
    public bool is3D;

    [Header("Image")]
    public Sprite menuImage;
    public Sprite[] imagenes;


    public Color[] materialColors;
    public GameObject prefab;

    [Header("descriptions & filter")]

    [TextArea]public string description;
    [TextArea]public string ahorro;
    public MaterialType materialType;
    public FilterNames[] filterOptions;

}

public enum MaterialType
{
    Cotton,
    Linen,
    Polyester,
    Polyamide
}
