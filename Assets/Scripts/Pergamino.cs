using UnityEngine;

public class InteraccionPergamino : MonoBehaviour
{
    // Esta es la variable que guardará el panel grande de la interfaz
    [SerializeField] private GameObject panelPergaminoGrande;

    // Se activa automáticamente cuando el personaje entra en el área del pergamino
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos si el objeto que entró es el jugador (puedes usar el Tag "Player")
        if (other.CompareTag("Player"))
        {
            MostrarPanel(true);
        }
    }

    // Se activa automáticamente cuando el personaje se aleja del pergamino
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MostrarPanel(false);
        }
    }

    private void MostrarPanel(bool estado)
    {
        if (panelPergaminoGrande != null)
        {
            panelPergaminoGrande.SetActive(estado);
        }
        else
        {
            Debug.LogError("¡Ojo! No has arrastrado el Panel al Inspector de este pergamino.");
        }
    }
}