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
        RegisterScroll(root.Q<ScrollView>("DetailScroll"));
        // Agrega más ScrollViews aquí si es necesario
    }

    void RegisterScroll(ScrollView scrollView)
    {
        var root = uiDocument.rootVisualElement;

        // Registramos los eventos en el panel general para asegurarnos que no se pierdan
        root.RegisterCallback<PointerDownEvent>(evt => OnPointerDown(evt, scrollView), TrickleDown.TrickleDown);
        root.RegisterCallback<PointerMoveEvent>(evt => OnPointerMove(evt, scrollView), TrickleDown.TrickleDown);
        root.RegisterCallback<PointerUpEvent>(evt => OnPointerUp(evt, scrollView), TrickleDown.TrickleDown);
    }

    void OnPointerDown(PointerDownEvent evt, ScrollView scrollView)
    {
        // IMPORTANTE: ¿El puntero realmente está sobre este scrollView?
        VisualElement hit = uiDocument.rootVisualElement.panel?.Pick(evt.position);
        if (hit == null || !IsChildOf(hit, scrollView)) return;

        // ¿Está en zona válida?
        PointerZoneManager.Instance.EvaluatePointerZone(evt.position);
        if (!PointerZoneManager.Instance.IsZone(DragZoneType.Center)) return;

        isDragging = true;
        lastMousePosition = evt.position;
        currentScrollView = scrollView;
    }

    private bool IsChildOf(VisualElement element, VisualElement potentialParent)
    {
        while (element != null)
        {
            if (element == potentialParent) return true;
            element = element.parent;
        }
        return false;
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

    void Update()
    {
        // Fallback por si PointerUp se pierde
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            PointerZoneManager.Instance.ResetZone();
            currentScrollView = null;
        }
    }

    private void ClampScroll(ScrollView scrollView)
    {
        float topMargin = 0f;
        float bottomMargin = 0f;

        float maxY = scrollView.contentContainer.layout.height - scrollView.layout.height;
        maxY = Mathf.Max(0, maxY);

        Vector2 offset = scrollView.scrollOffset;
        float clampedY = Mathf.Clamp(offset.y, topMargin, maxY + bottomMargin);

        scrollView.scrollOffset = new Vector2(0, clampedY);
    }


    private bool IsPointerOverNoScrollZone(Vector2 position)
    {
        var root = uiDocument.rootVisualElement;
        VisualElement hit = root.panel?.Pick(position);
        if (hit == null) return false;

        // Condiciones de bloqueo personalizadas
        if (hit.name == "PrendaTexture") return true;
        if (hit.ClassListContains("no-scroll-zone")) return true;

        return false;
    }

    private void OnDisable()
    {
        isDragging = false;
        currentScrollView = null;
        PointerZoneManager.Instance.ResetZone();
    }
}
