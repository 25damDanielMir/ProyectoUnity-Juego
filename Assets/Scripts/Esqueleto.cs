using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemigo basado en el Patrullante de PrimerProyecto:
// va y viene en horizontal y, si el jugador se acerca, le persigue.
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

    void Start()
    {
        inicio = transform.position;
        cuerpo = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        GameObject j = GameObject.FindWithTag("Player");
        if (j != null) jugador = j.transform;
    }

    void Update()
    {
        if (jugador != null && Vector2.Distance(transform.position, jugador.position) < radioVision)
        {
            // Perseguir: ir en línea recta hacia el jugador
            movimiento = ((Vector2)jugador.position - (Vector2)transform.position).normalized;
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

    // Dibuja en la escena el círculo de visión cuando seleccionas el esqueleto
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioVision);
    }
}