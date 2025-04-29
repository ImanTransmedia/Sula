using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemController : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Clothes prendaActual;
    [SerializeField] CanvasManager canvasManager;

    private void Start()
    {
        this.canvasManager = FindFirstObjectByType<CanvasManager>();
        if (canvasManager != null)
        {
            GameObject obj = canvasManager.gameObject;
            Debug.Log("Objeto encontrado: " + obj.name);
        }
        else
        {
            Debug.Log("No se encontró ningún objeto con CanvasManager.");
        }


    }




    public void SetPrendaActual(Clothes prendaData)
    {
        this.prendaActual = prendaData;
    }

    public void AditiveLoad()
    {
        GameManager.Instance.actualClothe = this.prendaActual;

        canvasManager.GoToEndFromMiddle();
    }


    public void OnClick_Bufa()
    {
    }
}
