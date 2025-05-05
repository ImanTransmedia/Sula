using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FilterOption : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualTreeAsset buttonTemplate;

    [SerializeField] private Color defaultBackgroundColor;
    [SerializeField] private Color selectedBackgroundColor;
    [SerializeField] private Color fontDefault;
    [SerializeField] private Color fontSelected;
    [SerializeField] private Color borderColor;
    [SerializeField] private float borderWidth = 3f;

    private ScrollView scrollView;
    private List<Button> buttons = new List<Button>();
    private Dictionary<Button, FilterNames> filterNames = new Dictionary<Button, FilterNames>();
    private Dictionary<Button, bool> buttonStates = new Dictionary<Button, bool>();

    [SerializeField] private InfiniteScroll infiniteScroll;

    private Button hombreButton;
    private Button mujerButton;
    private Button clearAllButton;
    private bool hombreSelected = false;
    private bool mujerSelected = false;


    private VisualElement filterPopPanel;

    private void Start()
    {
        infiniteScroll = GetComponent<InfiniteScroll>();

        defaultBackgroundColor = GameManager.Instance.actualRegion.accentColor;
        selectedBackgroundColor = GameManager.Instance.actualRegion.darkColor;
        fontSelected = GameManager.Instance.actualRegion.accentColor;
        fontDefault = GameManager.Instance.actualRegion.darkColor;
        borderColor = GameManager.Instance.actualRegion.darkColor;
        OnEnable();
    }

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        scrollView = root.Q<ScrollView>("FilterScroll");

        scrollView.Clear();
        buttons.Clear();
        filterNames.Clear();
        buttonStates.Clear();
        hombreButton = null;
        mujerButton = null;
        clearAllButton = null;
        hombreSelected = false;
        mujerSelected = false;

        var buttonTypesList = System.Enum.GetValues(typeof(FilterNames)).Cast<FilterNames>().ToList();

        clearAllButton = new Button { text = "Clear All" };
        clearAllButton.AddToClassList("button-item");
        SetButtonDefaultStyle(clearAllButton);
        SetButtonSelectedStyle(clearAllButton);
        clearAllButton.clicked += ClearAllFilters;
        clearAllButton.RegisterCallback<PointerDownEvent>(evt => { }, TrickleDown.TrickleDown);

        scrollView.Add(clearAllButton);
        buttons.Insert(0, clearAllButton);

        foreach (var type in buttonTypesList)
        {
            Button button;

            if (type == FilterNames.Men || type == FilterNames.Women || type == FilterNames.ClearAll)
                continue;

            else
            {
                button = new Button { text = type.ToString() };
                filterNames[button] = type;
                buttonStates[button] = false;
                button.clicked += () => OnButtonClicked(button);
            }

            button.AddToClassList("button-item");
            SetButtonDefaultStyle(button);
            button.RegisterCallback<PointerDownEvent>(evt => { }, TrickleDown.TrickleDown);

            scrollView.Add(button);
            buttons.Add(button);
        }

        filterPopPanel = root.Q<VisualElement>("FilterPanel");

        InitializeGenderButtons(filterPopPanel);
    }

    public void InitializeGenderButtons(VisualElement container)
    {
        container.Clear();

        hombreButton = new Button { text = "Men" };
        mujerButton = new Button { text = "Women" };

        SetButtonDefaultStyle(hombreButton);
        SetButtonDefaultStyle(mujerButton);

        hombreButton.clicked += () =>
        {
            hombreSelected = !hombreSelected;
            UpdateButtonStyle(hombreButton, hombreSelected);

            // Desactivar Clear All si se selecciona género
            if ((hombreSelected || mujerSelected) && clearAllButton != null)
                UpdateButtonStyle(clearAllButton, false);

            UpdateList();
        };

        mujerButton.clicked += () =>
        {
            mujerSelected = !mujerSelected;
            UpdateButtonStyle(mujerButton, mujerSelected);

            // Desactivar Clear All si se selecciona género
            if ((hombreSelected || mujerSelected) && clearAllButton != null)
                UpdateButtonStyle(clearAllButton, false);

            UpdateList();
        };

        hombreButton.AddToClassList("button-item");
        SetButtonDefaultStyle(hombreButton);

        mujerButton.AddToClassList("button-item");
        SetButtonDefaultStyle(mujerButton);

        container.Add(hombreButton);
        container.Add(mujerButton);
    }



    private void OnButtonClicked(Button clickedButton)
    {
        bool isSelected = !buttonStates[clickedButton];
        buttonStates[clickedButton] = isSelected;
        UpdateButtonStyle(clickedButton, isSelected);

        // Si se selecciona cualquier filtro normal, desactiva Clear All
        if (isSelected && clearAllButton != null)
            UpdateButtonStyle(clearAllButton, false);

        UpdateList();
    }

    private void OnGenderButtonClicked(FilterNames gender)
    {
        if (gender == FilterNames.Men)
        {
            hombreSelected = !hombreSelected;
            if (hombreButton != null)
                UpdateButtonStyle(hombreButton, hombreSelected);
        }
        else if (gender == FilterNames.Women)
        {
            mujerSelected = !mujerSelected;
            if (mujerButton != null)
                UpdateButtonStyle(mujerButton, mujerSelected);
        }

        // Si se selecciona cualquier género, desactiva Clear All
        if ((hombreSelected || mujerSelected) && clearAllButton != null)
            UpdateButtonStyle(clearAllButton, false);

        UpdateList();
    }


    private void ClearAllFilters()
    {
        bool wasSelected = clearAllButton.style.backgroundColor == selectedBackgroundColor;
        bool nowSelected = !wasSelected;

        // Actualiza su estado
        UpdateButtonStyle(clearAllButton, nowSelected);

        if (nowSelected)
        {
            // Si se selecciona, desactiva todos los demás
            foreach (var button in buttonStates.Keys.ToList())
            {
                buttonStates[button] = false;
                UpdateButtonStyle(button, false);
            }

            hombreSelected = false;
            if (hombreButton != null) UpdateButtonStyle(hombreButton, false);

            mujerSelected = false;
            if (mujerButton != null) UpdateButtonStyle(mujerButton, false);
        }

        Debug.Log("Botón Clear All clickeado. Estado: " + (nowSelected ? "seleccionado" : "no seleccionado"));
        UpdateList();
    }


    private void UpdateButtonStyle(Button button, bool isSelected)
    {
        UpdateColorsFromRegion();
                button.style.borderLeftColor = borderColor;
        button.style.borderRightColor = borderColor;
        button.style.borderTopColor = borderColor;
        button.style.borderBottomColor = borderColor;
        if (isSelected)
        {
            button.style.backgroundColor = selectedBackgroundColor;
            button.style.color = fontSelected;
        }
        else
        {
            button.style.backgroundColor = defaultBackgroundColor;
            button.style.color = fontDefault;
        }
    }

    private void SetButtonDefaultStyle(Button button)
    {
        button.style.backgroundColor = defaultBackgroundColor;
        button.style.borderLeftColor = borderColor;
        button.style.borderRightColor = borderColor;
        button.style.borderTopColor = borderColor;
        button.style.borderBottomColor = borderColor;
        button.style.borderLeftWidth = borderWidth;
        button.style.borderRightWidth = borderWidth;
        button.style.borderTopWidth = borderWidth;
        button.style.borderBottomWidth = borderWidth;
        button.style.color = fontDefault;
    }

    private void SetButtonSelectedStyle(Button button)
    {
        button.style.backgroundColor = selectedBackgroundColor;
        button.style.borderLeftColor = borderColor;
        button.style.borderRightColor = borderColor;
        button.style.borderTopColor = borderColor;
        button.style.borderBottomColor = borderColor;
        button.style.borderLeftWidth = borderWidth;
        button.style.borderRightWidth = borderWidth;
        button.style.borderTopWidth = borderWidth;
        button.style.borderBottomWidth = borderWidth;
        button.style.color = fontSelected;
    }

    public void UpdateList()
    {
        List<Clothes> allClothes = new List<Clothes>(GameManager.Instance.actualRegion.clothes);

        // Ignorar ClearAll en la lógica de filtros
        List<FilterNames> selectedFilters = buttonStates
            .Where(pair => pair.Value && filterNames.ContainsKey(pair.Key))
            .Select(pair => filterNames[pair.Key])
            .ToList();

        List<Clothes> filteredClothes;

        bool clearAllActive = clearAllButton != null && clearAllButton.style.backgroundColor == selectedBackgroundColor;

        if (clearAllActive)
        {
            filteredClothes = allClothes;
        }
        else if (selectedFilters.Count == 0 && !hombreSelected && !mujerSelected)
        {
            filteredClothes = allClothes;
        }
        else
        {
            filteredClothes = allClothes.Where(clothe =>
            {
                bool matchesRegularFilters = selectedFilters.Count == 0 || clothe.filterOptions.Any(filter => selectedFilters.Contains(filter));
                bool matchesGenderFilter = (!hombreSelected && !mujerSelected) ||
                                           (hombreSelected && clothe.filterOptions.Contains(FilterNames.Men)) ||
                                           (mujerSelected && clothe.filterOptions.Contains(FilterNames.Women));
                return matchesRegularFilters && matchesGenderFilter;
            }).ToList();
        }

        infiniteScroll.FillInstance(filteredClothes);
        UpdateColorsFromRegion();
        RefreshButtonStyles();
    }


    private void UpdateColorsFromRegion()
    {
        defaultBackgroundColor = GameManager.Instance.actualRegion.accentColor;
        selectedBackgroundColor = GameManager.Instance.actualRegion.darkColor;
        fontSelected = GameManager.Instance.actualRegion.accentColor;
        fontDefault = GameManager.Instance.actualRegion.darkColor;
        borderColor = GameManager.Instance.actualRegion.darkColor;
    }

    private void RefreshButtonStyles()
    {
        foreach (var button in buttons)
        {
            bool isSelected = false;
            if (button == clearAllButton)
                isSelected = clearAllButton.style.backgroundColor == selectedBackgroundColor;
            else if (filterNames.ContainsKey(button))
                isSelected = buttonStates.ContainsKey(button) && buttonStates[button];

            UpdateButtonStyle(button, isSelected);
        }

        if (hombreButton != null)
            UpdateButtonStyle(hombreButton, hombreSelected);
        if (mujerButton != null)
            UpdateButtonStyle(mujerButton, mujerSelected);
    }

}