using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class SwipeDownDetector : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("Swipe Settings")]
    [Tooltip("Distancia mínima para considerar un swipe válido (en pixeles)")]
    public float minSwipeDistance = 100f;

    [Tooltip("Zona superior de la pantalla (0-1) donde comienza el swipe")]
    [Range(0, 1)]
    public float topScreenArea = 0.2f;

    [Header("Debug")]
    public bool enableLogs = true;

    private Vector2 startTouchPosition;
    private bool swipeStarted = false;

    [SerializeField] private GameObject prendaContainer;

    [SerializeField] private VisualElement prendaRoot   ;
    [SerializeField] private VisualElement prendaPanel;

    private void Start()
    {

        prendaRoot = prendaContainer.GetComponent<UIDocument>().rootVisualElement;
        prendaPanel = prendaRoot.Q<VisualElement>("mainContainer");

    }
    void Update()
    {
        // Detección para touch en móviles
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && IsInTopScreenArea(touch.position))
            {
                StartSwipe(touch.position);
            }
            else if (swipeStarted && touch.phase == TouchPhase.Moved)
            {
                CheckSwipe(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                swipeStarted = false;
                Log("Swipe cancelado o terminado sin completar");
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsInTopScreenArea(eventData.position))
        {
            StartSwipe(eventData.position);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (swipeStarted)
        {
            CheckSwipe(eventData.position);
        }
    }

    private void StartSwipe(Vector2 position)
    {
        startTouchPosition = position;
        swipeStarted = true;
        Log($"Swipe iniciado en posición: {position}");
    }

    private void CheckSwipe(Vector2 currentPosition)
    {
        Vector2 swipeDelta = currentPosition - startTouchPosition;

        // Solo nos interesa el movimiento vertical hacia abajo
        if (swipeDelta.y < 0 && Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x))
        {
            float swipeDistance = Mathf.Abs(swipeDelta.y);
            Log($"Desplazamiento hacia abajo detectado. Distancia: {swipeDistance}px");

            if (swipeDistance >= minSwipeDistance)
            {
                Log("¡Swipe hacia abajo completado!");
                swipeStarted = false;


                //FUNCIONALIDAD


                prendaRoot.RemoveFromClassList("hide-up");
                gameObject.SetActive(false);
               
            }
        }
        else if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            Log("Movimiento horizontal detectado - ignorando swipe");
            swipeStarted = false;
        }
    }

    private bool IsInTopScreenArea(Vector2 screenPosition)
    {
        float screenHeight = Screen.height;
        float topAreaHeight = screenHeight * topScreenArea;
        bool inTopArea = screenPosition.y > (screenHeight - topAreaHeight);

        Log($"Posición Y: {screenPosition.y} - Límite superior: {screenHeight - topAreaHeight} - En zona superior: {inTopArea}");
        return inTopArea;
    }

    private void Log(string message)
    {
        if (enableLogs) Debug.Log($"[SwipeDetector] {message}");
    }
}