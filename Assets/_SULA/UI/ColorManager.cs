using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorManager : MonoBehaviour
{

    // Singleton instance
    private static ColorManager _instance;
    public static ColorManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindFirstObjectByType<ColorManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("NavigationManager");
                    _instance = go.AddComponent<ColorManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    //----------------------------------------------------
    [SerializeField] private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement clothesPanel;
    private VisualElement helpButton;
    private VisualElement settingsButton;
    private VisualElement instructionPanel;
    private VisualElement hand;
    private VisualElement hand2;
    private VisualElement hand3;
    private VisualElement arrow;
    private VisualElement arrow2;
    private VisualElement arrow3;
    private VisualElement filterPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private VisualElement regionName;

    private List<VisualElement> allHelpButtons;
    void Start()
    {
        root = uiDocument.rootVisualElement;
        clothesPanel = root.Q<VisualElement>("ScrollContainer");
        helpButton = root.Q<VisualElement>("HelpButton");

         allHelpButtons = root.Query<VisualElement>().Where(e => e.name == "HelpButton").ToList();



        settingsButton = root.Q<VisualElement>("SettingsButton");

        regionName = root.Q<VisualElement>("DetailRegionName");

        filterPanel = root.Q<VisualElement>("FilterPanel");


        instructionPanel = root.Q<VisualElement>("instructionsPanel");
        hand = root.Q<VisualElement>("Mano");
        hand2 = root.Q<VisualElement>("Mano1");
        hand3 = root.Q<VisualElement>("Mano2");
        arrow = root.Q<VisualElement>("Flecha");
        arrow2 = root.Q<VisualElement>("Flecha1");
        arrow3 = root.Q<VisualElement>("Flecha2");

        UpdateColors();

    }

    public void UpdateColors()
    {
        // Update the colors of the UI elements
        clothesPanel.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.accentColor;
        foreach (var btn in allHelpButtons)
        {
            if (NavigationManager.Instance.actualScren == ScreenOptions.Artisans || NavigationManager.Instance.actualScren == ScreenOptions.OurStory)
            {
                btn.style.unityBackgroundImageTintColor = GameManager.Instance.regions[0].darkColor;
            }
            else
            {
                btn.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
            }
        }
        settingsButton.style.backgroundColor = GameManager.Instance.actualRegion.darkColor;
        if (NavigationManager.Instance.actualScren == ScreenOptions.Artisans || NavigationManager.Instance.actualScren == ScreenOptions.OurStory)
        {
            instructionPanel.style.unityBackgroundImageTintColor = GameManager.Instance.regions[0].accentColor;
            hand.style.unityBackgroundImageTintColor = GameManager.Instance.regions[0].darkColor;
            hand2.style.unityBackgroundImageTintColor = GameManager.Instance.regions[0].darkColor;
            hand3.style.unityBackgroundImageTintColor = GameManager.Instance.regions[0].darkColor;
            arrow.style.unityBackgroundImageTintColor = GameManager.Instance.regions[0].darkColor;
            arrow2.style.unityBackgroundImageTintColor = GameManager.Instance.regions[0].darkColor;
            arrow3.style.unityBackgroundImageTintColor = GameManager.Instance.regions[0].darkColor; 
        }
        else
        {
            instructionPanel.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.accentColor;
            hand.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
            hand2.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
            hand3.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
            arrow.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
            arrow2.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
            arrow3.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
        }

        regionName.style.color = GameManager.Instance.actualRegion.darkColor;
        filterPanel.style.backgroundColor = GameManager.Instance.actualRegion.accentColor;
        filterPanel.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.accentColor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
