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
    private VisualElement gradientleft;
    private VisualElement gradientright;
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
    private VisualElement button;
    private VisualElement buttonSelected;
    private VisualElement leaflement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = uiDocument.rootVisualElement;
        gradientleft = root.Q<VisualElement>("GradientLeft");
        gradientright = root.Q<VisualElement>("GradientRight");
        clothesPanel = root.Q<VisualElement>("ScrollContainer");
        helpButton = root.Q<VisualElement>("HelpButton");
        settingsButton = root.Q<VisualElement>("SettingsButton");



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
        gradientleft.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.accentColor;
        gradientright.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.accentColor;
        clothesPanel.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.accentColor;
        helpButton.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
        settingsButton.style.backgroundColor = GameManager.Instance.actualRegion.darkColor;
        instructionPanel.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.accentColor;
        hand.style.unityBackgroundImageTintColor= GameManager.Instance.actualRegion.darkColor;
        hand2.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
        hand3.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
        arrow.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
        arrow2.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;
        arrow3.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
