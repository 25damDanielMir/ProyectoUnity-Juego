using UnityEngine;
using UnityEngine.UI;

// Muestra los corazones del jugador.
public class HUDVida : MonoBehaviour
{
    public Vida vidaJugador;
    public Image[] corazones;
    public Sprite lleno;
    public Sprite vacio;

    void Update()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].enabled = i < vidaJugador.vidaMax;          // por si sube la vida máxima
            corazones[i].sprite = i < vidaJugador.vida ? lleno : vacio;
        }
    }
}