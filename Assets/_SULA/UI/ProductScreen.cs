using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductScreen : MonoBehaviour
{
    [SerializeField] private VisualElement root;

    [SerializeField] private GameObject ourStory;
    [SerializeField] private VisualElement ourStoryRoot;
    [SerializeField] private VisualElement ourStoryPanel;

    [SerializeField] private GameObject artisans;
    [SerializeField] private VisualElement artisansRoot;
    [SerializeField] private VisualElement artisansPanel;

    // Variables para el swipe
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private const float swipeThreshold = 25f;  //UMBRAL


    [Header("Items")]
    [SerializeField] private VisualTreeAsset imageItemTemplate;
    [SerializeField] private List<Clothes> imageDataList;



    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.RegisterCallback<PointerDownEvent>(OnPointerDown);
        root.RegisterCallback<PointerUpEvent>(OnPointerUp);

        ourStoryRoot = ourStory.GetComponent<UIDocument>().rootVisualElement;
        ourStoryPanel = ourStoryRoot.Q<VisualElement>("mainContainer");

        artisansRoot = artisans.GetComponent<UIDocument>().rootVisualElement;
        artisansPanel = artisansRoot.Q<VisualElement>("mainContainer");

        
        imageDataList = new List<Clothes>(GameManager.Instance.defaulRegion.clothes);

    }


    private void OnPointerDown(PointerDownEvent evt)
    {
        touchStartPos = evt.position;
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        touchEndPos = evt.position;
        //DetectSwipe();
    }

    private void DetectSwipe()
    {
        Vector2 swipeDelta = touchEndPos - touchStartPos;

        if (swipeDelta.magnitude >= swipeThreshold)
        {
            // Determinar si el swipe es horizontal o vertical
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                // Swipe horizontal
                if (swipeDelta.x > 0)
                {
                    Debug.Log("Swipe Derecha");
                    // Acción para swipe derecha
                    OnSwipeRight();
                }
                else
                {
                    Debug.Log("Swipe Izquierda");
                    // Acción para swipe izquierda
                    OnSwipeLeft();
                }
            }
            else
            {
                // Swipe vertical
                if (swipeDelta.y > 0)
                {
                    Debug.Log("Swipe Abajo");
                    // Acción para swipe abajo
                    OnSwipeDown();
                }
                else
                {
                    Debug.Log("Swipe Arriba");
                    // Acción para swipe arriba
                    OnSwipeUp();
                }
            }
        }
    }

    // Métodos para acciones específicas de cada dirección
    private void OnSwipeRight()
    {
        if (!root.ClassListContains("hide-right"))
        {
            root.AddToClassList("hide-right");
        }
        if (ourStoryPanel.ClassListContains("hide-left"))
        {
            ourStoryPanel.RemoveFromClassList("hide-left");
        }

    }

    private void OnSwipeLeft()
    {
        if (!root.ClassListContains("hide-left"))
        {
            root.AddToClassList("hide-left");
        }
        if (artisansPanel.ClassListContains("hide-right"))
        {
            artisansPanel.RemoveFromClassList("hide-right");
        }

    }

    private void OnSwipeDown()
    {
        
    }

    private void OnSwipeUp()
    {
        
    }


    //[Header("Configuration")]
    //[SerializeField] private VisualTreeAsset itemTemplate;
    //[SerializeField] private List<Clothes> itemsData;
    //[SerializeField] private int itemsVisible = 3;
    //[SerializeField] private float itemSpacing = 20f;
    //[SerializeField] private float centerScale = 1.2f;
    //[SerializeField] private float sideScale = 0.9f;
    //[SerializeField] private float snapDuration = 0.5f;

    //private ScrollView scrollView;
    //private List<VisualElement> activeItems = new List<VisualElement>();
    //private List<int> activeItemsDataIndex = new List<int>();
    //private float itemWidth;
    //private float scrollWidth;
    //private int centerIndex;
    //private bool isDragging;
    //private Vector2 startDragPosition;
    //private bool isSnapping;

    //private void OnEnable()
    //{
    //    var root = GetComponent<UIDocument>().rootVisualElement;
    //    scrollView = root.Q<ScrollView>("ItemScroll");

    //    InitializeScrollView();
    //}

    //private void InitializeScrollView()
    //{
    //    scrollView.Clear();
    //    activeItems.Clear();
    //    activeItemsDataIndex.Clear();

    //    scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
    //    scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;
    //    scrollView.touchScrollBehavior = ScrollView.TouchScrollBehavior.Elastic;
    //    scrollView.mode = ScrollViewMode.Horizontal;

    //    // Calcular dimensiones
    //    scrollWidth = scrollView.layout.width;
    //    itemWidth = (scrollWidth - (itemSpacing * (itemsVisible - 1))) / itemsVisible;

    //    // Crear items iniciales
    //    for (int i = 0; i < itemsVisible + 2; i++) // +2 como buffer
    //    {
    //        CreateItem(i - (itemsVisible / 2));
    //    }

    //    // Configurar eventos
    //    scrollView.RegisterCallback<PointerDownEvent>(OnPointerDown);
    //    scrollView.RegisterCallback<PointerUpEvent>(OnPointerUp);
    //    scrollView.RegisterCallback<PointerMoveEvent>(OnPointerMove);
    //    scrollView.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    //}

    //private void CreateItem(int dataIndexOffset)
    //{
    //    int dataIndex = GetCircularIndex(centerIndex + dataIndexOffset);
    //    var item = itemTemplate.Instantiate();

    //    // Configurar item
    //    item.style.width = itemWidth;
    //    item.style.marginRight = itemSpacing;
    //    item.AddToClassList("scroll-item");

    //    // Actualizar contenido
    //    UpdateItemContent(item, dataIndex);

    //    // Añadir al scroll view
    //    scrollView.Add(item);
    //    activeItems.Add(item);
    //    activeItemsDataIndex.Add(dataIndex);
    //}

    //private void UpdateItemContent(VisualElement item, int dataIndex)
    //{
    //    var data = itemsData[dataIndex];
    //    var image = item.Q<VisualElement>("Image");
    //    var label = item.Q<Label>("Name");

    //    if (image != null && data.imagen != null)
    //    {
    //        image.style.backgroundImage = new StyleBackground(data.imagen);
    //    }

    //    if (label != null)
    //    {
    //        label.text = data.clotheName;
    //    }
    //}

    //private int GetCircularIndex(int index)
    //{
    //    if (itemsData.Count == 0) return 0;

    //    while (index < 0) index += itemsData.Count;
    //    return index % itemsData.Count;
    //}

    //private void OnPointerDown(PointerDownEvent evt)
    //{
    //    if (isSnapping) return;

    //    isDragging = true;
    //    startDragPosition = evt.position;
    //}

    //private void OnPointerMove(PointerMoveEvent evt)
    //{
    //    if (!isDragging || isSnapping) return;

    //    // Actualizar escala mientras se arrastra
    //    UpdateItemsScale();
    //}

    //private void OnPointerUp(PointerUpEvent evt)
    //{
    //    if (!isDragging || isSnapping) return;

    //    isDragging = false;

    //    // Detectar si fue un swipe
    //    if (Mathf.Abs(evt.position.x - startDragPosition.x) > swipeThreshold)
    //    {
    //        int direction = evt.position.x < startDragPosition.x ? 1 : -1;
    //        SnapToItem(centerIndex + direction);
    //    }
    //    else
    //    {
    //        SnapToItem(centerIndex);
    //    }
    //}

    //private void SnapToItem(int targetIndex)
    //{
    //    if (isSnapping || itemsData.Count == 0) return;

    //    StartCoroutine(SnapAnimation(targetIndex));
    //}

    //private IEnumerator SnapAnimation(int targetIndex)
    //{
    //    isSnapping = true;
    //    centerIndex = GetCircularIndex(targetIndex);

    //    float startOffset = scrollView.scrollOffset.x;
    //    float targetOffset = centerIndex * (itemWidth + itemSpacing);
    //    float elapsed = 0f;

    //    while (elapsed < snapDuration)
    //    {
    //        float t = elapsed / snapDuration;
    //        t = Mathf.Sin(t * Mathf.PI * 0.5f); // Ease-out

    //        float newOffset = Mathf.Lerp(startOffset, targetOffset, t);
    //        scrollView.scrollOffset = new Vector2(newOffset, 0);

    //        UpdateItemsScale();
    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    scrollView.scrollOffset = new Vector2(targetOffset, 0);
    //    RecycleItemsIfNeeded();
    //    UpdateItemsScale();
    //    isSnapping = false;
    //}

    //private void RecycleItemsIfNeeded()
    //{
    //    // Implementar reciclaje de items cuando sea necesario
    //    // (Opcional para mejor rendimiento con muchas items)
    //}

    //private void UpdateItemsScale()
    //{
    //    float centerPos = scrollView.scrollOffset.x + (scrollWidth / 2f);

    //    for (int i = 0; i < activeItems.Count; i++)
    //    {
    //        var item = activeItems[i];
    //        float itemPos = item.layout.x + (itemWidth / 2f);
    //        float distance = Mathf.Abs(itemPos - centerPos);
    //        float viewportWidth = scrollWidth;

    //        // Calcular escala basada en la distancia al centro
    //        if (distance < viewportWidth * 0.3f)
    //        {
    //            item.style.scale = new StyleScale(new Vector2(centerScale, centerScale));
    //            item.AddToClassList("center-item");
    //            item.RemoveFromClassList("side-item");
    //        }
    //        else
    //        {
    //            item.style.scale = new StyleScale(new Vector2(sideScale, sideScale));
    //            item.AddToClassList("side-item");
    //            item.RemoveFromClassList("center-item");
    //        }
    //    }
    //}

    //private void OnGeometryChanged(GeometryChangedEvent evt)
    //{
    //    // Recalcular cuando cambia el tamaño
    //    scrollWidth = scrollView.layout.width;
    //    itemWidth = (scrollWidth - (itemSpacing * (itemsVisible - 1))) / itemsVisible;

    //    foreach (var item in activeItems)
    //    {
    //        item.style.width = itemWidth;
    //    }

    //    UpdateItemsScale();
    //}

    //// Métodos de navegación pública
    //public void NextItem()
    //{
    //    SnapToItem(centerIndex + 1);
    //}

    //public void PreviousItem()
    //{
    //    SnapToItem(centerIndex - 1);
    //}
}
