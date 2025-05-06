using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private InfiniteScroll infiniteScroll;

    private VisualElement genderFilterPanel;
    private VisualElement categoryFilterPanel;

    private List<Button> buttons = new List<Button>();
    private Dictionary<Button, FilterNames> filterNames = new Dictionary<Button, FilterNames>();
    private Dictionary<Button, bool> buttonStates = new Dictionary<Button, bool>();

    private Button hombreButton;
    private Button mujerButton;
    private Button clearAllButton;
    private bool hombreSelected = false;
    private bool mujerSelected = false;

    private void Start()
    {
        infiniteScroll = GetComponent<InfiniteScroll>();

        UpdateColorsFromRegion();
        OnEnable();
    }

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        genderFilterPanel = root.Q<VisualElement>("FilterContainer");
        categoryFilterPanel = root.Q<VisualElement>("FilterPanel");

        genderFilterPanel.Clear();
        categoryFilterPanel.Clear();
        buttons.Clear();
        filterNames.Clear();
        buttonStates.Clear();
        hombreButton = null;
        mujerButton = null;
        clearAllButton = null;
        hombreSelected = false;
        mujerSelected = false;

        var buttonTypesList = System.Enum.GetValues(typeof(FilterNames)).Cast<FilterNames>().ToList();

        // Clear All
        clearAllButton = new Button { text = "Clear All" };
        clearAllButton.AddToClassList("button-item");
        SetButtonDefaultStyle(clearAllButton);
        SetButtonSelectedStyle(clearAllButton);
        clearAllButton.clicked += ClearAllFilters;
        genderFilterPanel.Add(clearAllButton);
        buttons.Add(clearAllButton);

        // Gender Buttons
        InitializeGenderButtons(genderFilterPanel);

        // Other Filters
        foreach (var type in buttonTypesList)
        {
            if (type == FilterNames.Men || type == FilterNames.Women || type == FilterNames.ClearAll)
                continue;

            var button = new Button { text = type.ToString() };
            button.AddToClassList("button-item");
            SetButtonDefaultStyle(button);

            filterNames[button] = type;
            buttonStates[button] = false;
            button.clicked += () => OnButtonClicked(button);

            categoryFilterPanel.Add(button);
            buttons.Add(button);
        }
    }

    private void InitializeGenderButtons(VisualElement container)
    {
        hombreButton = new Button { text = "Men" };
        mujerButton = new Button { text = "Women" };

        SetButtonDefaultStyle(hombreButton);
        SetButtonDefaultStyle(mujerButton);

        hombreButton.clicked += () =>
        {
            hombreSelected = !hombreSelected;
            UpdateButtonStyle(hombreButton, hombreSelected);

            if ((hombreSelected || mujerSelected) && clearAllButton != null)
                UpdateButtonStyle(clearAllButton, false);

            UpdateList();
        };

        mujerButton.clicked += () =>
        {
            mujerSelected = !mujerSelected;
            UpdateButtonStyle(mujerButton, mujerSelected);

            if ((hombreSelected || mujerSelected) && clearAllButton != null)
                UpdateButtonStyle(clearAllButton, false);

            UpdateList();
        };

        hombreButton.AddToClassList("button-item");
        mujerButton.AddToClassList("button-item");

        container.Add(hombreButton);
        container.Add(mujerButton);
    }

    private void OnButtonClicked(Button clickedButton)
    {
        bool isSelected = !buttonStates[clickedButton];
        buttonStates[clickedButton] = isSelected;
        UpdateButtonStyle(clickedButton, isSelected);

        if (isSelected && clearAllButton != null)
            UpdateButtonStyle(clearAllButton, false);

        UpdateList();
    }

    private void ClearAllFilters()
    {
        bool wasSelected = clearAllButton.style.backgroundColor == selectedBackgroundColor;
        bool nowSelected = !wasSelected;

        UpdateButtonStyle(clearAllButton, nowSelected);

        if (nowSelected)
        {
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

        Debug.Log("Clear All clicked. State: " + (nowSelected ? "selected" : "not selected"));
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
        button.style.color = fontSelected;
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

    public void UpdateList()
    {
        List<Clothes> allClothes = new List<Clothes>(GameManager.Instance.actualRegion.clothes);

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
}
