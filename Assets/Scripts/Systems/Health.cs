// ==============================
// Health.cs
// Base class for managing health, damage, healing, and death behavior
// ==============================

using UnityEngine;

namespace Systems
{
    public class Health : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] protected int _maxHealth = 100;
        protected int _currentHealth;
        protected bool Alive = true;

        // === Health Properties ===
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public virtual void TakeDamage(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError("Damage taken cannot be less than 0.");
                return;
            }

            if (!Alive) return;

            _currentHealth = Mathf.Max(_currentHealth - amount, 0);
            Debug.Log($"{gameObject.name} recibió {amount} de daño.");
            Debug.Log($"Salud actual: {_currentHealth}");

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            Debug.Log($"{gameObject.name} has died.");
            Alive = false;
            // TODO: Die logic.
        }

        public void SetMaxHealth(int value)
        {
            _maxHealth = value;
            _currentHealth = _maxHealth;
        }

        public virtual void Heal(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogError("Healing amount must be greater than 0.");
                return;
            }

            if (_currentHealth < _maxHealth && Alive)
            {
                _currentHealth += amount;
                Debug.Log($"{gameObject.name} regeneró {amount} de salud. Salud actual: {_currentHealth}");

                if (_currentHealth > _maxHealth)
                    _currentHealth = _maxHealth;
            }
            else
            {
                Debug.Log(Alive
                    ? $"{gameObject.name} ya tiene salud completa."
                    : $"{gameObject.name} no puede regenerar porque está muerto.");
            }
        }
    }
}
