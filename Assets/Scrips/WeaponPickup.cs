using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    // Nombre de la etiqueta que identifica al jugador.
    private const string PlayerTag = "Player"; 

    // Bandera para asegurar que solo se detecte una vez
    private bool isBeingHeld = false; 

    // Se llama cuando otro collider entra en el área de trigger
    private void OnTriggerEnter(Collider other)
    {
        // 1. Verificar si el objeto que colisionó es el jugador
        if (other.CompareTag(PlayerTag) && !isBeingHeld)
        {
            // 2. Intentar obtener el WeaponManager del jugador (el objeto padre)
            WeaponManager playerWeaponManager = other.GetComponent<WeaponManager>();

            if (playerWeaponManager != null)
            {
                // Marcar como sostenida para evitar llamadas múltiples
                isBeingHeld = true; 
                
                // 3. Llamar al método de equipar
                // Pasamos el GameObject raíz de la recogida (este script está en él)
                playerWeaponManager.EquipWeapon(this.gameObject);
                
                // Nota: Si quieres un botón de "recoger" (E), esta lógica debe ir
                // en el Update del jugador, usando un raycast para detectar el arma.
            }
        }
    }
}