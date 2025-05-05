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
        scrollView.Add(clearAllButton);
        buttons.Insert(0, clearAllButton);

        foreach (var type in buttonTypesList)
        {
            Button button;

            if (type == FilterNames.Men)
            {
                button = new Button { text = "Men" };
                hombreButton = button;
                button.clicked += () => OnGenderButtonClicked(FilterNames.Men);
            }
            else if (type == FilterNames.Women)
            {
                button = new Button { text = "Women" };
                mujerButton = button;
                button.clicked += () => OnGenderButtonClicked(FilterNames.Women);
            }
            else
            {
                button = new Button { text = type.ToString() };
                filterNames[button] = type;
                buttonStates[button] = false;
                button.clicked += () => OnButtonClicked(button);
            }

            button.AddToClassList("button-item");
            SetButtonDefaultStyle(button);
            scrollView.Add(button);
            buttons.Add(button);
        }
    }


    private void OnButtonClicked(Button clickedButton)
    {
        // Cambiar el estado del botón
        bool isSelected = !buttonStates[clickedButton];
        buttonStates[clickedButton] = isSelected;

        // Cambiar el estilo CSS según el estado
        UpdateButtonStyle(clickedButton, isSelected);

        FilterNames type = filterNames[clickedButton];
        Debug.Log($"Botón {type} clickeado. Estado: {(isSelected ? "seleccionado" : "no seleccionado")}");

        // Actualizar la lista cada vez que se hace clic en un botón de filtro
        UpdateList();
    }

    private void OnGenderButtonClicked(FilterNames gender)
    {
        if (gender == FilterNames.Men)
        {
            hombreSelected = !hombreSelected;
            if (hombreButton != null)
            {
                UpdateButtonStyle(hombreButton, hombreSelected);
                Debug.Log($"Botón Hombre clickeado. Estado: {(hombreSelected ? "seleccionado" : "no seleccionado")}");
            }
        }
        else if (gender == FilterNames.Women)
        {
            mujerSelected = !mujerSelected;
            if (mujerButton != null)
            {
                UpdateButtonStyle(mujerButton, mujerSelected);
                Debug.Log($"Botón Mujer clickeado. Estado: {(mujerSelected ? "seleccionado" : "no seleccionado")}");
            }
        }
        UpdateList();
    }

    private void ClearAllFilters()
    {
        // Desmarcar todos los botones de filtro regulares
        foreach (var button in buttonStates.Keys.ToList())
        {
            buttonStates[button] = false;
            UpdateButtonStyle(button, false);
        }

        // Desmarcar los botones de género
        hombreSelected = false;
        if (hombreButton != null)
        {
            UpdateButtonStyle(hombreButton, false);
        }
        mujerSelected = false;
        if (mujerButton != null)
        {
            UpdateButtonStyle(mujerButton, false);
        }

        // Marcar el botón "Clear All" como seleccionado
        if (clearAllButton != null)
        {
            UpdateButtonStyle(clearAllButton, true);
        }

        Debug.Log("Botón Clear All clickeado. Todos los filtros deseleccionados.");
        UpdateList();
    }

    private void UpdateButtonStyle(Button button, bool isSelected)
    {
        if (isSelected)
        {
            button.style.backgroundColor = selectedBackgroundColor;
        }
        else
        {
            button.style.backgroundColor = defaultBackgroundColor;
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

        defaultBackgroundColor = GameManager.Instance.actualRegion.accentColor;
        selectedBackgroundColor = GameManager.Instance.actualRegion.darkColor;
        fontSelected = GameManager.Instance.actualRegion.accentColor;
        fontDefault = GameManager.Instance.actualRegion.darkColor;
        borderColor = GameManager.Instance.actualRegion.darkColor;



        List<Clothes> allClothes = new List<Clothes>(GameManager.Instance.actualRegion.clothes);

        List<FilterNames> selectedFilters = buttonStates
            .Where(pair => pair.Value)
            .Select(pair => filterNames[pair.Key])
            .ToList();

        List<Clothes> filteredClothes;

        if (selectedFilters.Count == 0 && !hombreSelected && !mujerSelected)
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
    }
}