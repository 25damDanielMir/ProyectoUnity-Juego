using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Muestra solo una sala a la vez y centra la cámara en ella.
public class GestorSalas : MonoBehaviour
{
    public GameObject[] salas;   // Sala1, Sala2... en orden
    public int salaInicial = 0;

    void Start()
    {
        ActivarSala(salaInicial);
    }

    public void ActivarSala(int indice)
    {
        for (int i = 0; i < salas.Length; i++)
        {
            salas[i].SetActive(i == indice);
        }

        Vector3 centro = CentroDe(indice);
        transform.position = new Vector3(centro.x, centro.y, transform.position.z);
    }

    // El centro de la sala es el centro de su Tilemap de muros.
    public Vector3 CentroDe(int indice)
    {
        Tilemap muros = salas[indice].transform.Find("Muros").GetComponent<Tilemap>();
        muros.CompressBounds();   // ajusta el rectángulo a los tiles que hay de verdad
        return muros.transform.TransformPoint(muros.localBounds.center);
    }
}
