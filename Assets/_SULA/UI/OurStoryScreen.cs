using UnityEngine;
using UnityEngine.UIElements;

public class OurStoryScreen : MonoBehaviour
{
    [Header("Default Configuration")]
    private VisualElement root;
    private VisualElement rootPanel;

    [Header("Other Screens")]
    [SerializeField] private GameObject products; // Asigna el GameObject de ArtisansScreen
    private VisualElement productsRoot;
    private VisualElement productsPanel;

    [Header("Swipe Configuration")]
    public float swipeThreshold = 50f;
    public float swipeTimeThreshold = 0.3f;
    public float horizontalSwipeMarginPercentage = 0.1f; 

    private Vector2 startPosition;
    private float startTime;
    private bool isSwiping;

    private void OnEnable()
    {

        root = gameObject.GetComponent<UIDocument>().rootVisualElement;

        rootPanel = root.Q<VisualElement>("mainContainer");

        productsRoot = products.GetComponent<UIDocument>().rootVisualElement;
        productsPanel = productsRoot.Q<VisualElement>("mainContainer");


        root.RegisterCallback<PointerDownEvent>(OnPointerDown);
        root.RegisterCallback<PointerUpEvent>(OnPointerUp);
        root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
    }

    private void OnDisable()
    {
        root.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        root.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        root.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        startPosition = evt.position;
        startTime = (float)evt.timestamp / 1000f;
        isSwiping = true;
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!isSwiping) return;
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (!isSwiping) return;
        isSwiping = false;

        Vector2 endPosition = evt.position;
        float deltaTime = ((float)evt.timestamp / 1000f) - startTime;
        Vector2 swipeVector = endPosition - startPosition;
        float swipeDistance = swipeVector.magnitude;

        if (deltaTime > swipeTimeThreshold || swipeDistance < swipeThreshold)
        {
            return; // No es un swipe válido
        }

        Rect rootRect = root.worldBound; 
        float horizontalMargin = rootRect.width * horizontalSwipeMarginPercentage;
        // Detectar swipe hacia la izquierda desde el borde derecho
        if (startPosition.x >= rootRect.xMax - horizontalMargin &&
            swipeVector.x < 0 &&
            Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
        {
            Debug.Log("Swipe hacia la izquierda desde el borde derecho detectado. Navegando a Productos.");
            NavigateLeft();
        }
    }

    public void NavigateLeft()
    {
        Debug.Log("Navigate Left");
        
        root.AddToClassList("hide-left");

        productsPanel.ClearClassList();
        productsPanel.AddToClassList("show");

        products.gameObject.SetActive(true); // Asegúrate de que el GameObject esté activo
    }
}
