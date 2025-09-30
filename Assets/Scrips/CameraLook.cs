using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Look Settings")]
    public float sensitivity = 100f; 
    public float lookLimit = 80f;    

    private InputManager inputManager;
    private float rotationX = 0f; 
    
    private void Awake()
    {
        // Bloquear y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        
        // Obtener la referencia al InputManager del objeto padre (Player)
        inputManager = GetComponentInParent<InputManager>();
        
        if (inputManager == null)
        {
            Debug.LogError("ERROR: CameraLook no pudo encontrar el InputManager en el objeto padre.");
        }
    }

    private void LateUpdate()
    {
        // Ejecutar solo si tenemos el InputManager
        if (inputManager != null)
        {
            HandleCameraLook();
        }
    }

    // Dentro de CameraLook.cs

private void HandleCameraLook()
{
    // Obtener la entrada del ratón
    // CRÍTICO: Asegúrate de que Time.deltaTime esté aquí para suavizar el movimiento
    Vector2 look = inputManager.lookInput * sensitivity * Time.deltaTime; 
    
    // --- ROTACIÓN VERTICAL (Cámara) ---
    // La resta (-) es estándar para inversión de ejes vertical (mirar arriba = Y negativo)
    rotationX -= look.y;
    
    // LIMITACIÓN: Asegura que la rotación no se descontrole
    rotationX = Mathf.Clamp(rotationX, -lookLimit, lookLimit); 
    
    // Aplica la rotación vertical
    transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    
    // --- ROTACIÓN HORIZONTAL (Cuerpo) ---
    if (transform.parent != null)
    {
        // Esta línea solo usa look.x (Eje X) y no debería afectar el problema del Eje Y.
        transform.parent.Rotate(Vector3.up * look.x);
    }
}
}