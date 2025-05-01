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


    private int currentImageIndex = 0;

    private void OnEnable()
    {
        ActiveLoad();
    }

    public void ActiveLoad()
    {
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

