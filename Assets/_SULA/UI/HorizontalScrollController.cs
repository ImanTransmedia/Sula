using UnityEngine.UIElements;
using UnityEngine;

public class HorizontalScrollController : MonoBehaviour, ICustomScrollHandler
{
    public UIDocument uiDocument;

    private Vector2 lastMousePosition;
    private bool isDragging = false;
    private bool draggedEnough = false;

    private ScrollView currentScrollView;
    private const float dragThreshold = 10f;

    [SerializeField] private InfiniteScroll infiniteScroll;

    void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        RegisterScroll(root.Q<ScrollView>("FilterScroll"));
        RegisterScroll(root.Q<ScrollView>("ItemScroll"));

        if (infiniteScroll == null)
            infiniteScroll = FindFirstObjectByType<InfiniteScroll>();
    }

    void RegisterScroll(ScrollView scrollView)
    {
        scrollView.RegisterCallback<PointerDownEvent>(evt => OnPointerDown(evt, scrollView));
        scrollView.RegisterCallback<PointerMoveEvent>(evt => OnPointerMove(evt, scrollView));
        scrollView.RegisterCallback<PointerUpEvent>(evt => OnPointerUp(evt, scrollView));
    }

    void OnPointerDown(PointerDownEvent evt, ScrollView scrollView)
    {
        isDragging = true;
        draggedEnough = false;
        lastMousePosition = evt.position;
        currentScrollView = scrollView;
    }

    void OnPointerMove(PointerMoveEvent evt, ScrollView scrollView)
    {
        if (!isDragging || currentScrollView != scrollView) return;

        Vector2 currentMousePosition = evt.position;
        Vector2 delta = currentMousePosition - lastMousePosition;

        if (!draggedEnough && Mathf.Abs(delta.x) > dragThreshold)
            draggedEnough = true;

        if (draggedEnough)
        {
            scrollView.scrollOffset -= new Vector2(delta.x, 0);
            ClampScroll(scrollView);
        }

        lastMousePosition = currentMousePosition;
    }

    void OnPointerUp(PointerUpEvent evt, ScrollView scrollView)
    {
        if (!isDragging || currentScrollView != scrollView) return;

        isDragging = false;
        scrollView.ReleasePointer(evt.pointerId);
        currentScrollView = null;
    }

    private void ClampScroll(ScrollView scrollView)
    {
        if (scrollView.name == "ItemScroll" && infiniteScroll != null && !infiniteScroll.IsElastic)
        {
            // Scroll infinito: no clamping
            return;
        }

        float leftMargin = -100f;
        float rightMargin = 100f;

        float maxX = scrollView.contentContainer.layout.width - scrollView.layout.width;
        maxX = Mathf.Max(0, maxX);

        Vector2 offset = scrollView.scrollOffset;
        float clampedX = Mathf.Clamp(offset.x, leftMargin, maxX + rightMargin);
        scrollView.scrollOffset = new Vector2(clampedX, 0);
    }


    public void OnExternalPointerMove(Vector2 delta)
    {
        if (!draggedEnough && Mathf.Abs(delta.x) > dragThreshold)
            draggedEnough = true;

        if (draggedEnough && currentScrollView != null)
        {
            currentScrollView.scrollOffset -= new Vector2(delta.x, 0);
            ClampScroll(currentScrollView);
        }
    }
}
