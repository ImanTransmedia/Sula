using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

[System.Serializable]
public class SwipeDirectionEvent : UnityEvent<SwipeDirection> { }

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class GenericSwipeDetector : MonoBehaviour
{
    [SerializeField] private UIDocument targetDocument;
    [SerializeField] private string returnAreaName = "ReturnArea";
    [SerializeField] private string navigateAreaName = "NavigateArea";
    [SerializeField] [Range(25,100)] private float swipeThreshold = 25f;

    [Header("Return Area Events (Swipe Up Only)")]
    public SwipeDirectionEvent OnReturnSwipeUp;
    private Vector2 returnTouchStartPos;
    private VisualElement returnArea;

    [Header("Navigate Area Events (Swipe Left/Right Only)")]
    public SwipeDirectionEvent OnNavigateSwipeLeft;
    public SwipeDirectionEvent OnNavigateSwipeRight;
    private Vector2 navigateTouchStartPos;
    private VisualElement navigateArea;

    void OnEnable()
    {
        if (targetDocument == null || targetDocument.rootVisualElement == null)
        {
            Debug.LogError("Target UIDocument is not assigned or its root is null in GenericSwipeDetector on " + gameObject.name);
            return;
        }

        returnArea = targetDocument.rootVisualElement.Q<VisualElement>(returnAreaName);
        navigateArea = targetDocument.rootVisualElement.Q<VisualElement>(navigateAreaName);

        if (returnArea == null)
        {
            Debug.LogWarning($"Return Area with name '{returnAreaName}' not found in the UIDocument.");
        }
        else
        {
            returnArea.RegisterCallback<PointerDownEvent>(OnReturnPointerDown);
            returnArea.RegisterCallback<PointerUpEvent>(OnReturnPointerUp);
        }

        if (navigateArea == null)
        {
            Debug.LogWarning($"Navigate Area with name '{navigateAreaName}' not found in the UIDocument.");
        }
        else
        {
            navigateArea.RegisterCallback<PointerDownEvent>(OnNavigatePointerDown);
            navigateArea.RegisterCallback<PointerUpEvent>(OnNavigatePointerUp);
        }

        // Inicializar los UnityEvents si son nulos
        if (OnReturnSwipeUp == null) OnReturnSwipeUp = new SwipeDirectionEvent();
        if (OnNavigateSwipeLeft == null) OnNavigateSwipeLeft = new SwipeDirectionEvent();
        if (OnNavigateSwipeRight == null) OnNavigateSwipeRight = new SwipeDirectionEvent();
    }

    private void OnReturnPointerDown(PointerDownEvent evt)
    {
        returnTouchStartPos = evt.position;
    }

    private void OnReturnPointerUp(PointerUpEvent evt)
    {
        Vector2 touchEndPos = evt.position;
        Vector2 swipeDelta = touchEndPos - returnTouchStartPos;

        if (swipeDelta.magnitude >= swipeThreshold && Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x) && swipeDelta.y > 0)
        {
            OnReturnSwipeUp?.Invoke(SwipeDirection.Up);
        }
    }

    private void OnNavigatePointerDown(PointerDownEvent evt)
    {
        navigateTouchStartPos = evt.position;
    }

    private void OnNavigatePointerUp(PointerUpEvent evt)
    {
        Vector2 touchEndPos = evt.position;
        Vector2 swipeDelta = touchEndPos - navigateTouchStartPos;

        if (swipeDelta.magnitude >= swipeThreshold && Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0)
            {
                OnNavigateSwipeRight?.Invoke(SwipeDirection.Right);
            }
            else
            {
                OnNavigateSwipeLeft?.Invoke(SwipeDirection.Left);
            }
        }
    }
}