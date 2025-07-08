// ==============================
// PlayerHealth.cs
// Extends Health.cs to provide visual feedback for damage and healing, and updates HUD
// ==============================

using Systems;
using UnityEngine;
using System.Collections;
using Managers;

namespace PlayerController
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

            if (TryGetComponent(out Player player))
            {
                if (GameManager.Instance.HasCheckpoint())
                {
                    GameManager.Instance.RestoreCheckpoint(player);
                    player.ChangeToDeathState(); // Optional: puede cambiar a Idle directamente
                }
                else
                {
                    player.ChangeToDeathState();
                    StartCoroutine(RestartSceneAfterDelay(2f));
                }
            }

            enabled = false;
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