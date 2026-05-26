using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    private bool estaActivo = false;

    public void ActivarPortal()
    {
        estaActivo = true;
        // Opcional: Aquí puedes activar una animación o luz para mostrar que el portal sirve
        Debug.Log("¡Portal activado! Ya puedes cruzar.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el portal está activo y lo toca el jugador
        if (estaActivo && other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}