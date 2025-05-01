using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class ArtisansController : MonoBehaviour
{

    private VisualElement root;
    private VisualElement artisansMiniature;
    private VisualElement artisansFull;
    private VisualElement carbonoMiniature;
    private VisualElement carbonoFull;


    [SerializeField] private VideoPlayer artisansVideoPlayer;
    [SerializeField] private VideoPlayer carbonoVideoPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        artisansMiniature = root.Q<VisualElement>("ArtisansMini");
        artisansMiniature.RegisterCallback<ClickEvent>(evt =>
        {
            artisansFull.ToggleInClassList("artisans");
            artisansVideoPlayer.SetDirectAudioMute (0, false);
        });

        artisansFull = root.Q<VisualElement>("ArtisansVideo");
        artisansFull.RegisterCallback<ClickEvent>(evt =>
        {
            artisansFull.ToggleInClassList("artisans");
            artisansVideoPlayer.SetDirectAudioMute(0, true);
        });


        carbonoMiniature = root.Q<VisualElement>("CarbonMini");
        carbonoMiniature.RegisterCallback<ClickEvent>(evt =>
        {
            carbonoFull.ToggleInClassList("carbono");
            carbonoVideoPlayer.SetDirectAudioMute(0, false);
        });

        carbonoFull = root.Q<VisualElement>("CarbonoVideo");
        carbonoFull.RegisterCallback<ClickEvent>(evt =>
        {
            carbonoFull.ToggleInClassList("carbono");
            carbonoVideoPlayer.SetDirectAudioMute(0, true);
        });

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
