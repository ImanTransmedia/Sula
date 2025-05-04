using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailController : MonoBehaviour
{
    [SerializeField] private GameObject prendaContainer;
    [SerializeField] private GameObject imageContainer;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI reduction;
    [SerializeField] private TextMeshProUGUI nombre;

    [SerializeField] private TextMeshProUGUI regionName;

    [SerializeField] private Image sizeImage;
    [SerializeField] private Sprite allSize;
    [SerializeField] private Sprite uniqueImage;
    [SerializeField] private Image colorImage;

    [SerializeField] private Materials[] materialList;
    [SerializeField] private Image materialContainer;
    [SerializeField] private Image materialImage;
    [SerializeField] private TextMeshProUGUI materialName;
    [SerializeField] private TextMeshProUGUI materialDescription;


    [SerializeField] private Image washingInstruction;




    private int currentImageIndex = 0;



    public void ActiveLoad()
    {

        regionName.text = GameManager.Instance.actualRegion.regionName.ToUpper();
        regionName.color = GameManager.Instance.actualRegion.darkColor;
        sizeImage.GetComponent<Image>().sprite = GameManager.Instance.actualClothe.isUniqueSize ? uniqueImage : allSize;
        colorImage.GetComponent<Image>().color = GameManager.Instance.actualRegion.darkColor;

        washingInstruction.GetComponent<Image>().color = GameManager.Instance.actualRegion.darkColor;

        materialContainer.GetComponent<Image>().color = GameManager.Instance.actualRegion.accentColor;
        materialImage.GetComponent<Image>().color = GameManager.Instance.actualRegion.darkColor;


        switch (GameManager.Instance.actualClothe.materialType)
        {
            case MaterialType.Cotton:
                materialName.text = materialList[0].materialName;
                materialDescription.text = materialList[0].materialDescription;
                materialImage.GetComponent<Image>().sprite = materialList[0].image;
                break;
            case MaterialType.Linen:
                materialName.text = materialList[1].materialName;
                materialDescription.text = materialList[1].materialDescription;
                materialImage.GetComponent<Image>().sprite = materialList[1].image;
                break;
            case MaterialType.Polyamide:
                materialName.text = materialList[2].materialName;
                materialDescription.text = materialList[2].materialDescription;
                materialImage.GetComponent<Image>().sprite = materialList[2].image;
                break;
            case MaterialType.Polyester:
                materialName.text = materialList[3].materialName;
                materialDescription.text = materialList[3].materialDescription;
                materialImage.GetComponent<Image>().sprite = materialList[3].image;
                break;
            default:
                break;
        }



        ClearContainer();
        var prendaActual = GameManager.Instance.actualClothe.name;

        Debug.Log("Prenda actual: " + prendaActual);
        description.text = GameManager.Instance.actualClothe.description;
        reduction.text = GameManager.Instance.actualClothe.ahorro;
        nombre.text = GameManager.Instance.actualClothe.clotheName;


        if (!GameManager.Instance.actualClothe.is3D)
        {
         
            prendaContainer.SetActive(false);
            imageContainer.gameObject.SetActive(true);
            imageContainer.GetComponent<Image>().sprite = GameManager.Instance.actualClothe.menuImage;

            if (GameManager.Instance.actualClothe.imagenes != null && GameManager.Instance.actualClothe.imagenes.Length > 0)
            {
                InvokeRepeating("ChangeImage", 3f, 3f);
            }
            else
            {
                CancelInvoke("ChangeImage");
            }
            return;
        }
        else
        {
            CancelInvoke("ChangeImage");
            prendaContainer.SetActive(true);
            imageContainer.gameObject.SetActive(false);

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


    }


    private void ClearContainer()
    {
        foreach (Transform child in prendaContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ChangeImage()
    {
        if (GameManager.Instance.actualClothe != null && GameManager.Instance.actualClothe.imagenes != null && GameManager.Instance.actualClothe.imagenes.Length > 0)
        {
            currentImageIndex++;

            if (currentImageIndex >= GameManager.Instance.actualClothe.imagenes.Length)
            {
                currentImageIndex = 0;
            }

            imageContainer.GetComponent<Image>().sprite = GameManager.Instance.actualClothe.imagenes[currentImageIndex];
        }
        else
        {
            CancelInvoke("ChangeImage");
        }
    }

    private void OnDisable()
    {
        CancelInvoke("ChangeImage");
    }

    private void OnDestroy()
    {
        CancelInvoke("ChangeImage");
    }
}

