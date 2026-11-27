using UnityEngine;

// Reja cerrada que solo se abre si el jugador lleva la llave.
// Si no la lleva, enseña un aviso encima de la reja.
public class PuertaLlave : MonoBehaviour
{
    public SpriteRenderer arriba;
    public SpriteRenderer abajo;
    public Sprite arribaAbierta;
    public Sprite abajoAbierta;
    public GameObject aviso;          // texto "Necesitas la llave"
    public float tiempoAviso = 2f;

    Collider2D bloqueo;
    float temporizador;

    void Start()
    {
        bloqueo = GetComponent<Collider2D>();
        if (aviso != null) aviso.SetActive(false);
    }

    void Update()
    {
        if (temporizador > 0)
        {
            temporizador -= Time.deltaTime;
            if (temporizador <= 0 && aviso != null) aviso.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D choque)
    {
        if (!choque.gameObject.CompareTag("Player") || GameManager.instancia == null) return;

        if (GameManager.instancia.tieneLlave)
        {
            Abrir();
        }
        else if (aviso != null)
        {
            aviso.SetActive(true);
            temporizador = tiempoAviso;
        }
    }

    void Abrir()
    {
        GameManager.instancia.tieneLlave = false;   // la llave se queda en la cerradura
        bloqueo.enabled = false;                    // ya se puede pasar
        if (arriba != null) arriba.sprite = arribaAbierta;
        if (abajo != null) abajo.sprite = abajoAbierta;
        if (aviso != null) aviso.SetActive(false);
    }
}
