using UnityEngine;

// Espíritu que invoca el jefe: persigue al jugador un rato y luego se deshace.
// Se le puede matar de un espadazo (tiene Vida y el tag Enemigo).
public class Espiritu : MonoBehaviour
{
    public float velocidad = 1.8f;
    public float duracion = 7f;
    public float retraso = 0.6f;   // tiempo que tarda en aparecer antes de moverse

    Transform jugador;
    float nacimiento;

    void Start()
    {
        nacimiento = Time.time;
        GameObject j = GameObject.FindWithTag("Player");
        if (j != null) jugador = j.transform;
        Destroy(gameObject, duracion);
    }

    void Update()
    {
        if (jugador == null || Time.time - nacimiento < retraso) return;

        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
    }
}
