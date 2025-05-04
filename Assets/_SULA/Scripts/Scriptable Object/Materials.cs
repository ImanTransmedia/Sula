using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Materials", menuName = "Scriptable Objects/Materials")]
public class Materials : ScriptableObject
{

    public string materialName;
    public string materialDescription;
    public Sprite image;


}
