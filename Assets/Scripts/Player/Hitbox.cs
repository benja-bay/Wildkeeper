// ==============================
// Hitbox.cs
// Manages position, rotation, and collision detection of the melee hitbox
// ==============================

using Enemies;
using Objects;
using UnityEngine;

namespace PlayerController
{
    public enum HitboxMode
    {
        KAttack,   // Melee attack mode
        KInteract, // Interaction mode
    }

    public class Hitbox : MonoBehaviour
    {
        // === References ===
        private Transform _playerTransform; // Reference to the player's transform
        private Player _player;             // Reference to the Player script

        // === Configuration ===
        [SerializeField] private float _distance = 1f; // Distance from the player to place the hitbox
        public int damage = 1;                         // Damage dealt by the hitbox
        [SerializeField] private Transform _pivot;

        private HitboxMode _mode = HitboxMode.KAttack; // Current operating mode of the hitbox

        // === Initialization ===
        public void Initialize(Player playerRef, PlayerInputHandler inputHandler, Transform playerTransform)
        {
            // Initialize references from the player
            _player = playerRef;
            _playerTransform = playerTransform;
        }

        public void SetMode(HitboxMode mode)
        {
            // Change the hitbox behavior based on current interaction type
            _mode = mode;
        }

        // === Updates hitbox position and rotation to match the player's aim direction ===
        public void UpdatePositionAndRotation()
        {
            if (_player == null || _playerTransform == null)
            {
                Debug.LogError("MeleeAttackHitbox: References not initialized. Call Initialize() before using.");
                return;
            }

            // Use the player's current aim direction
            Vector2 direction = _player.AimDirection;

            // Position the hitbox at the correct distance in that direction
            transform.position = _pivot.position + (Vector3)(direction.normalized * _distance);

            // Rotate hitbox to face the aim direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Flip the hitbox vertically if facing left
            Vector3 localScale = Vector3.one;
            localScale.y = (angle > 90 || angle < -90) ? -1f : 1f;
            transform.localScale = localScale;
        }

        // === Collision Handling ===
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_player == null) return;

            switch (_mode)
            {
                case HitboxMode.KAttack:
                    // Handle melee attack collision
                    if (!_player.isAttacking) return;

                    if (other.CompareTag("Enemy"))
                    {
                        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeDamage(damage);
                            Debug.Log("Damage was caused to the enemy");
                        }
                    }
                    break;

                case HitboxMode.KInteract:
                    // Handle interaction with objects
                    if (!_player.isInteracting) return;

                    if (other.CompareTag("Interactable"))
                    {
                        var interactable = other.GetComponent<IInteractable>();
                        if (interactable != null)
                        {
                            interactable.Interact(_player);
                            Debug.Log("Interaction triggered");
                        }
                    }
                    break;
            }
        }
    }
}