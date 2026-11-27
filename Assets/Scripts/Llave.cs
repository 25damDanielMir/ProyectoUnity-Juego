using UnityEngine;

// Llave que abre la reja del piso. Al cogerla se apunta en el GameManager.
public class Llave : MonoBehaviour
{
    public int puntos = 25;
    public float flotar = 0.12f;   // sube y baja para que llame la atención

    Vector3 inicio;

    void Start()
    {
        inicio = transform.position;
    }

    void Update()
    {
        transform.position = inicio + Vector3.up * Mathf.Sin(Time.time * 3f) * flotar;
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player") || GameManager.instancia == null) return;

        GameManager.instancia.tieneLlave = true;
        GameManager.instancia.SumarPuntos(puntos);
        Destroy(gameObject);
    }
}
