using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemController : MonoBehaviour
{
    [SerializeField] private GameObject Panel;

    public void AditiveLoad()
    {
        // Carga escena aditiva
        SceneManager.LoadScene("Galapagos", LoadSceneMode.Additive);
        Panel.SetActive(false);

    }
}
