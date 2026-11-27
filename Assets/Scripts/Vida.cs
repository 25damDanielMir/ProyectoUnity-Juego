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
    public int puntosAlMorir = 50;        // puntos que da un enemigo al morir

    SpriteRenderer sprite;
    bool invencible = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        vida = vidaMax;

        // El jugador recupera la vida que tenía en el piso anterior
        if (esJugador && GameManager.instancia != null)
        {
            vidaMax = GameManager.instancia.vidaMax;
            vida = GameManager.instancia.vida;
        }
    }

    public void RecibirDanno(int danno)
    {
        if (invencible || vida <= 0)
            return;

        vida -= danno;

        // Aviso a otros scripts del mismo objeto (el Jefe lo usa para su destello)
        SendMessage("AlRecibirDanno", SendMessageOptions.DontRequireReceiver);

        if (vida <= 0)
        {
            Morir();
        }
        else
        {
            StartCoroutine(Parpadeo());
        }
        GuardarVida();
    }

    // Lo usan las pociones
    public void Curar(int cantidad)
    {
        vida = Mathf.Min(vida + cantidad, vidaMax);
        GuardarVida();
    }

    // Apunta la vida del jugador en el GameManager para el siguiente piso
    void GuardarVida()
    {
        if (esJugador && GameManager.instancia != null)
        {
            GameManager.instancia.vida = vida;
        }
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
            if (GameManager.instancia != null)
                GameManager.instancia.GameOver();
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (GameManager.instancia != null)
            GameManager.instancia.SumarPuntos(puntosAlMorir);

        // El jefe tiene su propia muerte (animación y luego Victoria)
        Jefe jefe = GetComponent<Jefe>();
        if (jefe != null)
        {
            jefe.Morir();
            return;
        }

        if (CompareTag("Jefe") && GameManager.instancia != null)
            GameManager.instancia.Victoria();
        Destroy(gameObject);
    }
}