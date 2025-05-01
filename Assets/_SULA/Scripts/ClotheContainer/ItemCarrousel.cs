using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCarrousel : MonoBehaviour
{
    public Image prendaImage;
    public TextMeshProUGUI prendaName;

    //public Color[] prendaColors;

    public void Configurar(Clothes prenda)
    {
        prendaImage.sprite = prenda.menuImage;
        prendaName.text = prenda.clotheName;
    }
}

