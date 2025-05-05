using UnityEngine;

public enum DragZoneType { None, Left, Right, Top, Bottom, Center }

public class PointerZoneManager : MonoBehaviour
{
    public static PointerZoneManager Instance { get; private set; }

    public float edgeThreshold = 50f;     
    public float topThreshold = 50f;      
    public float bottomThreshold = 50f;   

    private DragZoneType currentZone = DragZoneType.None;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void EvaluatePointerZone(Vector2 position)
    {
        if (position.y <= topThreshold)
            currentZone = DragZoneType.Top;
        else if (position.y >= Screen.height - bottomThreshold)
            currentZone = DragZoneType.Bottom;
        else if (position.x <= edgeThreshold)
            currentZone = DragZoneType.Left;
        else if (position.x >= Screen.width - edgeThreshold)
            currentZone = DragZoneType.Right;
        else
            currentZone = DragZoneType.Center;
    }

    public DragZoneType GetCurrentZone() => currentZone;

    public bool IsZone(DragZoneType zone) => currentZone == zone;

    public void ResetZone() => currentZone = DragZoneType.None;
}
