using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductScreen : MonoBehaviour
{
    [Header("Default Configuration")]
    [SerializeField] private VisualElement root;

    [Header("Variable Elements")]
    [SerializeField] private VisualElement scrollContainer;
    [SerializeField] private VisualElement backgroundImage;
    [SerializeField] private VisualElement helpButton, settingsButton;
    [SerializeField] private Label regionName;



    [Header("Other Screens")]
    [SerializeField] private GameObject ourStory;
    [SerializeField] private VisualElement ourStoryRoot;
    [SerializeField] private VisualElement ourStoryPanel;

    [SerializeField] private GameObject artisans;
    [SerializeField] private VisualElement artisansRoot;
    [SerializeField] private VisualElement artisansPanel;


    [SerializeField] private RegionType actualRegion;

    [SerializeField] private InfiniteScroll infiniteScroll;


    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        backgroundImage = root.Q<VisualElement>("ImageContainer");
        helpButton = root.Q<VisualElement>("HelpButton");
        settingsButton = root.Q<VisualElement>("SettingsButton");
        regionName = root.Q<Label>("Region");
        scrollContainer = root.Q<VisualElement>("ScrollContainer");



        ourStoryRoot = ourStory.GetComponent<UIDocument>().rootVisualElement;
        ourStoryPanel = ourStoryRoot.Q<VisualElement>("mainContainer");

        artisansRoot = artisans.GetComponent<UIDocument>().rootVisualElement;
        artisansPanel = artisansRoot.Q<VisualElement>("mainContainer");
        
        infiniteScroll = GetComponent<InfiniteScroll>();
    }

    public void NavigateReturn()
    {
        Debug.Log("Return");
    }
    


    // Métodos para acciones específicas de cada dirección
    public void NavigateRight()
    {
        actualRegion = GameManager.Instance.actualRegion.regionType;

        switch (actualRegion)
        {
            case RegionType.Amazonia:
                GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
                Debug.Log("Navigate to Galapagos");
                infiniteScroll.FillInstance(new List<Clothes>(GameManager.Instance.actualRegion.clothes));
                break;
            case RegionType.Andes:
                if (!root.ClassListContains("hide-right"))
                {
                    root.AddToClassList("hide-right");
                }
                if (ourStoryPanel.ClassListContains("hide-left"))
                {
                    ourStoryPanel.RemoveFromClassList("hide-left");
                }
                break;
            case RegionType.Galapagos:
                GameManager.Instance.actualRegion = GameManager.Instance.regions[1];
                Debug.Log("Navigate to Andes");
                infiniteScroll.FillInstance(new List<Clothes>(GameManager.Instance.actualRegion.clothes));
                break;
        }
        
        backgroundImage.style.backgroundImage = new StyleBackground(GameManager.Instance.actualRegion.imagen);
        regionName.text = GameManager.Instance.actualRegion.regionName;
        helpButton.style.backgroundColor = GameManager.Instance.actualRegion.darkColor;
        settingsButton.style.backgroundColor = GameManager.Instance.actualRegion.darkColor;
        scrollContainer.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.accentColor;
    }

    public void Navigateleft()
    {
        actualRegion = GameManager.Instance.actualRegion.regionType;

        switch (actualRegion)
        {
            case RegionType.Amazonia:
                if (!root.ClassListContains("hide-left"))
                {
                    root.AddToClassList("hide-left");
                }
                if (artisansPanel.ClassListContains("hide-right"))
                {
                    artisansPanel.RemoveFromClassList("hide-right");
                }
                break;
            case RegionType.Andes:
                GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
                Debug.Log("Navigate to Galapagos");
                infiniteScroll.FillInstance(new List<Clothes>(GameManager.Instance.actualRegion.clothes));
                break;
            case RegionType.Galapagos:
                GameManager.Instance.actualRegion = GameManager.Instance.regions[2];
                Debug.Log("Navigate to Amazonia");
                infiniteScroll.FillInstance(new List<Clothes>(GameManager.Instance.actualRegion.clothes));
                break;
        }
        backgroundImage.style.backgroundImage = new StyleBackground(GameManager.Instance.actualRegion.imagen);
        regionName.text = GameManager.Instance.actualRegion.regionName;
        helpButton.style.backgroundColor = GameManager.Instance.actualRegion.darkColor;
        settingsButton.style.backgroundColor = GameManager.Instance.actualRegion.darkColor;
        scrollContainer.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.accentColor;
    }

}
