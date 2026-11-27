using UnityEngine;

// Trigger delante de una puerta entre pisos.
//  - Puerta de salida: baja al piso siguiente (o da la victoria si es la del final).
//  - Puerta de entrada (vuelve marcado): vuelve al piso anterior.
public class Escaleras : MonoBehaviour
{
    public bool esFinal = false;   // la salida del último piso: victoria
    public bool vuelve = false;    // la entrada de un piso: vuelve al anterior

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player") || GameManager.instancia == null) return;

        if (esFinal)
            GameManager.instancia.Victoria();         // pantalla de victoria + bonus por tiempo
        else if (vuelve)
            GameManager.instancia.CambiarPiso(-1);    // al piso anterior, por su puerta de salida
        else
            GameManager.instancia.CambiarPiso(1);     // al piso siguiente, por su puerta de entrada
    }
}
