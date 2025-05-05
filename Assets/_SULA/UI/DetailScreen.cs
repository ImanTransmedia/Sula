using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class DetailScreen : MonoBehaviour
{
    private VisualElement root;
    private Label regionName;
    private Label clotheName;
    private VisualElement clotheImage;
    private VisualElement clotheSize;
    private Label clotheDescription;
    private Label clotheReduction;
    private VisualElement reductionImage;
    private Label clotheMaterialName;
    private Label clotheMaterialDescription;
    private VisualElement clotheMaterialImage;
    private VisualElement clotheMaterialContainer;
    private VisualElement washInstructionsContainer;
    private VisualElement returnButton;

    [SerializeField] private Sprite uniqueSize;
    [SerializeField] private Sprite allSize;
    [SerializeField] private Materials[] materialDataList;
    [SerializeField] private RenderTexture renderTex;
    [SerializeField] private GameObject prendaContainer;

    [SerializeField] private Vector2 lastPos = Vector2.zero;
    [SerializeField] private bool dragging = false;
    [SerializeField] private float rotationY = 0f;
    [SerializeField] private float elasticRotation = 0f;

    [Header("Autorotation")]
    [SerializeField] private bool autoRotate = true;
    [SerializeField] private float autoRotationSpeed = 10f;
    [SerializeField] private float zoomInZ = -1.5f;
    [SerializeField] private float zoomDuration = 1.5f;

    private Coroutine autoRotateCoroutine;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        regionName = root.Q<Label>("DetailRegionName");
        clotheName = root.Q<Label>("ClotheName");
        clotheImage = root.Q<VisualElement>("PrendaTexture");
        clotheSize = root.Q<VisualElement>("Sizes");
        clotheDescription = root.Q<Label>("clotheDescription");
        clotheReduction = root.Q<Label>("ReductionDescription");
        reductionImage = root.Q<VisualElement>("ReductionColor");
        clotheMaterialName = root.Q<Label>("MaterialName");
        clotheMaterialDescription = root.Q<Label>("MaterialDescription");
        clotheMaterialImage = root.Q<VisualElement>("MaterialImage");
        clotheMaterialContainer = root.Q<VisualElement>("MaterialsContainer");
        washInstructionsContainer = root.Q<VisualElement>("WashingContainer");
        returnButton = root.Q<VisualElement>("ReturnButton");
    }

    public void ActiveLoad()
    {
        regionName.style.color = GameManager.Instance.actualRegion.darkColor;
        regionName.text = GameManager.Instance.actualRegion.regionName;

        clotheName.text = GameManager.Instance.actualClothe.clotheName;
        clotheSize.style.backgroundImage = new StyleBackground(GameManager.Instance.actualClothe.isUniqueSize ? uniqueSize : allSize);
        clotheDescription.text = GameManager.Instance.actualClothe.description;
        clotheReduction.text = GameManager.Instance.actualClothe.ahorro;
        reductionImage.style.backgroundColor = GameManager.Instance.actualRegion.darkColor;
        clotheMaterialContainer.style.backgroundColor = GameManager.Instance.actualRegion.accentColor;
        clotheMaterialName.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;

        switch (GameManager.Instance.actualClothe.materialType)
        {
            case MaterialType.Linen:
                clotheMaterialName.text = materialDataList[0].materialName;
                clotheMaterialDescription.text = materialDataList[0].materialDescription;
                clotheMaterialImage.style.backgroundImage = new StyleBackground(materialDataList[0].image);
                break;
            case MaterialType.Cotton:
                clotheMaterialName.text = materialDataList[1].materialName;
                clotheMaterialDescription.text = materialDataList[1].materialDescription;
                clotheMaterialImage.style.backgroundImage = new StyleBackground(materialDataList[1].image);
                break;
            case MaterialType.Polyamide:
                clotheMaterialName.text = materialDataList[2].materialName;
                clotheMaterialDescription.text = materialDataList[2].materialDescription;
                clotheMaterialImage.style.backgroundImage = new StyleBackground(materialDataList[2].image);
                break;
            case MaterialType.Polyester:
                clotheMaterialName.text = materialDataList[3].materialName;
                clotheMaterialDescription.text = materialDataList[3].materialDescription;
                clotheMaterialImage.style.backgroundImage = new StyleBackground(materialDataList[3].image);
                break;
        }

        washInstructionsContainer.style.backgroundColor = GameManager.Instance.actualRegion.darkColor;
        ClearContainer();

        if (GameManager.Instance.actualClothe.is3D)
        {
            clotheImage.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(renderTex));
            GameObject instancia = Instantiate(GameManager.Instance.actualClothe.prefab, prendaContainer.transform);
            instancia.transform.localPosition = Vector3.zero;
            instancia.layer = LayerMask.NameToLayer("RenderObjects");

            foreach (Renderer renderer in instancia.GetComponentsInChildren<Renderer>())
            {
                renderer.gameObject.layer = LayerMask.NameToLayer("RenderObjects");
            }

            if (autoRotate && autoRotateCoroutine == null)
                autoRotateCoroutine = StartCoroutine(StartAutoRotate(instancia.transform));

            clotheImage.RegisterCallback<PointerDownEvent>(OnPointerDown);
            clotheImage.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            clotheImage.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }
        else
        {
            clotheImage.style.backgroundImage = new StyleBackground(GameManager.Instance.actualClothe.menuImage);
        }
    }

    private void ClearContainer()
    {
        foreach (Transform child in prendaContainer.transform)
        {
            Destroy(child.gameObject);
        }

        clotheImage.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        clotheImage.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        clotheImage.UnregisterCallback<PointerUpEvent>(OnPointerUp);

        if (autoRotateCoroutine != null)
        {
            StopCoroutine(autoRotateCoroutine);
            autoRotateCoroutine = null;
        }

        dragging = false;
        rotationY = 0f;
        elasticRotation = 0f;
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        dragging = true;
        lastPos = evt.position;

        if (autoRotateCoroutine != null)
        {
            StopCoroutine(autoRotateCoroutine);
            autoRotateCoroutine = null;
        }

        if (prendaContainer.transform.childCount > 0)
        {
            Transform obj = prendaContainer.transform.GetChild(0);
            Vector3 currentRotation = obj.localEulerAngles;
            rotationY = currentRotation.y;
            elasticRotation = currentRotation.x > 180f ? currentRotation.x - 360f : currentRotation.x;
            obj.DOLocalMoveZ(0f, zoomDuration).SetEase(Ease.OutExpo);
        }

        evt.StopPropagation();
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!dragging || prendaContainer.transform.childCount == 0) return;

        var delta = (Vector2)evt.position - lastPos;
        lastPos = evt.position;

        Transform obj = prendaContainer.transform.GetChild(0);
        rotationY += -delta.x * 0.3f;
        elasticRotation = Mathf.Clamp(elasticRotation - delta.y * 0.4f, -25f, 25f);
        obj.localRotation = Quaternion.Euler(elasticRotation, rotationY, 0);
        evt.StopPropagation();
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        dragging = false;
        if (prendaContainer.transform.childCount == 0) return;

        Transform obj = prendaContainer.transform.GetChild(0);

        if (autoRotate)
        {
            if (autoRotateCoroutine != null)
                StopCoroutine(autoRotateCoroutine);
            autoRotateCoroutine = StartCoroutine(StartAutoRotate(obj));
        }

        DOTween.To(() => elasticRotation, x =>
        {
            elasticRotation = x;
            obj.localRotation = Quaternion.Euler(elasticRotation, rotationY, 0);
        }, 0f, 0.5f).SetEase(Ease.OutExpo);

        evt.StopPropagation();
    }

    private System.Collections.IEnumerator StartAutoRotate(Transform obj)
    {
        yield return new WaitForSeconds(3f);

        if (!dragging && obj != null)
        {
            obj.DOLocalMoveZ(zoomInZ, zoomDuration).SetEase(Ease.InOutSine);

            while (!dragging && obj != null)
            {
                obj.Rotate(Vector3.up, autoRotationSpeed * Time.deltaTime, Space.Self);
                yield return null;
            }

            obj.DOLocalMoveZ(0f, zoomDuration).SetEase(Ease.OutExpo);
        }

        autoRotateCoroutine = null;
    }
}
