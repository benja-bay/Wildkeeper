// ==============================
// EnemyMeleeHitbox.cs
// Sistema de ataque melee del enemigo. Gestiona el daño al jugador mediante una zona de colisión (hitbox).
// ==============================

using UnityEngine;

namespace Enemies
{
    // Esta clase representa el sistema de ataque cuerpo a cuerpo del enemigo.
    // Controla la posición del hitbox, detecta al jugador y aplica daño con un cooldown.
    public class EnemyMeleeHitbox : MonoBehaviour
    {
        // Cantidad de daño que inflige el ataque
        public int DamageAmount { get; set; }

        // Tiempo mínimo entre ataques consecutivos
        public float DamageCooldown { get; set; }

        // Distancia frente al enemigo donde se posiciona el hitbox
        public float AttackDistance { get; set; }

        // Tiempo en el que se aplicó el último daño
        private float _lastDamageTime = float.MinValue;

        // Referencias al jugador y su salud
        private Transform _playerTransform;
        private PlayerController.PlayerHealth _playerHealth;

        // Indica si el jugador está dentro del rango de ataque
        private bool _isPlayerInRange;

        private void Update()
        {
            // Inicializa referencias al jugador si no se han establecido aún
            if (_playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    _playerTransform = player.transform;
                    _playerHealth = player.GetComponent<PlayerController.PlayerHealth>();
                }
            }

            // Reposiciona el hitbox para que esté frente al enemigo apuntando al jugador
            if (_playerTransform != null)
            {
                Vector3 direction = (_playerTransform.position - transform.parent.position).normalized;
                transform.position = transform.parent.position + direction * AttackDistance;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            // Si el jugador está en rango y el cooldown se cumplió, se aplica daño
            if (_isPlayerInRange && Time.time - _lastDamageTime >= DamageCooldown)
            {
                ApplyDamage();
            }
        }

        // Detecta cuando el jugador entra en la zona de daño (hitbox)
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInRange = true;

                // Asegura tener una referencia válida a la salud del jugador
                if (_playerHealth == null)
                    _playerHealth = other.GetComponent<PlayerController.PlayerHealth>();

                // Aplica daño si el cooldown se ha cumplido
                if (Time.time - _lastDamageTime >= DamageCooldown)
                {
                    ApplyDamage();
                }
            }
        }

        // Detecta cuando el jugador sale de la zona de daño
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInRange = false;
            }
        }

        // Método encargado de aplicar daño al jugador
        private void ApplyDamage()
        {
            if (_playerHealth != null)
            {
                _playerHealth.TakeDamage(DamageAmount);
                _lastDamageTime = Time.time; // Reinicia el tiempo de cooldown
            }
        }
    }
}
