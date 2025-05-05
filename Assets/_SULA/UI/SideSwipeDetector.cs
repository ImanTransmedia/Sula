using UnityEngine;
using UnityEngine.UIElements;

public class SideSwipeDetector : MonoBehaviour
{
    private const float SwipeThreshold = 50f; // Umbral mínimo de desplazamiento para considerarlo "swipe"
    private const float MaxTimeForSwipe = 0.5f; // Tiempo máximo entre el inicio y el final del "swipe"
    [SerializeField] private float dragThreshold = 200f; // Umbral de desplazamiento para el drag lateral
    [SerializeField] private float edgeThreshold = 50f; // Ancho de los bordes para iniciar la detección

    private Vector2 _startPosition;
    private float _startTime;
    private bool _isDraggingFromSide = false;
    private bool _isDraggingFromLeft = false; // Para distinguir izquierda y derecha
    private VisualElement _root;

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _root.RegisterCallback<PointerDownEvent>(OnPointerDown);
        _root.RegisterCallback<PointerUpEvent>(OnPointerUp);
        _root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
    }

    public bool IsSwipeZone(Vector2 position)
    {
        return (position.x <= edgeThreshold || position.x >= Screen.width - edgeThreshold);
    }


    private void OnDisable()
    {
        _root.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        _root.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        _root.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        PointerZoneManager.Instance.EvaluatePointerZone(evt.position);
        if (PointerZoneManager.Instance.IsZone(DragZoneType.Left) || PointerZoneManager.Instance.IsZone(DragZoneType.Right))
        {
            _isDraggingFromSide = true;
            _isDraggingFromLeft = (evt.position.x <= PointerZoneManager.Instance.edgeThreshold);
            _startPosition = evt.position;
            _startTime = Time.time;
        }
    }


    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (_isDraggingFromSide)
        {
            Vector2 currentPosition = evt.position;
            Vector2 deltaPosition = currentPosition - _startPosition;

            Debug.Log($"Delta position: {deltaPosition}");

            // Aplicar el desplazamiento horizontal
            if ( Mathf.Abs( deltaPosition.x) <= dragThreshold ) _root.style.translate = new Translate(deltaPosition.x * 1f, 0f);
        }
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (_isDraggingFromSide)
        {
            _isDraggingFromSide = false;
            Vector2 endPosition = evt.position;
            float endTime = Time.time;
            float swipeDistance = Mathf.Abs(endPosition.x - _startPosition.x); // Distancia horizontal
            float swipeTime = endTime - _startTime;
            float totalDrag = endPosition.x - _startPosition.x;

            if (swipeTime <= MaxTimeForSwipe && swipeDistance >= SwipeThreshold)
            {
                Debug.Log($"¡Se detectó un swipe hacia {(_isDraggingFromLeft ? "la derecha" : "la izquierda")}!");
                _root.style.translate = new Translate(0, 0);
                if(_isDraggingFromLeft)
                {
                    NavigationManager.Instance.GoLeft(NavigationManager.Instance.actualScren);
                }
                else
                {
                    NavigationManager.Instance.GoRight(NavigationManager.Instance.actualScren);
                }
            }
            else if (Mathf.Abs(totalDrag) >= dragThreshold)
            {
                Debug.Log($"¡Drag completado hacia {(_isDraggingFromLeft ? "la derecha" : "la izquierda")}!");
                
                if(totalDrag>0)
                {
                    NavigationManager.Instance.GoLeft(NavigationManager.Instance.actualScren);
                }
                else
                {
                    NavigationManager.Instance.GoRight(NavigationManager.Instance.actualScren);
                }

                _root.style.translate = new Translate(0, 0);
            }
            else if (totalDrag != 0)
            {
                _root.style.translate = new Translate(0, 0);
                Debug.Log($"Drag desde {(evt.position.x <= edgeThreshold ? "la izquierda" : "la derecha")} no superó el umbral.");
                // Aquí puedes añadir lógica para un drag lateral incompleto.
            }
        }

        PointerZoneManager.Instance.ResetZone();

    }
}