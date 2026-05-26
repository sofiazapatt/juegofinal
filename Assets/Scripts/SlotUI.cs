using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Image imagenIcono;
    public TextMeshProUGUI textoCantidad;

    // Esta función limpia el slot si no hay objeto
    public void LimpiarSlot()
    {
        imagenIcono.sprite = null;
        imagenIcono.enabled = false;
        textoCantidad.text = "";
    }

    // Esta función dibuja el objeto real en la pantalla
    public void ActualizarSlot(ItemData objeto, int cantidad)
    {
        if (objeto != null)
        {
            imagenIcono.sprite = objeto.icono;
            imagenIcono.enabled = true;

            // Si es acumulable y hay más de 1, muestra el número
            if (objeto.esAcumulable && cantidad > 1)
            {
                textoCantidad.text = cantidad.ToString();
            }
            else
            {
                textoCantidad.text = ""; // Oculta el número si es 1 o no acumulable
            }
        }
        else
        {
            LimpiarSlot();
        }
    }
}