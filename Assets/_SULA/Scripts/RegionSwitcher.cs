using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RegionSwitcher : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Region Settings")]
    [SerializeField] private Regions[] allRegions; // [0]Amazonia, [1]Gal�pagos, [2]Andes
    [SerializeField] private int currentRegionIndex = 1; // Empieza en Gal�pagos

    [Header("UI References")]
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image regionImage;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button filterButton;
    [SerializeField] private RectTransform contentTransform;

    [Header("Swipe Settings")]
    [SerializeField] private float swipeThreshold = 100f;
    [SerializeField] private float transitionDuration = 0.4f;
    [SerializeField] private float swipeMoveAmount = 80f;

    private Vector2 dragStartPosition;
    private bool isAnimating = false;

    private void Start()
    {
        Debug.Log("Inicializando RegionSwitcher. Regi�n actual: " + allRegions[currentRegionIndex].regionName);
        ApplyCurrentRegion();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isAnimating) return;
        dragStartPosition = eventData.position;
        Debug.Log("Drag iniciado en posici�n: " + dragStartPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isAnimating) return;
        float dragDifference = eventData.position.x - dragStartPosition.x;
        contentTransform.anchoredPosition = new Vector2(dragDifference * 0.3f, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isAnimating) return;

        Vector2 dragEndPosition = eventData.position;
        Vector2 dragDelta = dragEndPosition - dragStartPosition;
        float horizontalDistance = Mathf.Abs(dragDelta.x);
        float verticalDistance = Mathf.Abs(dragDelta.y);

        Debug.Log($"Drag finalizado. Delta X: {dragDelta.x}, Delta Y: {dragDelta.y}");

        // Determinar si el swipe es principalmente horizontal o vertical
        if (horizontalDistance > verticalDistance && horizontalDistance > swipeThreshold)
        {
            // Swipe horizontal
            if (dragDelta.x > 0)
            {
                Debug.Log("Swipe derecha detectado");
                SwitchToRegion(currentRegionIndex - 1);
            }
            else
            {
                Debug.Log("Swipe izquierda detectado");
                SwitchToRegion(currentRegionIndex + 1);
            }
        }
        else if (verticalDistance > horizontalDistance && verticalDistance > swipeThreshold)
        {
            // Swipe vertical
            if (dragDelta.y > 0)
            {
                Debug.Log("Swipe arriba detectado (sin funcionalidad a�n)");
            }
            else
            {
                Debug.Log("Swipe abajo detectado (sin funcionalidad a�n)");
            }

            // Resetear posici�n
            contentTransform.DOAnchorPosX(0, 0.2f).SetEase(Ease.OutQuad);
        }
        else
        {
            Debug.Log("Swipe no alcanz� el umbral m�nimo");
            contentTransform.DOAnchorPosX(0, 0.2f).SetEase(Ease.OutQuad);
        }
    }

    private void SwitchToRegion(int newIndex)
    {
        // Ajustar �ndice si se sale de los l�mites
        if (newIndex < 0) newIndex = allRegions.Length - 1;
        if (newIndex >= allRegions.Length) newIndex = 0;

        Debug.Log($"Intentando cambiar de {allRegions[currentRegionIndex].regionName} a {allRegions[newIndex].regionName}");

        // No hacer nada si es la misma regi�n
        if (newIndex == currentRegionIndex)
        {
            Debug.Log("Ya est� en la regi�n destino");
            contentTransform.DOAnchorPosX(0, 0.2f).SetEase(Ease.OutQuad);
            return;
        }

        isAnimating = true;
        int direction = newIndex > currentRegionIndex ? -1 : 1;

        Debug.Log($"Iniciando animaci�n de transici�n. Direcci�n: {(direction == -1 ? "izquierda" : "derecha")}");

        contentTransform.DOAnchorPosX(direction * swipeMoveAmount, transitionDuration * 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                currentRegionIndex = newIndex;
                Debug.Log($"Regi�n cambiada a: {allRegions[currentRegionIndex].regionName}");
                ApplyCurrentRegion();
                contentTransform.anchoredPosition = new Vector2(-direction * swipeMoveAmount, 0);
                contentTransform.DOAnchorPosX(0, transitionDuration * 0.5f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => {
                        isAnimating = false;
                        Debug.Log("Animaci�n completada");
                    });
            });
    }

    private void ApplyCurrentRegion()
    {
        Regions current = allRegions[currentRegionIndex];
        Debug.Log($"Aplicando cambios visuales para {current.regionName}");

        // Animaci�n de fondo
        background.DOColor(current.accentColor, transitionDuration);

        // Cambiar t�tulo
        titleText.text = current.regionName;
        titleText.DOColor(current.lightColor, transitionDuration);

        // Cambiar imagen
        regionImage.sprite = current.imagen;

        // Cambiar color de botones
        helpButton.image.DOColor(current.darkColor, transitionDuration);
        filterButton.image.DOColor(current.darkColor, transitionDuration);
    }

    // M�todos p�blicos para botones
    public void GoToAmazonia()
    {
        Debug.Log("Bot�n: Ir a Amazonia");
        SwitchToRegion(0);
    }

    public void GoToGalapagos()
    {
        Debug.Log("Bot�n: Ir a Gal�pagos");
        SwitchToRegion(1);
    }

    public void GoToAndes()
    {
        Debug.Log("Bot�n: Ir a Andes");
        SwitchToRegion(2);
    }
}