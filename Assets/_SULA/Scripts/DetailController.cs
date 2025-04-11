using UnityEngine;

public class DetailController : MonoBehaviour
{
    [SerializeField] GameObject prendaContainer;

    private void Start()
    {
        ActiveLoad();
    }

    public void ActiveLoad()
    {
        Debug.Log("Prenda actual: " + GameManager.Instance.actualClothe.name);
        GameObject instancia = Instantiate(GameManager.Instance.actualClothe.prefab, prendaContainer.transform) ;
        instancia.transform.localPosition = Vector3.zero; // o cualquier otra posición local

    }
}
