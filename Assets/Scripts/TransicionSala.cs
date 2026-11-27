using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Va en un trigger en el hueco de la puerta. Cada vez que el jugador lo cruza,
// la cámara salta a la sala del otro lado y solo se ve esa sala.
public class TransicionSala : MonoBehaviour
{
    public Transform origen;        // punto de la cámara en la sala de este lado
    public Transform destino;       // punto de la cámara en la sala del otro lado
    public GameObject salaOrigen;   // la sala de este lado (Sala1, Sala2...)
    public GameObject salaDestino;  // la sala del otro lado

    float temporizador;
    BoxCollider2D colision;

    void Start()
    {
        colision = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (temporizador > 0)
        {
            temporizador -= Time.deltaTime;
        }
        else
        {
            colision.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Miramos qué sala está encendida para saber hacia dónde cruza.
        // Así funciona aunque el jugador aparezca en otra sala al volver de otro piso.
        if (salaOrigen.activeSelf)
        {
            MoverCamara(destino);
            salaDestino.SetActive(true);
            salaOrigen.SetActive(false);
        }
        else
        {
            MoverCamara(origen);
            salaOrigen.SetActive(true);
            salaDestino.SetActive(false);
        }

        // Igual que en el del profesor: lo apagamos un rato para que no salte dos veces
        colision.enabled = false;
        temporizador = 2.0f;
    }

    void MoverCamara(Transform punto)
    {
        Vector3 pos = punto.position;
        pos.z = Camera.main.transform.position.z;   // la cámara tiene que seguir en Z = -10
        Camera.main.transform.position = pos;
    }
}