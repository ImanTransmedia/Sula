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

        var allHelpButtons = root.Query<VisualElement>().Where(e => e.name == "HelpButton").ToList();

        foreach (var btn in allHelpButtons)
        {
            btn.RegisterCallback<ClickEvent>(evt => {
                instructionsPanel.ToggleInClassList("hide-panel");
            });
        }


        returnPanel = root.Q<VisualElement>("ReturnPanel");
        returnPanel.RegisterCallback<ClickEvent>(evt =>
        {
            instructionsPanel.ToggleInClassList("hide-panel");
        });


    }

}
