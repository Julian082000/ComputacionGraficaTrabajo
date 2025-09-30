using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Asegúrate de arrastrar tu objeto "HandPosition" a este campo en el Inspector.
    [SerializeField]
    private Transform weaponHolder; 

    // Referencia al arma equipada actualmente
    public GameObject currentWeapon { get; private set; }

    
    public void EquipWeapon(GameObject weaponToEquip)
    {
        // 1. Desequipar arma anterior (opcional, pero buena práctica)
        if (currentWeapon != null)
        {
            // En un juego completo, aquí soltarías o guardarías la antigua arma.
            // Por ahora, simplemente la destruimos.
            Destroy(currentWeapon);
        }

        // 2. Establecer el arma como el arma actual
        currentWeapon = weaponToEquip;

        // 3. Modificar la jerarquía: Mover el arma al punto de sujeción (HandPosition)
        currentWeapon.transform.parent = weaponHolder;

        // 4. Resetear Posición, Rotación y Escala
        
        // Posición: Cero, para que se alinee con la posición de "HandPosition".
        currentWeapon.transform.localPosition = Vector3.zero;
        
        // ROTACIÓN CRÍTICA: La rotación local cero (Quaternion.identity) hace que el arma
        // adopte la rotación de HandPosition. Si esto invierte los controles, significa que:
        // A) Tu modelo de arma mira hacia atrás. Usa Quaternion.Euler(0f, 180f, 0f).
        // B) Tu HandPosition mira hacia atrás. Rota HandPosition en el editor.
        
        // Empezaremos con identidad (rotación cero)
        currentWeapon.transform.localRotation = Quaternion.identity;
        
        // Escala
        currentWeapon.transform.localScale = Vector3.one;

        // 5. Desactivar la física y colisión
        
        // Desactivar Rigidbody
        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Usamos isKinematic = true para que el motor de física no lo mueva.
            rb.isKinematic = true;
        }

        // Desactivar Collider (para que no colisiones con tu propia cámara/cuerpo)
        Collider weaponCollider = currentWeapon.GetComponent<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }

        Debug.Log($"Arma '{currentWeapon.name}' equipada correctamente.");
    }
}