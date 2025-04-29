using DG.Tweening;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    public float zoomScale = 1.2f;
    public float zoomDuration = 0.3f;

    public void OnButtonClick()
    {
        transform.DOScale(zoomScale, zoomDuration)
                 .SetEase(Ease.OutBack)
                 .OnComplete(() => {
                     transform.DOScale(1f, zoomDuration);
                 });
    }
}