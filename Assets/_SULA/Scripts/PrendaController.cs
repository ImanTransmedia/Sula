using UnityEngine;
using DG.Tweening;

public class PrendaController : MonoBehaviour
{
    public float rotationSpeed = 0.3f;
    public float bounceAmount = 10f;
    public float bounceDuration = 0.3f;

    public float idleTimeToTrigger = 4f;
    public float autoRotationSpeed = 20f;
    public Transform cameraTransform;
    public Vector3 lastPosition;
    public Vector3 zoomInPosition = new Vector3(0, 1, -7);
    public float zoomDuration = 1.5f;
    public float pinchZoomSensitivity = 0.05f;
    public float minZoomDistance = 3f;
    public float maxZoomDistance = 10f;

    private Vector2 startTouchPos;
    private bool isDragging = false;
    private bool isIdle = false;
    private float lastInteractionTime;

    private Tween zoomTween;
    private Tween autoRotationTween;
    private float currentZoomDistance;
    private Vector2 initialPinchDistance;
    private Vector3 initialCameraPosition;

    public bool is3D;

    private void OnEnable()
    {
        is3D = GameManager.Instance.actualClothe.is3D;
    }

    void Start()
    {
        lastInteractionTime = Time.time;
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
        lastPosition = cameraTransform.position;
        currentZoomDistance = Vector3.Distance(transform.position, cameraTransform.position);
        initialCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        if (!is3D) return;
        HandleInput();

        // Verifica si está inactivo
        if (!isIdle && Time.time - lastInteractionTime > idleTimeToTrigger)
        {
            StartIdleMode();
        }

        // Para probar el zoom con el ratón en PC
        if (Input.GetKey(KeyCode.LeftControl))
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                HandleMouseZoom(scrollDelta);
                lastInteractionTime = Time.time;
                if (isIdle)
                    ExitIdleMode();
            }
        }
    }

    void HandleInput()
    {
        bool interacted = false;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            interacted = HandleTouch(touch.phase, touch.position);
        }
        else if (Input.touchCount == 2)
        {
            interacted = HandlePinchZoom();
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

    bool HandlePinchZoom()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;

        currentZoomDistance -= difference * pinchZoomSensitivity;
        currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);

        Vector3 targetPosition = transform.position - cameraTransform.forward * currentZoomDistance;
        cameraTransform.position = targetPosition;

        lastInteractionTime = Time.time;
        return true;
    }

    void HandleMouseZoom(float delta)
    {
        currentZoomDistance -= delta * 10f; // Ajusta la sensibilidad del scroll del ratón si es necesario
        currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);

        Vector3 targetPosition = transform.position - cameraTransform.forward * currentZoomDistance;
        cameraTransform.position = targetPosition;
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
        if (autoRotationTween != null && autoRotationTween.IsActive())
            autoRotationTween.Kill();

        if (zoomTween != null && zoomTween.IsActive())
            zoomTween.Kill();

        // Volver la cámara a su posición original
        cameraTransform.DOMove(lastPosition, zoomDuration).SetEase(Ease.OutSine);
    }
}