using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Power-up: la poción pequeña cura 1 corazón y la grande cura todos.
public class Pocion : MonoBehaviour
{
    public bool esGrande = false;

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            Vida vida = otro.GetComponent<Vida>();

            if (esGrande)
                vida.Curar(vida.vidaMax);   // cura todos los corazones
            else
                vida.Curar(1);              // cura un corazón

            Destroy(gameObject);
        }
    }
}