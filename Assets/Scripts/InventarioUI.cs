using UnityEngine;

public class InventarioUI : MonoBehaviour
{
    public GameObject panelInventarioVisual; // El PanelInventario que tiene el fondo de libro
    public SlotUI[] slotsUI; // Array donde arrastraste los 12 slots del contenedor
    private InventarioManager inventarioPlayer;

    void Start()
    {
        // Esto hace que este Canvas sea inmortal y viaje entre escenas sin borrarse
        DontDestroyOnLoad(gameObject);

        // Buscamos al hongo en la escena inicial para leer su mochila
        BuscarAlPlayer();

        // El inventario empieza cerrado al iniciar el juego
        panelInventarioVisual.SetActive(false);
        ActualizarInterfaz();
    }

    void Update()
    {
        // Abrir y cerrar con la tecla I
        if (Input.GetKeyDown(KeyCode.I))
        {
            panelInventarioVisual.SetActive(!panelInventarioVisual.activeSelf);

            // Si lo abrimos, actualizamos lo que hay dentro
            if (panelInventarioVisual.activeSelf)
            {
                ActualizarInterfaz();
            }
        }
    }

    // Función auxiliar para asegurarnos de encontrar al hongo en cualquier nivel
    private void BuscarAlPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            inventarioPlayer = player.GetComponent<InventarioManager>();
        }
    }

    public void ActualizarInterfaz()
    {
        // Si por algún cambio de escena se perdió la referencia del hongo, la volvemos a buscar
        if (inventarioPlayer == null)
        {
            BuscarAlPlayer();
        }

        // Si después de buscarlo sigue sin existir (por seguridad), detenemos el proceso
        if (inventarioPlayer == null) return;

        // Recorremos los 12 slots visuales
        for (int i = 0; i < slotsUI.Length; i++)
        {
            // Si el jugador tiene un objeto en esta posición de la lista
            if (i < inventarioPlayer.listaInventario.Count)
            {
                var slotLogico = inventarioPlayer.listaInventario[i];
                slotsUI[i].ActualizarSlot(slotLogico.objeto, slotLogico.cantidad);
            }
            else
            {
                // Si no hay objeto en esta posición, el slot visual se limpia
                slotsUI[i].LimpiarSlot();
            }
        }
    }
}