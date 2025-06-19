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
        protected override void Start()
        {
            base.Start();

            if (GameManager.Instance != null)
            {
                _maxHealth = GameManager.Instance.maxHealth;
                _currentHealth = GameManager.Instance.maxHealth; // ← siempre inicia con vida completa
            }
        }

        public void Regenerate(int amount)
        {
            Heal(amount);
            FlashHeal();

            if (GameManager.Instance != null)
                GameManager.Instance.currentHealth = _currentHealth;
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            FlashDamage();

            if (GameManager.Instance != null)
                GameManager.Instance.currentHealth = _currentHealth;
        }

        public override void Die()
        {
            base.Die();
            Debug.Log("¡El jugador ha muerto!");

            if (TryGetComponent(out PlayerInputHandler input)) input.enabled = false;
            if (TryGetComponent(out Rigidbody2D rb)) rb.velocity = Vector2.zero;
            
            if (TryGetComponent(out Player player))
            {
                player.Move(Vector2.zero);
                player.ChangeToIdleState();
            }

            Animator anim = GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Die");

            enabled = false;

            StartCoroutine(RestartSceneAfterDelay(2f));
        }

        private IEnumerator RestartSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}