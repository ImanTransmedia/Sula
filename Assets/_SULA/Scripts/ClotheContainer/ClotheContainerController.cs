using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClotheContainerController : MonoBehaviour
{
    [Header("Configuración")]
    public Clothes[] productos;
    public GameObject prefabItem;
    public Transform contenedor;
    public int cantidadVisible = 5;
    public float animDuration = 0.3f;
    public float itemSpacing = 250f;

    [Header("Efectos")]
    public float scaleFactor = 0.95f;
    public float scaleDuration = 0.2f;

    private List<GameObject> items = new List<GameObject>();
    private int currentIndex = 0;
    private bool isAnimating = false;

    void Start()
    {
        InitializeCarousel();
    }

    private void InitializeCarousel()
    {
        // Limpiar contenedor por si acaso
        foreach (Transform child in contenedor)
        {
            Destroy(child.gameObject);
        }
        items.Clear();

        // Crear items iniciales (un poco más para efecto buffer)
        int itemsToCreate = Mathf.Min(cantidadVisible + 2, productos.Length);

        for (int i = 0; i < itemsToCreate; i++)
        {
            CreateItem(i);
        }

        // Posicionar inicialmente
        RepositionItems(instant: true);
    }

    private void CreateItem(int positionIndex)
    {
        if (productos.Length == 0) return;

        int productIndex = (currentIndex + positionIndex) % productos.Length;
        if (productIndex < 0) productIndex += productos.Length;

        GameObject newItem = Instantiate(prefabItem, contenedor);

        // Configurar el item sin modificar el prefab original
        if (newItem.TryGetComponent<ItemCarrousel>(out var itemCarrousel))
        {
            itemCarrousel.Configurar(productos[productIndex]);
        }

        if (newItem.TryGetComponent<ItemController>(out var itemController))
        {
            itemController.SetPrendaActual(productos[productIndex]);
        }

        // Guardar referencia
        items.Add(newItem);
    }

    public void MoveRight()
    {
        if (isAnimating || productos.Length <= cantidadVisible) return;
        StartCoroutine(AnimateMove(1));
    }

    public void MoveLeft()
    {
        if (isAnimating || productos.Length <= cantidadVisible) return;
        StartCoroutine(AnimateMove(-1));
    }

    private System.Collections.IEnumerator AnimateMove(int direction)
    {
        isAnimating = true;

        // 1. Animación de salida
        foreach (var item in items)
        {
            // Mover horizontalmente
            item.transform.DOLocalMoveX(-direction * itemSpacing, animDuration)
                .SetRelative()
                .SetEase(Ease.OutQuad);

            // Efecto de escala opcional
            item.transform.DOScale(scaleFactor, scaleDuration)
                .SetLoops(2, LoopType.Yoyo);
        }

        yield return new WaitForSeconds(animDuration);

        // 2. Reorganizar elementos
        if (direction > 0)
        {
            // Mover derecha: eliminar el primero y añadir nuevo al final
            Destroy(items[0]);
            items.RemoveAt(0);
            currentIndex = (currentIndex + 1) % productos.Length;
            CreateItem(items.Count);
        }
        else
        {
            // Mover izquierda: eliminar el último y añadir nuevo al principio
            Destroy(items[items.Count - 1]);
            items.RemoveAt(items.Count - 1);
            currentIndex = (currentIndex - 1 + productos.Length) % productos.Length;
            CreateItem(0);
            items.Insert(0, items[items.Count - 1]);
            items.RemoveAt(items.Count - 1);
        }

        // 3. Reposicionar sin animación
        RepositionItems(instant: true);

        isAnimating = false;
    }

    private void RepositionItems(bool instant)
    {
        for (int i = 0; i < items.Count; i++)
        {
            float targetX = (i - cantidadVisible / 2) * itemSpacing;
            Vector3 targetPos = new Vector3(targetX, 0, 0);

            if (instant)
            {
                items[i].transform.localPosition = targetPos;
                items[i].transform.localScale = Vector3.one;
            }
            else
            {
                items[i].transform.DOLocalMove(targetPos, animDuration).SetEase(Ease.OutBack);
                items[i].transform.DOScale(Vector3.one, scaleDuration);
            }
        }
    }
}