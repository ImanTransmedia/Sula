using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PrendaController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    public float rotationSpeed = 0.3f;
    public float bounceAmount = 10f;
    public float bounceDuration = 0.3f;

    public float zoomSensitivity = 0.05f;
    public float panSpeed = 0.02f;
    public float minZoomDistance = 1f; // Ajusta seg�n necesites
    public float maxZoomDistance = 5f; // Ajusta seg�n necesites

    public Transform targetObject; // La prenda 3D o el GameObject del sprite 2D
    public Transform uiCameraTransform; // La Transform de la UICamera
    private Camera uiCamera;

    private Vector2 startDragPosition;
    private Vector2 currentDragDelta;
    private Vector3 initialObjectRotation;
    private Vector3 initialObjectPosition;
    private float currentZoomFactor = 1f;

    private bool isDragging = false;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("�Error! No se ha asignado el objeto objetivo (prenda).");
            enabled = false;
            return;
        }

        if (uiCameraTransform == null)
        {
            Debug.LogError("�Error! No se ha asignado la c�mara de la UI.");
            enabled = false;
            return;
        }

        uiCamera = uiCameraTransform.GetComponent<Camera>();
        if (uiCamera == null)
        {
            Debug.LogError("�Error! El uiCameraTransform no tiene un componente Camera.");
            enabled = false;
            return;
        }

        initialObjectRotation = targetObject.localEulerAngles;
        initialObjectPosition = targetObject.localPosition;
    }

    // Implementaci�n de la interfaz IBeginDragHandler
    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.position;
        isDragging = true;
    }

    // Implementaci�n de la interfaz IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        currentDragDelta = eventData.position - startDragPosition;

        // Rotaci�n en 3D (ajusta los ejes seg�n tu necesidad)
        if (GameManager.Instance.actualClothe.is3D)
        {
            float rotY = -currentDragDelta.x * rotationSpeed;
            float rotX = currentDragDelta.y * rotationSpeed;
            targetObject.Rotate(uiCameraTransform.up, rotY, Space.World);
            targetObject.Rotate(uiCameraTransform.right, rotX, Space.World);
        }
        // Traslaci�n (Pan) para 2D o 3D
        else
        {
            Vector3 panDirection = uiCameraTransform.right * -currentDragDelta.x * panSpeed + uiCameraTransform.up * currentDragDelta.y * panSpeed;
            targetObject.localPosition = initialObjectPosition + panDirection * currentZoomFactor; // El zoom afecta el pan
        }

        startDragPosition = eventData.position;
    }

    // Implementaci�n de la interfaz IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        BounceEffect(); // Opcional
    }

    // Implementaci�n de la interfaz IScrollHandler (para el zoom)
    public void OnScroll(PointerEventData eventData)
    {
        if (GameManager.Instance.actualClothe.is3D)
        {
            float zoomDelta = eventData.scrollDelta.y * zoomSensitivity;
            Vector3 localPosition = targetObject.localPosition;
            localPosition.z = Mathf.Clamp(localPosition.z + zoomDelta, -maxZoomDistance, -minZoomDistance); // Ajusta el eje Z seg�n tu configuraci�n de c�mara
            targetObject.localPosition = localPosition;
        }
        else
        {
            // Para sprites 2D, podr�as ajustar la escala con el zoom
            currentZoomFactor = Mathf.Clamp01(currentZoomFactor + eventData.scrollDelta.y * zoomSensitivity * 0.1f); // Factor de escala
            targetObject.localScale = Vector3.one * currentZoomFactor;
        }
    }

    void BounceEffect()
    {
        if (GameManager.Instance.actualClothe.is3D)
        {
            float bounceDirection = Random.Range(-1f, 1f) > 0 ? 1f : -1f;
            float bounce = bounceAmount * bounceDirection;

            targetObject.DORotate(new Vector3(targetObject.rotation.eulerAngles.x, targetObject.rotation.eulerAngles.y + bounce, targetObject.rotation.eulerAngles.z), bounceDuration)
                .SetEase(Ease.OutBack);
        }
    }

    // Funci�n para cambiar la prenda mostrada (3D o 2D)
    public void SetTargetObject(GameObject newTarget)
    {
        targetObject = newTarget.transform;
        initialObjectRotation = targetObject.localEulerAngles;
        initialObjectPosition = targetObject.localPosition;
        currentZoomFactor = 1f; // Reset de zoom al cambiar prenda
        targetObject.localScale = Vector3.one; // Reset de escala para sprites
    }
}