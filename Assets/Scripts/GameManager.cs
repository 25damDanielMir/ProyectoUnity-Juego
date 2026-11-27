using UnityEngine;
using UnityEngine.SceneManagement;

// Guarda los puntos y la vida entre escenas y controla
// el paso de pantalla, la derrota y la victoria.
public class GameManager : MonoBehaviour
{
    // Acceso desde cualquier script: GameManager.instancia
    public static GameManager instancia;

    public int puntos = 0;
    public int vidaMax = 3;
    public int vida = 3;

    // Por qué puerta tiene que aparecer el jugador al cargar el piso:
    // "Entrada" (viene del piso anterior) o "Salida" (vuelve del piso siguiente)
    public string llegada = "";

    float tiempoInicio;

    void Awake()
    {
        // Si ya hay un GameManager de la escena anterior, este sobra
        if (instancia != null)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);   // no se borra al cambiar de escena
        tiempoInicio = Time.time;
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
    }

    // Carga la escena que va después en Build Settings
    public void SiguienteEscena()
    {
        CambiarPiso(1);
    }

    // +1 = bajar al piso siguiente, -1 = volver al anterior
    public void CambiarPiso(int desplazamiento)
    {
        llegada = desplazamiento > 0 ? "Entrada" : "Salida";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + desplazamiento);
    }

    public void GameOver()
    {
        CargarSiExiste("GameOver");
    }

    public void Victoria()
    {
        // Bonus por tiempo: cuanto antes termines, más puntos (máximo 300)
        int segundos = (int)(Time.time - tiempoInicio);
        puntos += Mathf.Max(0, 300 - segundos);
        CargarSiExiste("Victoria");
    }

    // Para el botón Jugar / Reintentar
    public void NuevaPartida()
    {
        puntos = 0;
        vida = vidaMax;
        tiempoInicio = Time.time;
        llegada = "Entrada";
        SceneManager.LoadScene("Piso1");
    }

    // Mientras no existan las escenas GameOver/Victoria, reinicia la actual
    void CargarSiExiste(string escena)
    {
        if (Application.CanStreamedLevelBeLoaded(escena))
        {
            SceneManager.LoadScene(escena);
        }
        else
        {
            Debug.Log("Falta la escena " + escena + " en Build Settings");
            vida = vidaMax;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}