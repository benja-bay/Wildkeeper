using Enemies.State;

namespace Enemies.Factories
{
    public class EnemyStateFactory
    {
        private readonly EnemyController _controller;

        public EnemyStateFactory(EnemyController controller)
        {
            _controller = controller;
        }

        public IEnemyState GetState(BehaviorType type)
        {
            return type switch
            {
                BehaviorType.Patrol => new PatrolState(_controller),
                BehaviorType.Chase => new ChaseState(_controller),
                BehaviorType.Flee => new FleeState(_controller),
                BehaviorType.Dead => new DeadState(_controller),
                BehaviorType.Charge => new ChargeState(_controller),
                
                _ => null
            };
        }
    }
}