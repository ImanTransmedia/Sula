using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MouseTouchScrollDirect : MonoBehaviour
{
    public UIDocument uiDocument;

    private ScrollView scrollView;
    private bool isDragging = false;
    private Vector2 lastMousePosition;
    private float velocityY = 0f;
    private float inertia = 0.95f;

    void Start()
    {
        scrollView = uiDocument.rootVisualElement.Q<ScrollView>("OurStoryScroll");

        scrollView.RegisterCallback<PointerDownEvent>(OnPointerDown);
        scrollView.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        scrollView.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    void Update()
    {
        if (!isDragging && Mathf.Abs(velocityY) > 0.1f)
        {
            scrollView.scrollOffset -= new Vector2(0, velocityY * Time.deltaTime);
            velocityY *= inertia;
        }
    }

    void OnPointerDown(PointerDownEvent evt)
    {
        isDragging = true;
        velocityY = 0;
        lastMousePosition = evt.position;
    }

    void OnPointerMove(PointerMoveEvent evt)
    {
        if (isDragging)
        {
            Vector2 currentMousePosition = new Vector2(evt.position.x, evt.position.y);
            Vector2 delta = currentMousePosition - lastMousePosition;
            scrollView.scrollOffset -= new Vector2(0, delta.y);
            velocityY = delta.y / Time.deltaTime;
            lastMousePosition = currentMousePosition;
        }
    }

    void OnPointerUp(PointerUpEvent evt)
    {
        isDragging = false;
    }
}