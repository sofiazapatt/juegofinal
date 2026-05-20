using UnityEngine;

public class Pergamino : MonoBehaviour
{
    // Arrastra aquí tu PanelControles desde la jerarquía
    public GameObject PanelControles; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si lo que tocó el pergamino es el personaje
        if (other.CompareTag("Player")) 
        {
            PanelControles.SetActive(true); // Muestra la imagen
            Time.timeScale = 1f; // Opcional: Pausa el juego
        }
    }

    void Update()
    {
        // Cerrar la imagen si presionas una tecla (ej. Escape o E)
        if (PanelControles.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            PanelControles.SetActive(false);
            Time.timeScale = 1f; // Reanuda el juego
        }
    }
}