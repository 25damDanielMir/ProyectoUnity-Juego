using UnityEngine;

// Punto delante de una puerta donde aparece el jugador al llegar al piso.
// Si el GameManager dice que llegamos por esta puerta, colocamos al jugador aquí
// y encendemos la sala en la que está.
public class PuntoLlegada : MonoBehaviour
{
    public string puerta = "Entrada";   // "Entrada" o "Salida"
    public int sala = 0;                // índice de la sala en el GestorSalas (0 = Sala1)

    void Start()
    {
        if (GameManager.instancia == null || GameManager.instancia.llegada != puerta) return;

        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            jugador.transform.position = transform.position;
        }

        GestorSalas gestor = FindObjectOfType<GestorSalas>();
        if (gestor != null)
        {
            gestor.salaInicial = sala;   // por si el GestorSalas arranca después que este script
            gestor.ActivarSala(sala);
        }
    }
}
