using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualTreeAsset itemTemplate;
    [SerializeField] private List<Clothes> itemDataList;
    [SerializeField] private int visibleItemsCount = 1;
    [SerializeField] private float itemWidth = 400f;
    [SerializeField] private float itemHeight = 650f;
    [SerializeField] private GameObject detailsPanel;


    [SerializeField] private int bufferItems = 2;
    private VisualElement root;
    private ScrollView scrollView;
    private List<VisualElement> visibleItems = new List<VisualElement>();
    private List<int> dataIndices = new List<int>();
    private int totalItems => itemDataList?.Count ?? 0;
    private float itemFullWidth;
    private bool isInitializing = false;


    public void FillInstance(List<Clothes> clothesList)
    {
        itemDataList = clothesList;
        InitializeScroller();
    }

    private void InitializeScroller()
    {
        if (itemDataList == null || totalItems == 0)
        {
            Debug.LogWarning("No hay datos para mostrar en el scroller");
            // Clear existing elements if re-initializing with no data
            if (scrollView != null)
            {
                scrollView.Clear();
                visibleItems.Clear();
                dataIndices.Clear();
            }
            return;
        }

        isInitializing = true;

        root = uiDocument.rootVisualElement;
        scrollView = root.Q<ScrollView>("ItemScroll");

        if (scrollView == null)
        {
            Debug.LogError("ScrollView con el nombre 'ItemScroll' no encontrado en el UIDocument.");
            isInitializing = false;
            return;
        }

        // Configuración del ScrollView
        scrollView.mode = ScrollViewMode.Horizontal;
        scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;
        scrollView.contentContainer.style.flexDirection = FlexDirection.Row;
        scrollView.contentContainer.style.flexWrap = Wrap.NoWrap;
        scrollView.contentContainer.style.height = Length.Percent(100);

        scrollView.Clear();
        visibleItems.Clear();
        dataIndices.Clear();

        VisualElement dummyItem = itemTemplate.Instantiate();
        scrollView.Add(dummyItem);
        float marginRight = dummyItem.resolvedStyle.marginRight;
        float marginLeft = dummyItem.resolvedStyle.marginLeft;
        itemFullWidth = itemWidth + marginRight + marginLeft;
        scrollView.Remove(dummyItem); // Remove dummy item

        InitializeItems();

        scrollView.horizontalScroller.valueChanged -= OnScrollValueChanged;
        scrollView.horizontalScroller.valueChanged += OnScrollValueChanged;

        CenterInitialView();

        isInitializing = false;
    }

    private void InitializeItems()
    {
        if (totalItems == 0) return;

        int numVisualItems = Mathf.Min(totalItems + bufferItems * 2, totalItems > 0 ? totalItems * 2 : 0);

        if (totalItems < visibleItemsCount + bufferItems * 2)
        {
            numVisualItems = totalItems;
        }
        else
        {
            numVisualItems = visibleItemsCount + bufferItems * 2;
        }
        numVisualItems = Mathf.Max(numVisualItems, visibleItemsCount + bufferItems * 2);


        for (int i = 0; i < numVisualItems; i++)
        {
            int dataIndex = i % totalItems;
            AddNewItemAsync(dataIndex, i);
        }
    }

    private async Task AddNewItemAsync(int dataIndex, int visualPositionIndex)
    {
        if (dataIndex < 0 || dataIndex >= totalItems) return;

        var newItem = itemTemplate.Instantiate();
        newItem.style.width = itemWidth;
        newItem.style.height = itemHeight;
        newItem.style.position = Position.Absolute;
        newItem.style.left = visualPositionIndex * itemFullWidth;
        newItem.AddToClassList("show-item");
        newItem.AddToClassList("hide-item");

        newItem.RegisterCallback<ClickEvent>(evt =>
        {

            int clickedVisualIndex = visibleItems.IndexOf(newItem);
            if (clickedVisualIndex != -1 && clickedVisualIndex < dataIndices.Count)
            {
                int actualDataIndex = dataIndices[clickedVisualIndex];
                Debug.Log($"Item {itemDataList[actualDataIndex].name} clicked (Data Index: {actualDataIndex})");
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.actualClothe = itemDataList[actualDataIndex];
                    root.AddToClassList("hide-up");
                    if (detailsPanel != null)
                    {
                        detailsPanel.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogWarning("GameManager.Instance is not available.");
                }
            }
        });

        scrollView.Add(newItem);
        visibleItems.Add(newItem);
        dataIndices.Add(dataIndex);
        UpdateItemContent(newItem, dataIndex);

        await System.Threading.Tasks.Task.Delay(1);
        newItem.RemoveFromClassList("hide-item");

    }

    private void CenterInitialView()
    {
        if (visibleItems.Count == 0) return;


        float targetX = visibleItems[bufferItems].resolvedStyle.left - (scrollView.resolvedStyle.width / 2f - itemWidth / 2f);
        float minScroll = 0;
        float maxScroll = (visibleItems.Count - (scrollView.resolvedStyle.width / itemFullWidth)) * itemFullWidth;
        targetX = Mathf.Clamp(targetX, minScroll, maxScroll);


        scrollView.scrollOffset = new Vector2(targetX, 0);
        scrollView.schedule.Execute(() => { scrollView.scrollOffset = new Vector2(targetX, 0); }).ExecuteLater(50);
    }


    private void OnScrollValueChanged(float value)
    {
        if (isInitializing || totalItems <= visibleItemsCount || visibleItems.Count == 0) return;

        float scrollPosition = scrollView.scrollOffset.x;
        float viewportWidth = scrollView.resolvedStyle.width;

        foreach (var item in visibleItems)
        {
            float itemLeft = item.resolvedStyle.left;
            float itemRight = itemLeft + itemWidth;

            // Si el item está completamente fuera de la vista (izquierda o derecha)
            if (itemRight < scrollPosition || itemLeft > scrollPosition + viewportWidth)
            {
                if (!item.ClassListContains("hide-item"))
                {
                    item.AddToClassList("hide-item");
                }
            }
            // Si el item está al menos parcialmente en la vista
            else
            {
                if (item.ClassListContains("hide-item"))
                {
                    item.RemoveFromClassList("hide-item");
                }
            }
        }

        // Lógica de reciclaje (mantenerla como está)
        float recycleThreshold = bufferItems * itemFullWidth;

        if (visibleItems[0].resolvedStyle.left + itemFullWidth < scrollPosition - recycleThreshold)
        {
            var itemToMove = visibleItems[0];
            visibleItems.RemoveAt(0);
            dataIndices.RemoveAt(0);

            int newDataIndex = (dataIndices[dataIndices.Count - 1] + 1) % totalItems;
            float newVisualPosition = visibleItems[visibleItems.Count - 1].resolvedStyle.left + itemFullWidth;

            UpdateItemContent(itemToMove, newDataIndex);
            itemToMove.style.left = newVisualPosition;

            visibleItems.Add(itemToMove);
            dataIndices.Add(newDataIndex);
        }

        if (visibleItems.Count > 1 && visibleItems[visibleItems.Count - 1].resolvedStyle.left > scrollPosition + viewportWidth + recycleThreshold)
        {
            var itemToMove = visibleItems[visibleItems.Count - 1];
            visibleItems.RemoveAt(visibleItems.Count - 1);
            dataIndices.RemoveAt(dataIndices.Count - 1);

            int newDataIndex = (dataIndices[0] - 1 + totalItems) % totalItems;
            float newVisualPosition = visibleItems[0].resolvedStyle.left - itemFullWidth;

            UpdateItemContent(itemToMove, newDataIndex);
            itemToMove.style.left = newVisualPosition;

            visibleItems.Insert(0, itemToMove);
            dataIndices.Insert(0, newDataIndex);
        }
    }


    private void UpdateItemContent(VisualElement item, int dataIndex)
    {
        if (dataIndex < 0 || dataIndex >= totalItems) return;


        var itemData = itemDataList[dataIndex];

        var nameLabel = item.Q<Label>("Name");
        if (nameLabel != null)
        {
            nameLabel.text = itemData.clotheName;
        }

        var imageElement = item.Q<VisualElement>("Image");
        if (imageElement != null && itemData.menuImage != null)
        {

            if (itemData.menuImage is Sprite sprite)
            {
                imageElement.style.backgroundImage = new StyleBackground(sprite);
            }
            else
            {
                Debug.LogWarning($"Image data for item {itemData.name} is not a recognized type (Sprite or Texture2D).");
                imageElement.style.backgroundImage = null;
            }

        }
        else if (imageElement != null)
        {

            imageElement.style.backgroundImage = null;
        }
    }


}