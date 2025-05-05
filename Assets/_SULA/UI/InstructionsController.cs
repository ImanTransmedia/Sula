using UnityEngine;
using UnityEngine.UIElements;

public class InstructionsController : MonoBehaviour
{

    private VisualElement root;
    private VisualElement instructionsButton;
    private VisualElement returnPanel;
    private VisualElement instructionsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        instructionsPanel = root.Q<VisualElement>("InstructionsScreen");

        instructionsButton = root.Q<VisualElement>("HelpButton");
        instructionsButton.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("HitHelp");
            instructionsPanel.ToggleInClassList("hide-panel");
        });

        returnPanel = root.Q<VisualElement>("ReturnPanel");
        returnPanel.RegisterCallback<ClickEvent>(evt =>
        {
            instructionsPanel.ToggleInClassList("hide-panel");
        });


    }

}
