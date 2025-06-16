// ==============================
// PlayerHealth.cs
// Extends Health.cs to provide visual feedback for damage and healing, and updates HUD
// ==============================

using Systems;
using UnityEngine;
using System.Collections;

namespace Player
{
    
    public class PlayerHealth : Health
    {
        // === Visual Feedback Configuration ===
        [Header("Flash settings")]
        [SerializeField] private SpriteRenderer spriteRenderer; // Reference to player's sprite
        [SerializeField] private Color damageColor = Color.red; // Color when taking damage
        [SerializeField] private float flashDuration = 0.1f; // Duration of damage flash

        [SerializeField] private Color healColor = Color.green;   // Color when healing
        [SerializeField] private float healDuration = 0.3f;       // Duration of heal flash

        private Color _originalColor; // Original sprite color (restored after flash)

        // Called when player is healed through an item or effect
        public void Regenerate(int amount)
        {
            Heal(amount);
            StartCoroutine(FlashGreen());

            // Guardar salud actual
            if (GameManager.Instance != null)
                GameManager.Instance.currentHealth = _currentHealth;
        }

        // Called when player takes damage (includes visual feedback)
        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            StartCoroutine(FlashRed());

            // Guardar salud actual
            if (GameManager.Instance != null)
                GameManager.Instance.currentHealth = _currentHealth;
        }

        // Coroutine to flash red briefly when damaged
        private System.Collections.IEnumerator FlashRed()
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = _originalColor;
        }

        // Coroutine to flash green briefly when healed
        private System.Collections.IEnumerator FlashGreen()
        {
            spriteRenderer.color = healColor;
            yield return new WaitForSeconds(healDuration);
            spriteRenderer.color = _originalColor;
        }

        // Called when player health reaches 0
        public override void Die()
        {
            base.Die();
            Debug.Log("¡El jugador ha muerto!");

            // Desactivar input y movimiento
            if (TryGetComponent(out PlayerInputHandler input)) input.enabled = false;
            if (TryGetComponent(out Rigidbody2D rb)) rb.velocity = Vector2.zero;

            // Animación de muerte
            Animator anim = GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Die");

            // (Opcional) Desactivar la lógica de vida para evitar efectos extra
            enabled = false;

            // Reiniciar escena después de un delay
            StartCoroutine(RestartSceneAfterDelay(2f));
        }

        private IEnumerator RestartSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            );
        }

        // Setup initial color reference
        private void Start()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            _originalColor = spriteRenderer.color;
            // Cargar vida desde GameManager
            if (GameManager.Instance != null)
            {
                _maxHealth = GameManager.Instance.maxHealth;
                _currentHealth = GameManager.Instance.currentHealth;
            }
        }
    }
}