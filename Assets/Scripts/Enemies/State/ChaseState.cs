// ==============================
// ChaseState.cs
// Estado de persecución del enemigo: sigue al jugador hasta alcanzar su distancia de ataque
// ==============================

using UnityEngine;
using UnityEngine.AI;

namespace Enemies.State
{
    // Este estado representa la fase en la que el enemigo persigue activamente al jugador usando navegación.
    // Se ejecuta mientras el jugador esté dentro de un rango de detección, pero fuera del alcance de ataque.
    public class ChaseState : IEnemyState
    {
        private readonly EnemyController _enemy; // Referencia al controlador principal del enemigo
        private readonly NavMeshAgent _agent;    // Agente de navegación para seguir al jugador

        public ChaseState(EnemyController enemy)
        {
            _enemy = enemy;
            _agent = _enemy.GetComponent<NavMeshAgent>();
        }

        // Se ejecuta una vez al entrar al estado
        public void Enter()
        {
            Debug.Log($"{_enemy.name} started chasing.");
        }

        // Lógica que se ejecuta cada frame mientras el enemigo esté en este estado
        public void Update()
        {
            // Si el jugador desaparece o muere, vuelve al estado inicial
            if (_enemy.Target == null)
            {
                _enemy.SetInitialState();
                return;
            }

            // Persigue al jugador actualizando continuamente su destino
            _agent.SetDestination(_enemy.Target.position);
        }

        // Se ejecuta al salir del estado de persecución
        public void Exit()
        {
            Debug.Log($"{_enemy.name} stopped chasing.");
            _agent.ResetPath(); // Detiene cualquier movimiento activo al salir del estado
        }
    }
}