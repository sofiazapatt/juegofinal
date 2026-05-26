using UnityEngine;

public class ItemMundo : MonoBehaviour
{
    public ItemData datosDelObjeto; // Arrastra aquí la tarjeta del objeto creado en el Paso 1
    public int cantidadAEntregar = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobamos si el objeto que nos tocó es el jugador
        if (collision.CompareTag("Player"))
        {
            InventarioManager inventario = collision.GetComponent<InventarioManager>();

            if (inventario != null)
            {
                // Intentamos meter el objeto al inventario del hongo
                bool recogidoExitosamente = inventario.AgregarObjeto(datosDelObjeto, cantidadAEntregar);

                if (recogidoExitosamente)
                {
                    // Desaparece el objeto del piso del mapa
                    Destroy(gameObject);
                }
            }
        }
    }
}