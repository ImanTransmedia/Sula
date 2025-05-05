using System.Collections;
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

    private Coroutine hidePanelCoroutine;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        video = root.Q<VisualElement>("Video");
        panel = root.Q<VisualElement>("PanelContainer");
        startButtom = root.Q<VisualElement>("StartBurron");

        panel.RemoveFromClassList("showup");

        video.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("Video Tapped");

            // Mostrar el panel
            panel.AddToClassList("showup");

            // Reiniciar temporizador de ocultamiento
            if (hidePanelCoroutine != null)
                StopCoroutine(hidePanelCoroutine);

            hidePanelCoroutine = StartCoroutine(HidePanelAfterDelay());
        });

        startButtom.RegisterCallback<ClickEvent>(OnButtonTap);
    }

    private void OnButtonTap(ClickEvent clickEvent)
    {
        Debug.Log("Start Button Tap");
        videoPlayer.Stop();
        NavigationManager.Instance.StartToGalapagos();
    }

    private IEnumerator HidePanelAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        if (panel.ClassListContains("showup"))
        {
            panel.RemoveFromClassList("showup");
            Debug.Log("Panel auto-hidden after 5s.");
        }

        hidePanelCoroutine = null;
    }
}
