using UnityEngine;
using UnityEngine.SceneManagement;

public class Pasarnivel : MonoBehaviour
{
    // Candado de seguridad para evitar múltiples cargas en la misma fracción de segundo
    private bool yaPasoDeNivel = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo actúa si lo toca el Player Y si el portal no se ha usado todavía en esta escena
        if (collision.CompareTag("Player") && !yaPasoDeNivel)
        {
            // Cerramos el candado inmediatamente para ignorar futuros roces físicos
            yaPasoDeNivel = true;

            Debug.Log("🚨 [PORTAL] El hongo activó el portal con éxito. Viajando al siguiente nivel...");

            // Calculamos de forma segura el índice de la siguiente escena en el Build Settings
            int siguienteEscenaIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // Cargamos la escena
            SceneManager.LoadScene(siguienteEscenaIndex);
        }
    }
}