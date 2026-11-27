using UnityEngine;

// Para pinchos y enemigos: hace daño al jugador al tocarlo.
public class HacerDanno : MonoBehaviour
{
    public int danno = 1;

    // Si el collider es Trigger (pinchos)
    void OnTriggerStay2D(Collider2D otro)
    {
        Danar(otro.gameObject);
    }

    // Si el collider es sólido (enemigos)
    void OnCollisionStay2D(Collision2D choque)
    {
        Danar(choque.gameObject);
    }

    void Danar(GameObject objeto)
    {
        if (objeto.CompareTag("Player"))
        {
            Vida v = objeto.GetComponent<Vida>();
            if (v != null) v.RecibirDanno(danno);
        }
    }
}