using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 8f;
    public float fuerzaDeSalto = 14f;
    public float fuerzaDeSegundoSalto = 12f;

    [Header("Detección de Suelo (Solo Nivel 4)")]
    public Transform controladorSuelo;
    public float longitudLaser = 0.7f; // <- Lo subimos un poco por la escala grande de tu hongo
    public LayerMask capaSuelo;

    [Header("Configuración de Doble Salto")]
    public int saltosTotalesPermitidos = 2;
    private int saltosRestantes;

    private Rigidbody2D rb;
    private Animator animator;
    private float movimientoHorizontal = 0f;
    private bool enElSuelo;
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        saltosRestantes = saltosTotalesPermitidos;

        // Si estamos en el Nivel 4, busca automáticamente al objeto hijo en tus pies
        if (controladorSuelo == null)
        {
            Transform hijoSuelo = transform.Find("GroundCheck");
            if (hijoSuelo != null)
            {
                controladorSuelo = hijoSuelo;
                Debug.Log("¡Controlador de suelo (GroundCheck) enlazado para el Nivel 4!");
            }
        }
    }

    void Update()
    {
        // 1. Detección del suelo constante en Update para sincronía con las animaciones
        if (controladorSuelo != null)
        {
            enElSuelo = Physics2D.Raycast(controladorSuelo.position, Vector2.down, longitudLaser, capaSuelo);
            if (enElSuelo)
            {
                saltosRestantes = saltosTotalesPermitidos;
            }
        }
        else
        {
            enElSuelo = true; // Niveles 1, 2 y 3
        }

        // 2. Leer ejes de movimiento
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // 3. Control del Salto con la tecla W
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (enElSuelo)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaDeSalto);
                if (animator != null) animator.SetTrigger("Salto");
                enElSuelo = false; // Forzamos el estado inmediato para la transición visual
            }
            else if (saltosRestantes > 1)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaDeSegundoSalto);
                if (animator != null) animator.SetTrigger("DobleSalto");
                saltosRestantes--;
            }
        }

        // 4. ENVIAR DATOS AL ANIMATOR AL FINAL DEL UPDATE
        if (animator != null)
        {
            animator.SetBool("enElSuelo", enElSuelo);
            // Pasamos la velocidad absoluta para que tu estado 'playernivel4' sepa cuándo moverse
            animator.SetFloat("velocidadX", Mathf.Abs(movimientoHorizontal));
        }

        VoltearSprite();
    }

    void FixedUpdate()
    {
        // Aplicamos el movimiento físico real usando la velocidad configurada
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    private void VoltearSprite()
    {
        if (mirandoDerecha && movimientoHorizontal < 0f || !mirandoDerecha && movimientoHorizontal > 0f)
        {
            mirandoDerecha = !mirandoDerecha;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = enElSuelo ? Color.green : Color.red;
            Gizmos.DrawLine(controladorSuelo.position, controladorSuelo.position + Vector3.down * longitudLaser);
        }
    }
}