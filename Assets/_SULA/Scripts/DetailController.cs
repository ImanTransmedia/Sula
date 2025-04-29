using UnityEngine;

public class DetailController : MonoBehaviour
{
    [SerializeField] private GameObject prendaContainer;

    private void OnEnable()
    {
        ActiveLoad();
    }

    private void OnDisable()
    {
        ClearContainer();
    }

    public void ActiveLoad()
    {
        Debug.Log("Prenda actual: " + GameManager.Instance.actualClothe.name);
        GameObject instancia = Instantiate(GameManager.Instance.actualClothe.prefab, prendaContainer.transform);
        instancia.transform.localPosition = Vector3.zero;
    }

    private void ClearContainer()
    {
        foreach (Transform child in prendaContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
