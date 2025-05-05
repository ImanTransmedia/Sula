using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FrameAnimationDetails : MonoBehaviour
{
    private VisualElement root;

    [Tooltip("Lista de texturas (frames) para la animación.")]
    public List<Texture2D> frames;

    [Tooltip("Duración en segundos de cada frame.")]
    public float frameDuration = 0.1f;

    private VisualElement _animationElement;

    private int _currentFrameIndex = 0;
    private float _timer = 0f;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        _animationElement = root.Q<VisualElement>("LowAnimation");

        if (_animationElement == null)
        {
            Debug.LogError("No se encontró el VisualElement con el nombre 'animation-container'.");
            enabled = false;
            return;
        }

        if (frames.Count > 0)
        {
            _animationElement.style.backgroundImage = new StyleBackground(Background.FromTexture2D(frames[0]));
        }
    }

    private void Update()
    {
        if (frames.Count <= 1) return;
        _animationElement.style.unityBackgroundImageTintColor = GameManager.Instance.actualRegion.darkColor;

        _timer += Time.deltaTime;

        if (_timer >= frameDuration)
        {
            _timer -= frameDuration;
            _currentFrameIndex = (_currentFrameIndex + 1) % frames.Count;
            _animationElement.style.backgroundImage = new StyleBackground(Background.FromTexture2D(frames[_currentFrameIndex]));
        }
    }
}