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

    private void Start()
    {
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
        GameManager.Instance.actualClothe = GameManager.Instance.regions[0].clothes[0];
    }
}

public enum FilterNames
{
    Hombre,
    Mujer,
    ClearAll,
    Buffs,
    Rashguard,
    Hoodies,
    Hats,
    Shorts,
    Shirts,
    Pants
}
