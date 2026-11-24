using UnityEngine;

// Movimiento del jugador en 4 direcciones con WASD.
// Lo hemos basado en el PlayerController de PrimerProyecto, pero moviendo
// con el Rigidbody2D para que choque con los muros.
public class PlayerController : MonoBehaviour
{
    public float velocidad = 4f;

    Vector2 movimiento;
    Rigidbody2D cuerpo;
    Animator anim;

    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        LeerMovimiento();
        ActualizarAnimacion();
    }

    void FixedUpdate()
    {
        // Movemos con física para que choque con los muros
        cuerpo.velocity = movimiento * velocidad;
    }

    void LeerMovimiento()
    {
        movimiento = Vector2.zero;

        if (Input.GetKey(KeyCode.D)) movimiento.x += 1;
        if (Input.GetKey(KeyCode.A)) movimiento.x -= 1;
        if (Input.GetKey(KeyCode.W)) movimiento.y += 1;
        if (Input.GetKey(KeyCode.S)) movimiento.y -= 1;

        // Para que en diagonal no vaya más rápido
        movimiento.Normalize();
    }

    void ActualizarAnimacion()
    {
        if (movimiento != Vector2.zero)
        {
            // Solo actualizamos la dirección al movernos,
            // así al pararse se queda mirando hacia donde iba
            anim.SetFloat("MovX", movimiento.x);
            anim.SetFloat("MovY", movimiento.y);
            anim.SetBool("Quieto", false);
        }
        else
        {
            anim.SetBool("Quieto", true);
        }
    }
}
