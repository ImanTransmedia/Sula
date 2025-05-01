using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArtisansScreen : MonoBehaviour
{
    [Header("Default Configuration")]
    [SerializeField] private VisualElement root;
    [SerializeField] private VisualElement rootPanel;

    [Header("Other Screens")]
    [SerializeField] private GameObject start;
    [SerializeField] private VisualElement startRoot;
    [SerializeField] private VisualElement startPanel;

    [SerializeField] private GameObject products;
    [SerializeField] private VisualElement productsRoot;
    [SerializeField] private VisualElement productsPanel;

    [SerializeField] public VisualElement returnArea;
    [SerializeField] public VisualElement navigateArea;

    [Header("Swipe Configuration")]
    public float swipeThreshold = 50f;
    public float swipeTimeThreshold = 0.3f;

    private Vector2 startPosition;
    private float startTime;
    private bool isSwiping;
    private VisualElement currentSwipeArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        rootPanel = root.Q<VisualElement>("mainContainer");

        returnArea = root.Q<VisualElement>("ReturnArea");
        navigateArea = root.Q<VisualElement>("NavigateArea");

        startRoot = start.GetComponent<UIDocument>().rootVisualElement;
        startPanel = startRoot.Q<VisualElement>("Container");

        productsRoot = products.GetComponent<UIDocument>().rootVisualElement;
        productsPanel = productsRoot.Q<VisualElement>("mainContainer");

        if (returnArea != null)
        {
            returnArea.RegisterCallback<PointerDownEvent>(OnPointerDown);
            returnArea.RegisterCallback<PointerUpEvent>(OnPointerUp);
            returnArea.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        }
        else
        {
            Debug.LogError("ReturnArea no encontrado.");
        }

        if (navigateArea != null)
        {
            navigateArea.RegisterCallback<PointerDownEvent>(OnPointerDown);
            navigateArea.RegisterCallback<PointerUpEvent>(OnPointerUp);
            navigateArea.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        }
        else
        {
            Debug.LogError("NavigateArea no encontrado.");
        }
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        startPosition = evt.position;
        startTime = (float)evt.timestamp / 1000f;
        isSwiping = true;
        currentSwipeArea = evt.target as VisualElement; // Guarda el elemento donde comenzó el swipe
        evt.StopPropagation(); // Evita que otros elementos capturen el evento
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!isSwiping || currentSwipeArea == null) return;
        // Puedes agregar lógica para rastrear el movimiento si es necesario
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (!isSwiping || currentSwipeArea == null) return;
        isSwiping = false;

        Vector2 endPosition = evt.position;
        float deltaTime = ((float)evt.timestamp / 1000f) - startTime;
        Vector2 swipeVector = endPosition - startPosition;
        float swipeDistance = swipeVector.magnitude;

        if (deltaTime > swipeTimeThreshold || swipeDistance < swipeThreshold)
        {
            currentSwipeArea = null;
            return;
        }

        if (currentSwipeArea == returnArea)
        {
            // Detectar swipe hacia abajo en returnArea
            if (swipeVector.y > 0 && Mathf.Abs(swipeVector.y) > Mathf.Abs(swipeVector.x))
            {
                Debug.Log("Swipe hacia abajo en ReturnArea detectado.");
                NavigateReturn();
            }
        }
        else if (currentSwipeArea == navigateArea)
        {
            // Detectar swipe hacia la derecha en navigateArea
            if (swipeVector.x > 0 && Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
            {
                Debug.Log("Swipe hacia la derecha en NavigateArea detectado.");
                NavigateRight();
            }
        }

        currentSwipeArea = null;
    }

    public void NavigateReturn()
    {
        Debug.Log("Return");
        root.AddToClassList("hide-down");
        startPanel.ClearClassList();
        startPanel.AddToClassList("show");
    }

    public void NavigateRight()
    {
        Debug.Log("Navigate");
        root.AddToClassList("hide-right");
        productsPanel.ClearClassList();
        productsPanel.AddToClassList("show");
    }
}