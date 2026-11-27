using UnityEngine;
using UnityEngine.SceneManagement;

// Música de fondo de todo el juego. Se crea sola al arrancar (no hay que ponerla
// en ninguna escena) y no se borra al cambiar de escena, así la música no se corta.
//  - Menú, Game Over y Victoria: MusicaMenu
//  - Piso 1 y Piso 2: MusicaCripta
//  - Piso 3 (la sala del jefe): MusicaJefe
// Las canciones están en Assets/Resources/Musica para poder cargarlas por nombre.
public class Musica : MonoBehaviour
{
    public static Musica instancia;
    public float volumen = 0.45f;

    AudioSource fuente;
    AudioClip menu, cripta, jefe;

    // Unity llama a este método solo, después de cargar la primera escena
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Crear()
    {
        if (instancia == null) new GameObject("Musica").AddComponent<Musica>();
    }

    void Awake()
    {
        if (instancia != null)
        {
            Destroy(gameObject);
            return;
        }
        instancia = this;
        DontDestroyOnLoad(gameObject);

        fuente = gameObject.AddComponent<AudioSource>();
        fuente.loop = true;
        fuente.playOnAwake = false;
        fuente.volume = volumen;

        menu = Resources.Load<AudioClip>("Musica/MusicaMenu");
        cripta = Resources.Load<AudioClip>("Musica/MusicaCripta");
        jefe = Resources.Load<AudioClip>("Musica/MusicaJefe");

        SceneManager.sceneLoaded += AlCargarEscena;
        AlCargarEscena(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        if (instancia == this) SceneManager.sceneLoaded -= AlCargarEscena;
    }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        if (escena.name == "Piso3")
            Poner(jefe);            // el Piso 3 es la sala del jefe
        else if (escena.name.StartsWith("Piso"))
            Poner(cripta);
        else
            Poner(menu);
    }

    // Lo llama el Jefe cuando te ve
    public void MusicaJefe()
    {
        Poner(jefe);
    }

    void Poner(AudioClip clip)
    {
        if (clip == null || fuente.clip == clip) return;   // si ya suena, no la reinicia
        fuente.clip = clip;
        fuente.Play();
    }
}
