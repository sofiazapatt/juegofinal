using UnityEngine;

[CreateAssetMenu(fileName = "NuevoObjeto", menuName = "Inventario/Objeto")]
public class ItemData : ScriptableObject
{
    public string nombreObjeto;
    [TextArea(2, 4)] public string descripcion;
    public Sprite icono; // El dibujo pixel art que se verá en el inventario
    public bool esAcumulable; // ¿Se pueden tener muchos en un solo slot? (ej: pociones)
    public int maxAcumulable = 99;
}