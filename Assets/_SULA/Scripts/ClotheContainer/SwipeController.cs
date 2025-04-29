using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ClotheContainerController carrusel;

    [Header("Configuración")]
    public float swipeThreshold = 100f;
    public float swipeTimeThreshold = 0.5f;

    private Vector2 dragStartPosition;
    private float dragStartTime;
    private bool isDragging;
    private bool isValidSwipeArea;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(
            GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera))
        {
            isValidSwipeArea = false;
            return;
        }

        isValidSwipeArea = true;
        dragStartPosition = eventData.position;
        dragStartTime = Time.time;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // No necesitamos hacer nada durante el drag
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging || !isValidSwipeArea) return;
        isDragging = false;

        Vector2 dragEndPosition = eventData.position;
        Vector2 dragVector = dragEndPosition - dragStartPosition;
        float distance = dragVector.magnitude;

        if (Time.time - dragStartTime > swipeTimeThreshold || distance < swipeThreshold)
            return;

        // Determinar dirección principal del swipe
        bool isHorizontalSwipe = Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y);

        if (isHorizontalSwipe)
        {
            // Swipe horizontal
            if (dragVector.x > 0)
            {
                Debug.Log("Swipe derecho detectado");
                carrusel.MoveLeft();
            }
            else
            {
                Debug.Log("Swipe izquierdo detectado");
                carrusel.MoveRight();
            }
        }
        else
        {
            // Swipe vertical
            if (dragVector.y > 0)
            {
                Debug.Log("Swipe arriba detectado");
                // Aquí puedes agregar la funcionalidad para swipe arriba
            }
            else
            {
                Debug.Log("Swipe abajo detectado");
                // Aquí puedes agregar la funcionalidad para swipe abajo
            }
        }
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleTouchInput()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 currentPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            if (!isDragging)
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(
                    GetComponent<RectTransform>(),
                    currentPosition,
                    Camera.main))
                {
                    isValidSwipeArea = false;
                    return;
                }

                isValidSwipeArea = true;
                dragStartPosition = currentPosition;
                dragStartTime = Time.time;
                isDragging = true;
            }
        }
        else if (isDragging)
        {
            isDragging = false;

            if (Touchscreen.current != null && isValidSwipeArea)
            {
                Vector2 currentPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                ProcessSwipe(currentPosition);
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            if (!RectTransformUtility.RectangleContainsScreenPoint(
                GetComponent<RectTransform>(),
                mousePosition,
                Camera.main))
            {
                isValidSwipeArea = false;
                return;
            }

            isValidSwipeArea = true;
            dragStartPosition = mousePosition;
            dragStartTime = Time.time;
            isDragging = true;
        }
        else if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;

            if (isValidSwipeArea)
            {
                ProcessSwipe(Mouse.current.position.ReadValue());
            }
        }
    }

    private void ProcessSwipe(Vector2 endPosition)
    {
        if (Time.time - dragStartTime > swipeTimeThreshold) return;

        Vector2 dragVector = endPosition - dragStartPosition;
        float distance = dragVector.magnitude;

        if (distance < swipeThreshold) return;

        bool isHorizontalSwipe = Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y);

        if (isHorizontalSwipe)
        {
            if (dragVector.x > 0)
            {
                carrusel.MoveLeft();
            }
            else
            {
                carrusel.MoveRight();
            }
        }
        else
        {
            if (dragVector.y > 0)
            {
                // Funcionalidad para swipe arriba
                Debug.Log("Swipe arriba ejecutado");
            }
            else
            {
                // Funcionalidad para swipe abajo
                Debug.Log("Swipe abajo ejecutado");
            }
        }
    }
}