using Unity.VisualScripting;
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
    // LINEN COTTON POLYAMIDE POLIESTER
    [SerializeField] private Materials[] materialDataList;
    [SerializeField] private RenderTexture renderTex;

    [SerializeField] private GameObject prendaContainer;

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
            default:
                break;
        }


        washInstructionsContainer.style.backgroundColor = GameManager.Instance.actualRegion.darkColor;

        ClearContainer();


        if (GameManager.Instance.actualClothe.is3D)
        {
            var prendaActual = GameManager.Instance.actualClothe.name;
            // Fix for CS1503: Convert RenderTexture to Background using Background.FromRenderTexture
            clotheImage.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(renderTex));
            Debug.Log("Prenda actual: " + prendaActual);

            GameObject instancia = Instantiate(GameManager.Instance.actualClothe.prefab, prendaContainer.transform);
            instancia.transform.localPosition = Vector3.zero;
            var targetLayer = "RenderObjects";
            instancia.layer = LayerMask.NameToLayer(targetLayer);
            Renderer[] renderers = instancia.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.gameObject.layer = LayerMask.NameToLayer(targetLayer);
            }
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
    }
}