using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    [Header("Canvases")]
    public Canvas startCanvas;
    public Canvas middleCanvas;
    public Canvas endCanvas;

    [Header("Animation Settings")]
    public float transitionDuration = 0.5f;
    public Ease easeType = Ease.OutQuad;
    public float offScreenOffset = 2000f;

    [Header("Debug")]
    public bool enableLogs = true;

    private Vector3 offScreenUp;
    private Vector3 offScreenDown;
    private Vector3 onScreen = Vector3.zero;

    private Canvas currentCanvas;

    void Awake()
    {
        // Calcular posiciones fuera de pantalla
        offScreenUp = new Vector3(0, offScreenOffset, 0);
        offScreenDown = new Vector3(0, -offScreenOffset, 0);

        Log("Inicializando CanvasManager");
        Log($"Offset fuera de pantalla: {offScreenOffset}");

        // Desactivar todos los canvases
        startCanvas.gameObject.SetActive(false);
        middleCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(false);
        Log("Todos los canvases desactivados inicialmente");

        // Activar solo el canvas inicial
        currentCanvas = startCanvas;
        currentCanvas.gameObject.SetActive(true);
        currentCanvas.transform.localPosition = onScreen;
        Log($"Canvas inicial activado: {currentCanvas.name}");
    }

    private void SwitchCanvas(Canvas newCanvas, Vector3 currentExitDirection, Vector3 newEnterFrom, string transitionName)
    {
        if (newCanvas == currentCanvas || newCanvas == null)
        {
            LogWarning($"Intento de cambiar al mismo canvas o canvas nulo: {newCanvas?.name}");
            return;
        }

        Log($"Iniciando transición: {transitionName}");
        Log($"De {currentCanvas.name} a {newCanvas.name}");

        // Desactivar interacción
        SetCanvasInteractable(false);
        Log("Interacción desactivada");

        Canvas canvasToDeactivate = currentCanvas;

        // Animación de salida del canvas actual
        Log($"Animando salida de {canvasToDeactivate.name} en dirección {currentExitDirection}");
        currentCanvas.transform.DOLocalMove(currentExitDirection, transitionDuration)
            .SetEase(easeType)
            .OnComplete(() => {
                canvasToDeactivate.gameObject.SetActive(false);
                Log($"Canvas {canvasToDeactivate.name} desactivado completamente");
            });

        // Preparar nuevo canvas
        newCanvas.gameObject.SetActive(true);
        newCanvas.transform.localPosition = newEnterFrom;
        Log($"Preparando {newCanvas.name} en posición {newEnterFrom}");

        // Animación de entrada del nuevo canvas
        Log($"Animando entrada de {newCanvas.name}");
        newCanvas.transform.DOLocalMove(onScreen, transitionDuration)
            .SetEase(easeType)
            .OnComplete(() => {
                currentCanvas = newCanvas;
                SetCanvasInteractable(true);
                Log($"Transición completada. Canvas actual: {currentCanvas.name}");
                Log("Interacción reactivada");
            });
    }

    // Métodos públicos para las transiciones específicas
    public void GoToMiddleFromStart()
    {
        Log("Botón presionado: GoToMiddleFromStart");
        SwitchCanvas(middleCanvas, offScreenUp, offScreenDown, "Start → Middle (Push Up)");
    }

    public void GoToEndFromMiddle()
    {
        Log("Botón presionado: GoToEndFromMiddle");
        SwitchCanvas(endCanvas, offScreenUp, offScreenDown, "Middle → End (Push Up)");
    }

    public void GoBackToMiddleFromEnd()
    {
        Log("Botón presionado: GoBackToMiddleFromEnd");
        SwitchCanvas(middleCanvas, offScreenDown, offScreenUp, "End → Middle (Push Down)");
    }

    public void GoBackToStartFromMiddle()
    {
        Log("Botón presionado: GoBackToStartFromMiddle");
        SwitchCanvas(startCanvas, offScreenDown, offScreenUp, "Middle → Start (Push Down)");
    }

    private void SetCanvasInteractable(bool state)
    {
        if (currentCanvas != null)
        {
            GraphicRaycaster raycaster = currentCanvas.GetComponent<GraphicRaycaster>();
            if (raycaster != null)
            {
                raycaster.enabled = state;
                Log($"Interacción del canvas {currentCanvas.name} establecida a: {state}");
            }
        }
    }

    // Métodos de logging condicional
    private void Log(string message)
    {
        if (enableLogs) Debug.Log($"[CanvasManager] {message}");
    }

    private void LogWarning(string message)
    {
        if (enableLogs) Debug.LogWarning($"[CanvasManager] {message}");
    }
}