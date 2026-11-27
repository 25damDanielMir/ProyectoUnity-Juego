using System.Collections;
using UnityEngine;

// Movimiento en 4 direcciones con WASD y ataque con Espacio.
//Esta basado en el movimiento jugador que hicimos en el primer proyecto( el movimientojugador vaya)
public class PlayerController : MonoBehaviour
{
    public float velocidad = 4f;
    public GameObject ataque;            // el hijo "Ataque" (espada + collider)
    public float duracionAtaque = 0.3f;  // lo mismo que dura la animación AtaqueEspada
    public float multiplicadorVelocidad = 1f;   // lo sube la Poción de Velocidad
    public Sprite espadaMejorada;               // espada dorada del power-up Espada del Héroe

    Vector2 movimiento;
    Vector2 direccion = Vector2.down;    // hacia dónde mira (empieza mirando abajo)
    bool atacando = false;
    Rigidbody2D cuerpo;
    Animator anim;
    SpriteRenderer espada;

    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        espada = ataque.GetComponentInChildren<SpriteRenderer>(true);
        ataque.SetActive(false);
    }

    void Update()
    {
        // El jugador puede moverse también mientras ataca
        LeerMovimiento();
        ActualizarAnimacion();

        // Solo se puede empezar un ataque si no hay otro en marcha
        if (Input.GetKeyDown(KeyCode.Space) && !atacando)
        {
            StartCoroutine(Atacar());
        }
    }

    void FixedUpdate()
    {
        cuerpo.velocity = movimiento * velocidad * multiplicadorVelocidad;
    }

    void LeerMovimiento()
    {
        movimiento = Vector2.zero;

        if (Input.GetKey(KeyCode.D)) movimiento.x += 1;
        if (Input.GetKey(KeyCode.A)) movimiento.x -= 1;
        if (Input.GetKey(KeyCode.W)) movimiento.y += 1;
        if (Input.GetKey(KeyCode.S)) movimiento.y -= 1;

        movimiento.Normalize();

        if (movimiento != Vector2.zero)
        {
            direccion = movimiento;      // guardamos hacia dónde mira
        }
    }

    void ActualizarAnimacion()
    {
        if (movimiento != Vector2.zero)
        {
            anim.SetFloat("MovX", movimiento.x);
            anim.SetFloat("MovY", movimiento.y);
            anim.SetBool("Quieto", false);
        }
        else
        {
            anim.SetBool("Quieto", true);
        }
    }

    IEnumerator Atacar()
    {
        atacando = true;

        // La espada sale de la mano (un poco por delante del cuerpo)
        // y la giramos hacia donde mira el jugador
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        ataque.transform.localPosition = (Vector3)(direccion * 0.15f) + new Vector3(0, -0.3f, 0);
        ataque.transform.localRotation = Quaternion.Euler(0, 0, angulo - 90);

        // Con la Espada del Héroe, la espada y su golpe son más largos (el daño es el mismo)
        float alcance = GameManager.instancia != null ? GameManager.instancia.alcanceEspada : 1f;
        ataque.transform.localScale = Vector3.one * alcance;
        if (alcance > 1f && espadaMejorada != null) espada.sprite = espadaMejorada;

        // Si mira hacia arriba, la espada va detrás del jugador
        espada.sortingOrder = direccion.y > 0.5f ? 9 : 11;

        ataque.SetActive(true);
        anim.SetTrigger("Atacar");           // reproduce el tajo de la espada
        yield return new WaitForSeconds(duracionAtaque);
        ataque.SetActive(false);

        atacando = false;
    }

    // Lo llama la Poción de Velocidad
    Coroutine aceleracion;

    public void Acelerar(float multiplicador, float duracion)
    {
        // Si ya estaba acelerado, se vuelve a contar el tiempo desde cero
        if (aceleracion != null) StopCoroutine(aceleracion);
        aceleracion = StartCoroutine(Aceleracion(multiplicador, duracion));
    }

    IEnumerator Aceleracion(float multiplicador, float duracion)
    {
        multiplicadorVelocidad = multiplicador;
        yield return new WaitForSeconds(duracion);
        multiplicadorVelocidad = 1f;
    }
}