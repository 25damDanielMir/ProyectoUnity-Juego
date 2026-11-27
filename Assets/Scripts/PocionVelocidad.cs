using UnityEngine;

// Power-up: el jugador va más rápido durante un rato.
public class PocionVelocidad : PowerUp
{
    public float multiplicador = 1.5f;   // +50 % de velocidad
    public float duracion = 10f;

    protected override void Aplicar(GameObject jugador)
    {
        PlayerController pc = jugador.GetComponent<PlayerController>();
        if (pc != null) pc.Acelerar(multiplicador, duracion);
    }
}
