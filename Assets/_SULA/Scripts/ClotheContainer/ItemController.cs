using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemController : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Clothes prendaActual;
    
    public void SetPrendaActual(Clothes prendaData)
    {
        this.prendaActual = prendaData;
    }

    public void AditiveLoad()
    {
        GameManager.Instance.actualClothe = this.prendaActual;
        // Carga escena aditiva
        SceneManager.LoadScene("Galapagos", LoadSceneMode.Additive);
        

    }
}
