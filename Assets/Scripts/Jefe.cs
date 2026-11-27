using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Jefe final: el Verdugo (el héroe que entró antes y la cripta corrompió).
//  - Flota hacia el jugador.
//  - Si está cerca, da un tajo con la guadaña.
//  - Cada cierto tiempo invoca espíritus que persiguen al jugador.
//  - Al morir hace su animación y salta la pantalla de Victoria.
public class Jefe : MonoBehaviour
{
    public float velocidad = 1.3f;
    public float distanciaAtaque = 3.5f;
    public float tiempoEntreAtaques = 1.6f;
    public float tiempoEntreInvocaciones = 8f;
    public int espiritusPorInvocacion = 3;
    public float radioVision = 9f;            // no empieza hasta que el jugador se acerca

    public GameObject guadana;                // hijo con el collider del tajo (HacerDanno)
    public GameObject prefabEspiritu;
    public Light2D aura;                      // luz morada alrededor del jefe

    Transform jugador;
    Animator anim;
    SpriteRenderer sprite;
    Rigidbody2D cuerpo;
    bool ocupado = false;                     // atacando o invocando
    bool muerto = false;
    bool despierto = false;
    float siguienteAtaque;
    float siguienteInvocacion;
    float intensidadAura;

    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        cuerpo = GetComponent<Rigidbody2D>();
        GameObject j = GameObject.FindWithTag("Player");
        if (j != null) jugador = j.transform;
        if (guadana != null) guadana.SetActive(false);
        if (aura != null) intensidadAura = aura.intensity;
    }

    void Update()
    {
        if (muerto || jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (!despierto)
        {
            if (distancia > radioVision) return;
            despierto = true;
            siguienteInvocacion = Time.time + 3f;   // la primera invocación llega pronto
            if (Musica.instancia != null) Musica.instancia.MusicaJefe();
        }

        // Mira hacia el jugador (el sprite original mira a la derecha)
        if (!ocupado) sprite.flipX = jugador.position.x < transform.position.x;

        if (ocupado) return;

        if (Time.time >= siguienteInvocacion)
        {
            StartCoroutine(Invocar());
        }
        else if (distancia < distanciaAtaque && Time.time >= siguienteAtaque)
        {
            StartCoroutine(Atacar());
        }
    }

    void FixedUpdate()
    {
        if (muerto || !despierto || ocupado || jugador == null) return;

        // Flota hacia el jugador, sin pegarse del todo
        Vector2 dir = (Vector2)jugador.position - cuerpo.position;
        if (dir.magnitude > distanciaAtaque * 0.7f)
        {
            cuerpo.MovePosition(cuerpo.position + dir.normalized * velocidad * Time.fixedDeltaTime);
        }
    }

    IEnumerator Atacar()
    {
        ocupado = true;
        anim.SetTrigger("Atacar");

        // La guadaña hace daño en la parte central del tajo
        yield return new WaitForSeconds(0.35f);
        if (guadana != null)
        {
            Vector3 p = guadana.transform.localPosition;
            p.x = Mathf.Abs(p.x) * (sprite.flipX ? -1 : 1);
            guadana.transform.localPosition = p;
            guadana.SetActive(true);
        }
        yield return new WaitForSeconds(0.6f);
        if (guadana != null) guadana.SetActive(false);
        yield return new WaitForSeconds(0.35f);

        siguienteAtaque = Time.time + tiempoEntreAtaques;
        ocupado = false;
    }

    IEnumerator Invocar()
    {
        ocupado = true;
        anim.SetTrigger("Invocar");
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < espiritusPorInvocacion; i++)
        {
            // En círculo alrededor del jefe
            float angulo = i * Mathf.PI * 2f / espiritusPorInvocacion;
            Vector3 pos = transform.position + new Vector3(Mathf.Cos(angulo), Mathf.Sin(angulo)) * 2.5f;
            if (prefabEspiritu != null) Instantiate(prefabEspiritu, pos, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.5f);
        siguienteInvocacion = Time.time + tiempoEntreInvocaciones;
        ocupado = false;
    }

    // Lo llama Vida cada vez que le pegan: destello del aura
    void AlRecibirDanno()
    {
        if (aura != null && !muerto) StartCoroutine(Destello());
    }

    IEnumerator Destello()
    {
        aura.intensity = intensidadAura * 3f;
        aura.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        aura.intensity = intensidadAura;
        aura.color = new Color(0.65f, 0.35f, 1f);
    }

    // Lo llama Vida cuando se queda sin vida
    public void Morir()
    {
        if (muerto) return;
        muerto = true;
        StopAllCoroutines();
        if (guadana != null) guadana.SetActive(false);

        // Ya no hace daño ni se le puede pegar
        foreach (var c in GetComponents<Collider2D>()) c.enabled = false;
        var danno = GetComponent<HacerDanno>();
        if (danno != null) danno.enabled = false;

        // Desaparecen los espíritus que queden
        foreach (var e in FindObjectsOfType<Espiritu>()) Destroy(e.gameObject);

        anim.SetTrigger("Morir");
        StartCoroutine(Victoria());
    }

    IEnumerator Victoria()
    {
        // La animación de muerte dura unos 2 segundos
        float t = 0;
        while (t < 2.5f)
        {
            t += Time.deltaTime;
            if (aura != null) aura.intensity = Mathf.Lerp(intensidadAura, 0, t / 2.5f);
            yield return null;
        }
        if (GameManager.instancia != null) GameManager.instancia.Victoria();
    }
}
