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
            panel.ToggleInClassList("showup");
    }

    private void OnButtonTap(ClickEvent clickEvent)
    {
        Debug.Log("Start Button Tap");

        root.AddToClassList("hide");
        videoPlayer.Stop();
        GameManager.Instance.actualRegion = GameManager.Instance.regions[0];
        regionPanel.ClearClassList();
        regionPanel.AddToClassList("show");


        productsPanel.GetComponent<InfiniteScroll>().FillInstance(new List<Clothes> (GameManager.Instance.actualRegion.clothes));

    }

}
