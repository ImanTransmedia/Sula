using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

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
    [SerializeField] private GameObject UIComponent;

    [SerializeField] private InfiniteScroll infiniteScroll;
    [SerializeField] private FilterOption filterOptions;


    [SerializeField] private VideoPlayer videoPlayer;



    public ScreenOptions actualScren = ScreenOptions.Start;

    private VisualElement startContainer;
    private VisualElement clotheContainer;
    private VisualElement galapagosContainer;
    private VisualElement amazoniaContainer;
    private VisualElement andesContainer;
    private VisualElement ourStoryContainer;
    private VisualElement artisansContainer;
    private VisualElement detailContainer;
    private DetailScreen detailReference;



    private VisualElement rightAnimation;
    private VisualElement leftAnimation;


    private ScrollView artisansScroll;
    private ScrollView ourStoryScroll;
    private ScrollView detailsScroll;

    private VisualElement dotsContainer;

    private Coroutine animationHintCoroutine;
    private float animationHintDelay = 5f;


    private List<VisualElement> dots = new List<VisualElement>();


    private void UpdateDots(ScreenOptions screen)
    {


        Color darkColor = GameManager.Instance.actualRegion != null 
            ? screen == ScreenOptions.Artisans || screen == ScreenOptions.OurStory
            ? GameManager.Instance.regions[0].darkColor
            : GameManager.Instance.actualRegion.darkColor
            : GameManager.Instance.regions[0].darkColor;

        int index = screen switch
        {
            ScreenOptions.Galapagos => 2,
            ScreenOptions.Amazonia => 3,
            ScreenOptions.Artisans => 4,
            ScreenOptions.Andes => 1,
            ScreenOptions.OurStory => 0,
            _ => -1,
        };

        for (int i = 0; i < dots.Count; i++)
        {
            var dot = dots[i];

            // Aplicar color de borde dinámico
            dot.style.borderTopColor = darkColor;
            dot.style.borderBottomColor = darkColor;
            dot.style.borderLeftColor = darkColor;
            dot.style.borderRightColor = darkColor;
            dot.style.unityBackgroundImageTintColor = Color.clear;

            if (i == index)
            {
                dot.AddToClassList("active");
                dot.style.backgroundColor = darkColor;
            }
            else
            {
                dot.RemoveFromClassList("active");
                dot.style.backgroundColor = Color.clear;
            }
        }
    }


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
        detailContainer = root.Q<VisualElement>("DetailsScreen");

        rightAnimation = root.Q<VisualElement>("RighAnimation");
        leftAnimation = root.Q<VisualElement>("LefthAnimation");

        detailReference = UIComponent.GetComponent<DetailScreen>();

        artisansScroll = root.Q<ScrollView>("ArtisansScroll");
        ourStoryScroll = root.Q<ScrollView>("OurStoryScroll");
        detailsScroll = root.Q<ScrollView>("DetailScroll");



        startContainer.RegisterCallback<TransitionEndEvent>(evt =>
        {
            if (evt.stylePropertyNames.Contains("opacity") || evt.stylePropertyNames.Contains("transform"))
            {
                if (startContainer.ClassListContains("show") && actualScren == ScreenOptions.Start)
                {
                    
                    ResetReturn();
                    videoPlayer.Play();
                }
            }
        });

        ResetInactivityTimer();


       dotsContainer = root.Q<VisualElement>("DotsContainer");

        if (dotsContainer == null)
        {
            Debug.LogError("No se encontró el contenedor de puntos (DotsContainer). Revisa el UXML.");
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            var dot = dotsContainer.Q<VisualElement>($"Dot{i}");
            if (dot != null)
            {
                dots.Add(dot);
            }
            else
            {
                Debug.LogWarning($"No se encontró el punto Dot{i} dentro de DotsContainer.");
            }
        }

        UpdateDots(ScreenOptions.Start);

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
        detailContainer.ClearClassList();
        detailContainer.AddToClassList("hide-down");
        dotsContainer.ClearClassList();
        dotsContainer.AddToClassList("hide-down");

    }


    // START
    public void StartToGalapagos()
    {
        startContainer.AddToClassList("hide-up");
        galapagosContainer.ClearClassList();
        galapagosContainer.AddToClassList("show");
        clotheContainer.ClearClassList();
        clotheContainer.AddToClassList("show");
        dotsContainer.ClearClassList();
        dotsContainer.AddToClassList("show");
        actualScren = ScreenOptions.Galapagos;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
        ColorManager.Instance.UpdateColors();
        infiniteScroll.FillInstance(new List<Clothes>(GameManager.Instance.actualRegion.clothes));
        filterOptions.UpdateList();
        UpdateDots(actualScren);



    }

    public void BackToStart()
    {
        startContainer.ClearClassList();
        startContainer.AddToClassList("show");
        galapagosContainer.AddToClassList("hide-down");
        clotheContainer.AddToClassList("hide-down");
        andesContainer.AddToClassList("hide-down");
        amazoniaContainer.AddToClassList("hide-down");
        ourStoryContainer.AddToClassList("hide-down");
        artisansContainer.AddToClassList("hide-down");
        detailContainer.AddToClassList("hide-down");
        actualScren = ScreenOptions.Start;
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
        clotheContainer.AddToClassList("hide-down");

        startContainer.ClearClassList();
        startContainer.AddToClassList("show");
        actualScren = ScreenOptions.Start;
    }

    public void ArtisansToStart()
    {
        artisansContainer.AddToClassList("hide-down");
        clotheContainer.AddToClassList("hide-down");

        startContainer.ClearClassList();
        startContainer.AddToClassList("show");
        actualScren = ScreenOptions.Start;
    }



    // GALAPAGOS

    public void GalapagosToAmazonia()
    {
        galapagosContainer.AddToClassList("hide-left");
        amazoniaContainer.ClearClassList();
        amazoniaContainer.AddToClassList("show");
        actualScren = ScreenOptions.Amazonia;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[2];
        filterOptions.UpdateList();
        UpdateDots(actualScren);


    }

    public void GalapagosToAndes()
    {
        galapagosContainer.AddToClassList("hide-right");
        andesContainer.ClearClassList();
        andesContainer.AddToClassList("show");
        actualScren = ScreenOptions.Andes;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[1];
        filterOptions.UpdateList();
        UpdateDots(actualScren);


    }

    public void GalapagosToDetails()
    {
        galapagosContainer.AddToClassList("hide-up");
        clotheContainer.AddToClassList("hide-up");
        detailContainer.ClearClassList();
        detailContainer.AddToClassList("show");
        dotsContainer.ClearClassList();
        dotsContainer.AddToClassList("hide-up");
        actualScren = ScreenOptions.Details;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
        detailReference.ActiveLoad();
        if (detailsScroll != null)
            detailsScroll.scrollOffset = Vector2.zero;
    }


    // ANDES
    public void AndesToGalapagos()
    {
        andesContainer.AddToClassList("hide-left");
        galapagosContainer.ClearClassList();
        galapagosContainer.AddToClassList("show");
        actualScren = ScreenOptions.Galapagos;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
        filterOptions.UpdateList();
        UpdateDots(actualScren);



    }
    public void AndesToOurStory()
    {
        andesContainer.AddToClassList("hide-right");
        clotheContainer.AddToClassList("hide-right");
        ourStoryContainer.ClearClassList();
        ourStoryContainer.AddToClassList("show");
        actualScren = ScreenOptions.OurStory;
        if (ourStoryScroll != null)
            ourStoryScroll.scrollOffset = Vector2.zero;
        UpdateDots(actualScren);

    }

    public void AndesToDetails()
    {
        andesContainer.AddToClassList("hide-up");
        clotheContainer.AddToClassList("hide-up");
        dotsContainer.AddToClassList("hide-up");

        detailContainer.ClearClassList();
        detailContainer.AddToClassList("show");
        actualScren = ScreenOptions.Details;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[1];
        filterOptions.UpdateList();

        detailReference.ActiveLoad();

        if (detailsScroll != null)
            detailsScroll.scrollOffset = Vector2.zero;

    }

    // AMAZONIA

    public void AmazoniaToGalapagos()
    {
        amazoniaContainer.AddToClassList("hide-right");
        galapagosContainer.ClearClassList();
        galapagosContainer.AddToClassList("show");
        actualScren = ScreenOptions.Galapagos;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
        filterOptions.UpdateList();
        UpdateDots(actualScren);


    }

    public void AmazoniaToArtisans()
    {
        amazoniaContainer.AddToClassList("hide-left");
        artisansContainer.ClearClassList();
        artisansContainer.AddToClassList("show");
        actualScren = ScreenOptions.Artisans;

        if (artisansScroll != null)
            artisansScroll.scrollOffset = Vector2.zero;
        UpdateDots(actualScren);

    }


    public void AmazoniaToDetails()
    {
        amazoniaContainer.AddToClassList("hide-up");
        clotheContainer.AddToClassList("hide-up");
        dotsContainer.AddToClassList("hide-up");

        detailContainer.ClearClassList();
        detailContainer.AddToClassList("show");
        actualScren = ScreenOptions.Details;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[2];
        detailReference.ActiveLoad();

        if (detailsScroll != null)
            detailsScroll.scrollOffset = Vector2.zero;
    }


    // ARTISANS & OUR STORY

    public void OurStoryToAndes()
    {
        ourStoryContainer.AddToClassList("hide-left");
        andesContainer.ClearClassList();
        andesContainer.AddToClassList("show");
        clotheContainer.ClearClassList();
        clotheContainer.AddToClassList("show");
        actualScren = ScreenOptions.Andes;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[1];
        UpdateDots(actualScren);

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
        UpdateDots(actualScren);

    }

    // DETAILS

    public void ShowDetails()
    {

        switch ( actualScren)
        {
            case ScreenOptions.Galapagos:
                GalapagosToDetails();
                break;
            case ScreenOptions.Andes:
                AndesToDetails();
                break;
            case ScreenOptions.Amazonia:
                AmazoniaToDetails();
                break;
            default:
                break;
        }

        if (detailsScroll != null)
            detailsScroll.scrollOffset = Vector2.zero;

    }


    public void DetailsToGalapagos()
    {
        detailContainer.AddToClassList("hide-down");
        galapagosContainer.ClearClassList();
        galapagosContainer.AddToClassList("show");
        dotsContainer.ClearClassList();
        dotsContainer.AddToClassList("show");


        clotheContainer.ClearClassList();
        clotheContainer.AddToClassList("show");
        actualScren = ScreenOptions.Galapagos;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
    }

    public void DetailsToAndes()
    {
        detailContainer.AddToClassList("hide-down");
        andesContainer.ClearClassList();
        andesContainer.AddToClassList("show");
        clotheContainer.ClearClassList();
        clotheContainer.AddToClassList("show");
        dotsContainer.ClearClassList();
        dotsContainer.AddToClassList("show");
        actualScren = ScreenOptions.Andes;
        GameManager.Instance.actualRegion = GameManager.Instance.regions[1];
    }

    public void DetailsToAmazonia()
    {
        detailContainer.AddToClassList("hide-down");
        amazoniaContainer.ClearClassList();
        amazoniaContainer.AddToClassList("show");
        clotheContainer.ClearClassList();
        clotheContainer.AddToClassList("show");
        dotsContainer.ClearClassList();
        dotsContainer.AddToClassList("show");
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
                switch (GameManager.Instance.actualRegion.regionType)
                {
                    case RegionType.Galapagos:
                        DetailsToGalapagos();
                        break;
                    case RegionType.Andes:
                        DetailsToAndes();
                        break;
                    case RegionType.Amazonia:
                        DetailsToAmazonia();
                        break;
                    default:
                        break;
                }
                break;
            case ScreenOptions.Start:
                break;
            default:
                break;
        }

        //ResetReturn();
        ColorManager.Instance.UpdateColors();
        infiniteScroll.FillInstance(new List<Clothes>(GameManager.Instance.actualRegion.clothes));



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
        infiniteScroll.FillInstance(new List<Clothes>(GameManager.Instance.actualRegion.clothes));
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

        infiniteScroll.FillInstance(new List<Clothes>(GameManager.Instance.actualRegion.clothes));

    }

    private Coroutine inactivityCoroutine;
    private float inactivityTimeLimit = 20f;

    private void OnEnable()
    {
        uiDocument.rootVisualElement.RegisterCallback<PointerDownEvent>(OnUserInteraction);
        uiDocument.rootVisualElement.RegisterCallback<PointerMoveEvent>(OnUserInteraction);
    }

    private void OnDisable()
    {
        uiDocument.rootVisualElement.UnregisterCallback<PointerDownEvent>(OnUserInteraction);
        uiDocument.rootVisualElement.UnregisterCallback<PointerMoveEvent>(OnUserInteraction);
    }

    private void OnUserInteraction(EventBase evt)
    {
        ResetInactivityTimer();
    }

    private void ResetInactivityTimer()
    {
        if (inactivityCoroutine != null)
            StopCoroutine(inactivityCoroutine);
        if (animationHintCoroutine != null)
            StopCoroutine(animationHintCoroutine);

        HideAnimationHints();

        if (actualScren != ScreenOptions.Start && actualScren != ScreenOptions.Artisans)
        {
            inactivityCoroutine = StartCoroutine(StartInactivityTimer());
            animationHintCoroutine = StartCoroutine(StartAnimationHintTimer());
        }
    }


    private System.Collections.IEnumerator StartAnimationHintTimer()
    {
        yield return new WaitForSeconds(animationHintDelay);

        ShowAnimationHints();
        animationHintCoroutine = null;
    }


    private void ShowAnimationHints()
    {
        switch (actualScren)
        {
            case ScreenOptions.Galapagos:
            case ScreenOptions.Amazonia:
            case ScreenOptions.Andes:
                leftAnimation.RemoveFromClassList("hide-left");
                rightAnimation.RemoveFromClassList("hide-right");
                break;
            case ScreenOptions.Artisans:
                leftAnimation.RemoveFromClassList("hide-left");
                rightAnimation.AddToClassList("hide-right");
                break;
            case ScreenOptions.OurStory:
                rightAnimation.RemoveFromClassList("hide-right");
                leftAnimation.AddToClassList("hide-left");
                break;
            default: // Start, Details or any other
                leftAnimation.AddToClassList("hide-left");
                rightAnimation.AddToClassList("hide-right");
                break;
        }
    }


    private void HideAnimationHints()
    {
        if (leftAnimation != null) leftAnimation.AddToClassList("hide-left");
        if (rightAnimation != null) rightAnimation.AddToClassList("hide-right");
    }



    private System.Collections.IEnumerator StartInactivityTimer()
    {
        yield return new WaitForSeconds(inactivityTimeLimit);

        if (actualScren != ScreenOptions.Start)
        {
            Debug.Log("Inactividad deteFctada. Regresando a inicio...");
            BackToStart();
            ResetReturn();
        }

        inactivityCoroutine = null;
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
