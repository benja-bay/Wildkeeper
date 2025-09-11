using UnityEngine;

namespace Enemies
{
    public interface IEnemyFactory
    {
        GameObject CreateEnemy(EnemyConfig config, Vector3 spawnPosition, Transform[] patrolPoints);
    }
}