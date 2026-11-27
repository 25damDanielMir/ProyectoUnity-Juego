using UnityEngine;

// Power-up: la espada del héroe que entró antes. Alarga la espada y su golpe
// para el resto de la partida. No quita más vida, solo llega más lejos,
// así el jefe sigue necesitando varios golpes.
public class EspadaHeroe : PowerUp
{
    public float alcance = 2f;     // la espada y su golpe el doble de largos (mismo daño)

    protected override void Aplicar(GameObject jugador)
    {
        if (GameManager.instancia != null) GameManager.instancia.alcanceEspada = alcance;
    }
}
