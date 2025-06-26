// ==============================
// EnemyFactory.cs
// Fábrica responsable de instanciar enemigos y configurarlos con sus datos y puntos de patrulla
// ==============================

using UnityEngine;

namespace Enemies.Factories
{
    public class EnemyFactory : IEnemyFactory
    {
        // Crea e instancia un enemigo en una posición dada con una configuración específica y puntos de patrulla
        public GameObject CreateEnemy(EnemyConfig config, Vector3 spawnPosition, Transform[] patrolPoints)
        {
            // Instancia el prefab del enemigo
            GameObject enemyInstance = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity);

            // Obtiene el componente EnemyController del prefab
            EnemyController controller = enemyInstance.GetComponent<EnemyController>();
            if (controller == null)
            {
                Debug.LogError("El prefab del enemigo no contiene un componente EnemyController.");
                return null;
            }

            // Configura la visión del enemigo si tiene un componente EnemyVision
            EnemyVision vision = enemyInstance.GetComponentInChildren<EnemyVision>();
            if (vision != null)
            {
                float radius = Mathf.Max(config.AttackDistance, config.VisionDistance);
                vision.SetVisionRadius(radius);
            }

            // Asigna los puntos de patrulla y configura al enemigo
            controller.PatrolPoints = patrolPoints;
            controller.Initialize(config);

            return enemyInstance;
        }
    }
}
