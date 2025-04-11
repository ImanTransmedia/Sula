using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeController : MonoBehaviour
{
    public ClotheContainerController carrusel;

    private Vector2 inicioTacto;
    private bool tocando;

    void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 posicionActual = Touchscreen.current.primaryTouch.position.ReadValue();

            if (!tocando)
            {
                inicioTacto = posicionActual;
                tocando = true;
            }
            else
            {
                Vector2 desplazamiento = posicionActual - inicioTacto;

                if (Mathf.Abs(desplazamiento.x) > 100) // Sensibilidad swipe
                {
                    if (desplazamiento.x > 0)
                        carrusel.MoverIzquierda();
                    else
                        carrusel.MoverDerecha();

                    tocando = false; // para evitar múltiples llamadas
                }
            }
        }
        else
        {
            tocando = false;
        }

        // Alternativamente, para pruebas con mouse:
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Esta TOCANDOMEEEE!!!");
            inicioTacto = Mouse.current.position.ReadValue();
            tocando = true;
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed && tocando)
        {
            Vector2 desplazamiento = Mouse.current.position.ReadValue() - inicioTacto;
            if (Mathf.Abs(desplazamiento.x) > 100)
            {
                if (desplazamiento.x > 0)
                    carrusel.MoverIzquierda();
                else
                    carrusel.MoverDerecha();

                tocando = false;
            }
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Debug.Log("NO TE ESTOY TOCANDO!");
            tocando = false;
        }
    }
}
