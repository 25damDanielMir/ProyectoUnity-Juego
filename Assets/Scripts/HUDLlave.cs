using UnityEngine;
using UnityEngine.UI;

// Enseña el icono de la llave en el HUD mientras el jugador la lleva.
public class HUDLlave : MonoBehaviour
{
    public Image icono;

    void Update()
    {
        icono.enabled = GameManager.instancia != null && GameManager.instancia.tieneLlave;
    }
}
