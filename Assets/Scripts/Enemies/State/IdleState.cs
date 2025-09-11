// ==============================
// IdleState.cs
// Estado pasivo del enemigo: espera hasta que detecta al jugador en su rango de visión
// ==============================

using UnityEngine;

namespace Enemies.State
{
    // Este estado representa el comportamiento "en reposo" del enemigo.
    // No realiza ninguna acción hasta que el jugador entra en su rango de visión, momento en el que transiciona al estado de visión.
    public class IdleState : IEnemyState
    {
        private readonly EnemyController _enemy; // Referencia al controlador del enemigo

        public IdleState(EnemyController enemy)
        {
            _enemy = enemy;
        }

        // Se ejecuta una vez al entrar al estado
        public void Enter()
        {
            Debug.Log($"{_enemy.name} entered Idle State.");
        }

        // Se ejecuta en cada frame mientras el enemigo está en reposo
        public void Update()
        {
            if (_enemy.Target == null) return;

            // Calcula la distancia al objetivo
            float distance = Vector2.Distance(_enemy.transform.position, _enemy.Target.position);

            // Si el jugador entra en el rango de visión, cambia al estado de visión
            if (distance <= _enemy.VisionDistance)
                _enemy.SetVisionState();
        }

        // Se llama una vez al salir del estado de reposo
        public void Exit()
        {
            Debug.Log($"{_enemy.name} exited Idle State.");
        }
    }
}