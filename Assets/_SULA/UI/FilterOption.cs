using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FilterOption : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualTreeAsset buttonTemplate;

    private ScrollView scrollView;
    private List<Button> buttons = new List<Button>();
    private Dictionary<Button, FilterNames> filterNames = new Dictionary<Button, FilterNames>();
    private Dictionary<Button, bool> buttonStates = new Dictionary<Button, bool>();


    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        scrollView = root.Q<ScrollView>("FilterScroll");

        // Limpiar cualquier contenido existente
        scrollView.Clear();
        buttons.Clear();
        filterNames.Clear();
        buttonStates.Clear();

        // Obtener todos los valores de la enumeración
        var buttonTypesList = System.Enum.GetValues(typeof(FilterNames)).Cast<FilterNames>().ToList();

        // Crear un botón para cada tipo en la enumeración
        foreach (var type in buttonTypesList)
        {
            var button = new Button();
            button.AddToClassList("button-item");
            button.text = type.ToString();

            // Almacenar el tipo de botón
            filterNames[button] = type;
            buttonStates[button] = false; // Inicialmente no seleccionado

            // Configurar el evento de clic
            button.clicked += () => OnButtonClicked(button);

            // Agregar al scroll view y a la lista
            scrollView.Add(button);
            buttons.Add(button);
        }
    }

    private void OnButtonClicked(Button clickedButton)
    {
        // Cambiar el estado del botón
        bool isSelected = !buttonStates[clickedButton];
        buttonStates[clickedButton] = isSelected;

        // Cambiar la clase CSS según el estado
        if (isSelected)
        {
            clickedButton.AddToClassList("selected");
        }
        else
        {
            clickedButton.RemoveFromClassList("selected");
        }

        // Aquí puedes agregar lógica adicional basada en el tipo de botón
        FilterNames type = filterNames[clickedButton];
        Debug.Log($"Botón {type} clickeado. Estado: {(isSelected ? "seleccionado" : "no seleccionado")}");
    }
}

