using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenciaPlayer : MonoBehaviour
{
    private static PersistenciaPlayer instancia;

    void Awake()
    {
        Debug.Log("🚨 [CONTROL] ¡El script PersistenciaPlayer ha despertado dentro de: " + gameObject.name + "!");

        if (instancia == null)
        {
            instancia = this;

            // Forzamos a que el hongo no tenga ningún objeto padre en la jerarquía
            transform.SetParent(null);

            DontDestroyOnLoad(gameObject); // El hongo se vuelve inmortal
            SceneManager.sceneLoaded += AlCargarNivel;
        }
        else
        {
            Debug.LogWarning("[PERSISTENCIA] Se detectó un hongo duplicado en la escena y fue destruido para conservar el original.");
            Destroy(gameObject); // Elimina clones al regresar a niveles anteriores
        }
    }

    void AlCargarNivel(Scene escena, LoadSceneMode modo)
    {
        // Esperamos un instante corto (0.02s) a que Unity termine de montar la escena
        Invoke(nameof(TeletransportarAlSpawnPoint), 0.02f);
    }

    void TeletransportarAlSpawnPoint()
    {
        // 1. Detectamos cuál es la escena real cargada en este instante (ej: Nivel 2)
        Scene escenaActiva = SceneManager.GetActiveScene();
        GameObject puntoAparicion = null;

        // 2. Buscamos TODOS los objetos llamados SpawnPoint en la memoria global
        GameObject[] todosLosSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        // 3. Filtramos para quedarnos ÚNICAMENTE con la banderita nativa del nivel actual
        foreach (GameObject spawn in todosLosSpawnPoints)
        {
            if (spawn.scene == escenaActiva)
            {
                puntoAparicion = spawn;
                break; // ¡Encontrado! Frenamos el bucle.
            }
        }

        // ==================== LÓGICA DE GÉNERO / ESCENA ADAPTATIVA ====================
        // Comprobamos si el nombre de la escena es exactamente el nivel de plataformas
        if (escenaActiva.name == "Nivel 4")
        {
            // Ocultamos el renderizado visual del hongo viejo inmortal
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            // Apagamos sus colisiones para que no interfiera en el mapa de plataformas
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            // Desactivamos su script de control Top-Down viejo (Cambia "PlayerController" por el tuyo si difiere)
            MonoBehaviour scriptMovimientoTopDown = GetComponent("PlayerController") as MonoBehaviour;
            if (scriptMovimientoTopDown != null) scriptMovimientoTopDown.enabled = false;

            // Mantenemos la escala intacta en 1, 1, 1 (así el colisionador no se deforma)
            transform.localScale = Vector3.one;

            Debug.Log("🔍 [SISTEMA] ¡Nivel 4 Detectado! Hongo Top-Down silenciado e invisible. El PlayerPlataformas toma el control.");
        }
        else
        {
            // Si regresa al Nivel 1, 2 o 3, volvemos a encender todos sus componentes nativos
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = true;

            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = true;

            MonoBehaviour scriptMovimientoTopDown = GetComponent("PlayerController") as MonoBehaviour;
            if (scriptMovimientoTopDown != null) scriptMovimientoTopDown.enabled = true;

            transform.localScale = Vector3.one;
        }
        // ==============================================================================

        // 4. Proceso de teletransportación física segura
        if (puntoAparicion != null)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Ponemos el Rigidbody en Kinematic para que no choque con paredes mientras se mueve
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;

                // Movemos tanto el motor de físicas (rb) como el motor visual (transform) al mismo tiempo
                rb.position = puntoAparicion.transform.position;
                transform.position = puntoAparicion.transform.position;
            }
            else
            {
                transform.position = puntoAparicion.transform.position;
            }

            // Retrasamos la reactivación del modo Dynamic otro instante (solo si no estamos en el Nivel 4)
            if (escenaActiva.name != "Nivel 4")
            {
                Invoke(nameof(ReactivarFisicas), 0.02f);
            }

            Debug.Log($"✅ [SISTEMA] Hongo fijado con éxito en el SpawnPoint REAL del {escenaActiva.name}.");
        }
        else
        {
            Debug.LogWarning($"⚠️ [ADVERTENCIA] No se encontró ningún 'SpawnPoint' nativo dentro de la escena activa: {escenaActiva.name}");
        }
    }

    void ReactivarFisicas()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Devolvemos el cuerpo al modo Dynamic para que pueda caminar normalmente
            rb.bodyType = RigidbodyType2D.Dynamic;
            Debug.Log("🕹️ [SISTEMA] Físicas reactivadas con éxito. ¡Hongo libre para jugar!");
        }
    }

    void OnDestroy()
    {
        if (instancia == this)
        {
            SceneManager.sceneLoaded -= AlCargarNivel;
        }
    }
}