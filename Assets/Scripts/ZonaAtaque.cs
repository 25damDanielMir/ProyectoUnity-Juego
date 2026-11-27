using UnityEngine;

// Collider de ataque: quita vida a lo que toque con el tag indicado
// (los enemigos normales) y también al jefe final.
public class ZonaAtaque : MonoBehaviour
{
    public string tagObjetivo = "Enemigo";
    public int danno = 1;

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag(tagObjetivo) || otro.CompareTag("Jefe"))
        {
            Vida v = otro.GetComponent<Vida>();
            if (v != null) v.RecibirDanno(danno);
        }
    }
}
