using UnityEngine;
using UnityEngine.UIElements;

public class NavigationManager : MonoBehaviour
{
    // Singleton instance
    private static NavigationManager _instance;
    public static NavigationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindFirstObjectByType<NavigationManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("NavigationManager");
                    _instance = go.AddComponent<NavigationManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    //----------------------------------------------------

    [SerializeField] private UIDocument uiDocument;
    public ScreenOptions actualScren = ScreenOptions.Start;

    private VisualElement startContainer;
    private VisualElement clotheContainer;
    private VisualElement galapagosContainer;
    private VisualElement amazoniaContainer;
    private VisualElement andesContainer;
    private VisualElement ourStoryContainer;
    private VisualElement artisansContainer;
    private GameObject detailsContainer;



    private void Start()
    {
        var root = uiDocument.rootVisualElement;

        startContainer = root.Q<VisualElement>("StartScreen");
        clotheContainer = root.Q<VisualElement>("ClothesScreen");
        galapagosContainer = root.Q<VisualElement>("GalapagosScreen");
        andesContainer = root.Q<VisualElement>("AndesScreen");
        amazoniaContainer = root.Q<VisualElement>("AmazoniaScreen");
        ourStoryContainer = root.Q<VisualElement>("OurStoryScreen");
        artisansContainer = root.Q<VisualElement>("ArtisansScreen");

    }

    public void ResetReturn()
    {
        galapagosContainer.ClearClassList();
        galapagosContainer.AddToClassList("hide-down");
        clotheContainer.ClearClassList();
        clotheContainer.AddToClassList("show");
        clotheContainer.AddToClassList("hide-down");
        andesContainer.ClearClassList();
        andesContainer.AddToClassList("hide-left");
        amazoniaContainer.ClearClassList();
        amazoniaContainer.AddToClassList("hide-right");
        ourStoryContainer.ClearClassList();
        ourStoryContainer.AddToClassList("hide-left");
        artisansContainer.ClearClassList();
        artisansContainer.AddToClassList("hide-right");
    }

    public void StartToGalapagos()
    {
        startContainer.AddToClassList("hide-up");
        galapagosContainer.ClearClassList();
        galapagosContainer.AddToClassList("show");
        clotheContainer.ClearClassList();
        clotheContainer.AddToClassList("show");
        actualScren= ScreenOptions.Galapagos;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
    }


    public void GalapagosToStart()
    {
        galapagosContainer.AddToClassList("hide-down");
        clotheContainer.AddToClassList("hide-down");
        startContainer.ClearClassList();
        startContainer.AddToClassList("show");
        actualScren = ScreenOptions.Start;
    }

    public void AndesToStart()
    {
        andesContainer.AddToClassList("hide-down");
        clotheContainer.AddToClassList("hide-down");
        startContainer.ClearClassList();
        startContainer.AddToClassList("show");
        actualScren = ScreenOptions.Start;
    }



    public void AmazoniaToStart()
    {
        amazoniaContainer.AddToClassList("hide-down");
        clotheContainer.AddToClassList("hide-down");
        startContainer.ClearClassList();
        startContainer.AddToClassList("show");
        actualScren = ScreenOptions.Start;
    }

    public void OurStoryToStart()
    {
        ourStoryContainer.AddToClassList("hide-down");
        startContainer.ClearClassList();
        startContainer.AddToClassList("show");
        actualScren = ScreenOptions.Start;
    }

    public void ArtisansToStart()
    {
        artisansContainer.AddToClassList("hide-down");
        startContainer.ClearClassList();
        startContainer.AddToClassList("show");
        actualScren = ScreenOptions.Start;
    }



    public void GalapagosToAmazonia()
    {
        galapagosContainer.AddToClassList("hide-left");
        amazoniaContainer.ClearClassList();
        amazoniaContainer.AddToClassList("show");
        actualScren = ScreenOptions.Amazonia;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[2];


    }

    public void GalapagosToAndes()
    {
        galapagosContainer.AddToClassList("hide-right");
        andesContainer.ClearClassList();
        andesContainer.AddToClassList("show");
        actualScren = ScreenOptions.Andes;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[1];

    }

    public void AndesToGalapagos()
    {
        andesContainer.AddToClassList("hide-left");
        galapagosContainer.ClearClassList();
        galapagosContainer.AddToClassList("show");
        actualScren = ScreenOptions.Galapagos;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];


    }
    public void AndesToOurStory()
    {
        andesContainer.AddToClassList("hide-right");
        clotheContainer.AddToClassList("hide-right");
        ourStoryContainer.ClearClassList();
        ourStoryContainer.AddToClassList("show");
        actualScren = ScreenOptions.OurStory;
    }

    public void AmazoniaToGalapagos()
    {
        amazoniaContainer.AddToClassList("hide-right");
        galapagosContainer.ClearClassList();
        galapagosContainer.AddToClassList("show");
        actualScren = ScreenOptions.Galapagos;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];

    }

    public void AmazoniaToArtisans()
    {
        amazoniaContainer.AddToClassList("hide-left");
        artisansContainer.ClearClassList();
        artisansContainer.AddToClassList("show");
        actualScren = ScreenOptions.Artisans;
    }



    public void OurStoryToAndes()
    {
        ourStoryContainer.AddToClassList("hide-left");
        andesContainer.ClearClassList();
        andesContainer.AddToClassList("show");
        clotheContainer.ClearClassList();
        clotheContainer.AddToClassList("show");
        actualScren = ScreenOptions.Andes;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[1];
    }

    public void ArtisansToAmazonia()
    {
        artisansContainer.AddToClassList("hide-right");
        amazoniaContainer.ClearClassList();
        amazoniaContainer.AddToClassList("show");
        clotheContainer.ClearClassList();
        clotheContainer.AddToClassList("show");
        actualScren = ScreenOptions.Amazonia;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[2];
    }




    public void ReturnFrom(ScreenOptions actualScreen)
    {
        Debug.Log("Return Start");
        switch (actualScreen) {
            case ScreenOptions.Artisans:
                ArtisansToStart();
                break;
            case ScreenOptions.OurStory:
                OurStoryToStart();
                break;
            case ScreenOptions.Galapagos:
                GalapagosToStart();
                break;
            case ScreenOptions.Amazonia:
                AmazoniaToStart();
                break;
            case ScreenOptions.Andes:
                AndesToStart();
                break;
            case ScreenOptions.Details:
                break;
            case ScreenOptions.Start:
                break;
            default:
                break;
        }

        //ResetReturn();
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
        ColorManager.Instance.UpdateColors();



    }


    public void GoRight(ScreenOptions actualScreen)
    {
        switch (actualScreen)
        {
            case ScreenOptions.Artisans:
                break;
            case ScreenOptions.OurStory:
                OurStoryToAndes();
                break;
            case ScreenOptions.Galapagos:
                Debug.Log("Go Amazonia");
                GalapagosToAmazonia();
                break;
            case ScreenOptions.Amazonia:
                AmazoniaToArtisans();
                break;
            case ScreenOptions.Andes:
                Debug.Log("Go Galapagos");
                AndesToGalapagos();
                break;
            case ScreenOptions.Details:
                break;
            case ScreenOptions.Start:
                break;
            default:
                break;
        }
        ColorManager.Instance.UpdateColors();


    }

    public void GoLeft(ScreenOptions actualScreen)
    {
        switch (actualScreen)
        {
            case ScreenOptions.Artisans:
                ArtisansToAmazonia();
                break;
            case ScreenOptions.OurStory:
                break;
            case ScreenOptions.Galapagos:
                Debug.Log("Go Andes");
                GalapagosToAndes();
                break;
            case ScreenOptions.Amazonia:
                Debug.Log("Go Galapagos");
                AmazoniaToGalapagos();
                break;
            case ScreenOptions.Andes:
                AndesToOurStory();
                break;
            case ScreenOptions.Details:
                break;
            case ScreenOptions.Start:
                break;
            default:
                break;
        }
        ColorManager.Instance.UpdateColors();

    }

}



public enum ScreenOptions
{
    Start,
    Galapagos,
    Andes,
    Amazonia,
    OurStory,
    Artisans,
    Details
}
