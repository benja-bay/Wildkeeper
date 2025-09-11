// ==============================
// FleeState.cs
// Estado de huida del enemigo: se aleja del jugador calculando una dirección opuesta
// ==============================

using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    // Este estado permite que el enemigo huya del jugador.
    // Usa el NavMeshAgent para moverse en dirección opuesta al objetivo durante la ejecución del estado.
    public class FleeState : IEnemyState
    {
        private readonly EnemyController _enemy; // Referencia al controlador principal del enemigo
        private readonly NavMeshAgent _agent;    // Agente de navegación responsable del movimiento

        public FleeState(EnemyController enemy)
        {
            _enemy = enemy;
            _agent = _enemy.GetComponent<NavMeshAgent>();
        }

        // Se llama una vez al entrar en el estado
        public void Enter()
        {
            Debug.Log($"{_enemy.name} entered Flee State.");
        }

        // Lógica que se ejecuta en cada frame mientras el enemigo huye
        public void Update()
        {
            // Si no hay objetivo, vuelve al estado inicial
            if (_enemy.Target == null)
            {
                _enemy.SetInitialState();
                return;
            }

            // Calcula la dirección opuesta al jugador y establece un punto de destino a 5 unidades
            Vector2 fleeDirection = (_enemy.transform.position - _enemy.Target.position).normalized;
            Vector3 fleePosition = _enemy.transform.position + (Vector3)fleeDirection * 5f;

            _agent.SetDestination(fleePosition);
        }

        // Se llama al salir del estado de huida
        public void Exit()
        {
            Debug.Log($"{_enemy.name} exited Flee State.");
            _agent.ResetPath(); // Detiene el movimiento actual
        }
    }
}