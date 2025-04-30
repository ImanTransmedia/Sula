using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindFirstObjectByType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Header("Default Configuration")]
    public Regions[] regions;

    [Header("Actual Region")]
    public Regions actualRegion;
    public Clothes actualClothe;
}

public enum FilterNames
{
    Hombre,
    Mujer,
    Buffs,
    Rashguard,
    Hoodies,
    Hats,
    Shorts,
    Shirts
}
