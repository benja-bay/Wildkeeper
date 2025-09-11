// ==============================
// ChargeState.cs
// Estado de carga del enemigo: acelera en línea recta hacia el jugador durante un tiempo limitado
// ==============================

using UnityEngine;
using UnityEngine.AI;

namespace Enemies.State
{
    // Esta clase implementa un estado del enemigo en el que se lanza (charge) hacia el jugador.
    // Usa el NavMeshAgent para moverse rápidamente en línea recta durante una duración fija, y tiene un tiempo de recarga.
    public class ChargeState : IEnemyState
    {
        private readonly EnemyController _enemy; // Referencia al controlador general del enemigo
        private readonly NavMeshAgent _agent;    // Componente de navegación usado para mover al enemigo

        private float _chargeTimer;    // Temporizador que controla cuánto ha durado la carga actual
        private float _cooldownTimer;  // Temporizador que controla el tiempo entre cargas
        private bool _isCharging;      // Indica si el enemigo está actualmente en estado de carga
        private Vector3 _chargeDirection; // Dirección en la que se realiza la carga

        public ChargeState(EnemyController enemy)
        {
            _enemy = enemy;
            _agent = _enemy.GetComponent<NavMeshAgent>();
        }

        // Se llama al entrar en este estado
        public void Enter()
        {
            Debug.Log($"{_enemy.name} entered Charge State.");
            _cooldownTimer = 0f;
            StartCharge(); // Inicia la carga inmediatamente al entrar al estado
        }

        // Lógica que se ejecuta cada frame mientras el enemigo esté en este estado
        public void Update()
        {
            // Si no hay objetivo válido, vuelve al estado inicial
            if (_enemy.Target == null)
            {
                _enemy.SetInitialState();
                return;
            }

            if (_isCharging)
            {
                _chargeTimer += Time.deltaTime;

                // Si la duración de la carga se ha cumplido, se detiene
                if (_chargeTimer >= _enemy.ChargeDuration)
                {
                    StopCharge();
                }
            }
            else
            {
                // Cuenta el tiempo de enfriamiento antes de permitir otra carga
                _cooldownTimer += Time.deltaTime;
                if (_cooldownTimer >= _enemy.ChargeCooldown)
                {
                    StartCharge(); // Inicia una nueva carga
                }
            }
        }

        // Se llama al salir de este estado
        public void Exit()
        {
            Debug.Log($"{_enemy.name} exited Charge State.");
            StopCharge(); // Asegura que cualquier carga activa se detenga
        }

        // Inicia la carga del enemigo hacia el jugador
        private void StartCharge()
        {
            _isCharging = true;
            _chargeTimer = 0f;

            // Calcula la dirección hacia el jugador y establece la velocidad de carga
            _chargeDirection = (_enemy.Target.position - _enemy.transform.position).normalized;
            _agent.speed = _enemy.ChargeSpeed;

            // Define un destino adelante en la dirección del jugador para simular una carga recta
            _agent.SetDestination(_enemy.transform.position + _chargeDirection * 10f);
        }

        // Detiene la carga y reinicia el cooldown
        private void StopCharge()
        {
            _isCharging = false;
            _cooldownTimer = 0f;
            _agent.ResetPath(); // Detiene el movimiento actual
        }
    }
}