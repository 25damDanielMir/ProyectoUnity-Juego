using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Power-up: al cogerla cura un corazón y desaparece.
public class Pocion : MonoBehaviour
{
    public int curacion = 1;

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            otro.GetComponent<Vida>().Curar(curacion);
            Destroy(gameObject);
        }
    }
}