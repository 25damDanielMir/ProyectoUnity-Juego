using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Vida para el jugador y los enemigos.
public class Vida : MonoBehaviour
{
    public int vidaMax = 3;
    public int vida = 3;
    public bool esJugador = false;
    public float tiempoInvencible = 1f;   // tras un golpe no recibe más daño

    SpriteRenderer sprite;
    bool invencible = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        vida = vidaMax;
    }

    public void RecibirDanno(int danno)
    {
        if (invencible || vida <= 0)
            return;

        vida -= danno;

        if (vida <= 0)
        {
            Morir();
        }
        else
        {
            StartCoroutine(Parpadeo());
        }
    }

    // Lo usará la poción 
    public void Curar(int cantidad)
    {
        vida = Mathf.Min(vida + cantidad, vidaMax);
    }

    // Parpadea en rojo mientras es invencible
    IEnumerator Parpadeo()
    {
        invencible = true;
        float tiempo = 0;
        while (tiempo < tiempoInvencible)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            tiempo += 0.2f;
        }
        invencible = false;
    }

    void Morir()
    {
        if (esJugador)
        {
            // De momento reinicia la escena. Mañana será la pantalla de Game Over
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}