using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class StartController : MonoBehaviour
{

    private VisualElement root;
    private VisualElement video;
    private VisualElement panel;
    private VisualElement startButtom;


    [SerializeField] private VideoPlayer videoPlayer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        video = root.Q<VisualElement>("Video");
        video.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("Video Taped");
            panel.ToggleInClassList("showup");
        });

        panel = root.Q<VisualElement>("PanelContainer");
        panel.RemoveFromClassList("showup");
        startButtom = root.Q<VisualElement>("StartBurron");
        startButtom.RegisterCallback<ClickEvent>(OnButtonTap);
    }


    private void OnButtonTap(ClickEvent clickEvent)
    {
        Debug.Log("Start Button Tap");
        videoPlayer.Stop();
        NavigationManager.Instance.StartToGalapagos();


        


        //productsPanel.GetComponent<InfiniteScroll>().FillInstance(new List<Clothes>(GameManager.Instance.actualRegion.clothes));

    }
}
