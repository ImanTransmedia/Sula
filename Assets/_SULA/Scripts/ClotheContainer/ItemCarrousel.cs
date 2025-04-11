using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCarrousel : MonoBehaviour
{
    public Image prendaImage;
    public TextMeshProUGUI prendaName;
    public TextMeshProUGUI prendaPrice;
    //public Color[] prendaColors;

    public void Configurar(Clothes prenda)
    {
        prendaImage.sprite = prenda.imagen;
        prendaName.text = prenda.clotheName;
        prendaPrice.text = prenda.price.ToString();
    }
}

