using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClotheContainerController : MonoBehaviour
{
    public Clothes[] productos;
    public GameObject prefabItem;
    public Transform contenedor;
    public int cantidadVisible = 5;

    private List<GameObject> items = new List<GameObject>();
    private int indiceInicio = 0;

    void Start()
    {
        for (int i = 0; i < cantidadVisible; i++)
        {
            AgregarItem(i);
        }
    }

    void AgregarItem(int indice)
    {
        int indiceProducto = (indiceInicio + indice) % productos.Length;
        GameObject nuevoItem = Instantiate(prefabItem, contenedor);
        nuevoItem.GetComponent<ItemCarrousel>().Configurar(productos[indiceProducto]);
        nuevoItem.GetComponent<ItemController>().SetPrendaActual(productos[indiceProducto]);
        items.Add(nuevoItem);
    }

    public void MoverDerecha()
    {
        Destroy(items[0]);
        items.RemoveAt(0);
        indiceInicio = (indiceInicio + 1) % productos.Length;
        AgregarItem(cantidadVisible - 1);
    }

    public void MoverIzquierda()
    {
        int nuevoIndice = (indiceInicio - 1 + productos.Length) % productos.Length;
        GameObject nuevoItem = Instantiate(prefabItem, contenedor.GetChild(0));
        nuevoItem.GetComponent<ItemCarrousel>().Configurar(productos[nuevoIndice]);
        items.Insert(0, nuevoItem);
        Destroy(items[items.Count - 1]);
        items.RemoveAt(items.Count - 1);
        indiceInicio = nuevoIndice;
    }
}
