using UnityEngine;
using UnityEngine.UIElements;
using PointerType = UnityEngine.UIElements.PointerType;

public class VerticalScrollController : MonoBehaviour
{
    public UIDocument uiDocument;

    private Vector2 lastMousePosition;
    private bool isDragging = false;
    private ScrollView currentScrollView;

    [SerializeField] private SideSwipeDetector swipeDetector;

    void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        RegisterScroll(root.Q<ScrollView>("ArtisansScroll"));
        RegisterScroll(root.Q<ScrollView>("OurStoryScroll"));
        RegisterScroll(root.Q<ScrollView>("DetailScroll")); // Añade más aquí
    }

    void RegisterScroll(ScrollView scrollView)
    {
        scrollView.RegisterCallback<PointerDownEvent>(evt => OnPointerDown(evt, scrollView));
        scrollView.RegisterCallback<PointerMoveEvent>(evt => OnPointerMove(evt, scrollView));
        scrollView.RegisterCallback<PointerUpEvent>(evt => OnPointerUp(evt, scrollView));
    }

    void OnPointerDown(PointerDownEvent evt, ScrollView scrollView)
    {
        PointerZoneManager.Instance.EvaluatePointerZone(evt.position);
        if (PointerZoneManager.Instance.IsZone(DragZoneType.Center))
        {
            isDragging = true;
            lastMousePosition = evt.position;
            currentScrollView = scrollView;
        }
    }

    void OnPointerMove(PointerMoveEvent evt, ScrollView scrollView)
    {
        if (!isDragging || currentScrollView != scrollView) return;

        Vector2 currentMousePosition = evt.position;
        Vector2 delta = currentMousePosition - lastMousePosition;

        scrollView.scrollOffset -= new Vector2(0, delta.y);
        ClampScroll(scrollView);

        lastMousePosition = currentMousePosition;
    }

    void OnPointerUp(PointerUpEvent evt, ScrollView scrollView)
    {
        if (!isDragging || currentScrollView != scrollView) return;

        isDragging = false;
        scrollView.ReleasePointer(evt.pointerId);
        PointerZoneManager.Instance.ResetZone();
        currentScrollView = null;
    }

    private void ClampScroll(ScrollView scrollView)
    {
        float topMargin = 0;
        float bottomMargin = 0f;

        float maxY = scrollView.contentContainer.layout.height - scrollView.layout.height;
        maxY = Mathf.Max(0, maxY); // evitar negativos

        Vector2 offset = scrollView.scrollOffset;

        float clampedY = Mathf.Clamp(offset.y, topMargin, maxY + bottomMargin);

        scrollView.scrollOffset = new Vector2(0, clampedY);
    }

}
