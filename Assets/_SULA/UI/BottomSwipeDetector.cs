using UnityEngine;
using UnityEngine.UIElements;

public class BottomSwipeDetector : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string scrollViewName = "ItemScroll";

    [SerializeField] private float SwipeThreshold = 200f;
    private const float MaxTimeForSwipe = 0.5f;
    [SerializeField] private float dragThreshold = 150f;

    private Vector2 _startPosition;
    private float _startTime;
    private bool _isDraggingFromBottom = false;

    private ScrollView scrollView;
    private VisualElement root;

    private void OnEnable()
    {
        root = uiDocument.rootVisualElement;
        scrollView = root.Q<ScrollView>(scrollViewName);

        root.RegisterCallback<PointerDownEvent>(OnPointerDown);
        root.RegisterCallback<PointerUpEvent>(OnPointerUp);
        root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        PointerZoneManager.Instance.EvaluatePointerZone(evt.position);
        if (PointerZoneManager.Instance.IsZone(DragZoneType.Bottom) && IsScrollAtBottom())
        {
            _isDraggingFromBottom = true;
            _startPosition = evt.position;
            _startTime = Time.time;
        }
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (_isDraggingFromBottom)
        {
            Vector2 deltaPosition = (Vector2)evt.position - _startPosition;

            if (deltaPosition.y < 0 && Mathf.Abs(deltaPosition.y) <= dragThreshold)
            {
                root.style.translate = new Translate(0f, deltaPosition.y);
            }
        }
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (_isDraggingFromBottom)
        {
            _isDraggingFromBottom = false;
            Vector2 endPosition = evt.position;
            float endTime = Time.time;
            float swipeDistance = Vector2.Distance(_startPosition, endPosition);
            float swipeTime = endTime - _startTime;
            float totalDrag = endPosition.y - _startPosition.y;

            if (swipeTime <= MaxTimeForSwipe && swipeDistance >= SwipeThreshold && totalDrag < 0)
            {
                Debug.Log("¡Swipe hacia arriba desde el fondo detectado!");
                root.style.translate = new Translate(0, 0);
                NavigationManager.Instance.ReturnFrom(ScreenOptions.Details); // o tu lógica
            }
            else if (totalDrag <= -dragThreshold)
            {
                Debug.Log("¡Drag hacia arriba completado!");
                root.style.translate = new Translate(0, 0);
                NavigationManager.Instance.ReturnFrom(ScreenOptions.Details); // o tu lógica
            }
            else
            {
                root.style.translate = new Translate(0, 0);
                Debug.Log("Drag desde el fondo no superó el umbral.");
            }
        }

        PointerZoneManager.Instance.ResetZone();
    }

    private bool IsScrollAtBottom()
    {
        if (scrollView == null) return false;

        float contentHeight = scrollView.contentContainer.layout.height;
        float viewportHeight = scrollView.layout.height;
        float currentOffsetY = scrollView.scrollOffset.y;

        return currentOffsetY + viewportHeight >= contentHeight - 5f; // margen de tolerancia
    }
}
