using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movements : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float jumpHeight = 2.0f;
    
    [Header("Gravity Settings")]
    // Gravedad aumentada para asegurar que cae correctamente
    public float gravity = -9.81f * 3; 

    // Fuerza que empuja hacia abajo para asegurar el contacto con el suelo.
    private const float GroundedPush = -2.0f; 

    // --- Componentes ---
    private CharacterController characterController;
    private InputManager inputManager;
    private Animator animator; 
    
    // --- Variables de Estado ---
    private Vector3 moveDirection; 
    private float jumpVelocity; 
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>(); 
        
        // Busca el Animator en el propio objeto o en sus hijos (donde está el modelo)
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
             Debug.LogWarning("Animator no encontrado. La animación de caminar no funcionará.");
        }
    }

    private void Update()
    {
        // 1. Manejo de Gravedad y Salto (Eje Y)
        if (characterController.isGrounded)
        {
            // Aplicamos un empuje fuerte para evitar el flotado y asegurar isGrounded = true
            jumpVelocity = GroundedPush; 

            if (inputManager.jumpTriggered)
            {
                jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                inputManager.jumpTriggered = false; 
            }
        }
        else
        {
            // Aplicar aceleración de gravedad
            jumpVelocity += gravity * Time.deltaTime;
        }

        // 2. Manejo de Movimiento Horizontal (Ejes X y Z)
        Vector2 input = inputManager.horizontalInput; 

        Vector3 forward = transform.forward * input.y;
        Vector3 right = transform.right * input.x;
        
        Vector3 desiredMove = forward + right;

        // Normalizar solo si el input es significativo (para evitar el 'flotado' fantasma)
        if (desiredMove.magnitude > 1f) 
        {
             desiredMove.Normalize();
        }

        // 3. Ensamblar la Dirección Final
        moveDirection.x = desiredMove.x * moveSpeed;
        moveDirection.z = desiredMove.z * moveSpeed;
        moveDirection.y = jumpVelocity; 

        // 4. Aplicar el Movimiento
        characterController.Move(moveDirection * Time.deltaTime);
        
        // 5. SINCRONIZACIÓN DE ANIMACIÓN
        if (animator != null)
        {
            // Calcula la velocidad horizontal (magnitud de XZ)
            float currentHorizontalSpeed = new Vector3(moveDirection.x, 0, moveDirection.z).magnitude;
            
            // Suaviza la velocidad para transiciones de animación más suaves
            float animationSpeed = Mathf.Lerp(animator.GetFloat("Speed"), currentHorizontalSpeed, Time.deltaTime * 10f);
            
            // Establece el parámetro "Speed" en el Animator (CRÍTICO: debe ser "Speed")
            animator.SetFloat("Speed", animationSpeed);
        }
    }
}