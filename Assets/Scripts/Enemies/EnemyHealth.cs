// ==============================
// EnemyHealth.cs
// Clase que gestiona la salud del enemigo y actualiza su comportamiento al recibir daño
// ==============================

using Managers;
using Systems;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Esta clase gestiona la salud del enemigo y extiende la funcionalidad de una clase base llamada Health.
    /// </summary>
    public class EnemyHealth : Health
    {
        private EnemyController _controller;
        
        private void Awake()
        {
            _controller = GetComponent<EnemyController>();
        }
        
        /// <summary>
        /// Este método se llama cuando el enemigo recibe daño.
        /// </summary>
        /// <param name="amount">Cantidad de daño que recibe.</param>
        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount); // Aplica el daño usando la lógica de la clase base
            FlashDamage();
        }

        /// <summary>
        /// Este método se ejecuta cuando la salud llega a 0.
        /// </summary>
        public override void Die()
        {
            base.Die();

            if (_controller != null && _controller.isBoss)
            {
                GameManager.Instance?.NotifyBossDeath();
            }

            Destroy(gameObject);
        }
    }
}