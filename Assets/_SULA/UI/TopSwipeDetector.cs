using UnityEngine;
using UnityEngine.UIElements;

public class TopSwipeDetector : MonoBehaviour
{
    [SerializeField] private float SwipeThreshold = 50f; // Umbral m�nimo de desplazamiento para considerarlo "swipe"
    private const float MaxTimeForSwipe = 0.5f; // Tiempo m�ximo entre el inicio y el final del "swipe"
    private Vector2 _startPosition;
    private float _startTime;
    private bool _isDraggingFromTop = false;

    [SerializeField]private float dragThreshold = 500f;


    private VisualElement root;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.RegisterCallback<PointerDownEvent>(OnPointerDown);
        root.RegisterCallback<PointerUpEvent>(OnPointerUp);
        root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
    }




    private void OnPointerDown(PointerDownEvent evt)
    {
        if (evt.position.y <= 50f) 
        {
            _isDraggingFromTop = true;
            _startPosition = evt.position;
            _startTime = Time.time;
            Debug.Log("Inicio de posible swipe/drag desde la parte superior.");
        }
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (_isDraggingFromTop)
        {
            // Puedes realizar acciones continuas mientras se arrastra desde la parte superior aqu�
            // Por ejemplo, podr�as actualizar la posici�n de un elemento visual.

            Vector2 currentPosition = evt.position;
            Vector2 deltaPosition = currentPosition - _startPosition;

            Debug.Log($"Delta position: {deltaPosition}");

            // Aplicar el desplazamiento directamente a la propiedad translate

            if (Mathf.Abs(deltaPosition.y) <= dragThreshold)   root.style.translate = new Translate(0f, deltaPosition.y * 1f);

        }
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (_isDraggingFromTop)
        {
            _isDraggingFromTop = false;
            Vector2 endPosition = evt.position;
            float endTime = Time.time;
            float swipeDistance = Vector2.Distance(_startPosition, endPosition);
            float swipeTime = endTime - _startTime;

            float totalDrag = endPosition.y - _startPosition.y;

            if (swipeTime <= MaxTimeForSwipe && swipeDistance >= SwipeThreshold)
            {
                Debug.Log("�Se detect� un swipe desde la parte superior!");
                root.style.translate = new Translate(0,0);
                NavigationManager.Instance.ReturnFrom(NavigationManager.Instance.actualScren);
            }
            else if (totalDrag >= dragThreshold)
            {
                NavigationManager.Instance.ReturnFrom(NavigationManager.Instance.actualScren);
                Debug.Log("�Drag completado desde la parte superior!");
                root.style.translate = new Translate(0, 0);

            }
            else if (totalDrag != 0)
            {   
                root.style.translate = new Translate(0, 0);
                Debug.Log("Drag desde la parte superior no super� el umbral.");
            }
        }
    }
}