// ==============================
// DeadState.cs
// Estado final del enemigo tras morir: detiene la IA, reproduce animación de muerte y destruye el objeto
// ==============================

using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    // Este estado se activa cuando el enemigo muere.
    // Detiene su movimiento, desactiva su comportamiento y agenda su destrucción tras una animación.
    public class DeadState : IEnemyState
    {
        private readonly EnemyController _enemy;     // Referencia al controlador del enemigo
        private readonly NavMeshAgent _agent;        // Componente de navegación que debe detenerse al morir
        private bool _destroyScheduled;              // Previene múltiples intentos de destrucción

        public DeadState(EnemyController enemy)
        {
            _enemy = enemy;
            _agent = _enemy.GetComponent<NavMeshAgent>();
        }

        // Se ejecuta una sola vez al entrar al estado de muerte
        public void Enter()
        {
            Debug.Log($"{_enemy.name} has died.");

            // Detiene el movimiento si tiene un NavMeshAgent activo
            if (_agent != null)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }

            // Desactiva el hitbox del enemigo para evitar colisiones o daño
            _enemy.ActivateHitbox(false);

            // Opcional: desactiva el controlador para evitar ejecuciones en Update si no se usa un FSM explícito
            _enemy.enabled = false;

            // Activa la animación de muerte, si hay un Animator
            Animator anim = _enemy.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Die"); // Trigger esperado en el Animator
            }

            // Programa la destrucción del objeto tras una corta demora
            if (!_destroyScheduled)
            {
                Object.Destroy(_enemy.gameObject, 3f);
                _destroyScheduled = true;
            }
        }

        // En este estado no se realiza ninguna lógica por frame
        public void Update()
        {
            // Nada que hacer una vez muerto
        }

        // Aunque nunca se debe salir del estado de muerte, este método existe para cumplir el contrato de IEnemyState
        public void Exit()
        {
            // Intencionalmente vacío
        }
    }
}
