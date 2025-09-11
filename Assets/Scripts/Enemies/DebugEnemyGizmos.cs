using UnityEngine;

namespace Enemies
{
    [ExecuteAlways]
    public class DebugEnemyGizmos : MonoBehaviour
    {
        [Header("Opcional")]
        [SerializeField] private EnemyController controller;
        [SerializeField] private EnemyVision vision;

        private void OnDrawGizmosSelected()
        {
            if (controller != null)
            {
                Vector3 position = controller.transform.position;

                // Rango de ataque
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(position, controller.AttackDistance);

                // Puntos de patrulla
                if (controller.PatrolPoints != null && controller.PatrolPoints.Length > 0)
                {
                    Gizmos.color = Color.green;
                    for (int i = 0; i < controller.PatrolPoints.Length; i++)
                    {
                        if (controller.PatrolPoints[i] != null)
                        {
                            Gizmos.DrawSphere(controller.PatrolPoints[i].position, 0.2f);

                            if (i < controller.PatrolPoints.Length - 1 && controller.PatrolPoints[i + 1] != null)
                            {
                                Gizmos.DrawLine(controller.PatrolPoints[i].position, controller.PatrolPoints[i + 1].position);
                            }
                        }
                    }
                }
            }

            if (vision != null)
            {
                if (vision.TryGetComponent(out CircleCollider2D circle))
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(vision.transform.position, circle.radius);
                }
            }
        }
    }
}
