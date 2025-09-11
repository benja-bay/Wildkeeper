// ==============================
// EnemyVision.cs
// Sistema de detección 2D que permite al enemigo identificar al jugador usando raycasts y un collider circular
// ==============================

using UnityEngine;

namespace Enemies
{
    // Asegura que este componente tenga un CircleCollider2D para la detección de visión
    [RequireComponent(typeof(CircleCollider2D))]
    public class EnemyVision : MonoBehaviour
    {
        [SerializeField] private EnemyController _enemyController; // Referencia al controlador del enemigo
        private CircleCollider2D _circleCollider2D; // Collider que define el área de visión

        private void Awake()
        {
            // Inicializa el collider al comenzar
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        // Se ejecuta mientras otro collider permanece dentro del área de visión
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            // Calcula dirección hacia el jugador
            Vector2 direction = other.transform.position - transform.position;

            // Crea una máscara para detectar tanto al jugador como al entorno
            int layerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("World"));

            // Lanza un raycast para verificar visibilidad directa
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, _circleCollider2D.radius, layerMask);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // Si el jugador es visible y aún no es el objetivo, se establece como objetivo
                if (_enemyController.Target != other.transform)
                {
                    _enemyController.SetTarget(other.transform);
                }
            }
        }

        // Se ejecuta cuando el jugador sale del área de visión
        private void OnTriggerExit2D(Collider2D other)
        {
            // Si el jugador sale y era el objetivo actual, se limpia el objetivo
            if (other.CompareTag("Player") && _enemyController.Target == other.transform)
            {
                _enemyController.SetTarget(null);
            }
        }

        // Permite modificar dinámicamente el radio de visión
        public void SetVisionRadius(float radius)
        {
            _circleCollider2D.radius = radius;
            Debug.Log($"[EnemyVision] Radio ajustado a: {_circleCollider2D.radius}");
        }
    }
}
