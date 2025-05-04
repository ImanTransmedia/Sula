using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonGenerator : MonoBehaviour
{
    [Header("References")]
    public Transform buttonContainer; 
    public GameObject buttonPrefab;   

    [Header("Colors")]
    public Color[] colors;

    [Header("Settings")]
    public Vector2 buttonSize = new Vector2(100, 100);
    public float spacing = 10f;
    public float margin = 20f;

    [Header("Debug")]
    public bool debugMode = true;

    private List<GameObject> createdButtons = new List<GameObject>();



    public void GenerateColorButtons()
    {
        // Limpiar botones existentes
        ClearButtons();

        if (buttonContainer == null || buttonPrefab == null || colors == null || colors.Length == 0)
        {
            Debug.LogError("Configuraci�n incompleta para generar botones");
            return;
        }

        // Calcular layout
        RectTransform containerRect = buttonContainer.GetComponent<RectTransform>();
        float containerWidth = containerRect.rect.width;
        int buttonsPerRow = Mathf.FloorToInt((containerWidth - margin * 2 + spacing) / (buttonSize.x + spacing));

        if (debugMode) Debug.Log($"Generando {colors.Length} botones, {buttonsPerRow} por fila");

        // Crear botones
        for (int i = 0; i < colors.Length; i++)
        {
            // Calcular posici�n
            int row = Mathf.FloorToInt(i / buttonsPerRow);
            int col = i % buttonsPerRow;

            Vector2 position = new Vector2(
                margin + col * (buttonSize.x + spacing),
                -margin - row * (buttonSize.y + spacing)
            );

            // Instanciar bot�n
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
            RectTransform buttonRect = newButton.GetComponent<RectTransform>();

            // Configurar tama�o y posici�n
            buttonRect.anchoredPosition = position;
            buttonRect.sizeDelta = buttonSize;

            // Asignar color
            Image buttonImage = newButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = colors[i];
                if (debugMode) Debug.Log($"Asignando color {colors[i]} al bot�n {i}");
            }

            // Configurar bot�n (opcional)
            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                int index = i; // Copia para el closure
                buttonComponent.onClick.AddListener(() => OnColorButtonClick(index));
            }

            createdButtons.Add(newButton);
        }

        if (debugMode) Debug.Log($"Generaci�n completada. Total de botones: {createdButtons.Count}");
    }

    private void OnColorButtonClick(int colorIndex)
    {
        if (debugMode) Debug.Log($"Bot�n de color clickeado - �ndice: {colorIndex}, Color: {colors[colorIndex]}");
        // Aqu� puedes a�adir funcionalidad cuando se selecciona un color
    }

    public void ClearButtons()
    {
        foreach (GameObject button in createdButtons)
        {
            Destroy(button);
        }
        createdButtons.Clear();

        if (debugMode) Debug.Log("Todos los botones han sido eliminados");
    }

    // M�todo para actualizar colores en tiempo de ejecuci�n
    public void UpdateColors(Color[] newColors)
    {
        colors = newColors;
        GenerateColorButtons();
    }
}

