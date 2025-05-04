using UnityEngine;

public class EneableScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject prendaObject;

    private void OnEnable()
    {
        prendaObject.GetComponent<DetailController>().ActiveLoad();

    }
}
