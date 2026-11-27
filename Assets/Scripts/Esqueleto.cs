using UnityEngine;

// Enemigo basado en el Patrullante de PrimerProyecto:
// va y viene en horizontal y, si el jugador se acerca, le persigue.
// Lo usan el esqueleto y el murciélago (con distintos valores en el Inspector).
public class Esqueleto : MonoBehaviour
{
    public float velocidad = 1.5f;
    public float distanciaPatrulla = 3f;   // cuánto se aleja de donde empieza
    public float radioVision = 4f;         // a esta distancia empieza a perseguir

    Vector2 inicio;
    int direccion = 1;
    Vector2 movimiento;
    Transform jugador;
    Rigidbody2D cuerpo;
    SpriteRenderer sprite;
    Collider2D miCaja;
    Collider2D cajaJugador;

    void Start()
    {
        inicio = transform.position;
        cuerpo = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        miCaja = GetComponent<Collider2D>();

        GameObject j = GameObject.FindWithTag("Player");
        if (j != null)
        {
            jugador = j.transform;
            cajaJugador = j.GetComponent<Collider2D>();
        }
    }

    void Update()
    {
        if (jugador != null && Vector2.Distance(CentroPropio(), CentroJugador()) < radioVision)
        {
            // Perseguir: ir hacia la caja del jugador (sus pies), no hacia su posición,
            // para que los colliders lleguen a tocarse y HacerDanno funcione.
            movimiento = (CentroJugador() - CentroPropio()).normalized;
        }
        else
        {
            // Patrullar: si se ha alejado demasiado del inicio, da la vuelta
            float desplazamiento = transform.position.x - inicio.x;
            if (desplazamiento > distanciaPatrulla) direccion = -1;
            if (desplazamiento < -distanciaPatrulla) direccion = 1;
            movimiento = new Vector2(direccion, 0);
        }

        // Mira hacia donde anda (el sprite de 0x72 mira a la derecha)
        if (movimiento.x != 0) sprite.flipX = movimiento.x < 0;
    }

    void FixedUpdate()
    {
        cuerpo.velocity = movimiento * velocidad;
    }

    // Centro de su collider (tiene en cuenta el Offset), o su posición si no tiene
    Vector2 CentroPropio()
    {
        return miCaja != null ? (Vector2)miCaja.bounds.center : (Vector2)transform.position;
    }

    Vector2 CentroJugador()
    {
        return cajaJugador != null ? (Vector2)cajaJugador.bounds.center : (Vector2)jugador.position;
    }

    // Dibuja en la escena el círculo de visión cuando seleccionas el enemigo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioVision);
    }
}
