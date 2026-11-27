using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Botones de las pantallas Menu, GameOver y Victoria.
public class MenuBotones : MonoBehaviour
{
    public TMP_Text textoPuntos;   // opcional: donde enseñar la puntuación

    void Start()
    {
        if (textoPuntos != null && GameManager.instancia != null)
        {
            textoPuntos.text = "Puntos: " + GameManager.instancia.puntos;
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