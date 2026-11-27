using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Botones de las pantallas Menu, GameOver y Victoria.
public class MenuBotones : MonoBehaviour
{
    public TMP_Text textoPuntos;   // opcional: donde enseñar la puntuación
    public TMP_Text textoTiempo;   // opcional: el tiempo que ha durado la partida (Victoria)

    void Start()
    {
        if (GameManager.instancia == null) return;

        if (textoPuntos != null)
        {
            textoPuntos.text = "Puntos: " + GameManager.instancia.puntos;
        }

        if (textoTiempo != null)
        {
            int segundos = (int)GameManager.instancia.tiempoTotal;
            // "D2" escribe siempre dos cifras: 1:05 en vez de 1:5
            textoTiempo.text = "Tiempo: " + (segundos / 60) + ":" + (segundos % 60).ToString("D2");
        }
    }

    // Botón Jugar / Reintentar: empieza desde el piso 1 con todo a cero
    public void Jugar()
    {
        if (GameManager.instancia != null)
            GameManager.instancia.NuevaPartida();
        else
            SceneManager.LoadScene("Piso1");
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Salir()
    {
        Debug.Log("Salir del juego");   // en el editor no se cierra, en el .exe sí
        Application.Quit();
    }
}