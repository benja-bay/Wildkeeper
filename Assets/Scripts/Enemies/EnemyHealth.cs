// ==============================
// EnemyHealth.cs
// Clase que gestiona la salud del enemigo y actualiza su comportamiento al recibir daño
// ==============================

using Systems;

namespace Enemies
{
    // Esta clase gestiona la salud del enemigo y extiende la funcionalidad de una clase base llamada Health
    public class EnemyHealth : Health
    {
        // Este método se llama cuando el enemigo recibe daño
        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount); // Aplica el daño usando la lógica de la clase base
            FlashDamage();

            // Informa al EnemyController que revise si debe cambiar su comportamiento
            EnemyController controller = GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.UpdateBehaviorStates(); // Recalcula los estados del enemigo con base en su salud actual
            }
        }

        // Este método se ejecuta cuando la salud llega a 0
        public override void Die()
        {
            base.Die(); // Ejecuta cualquier lógica de muerte definida en la clase base
            Destroy(gameObject); // Elimina al enemigo de la escena
        }
    }
}