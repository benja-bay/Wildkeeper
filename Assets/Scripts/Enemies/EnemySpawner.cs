// ==============================
// EnemySpawner.cs
// Encargado de generar enemigos en una posición específica con una configuración y patrullaje definidos
// ==============================

using Enemies.Factories;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyConfig enemyConfig; // Configuración del enemigo a instanciar
        [SerializeField] private Transform[] patrolPoints; // Puntos de patrulla que se asignarán al enemigo
        [SerializeField] private Vector3 spawnOffset = Vector3.zero; // Desplazamiento desde la posición del spawner

        private IEnemyFactory _factory; // Referencia a la fábrica de enemigos

        private void Awake()
        {
            // Crea una instancia de la fábrica (puede ser reemplazada por una inyección de dependencias si se requiere)
            _factory = new EnemyFactory();
        }

        private void Start()
        {
            // Genera al enemigo al iniciar la escena
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            // Calcula la posición final de aparición y llama a la fábrica para crear el enemigo
            Vector3 spawnPosition = transform.position + spawnOffset;
            _factory.CreateEnemy(enemyConfig, spawnPosition, patrolPoints);
        }
    }
}
