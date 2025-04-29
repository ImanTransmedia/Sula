using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class ColorSwitcher : MonoBehaviour
{
    [Header("Settings")]
    public string modelTag = "Model";
    public string colorPropertyName = "_Color";

    [Header("Animation Settings")]
    public float colorChangeDuration = 0.5f;
    public Ease easeType = Ease.OutQuad;

    [Header("Debug")]
    public bool debugMode = true;

    [SerializeField] private Material targetMaterial;
    [SerializeField] private Color originalColor;
    [SerializeField] private Color targetColor;
    private Tween colorTween;

    private void OnEnable()
    {
        FindAndSetupModel();
        targetColor = GetComponent<Image>().color;
    }

    private void FindAndSetupModel()
    {
        Transform modelTransform = GameManager.Instance.actualClothe.prefab.gameObject.transform.FindDeepChildWithTag(modelTag);

        if (modelTransform == null)
        {
            Debug.LogError($"No se encontr� un hijo con tag '{modelTag}' en el prefab");
            return;
        }

        Renderer modelRenderer = modelTransform.GetComponent<Renderer>();
        if (modelRenderer == null)
        {
            Debug.LogError("El modelo encontrado no tiene componente Renderer");
            return;
        }

        targetMaterial = modelRenderer.sharedMaterial;
        originalColor = targetMaterial.GetColor(colorPropertyName);

        if (debugMode) Debug.Log($"Material inicializado. Color original: {originalColor}");
    }

    // Cambio de color con animaci�n DoTween
    public void ChangeColorWithTween(Color newColor)
    {
        if (targetMaterial == null)
        {
            Debug.LogWarning("Material no inicializado");
            return;
        }

        // Detener cualquier animaci�n previa
        colorTween?.Kill();

        if (debugMode) Debug.Log($"Animando cambio de color a {newColor}");

        // Animaci�n con DoTween
        colorTween = targetMaterial.DOColor(newColor, colorPropertyName, colorChangeDuration)
            .SetEase(easeType)
            .OnComplete(() => {
                if (debugMode) Debug.Log("Animaci�n de color completada");
            });
    }

    // Versi�n que permite especificar duraci�n
    public void ChangeColorWithTween(Color newColor, float duration)
    {
        colorChangeDuration = duration;
        ChangeColorWithTween(newColor);
    }

    // Restaurar color original con animaci�n
    public void RestoreOriginalColor()
    {
        ChangeColorWithTween(originalColor);
    }

    // Cambio inmediato sin animaci�n (para referencia)
    public void ChangeColorImmediate(Color newColor)
    {
        if (targetMaterial != null)
        {
            targetMaterial.SetColor(colorPropertyName, newColor);
        }
    }

    public void ChangeColor()
    {
        targetColor = GetComponent<Image>().color;
        ChangeColorWithTween(targetColor);
    }

    void OnDestroy()
    {
        // Limpiar tween al destruir el objeto
        colorTween?.Kill();
    }
}

public static class TransformExtensions
{
    public static Transform FindDeepChildWithTag(this Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                return child;

            Transform result = child.FindDeepChildWithTag(tag);
            if (result != null)
                return result;
        }
        return null;
    }
}