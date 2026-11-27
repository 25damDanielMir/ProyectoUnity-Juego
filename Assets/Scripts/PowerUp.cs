using UnityEngine;

// Clase base de los power-ups: flotan para que se vean y, al tocarlos el jugador,
// aplican su efecto y desaparecen. Cada power-up concreto hereda de esta clase
// y solo tiene que decir qué hace en Aplicar().
public abstract class PowerUp : MonoBehaviour
{
    public int puntos = 20;
    public float flotar = 0.1f;

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
        if (!otro.CompareTag("Player")) return;

        Aplicar(otro.gameObject);
        if (GameManager.instancia != null) GameManager.instancia.SumarPuntos(puntos);
        Destroy(gameObject);
    }

    // Lo que hace cada power-up
    protected abstract void Aplicar(GameObject jugador);
}
