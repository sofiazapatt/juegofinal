using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.025f;
    public Vector3 desplazamiento;

    private void Start()
    {
        // SOLUCIÓN: Si al iniciar el nivel la cámara no tiene a quién seguir (como en los niveles 2, 3, 4 y 5)
        if (objetivo == null)
        {
            // Busca al hongo en la escena usando su etiqueta "Player"
            GameObject jugadorInmortal = GameObject.FindWithTag("Player");

            if (jugadorInmortal != null)
            {
                objetivo = jugadorInmortal.transform;
                Debug.Log("¡Cámara conectada automáticamente al hongo viajero!");
            }
            else
            {
                Debug.LogWarning("La cámara no encontró a ningún objeto con la etiqueta 'Player'.");
            }
        }
    }

    private void LateUpdate()
    {
        // Protección: Si por alguna razón sigue sin haber objetivo, no hace nada para evitar errores rojos
        if (objetivo == null) return;

        Vector3 posicionDeseada = objetivo.position + desplazamiento;

        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);

        transform.position = posicionSuavizada;
    }
}