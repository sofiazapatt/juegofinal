using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlataformaController : MonoBehaviour
{
    [Header("Variables de Movimiento")]
    public float velocidadMover = 8f;
    public float fuerzaSalto = 14f;
    private float horizontal;
    private bool mirandoDerecha = true;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool enSuelo;

    [Header("Mecánica de Doble Salto")]
    private bool puedeDobleSalto;

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 1. Leer la entrada horizontal del teclado/mando
        horizontal = Input.GetAxisRaw("Horizontal");

        // 2. Detectar si el hongo está pisando el suelo físicamente
        enSuelo = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Control lógico de disponibilidad del doble salto
        if (enSuelo)
        {
            puedeDobleSalto = true; // Al tocar el piso, se recarga el doble salto
        }

        // 3. Control de impulsos físicos de salto (Se ejecuta antes de enviar datos al Animator)
        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo)
            {
                // Primer Salto Físico
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);

                // Disparar Trigger en el Animator
                if (anim != null)
                {
                    anim.ResetTrigger("DobleSalto"); // Limpieza de seguridad
                    anim.SetTrigger("Salto");
                }
                Debug.Log("⬆️ Primer Salto Ejecutado");

                // Forzamos temporalmente el estado a falso en este frame para evitar retrasos visuales
                enSuelo = false;
            }
            else if (puedeDobleSalto)
            {
                // Segundo Salto Físico (Doble Salto)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto * 0.85f);
                puedeDobleSalto = false; // Se gasta el cartucho del doble salto

                // Disparar Trigger en el Animator
                if (anim != null)
                {
                    anim.ResetTrigger("Salto"); // Limpieza de seguridad
                    anim.SetTrigger("DobleSalto");
                }
                Debug.Log("✨ ¡Doble Salto Ejecutado!");
            }
        }

        // 4. --- ENVIAR DATOS DEFINITIVOS AL ANIMATOR AL FINAL DE LA LOGICA ---
        if (anim != null)
        {
            // Enviamos el estado real del suelo a tu parámetro "enElSuelo"
            anim.SetBool("enElSuelo", enSuelo);

            // Enviamos la velocidad absoluta (siempre positiva) por si usas transiciones de movimiento
            anim.SetFloat("velocidadX", Mathf.Abs(horizontal));
        }

        VoltearSprite();
    }

    private void FixedUpdate()
    {
        // Aplicar el movimiento horizontal constante en el bucle de físicas
        rb.linearVelocity = new Vector2(horizontal * velocidadMover, rb.linearVelocity.y);
    }

    private void VoltearSprite()
    {
        // Voltea la escala del personaje en el eje X dependiendo de hacia dónde camine
        if (mirandoDerecha && horizontal < 0f || !mirandoDerecha && horizontal > 0f)
        {
            mirandoDerecha = !mirandoDerecha;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}