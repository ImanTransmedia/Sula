using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField] private VisualElement root;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualTreeAsset itemTemplate;
    [SerializeField] private List<Clothes> itemDataList;
    [SerializeField] private int visibleItemsCount = 3; 
    [SerializeField] private int bufferSize = 2; 

    private ScrollView scrollView;
    private List<VisualElement> visibleItems = new List<VisualElement>();
    private List<int> dataIndices = new List<int>();
    private float itemWidth = 400f; 
    private float itemHeight = 650f;
    private int totalItems => itemDataList.Count;

    [SerializeField] private GameObject detailsPanel;

    public void FillInstance(List<Clothes> clothesList)
    {
        itemDataList = clothesList;
        InitializeScroller();
    }

    private void InitializeScroller()
    {
        if (itemDataList == null || itemDataList.Count == 0)
        {
            Debug.LogWarning("No hay datos para mostrar en el scroller");
            return;
        }

        root = uiDocument.rootVisualElement;
        scrollView = root.Q<ScrollView>("ItemScroll");

        // Configuración crítica del ScrollView HORIZONTAL
        scrollView.mode = ScrollViewMode.Horizontal;
        scrollView.horizontalScrollerVisibility = ScrollerVisibility.AlwaysVisible;
        scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;

        // Configurar el contenedor de contenido
        scrollView.contentContainer.style.flexDirection = FlexDirection.Row;
        scrollView.contentContainer.style.flexWrap = Wrap.NoWrap;
        scrollView.contentContainer.style.height = Length.Percent(100);

        // Configurar ancho total del contenido
        scrollView.contentContainer.style.width = totalItems * itemWidth;
       

        // Limpiar items existentes
        scrollView.Clear();
        visibleItems.Clear();
        dataIndices.Clear();

        // Configurar el callback del scroll
        scrollView.horizontalScroller.valueChanged -= OnScrollValueChanged;
        scrollView.horizontalScroller.valueChanged += OnScrollValueChanged;

        // Inicializar items visibles
        InitializeVisibleItems();
    }

    private void InitializeVisibleItems()
    {
        // Crear items iniciales (visibles + buffer)
        int itemsToCreate = Mathf.Min(visibleItemsCount + bufferSize * 2, totalItems);

        for (int i = 0; i < itemsToCreate; i++)
        {
            AddNewItem(i);
        }
    }

    private void AddNewItem(int dataIndex)
    {
        var newItem = itemTemplate.Instantiate();

        // Configurar tamaño y posición
        newItem.style.width = itemWidth;
        newItem.style.height = Length.Percent(100);
        newItem.style.position = Position.Absolute;
        newItem.style.left = dataIndex * itemWidth;

        newItem.RegisterCallback<ClickEvent>(evt =>
        {
            //Logica para llevar a nueva vista
            Debug.Log($"Item {dataIndex} clicked");
            GameManager.Instance.actualClothe = itemDataList[dataIndex];
            root.AddToClassList("hide-up");
            detailsPanel.SetActive(true);
        });

        scrollView.Add(newItem);
        visibleItems.Add(newItem);
        dataIndices.Add(dataIndex);

        UpdateItemContent(newItem, dataIndex);
    }

    private void OnScrollValueChanged(float value)
    {
        if (totalItems == 0 || scrollView == null) return;

        // Calcular índice visible basado en la posición del scroll
        int firstVisibleIndex = Mathf.FloorToInt(value / itemWidth);

        UpdateVisibleItems(firstVisibleIndex);
    }

    private void UpdateVisibleItems(int firstVisibleIndex)
    {
        if (visibleItems.Count == 0) return;

        // Scroll hacia la izquierda
        while (firstVisibleIndex < dataIndices[0] && dataIndices[0] > 0)
        {
            MoveLastItemToStart();
        }

        // Scroll hacia la derecha
        int lastVisibleIndex = firstVisibleIndex + visibleItemsCount;
        while (lastVisibleIndex > dataIndices[dataIndices.Count - 1] &&
               dataIndices[dataIndices.Count - 1] < totalItems - 1)
        {
            MoveFirstItemToEnd();
        }
    }

    private void MoveLastItemToStart()
    {
        var lastItem = visibleItems[visibleItems.Count - 1];
        visibleItems.RemoveAt(visibleItems.Count - 1);
        visibleItems.Insert(0, lastItem);

        int newIndex = dataIndices[0] - 1;
        dataIndices.Insert(0, newIndex);
        dataIndices.RemoveAt(dataIndices.Count - 1);

        UpdateItemPosition(lastItem, newIndex);
        UpdateItemContent(lastItem, newIndex);
    }

    private void MoveFirstItemToEnd()
    {
        var firstItem = visibleItems[0];
        visibleItems.RemoveAt(0);
        visibleItems.Add(firstItem);

        int newIndex = dataIndices[dataIndices.Count - 1] + 1;
        dataIndices.Add(newIndex);
        dataIndices.RemoveAt(0);

        UpdateItemPosition(firstItem, newIndex);
        UpdateItemContent(firstItem, newIndex);
    }

    private void UpdateItemPosition(VisualElement item, int index)
    {
        item.style.left = index * itemWidth;
    }

    private void UpdateItemContent(VisualElement item, int dataIndex)
    {
        if (dataIndex < 0 || dataIndex >= totalItems) return;

        var itemData = itemDataList[dataIndex];

        // Actualizar nombre
        var nameLabel = item.Q<Label>("Name");
        if (nameLabel != null)
        {
            nameLabel.text = itemData.name;
        }

        // Actualizar imagen
        var imageElement = item.Q<VisualElement>("Image");
        if (imageElement != null && itemData.imagen != null)
        {
            imageElement.style.backgroundImage = new StyleBackground(itemData.imagen);
        }
    }

    public void SetData(List<Clothes> newData)
    {
        itemDataList = newData;
        InitializeScroller();
    }
}