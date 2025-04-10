using UnityEngine;
using DG.Tweening;


public class PrendaController : MonoBehaviour
{
    public float rotationSpeed = 0.3f;
    public float bounceAmount = 10f;
    public float bounceDuration = 0.3f;

    public float idleTimeToTrigger = 4f;
    public float autoRotationSpeed = 20f;
    public Transform cameraTransform; // Cámara que se acercará
    public Vector3 lastPosition;
    public Vector3 zoomInPosition = new Vector3(0, 1, -7); // Nueva posición de cámara al hacer zoom
    public float zoomDuration = 1.5f;

    private Vector2 startTouchPos;
    private bool isDragging = false;
    private bool isIdle = false;
    private float lastInteractionTime;

    private Tween zoomTween;
    private Tween autoRotationTween;

    void Start()
    {
        lastInteractionTime = Time.time;
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
            lastPosition = Camera.main.transform.position;
    }

    void Update()
    {
        HandleInput();

        // Verifica si está inactivo
        if (!isIdle && Time.time - lastInteractionTime > idleTimeToTrigger)
        {
            StartIdleMode();
        }
    }

    void HandleInput()
    {
        bool interacted = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            interacted = HandleTouch(touch.phase, touch.position);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                interacted = HandleTouch(TouchPhase.Began, Input.mousePosition);
            else if (Input.GetMouseButton(0))
                interacted = HandleTouch(TouchPhase.Moved, Input.mousePosition);
            else if (Input.GetMouseButtonUp(0))
                interacted = HandleTouch(TouchPhase.Ended, Input.mousePosition);
        }

        if (interacted)
        {
            lastInteractionTime = Time.time;
            if (isIdle)
                ExitIdleMode();
        }
    }

    bool HandleTouch(TouchPhase phase, Vector2 position)
    {
        switch (phase)
        {
            case TouchPhase.Began:
                startTouchPos = position;
                isDragging = true;
                return true;

            case TouchPhase.Moved:
                if (isDragging)
                {
                    Vector2 delta = position - startTouchPos;
                    float rotY = -delta.x * rotationSpeed;
                    transform.Rotate(Vector3.up, rotY, Space.World);
                    startTouchPos = position;
                    return true;
                }
                break;

            case TouchPhase.Ended:
                isDragging = false;
                BounceEffect();
                return true;
        }

        return false;
    }

    void BounceEffect()
    {
        float bounceDirection = Random.Range(-1f, 1f) > 0 ? 1f : -1f;
        float bounce = bounceAmount * bounceDirection;

        transform.DORotate(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + bounce, transform.rotation.eulerAngles.z), bounceDuration)
                 .SetEase(Ease.OutBack);
    }

    void StartIdleMode()
    {
        isIdle = true;

        autoRotationTween = transform.DORotate(
            new Vector3(0, -360, 0),
            10f,
            RotateMode.LocalAxisAdd) 
            .SetEase(Ease.Linear)
            .SetLoops(-1);

        // Zoom in cámara
        zoomTween = cameraTransform.DOMove(zoomInPosition, zoomDuration).SetEase(Ease.InOutSine);
    }

    void ExitIdleMode()
    {
        isIdle = false;

        // Detener rotación y zoom
        if (autoRotationTween.IsActive())
            autoRotationTween.Kill();

        if (zoomTween.IsActive())
            zoomTween.Kill();
        Debug.Log("Regresando a la Posicion Inicial");
        // Volver la cámara a su posición original
        cameraTransform.DOMove(lastPosition, zoomDuration).SetEase(Ease.OutSine);
    }
}