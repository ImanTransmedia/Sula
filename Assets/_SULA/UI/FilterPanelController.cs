using UnityEngine;
using UnityEngine.UIElements;

public class FilterPanelController : MonoBehaviour
{
    private VisualElement root;
    private VisualElement filterButton;
    private VisualElement returnPanel;
    private VisualElement filterPanel;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        filterPanel = root.Q<VisualElement>("FilterScreen");

        filterButton = root.Q<VisualElement>("SettingsButton");
        filterButton.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("Hit filter");
            filterPanel.ToggleInClassList("hide-panel");
        });

        returnPanel = root.Q<VisualElement>("ReturnFilterPanel");
        returnPanel.RegisterCallback<ClickEvent>(evt =>
        {
            filterPanel.ToggleInClassList("hide-panel");
        });


    }
}
