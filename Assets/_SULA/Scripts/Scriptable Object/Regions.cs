using UnityEngine;

[CreateAssetMenu(fileName = "Regions", menuName = "Scriptable Objects/Regions")]
public class Regions : ScriptableObject
{
    public RegionType regionType;
    public string regionName;
    public Color darkColor;
    public Color lightColor;
    public Color accentColor;
    public Sprite imagen;
    public Clothes[] clothes;
}

public enum RegionType
{
    Amazonia,
    Andes,
    Galapagos
}
