using System;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class StarupScreen : MonoBehaviour
{
    [SerializeField] private VisualElement root;
    [SerializeField] private VisualElement video;
    [SerializeField] private VisualElement panel;
    [SerializeField] private VisualElement startButtom;
    [SerializeField] private GameObject productsPanel;
    [SerializeField] private VisualElement regionPanelRoot;
    [SerializeField] private VisualElement regionPanel;
    [SerializeField] private VideoPlayer videoPlayer;
   

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        video = root.Q<VisualElement>("Video");
        video.RegisterCallback<ClickEvent>(OnVideoTap);
        panel = root.Q<VisualElement>("PanelContainer");
        panel.RemoveFromClassList("showup");
        startButtom = root.Q<VisualElement>("StartBurron");
        startButtom.RegisterCallback<ClickEvent>(OnButtonTap);

        regionPanelRoot = productsPanel.GetComponent<UIDocument>().rootVisualElement;
        regionPanel = regionPanelRoot.Q<VisualElement>("mainContainer");


    }


    private void OnVideoTap(ClickEvent clickEvent)
    {
        Debug.Log("Video tapped");
        if (panel.ClassListContains("showup"))
        {
            panel.RemoveFromClassList("showup");
        }
        else
        {
            panel.AddToClassList("showup");
        }

    }

    private void OnButtonTap(ClickEvent clickEvent)
    {
        Debug.Log("ButtonTap");
        if (!root.ClassListContains("hide"))
        {

            root.AddToClassList("hide");
            //root.AddToClassList("fade-out");
            videoPlayer.Stop();

        }

        if (regionPanel.ClassListContains("hide-down"))
        {
            regionPanel.RemoveFromClassList("hide-down");
        }

        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
        GameManager.Instance.actualClothe = GameManager.Instance.regions[0].clothes[0];

        productsPanel.GetComponent<InfiniteScroll>().FillInstance(new List<Clothes> (GameManager.Instance.actualRegion.clothes));

    }

}
