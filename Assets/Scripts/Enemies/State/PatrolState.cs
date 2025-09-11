// ==============================
// PatrolState.cs
// Estado de patrullaje del enemigo: se mueve entre puntos predefinidos y transiciona si detecta al jugador
// ==============================

using UnityEngine;
using UnityEngine.AI;

namespace Enemies.State
{
    // Este estado permite al enemigo moverse entre una serie de puntos de patrulla.
    // Puede esperar brevemente en cada punto y cambiar a estado de visión si detecta al jugador.
    public class PatrolState : IEnemyState
    {
        private readonly EnemyController _enemy; // Referencia al controlador del enemigo
        private readonly NavMeshAgent _agent;    // Agente de navegación responsable del movimiento

        private int _currentPointIndex;          // Índice del punto de patrulla actual
        private float _waitTimer;                // Temporizador de espera entre puntos
        private bool _waiting;                   // Indica si el enemigo está esperando en un punto

        public PatrolState(EnemyController enemy)
        {
            _enemy = enemy;
            _agent = _enemy.GetComponent<NavMeshAgent>();
        }

        // Se ejecuta al entrar al estado de patrullaje
        public void Enter()
        {
            Debug.Log($"{_enemy.name} entered Patrol State.");
            _currentPointIndex = 0;
            _waitTimer = 0f;
            _waiting = false;

            // Si hay puntos de patrulla definidos, comienza el recorrido
            if (_enemy.PatrolPoints.Length > 0)
                _agent.SetDestination(_enemy.PatrolPoints[_currentPointIndex].position);
        }

        // Se ejecuta en cada frame para controlar el movimiento y transiciones
        public void Update()
        {
            // Si hay un jugador cerca, transiciona al estado de visión
            if (_enemy.Target != null)
            {
                float distance = Vector2.Distance(_enemy.transform.position, _enemy.Target.position);
                if (distance <= _enemy.VisionDistance)
                {
                    _enemy.SetVisionState();
                    return;
                }
            }

            // Si no hay puntos de patrulla definidos, no hacer nada
            if (_enemy.PatrolPoints.Length == 0) return;

            // Verifica si el enemigo llegó al punto de destino
            if (!_agent.pathPending && _agent.remainingDistance <= 0.9f)
            {
                if (!_waiting)
                {
                    _waiting = true;
                    _waitTimer = 0f;
                }

                _waitTimer += Time.deltaTime;

                // Luego de esperar, avanza al siguiente punto de patrulla
                if (_waitTimer >= _enemy.PatrolWaitTime)
                {
                    _currentPointIndex = (_currentPointIndex + 1) % _enemy.PatrolPoints.Length;
                    _agent.SetDestination(_enemy.PatrolPoints[_currentPointIndex].position);
                    _waiting = false;
                }
            }
        }

        // Se ejecuta al salir del estado de patrulla
        public void Exit()
        {
            Debug.Log($"{_enemy.name} exited Patrol State.");
            _waiting = false;
            _waitTimer = 0f;
        }
    }
}