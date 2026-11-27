using UnityEngine;
using UnityEngine.Rendering.Universal;

// Hace que una luz 2D parpadee como la llama de una antorcha.
[RequireComponent(typeof(Light2D))]
public class LuzParpadeo : MonoBehaviour
{
    public float intensidadBase = 1.1f;
    public float variacion = 0.25f;   // cuánto sube y baja
    public float velocidad = 3f;      // lo rápido que cambia

    Light2D luz;
    float semilla;

    void Start()
    {
        luz = GetComponent<Light2D>();
        semilla = Random.Range(0f, 100f);   // para que no parpadeen todas a la vez
    }

    void Update()
    {
        // PerlinNoise da valores suaves entre 0 y 1, así no da saltos bruscos
        float ruido = Mathf.PerlinNoise(semilla, Time.time * velocidad) - 0.5f;
        luz.intensity = intensidadBase + ruido * 2f * variacion;
    }
}
