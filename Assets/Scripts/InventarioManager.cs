using System.Collections.Generic;
using UnityEngine;

public class InventarioManager : MonoBehaviour
{
    // Clase interna para representar una casilla del inventario
    [System.Serializable]
    public class SlotInventario
    {
        public ItemData objeto;
        public int cantidad;

        public SlotInventario(ItemData nuevoObjeto, int nuevaCantidad)
        {
            objeto = nuevoObjeto;
            cantidad = nuevaCantidad;
        }
    }

    public List<SlotInventario> listaInventario = new List<SlotInventario>();
    public int capacidadMaxima = 12; // Número de cuadritos de tu inventario

    // Función para añadir un objeto al inventario
    public bool AgregarObjeto(ItemData objetoNuevo, int cantidad = 1)
    {
        // 1. Si es acumulable, buscamos si ya tenemos uno igual en el inventario
        if (objetoNuevo.esAcumulable)
        {
            foreach (SlotInventario slot in listaInventario)
            {
                if (slot.objeto == objetoNuevo && slot.cantidad < objetoNuevo.maxAcumulable)
                {
                    slot.cantidad += cantidad;
                    Debug.Log($"🎒 [INVENTARIO] Sumados {cantidad} a {objetoNuevo.nombreObjeto}. Total: {slot.cantidad}");
                    return true;
                }
            }
        }

        // 2. Si no es acumulable o no teníamos uno antes, revisamos si hay espacio libre
        if (listaInventario.Count < capacidadMaxima)
        {
            listaInventario.Add(new SlotInventario(objetoNuevo, cantidad));
            Debug.Log($"🎒 [INVENTARIO] ¡Nuevo objeto añadido: {objetoNuevo.nombreObjeto}!");
            return true;
        }

        Debug.LogWarning("🎒 [INVENTARIO] ¡El inventario está completamente lleno!");
        return false;
    }
}