using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemies/New Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("General Settings")]
        [Tooltip("The display name of the enemy.")]
        [SerializeField] private string _name;

        [Tooltip("The base health of the enemy.")]
        [SerializeField] private int _health;

        [Tooltip("Movement speed of the enemy.")]
        [SerializeField] private float _speed;

        [Header("Combat Settings")]
        [Tooltip("The damage this enemy deals per attack.")]
        [SerializeField] private int _damage;

        [Tooltip("Cooldown time between attacks.")]
        [SerializeField] private float _attackCooldown;

        [Tooltip("The maximum distance at which this enemy can attack.")]
        [SerializeField] private float _attackDistance;
        

        [Tooltip("Types of attacks this enemy can perform.")]
        [SerializeField] private AttackType[] _attackType; // Melee, Range

        [Header("Behavior Settings")]
        [Tooltip("Behaviors/states this enemy can transition between.")]
        [SerializeField] private BehaviorType _startState; // Patrol
        [SerializeField] private BehaviorType[] _onVisionState; // Chase, Flee, Charge
        
        [Header("Prefab")]
        [Tooltip("The prefab used to instantiate this enemy in the scene.")]
        [SerializeField] private GameObject _prefab;
        
        [Header("Vision Settings")]
        [Tooltip("Detection radius for the enemy's vision system.")]
        [SerializeField] private float _visionDistance;
        
        [Header("Patrol Settings")]
        [Tooltip("Tiempo que el enemigo espera antes de moverse al siguiente punto de patrulla.")]
        [SerializeField] private float _patrolWaitTime = 2f;
        
        [Header("Ranged Attack Settings")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileSpeed;
        
        [Header("Charge Settings")]
        [SerializeField] private float _chargeSpeed;
        [SerializeField] private float _chargeDuration;
        [SerializeField] private float _chargeCooldown;

        // Public accessors
        public string Name => _name;
        public int Health => _health;
        public float Speed => _speed;
        public int Damage => _damage;
        public float AttackCooldown => _attackCooldown;
        public float AttackDistance => _attackDistance;
        public AttackType[] AttackTypes => _attackType;
        public float VisionDistance => _visionDistance;
        public BehaviorType StartState => _startState;
        public BehaviorType[] OnVisionState => _onVisionState;
        public float PatrolWaitTime => _patrolWaitTime;
        public GameObject Prefab => _prefab;
        public GameObject ProjectilePrefab => _projectilePrefab;
        public float ProjectileSpeed => _projectileSpeed;
        public float ChargeSpeed => _chargeSpeed;
        public float ChargeDuration => _chargeDuration;
        public float ChargeCooldown => _chargeCooldown;
    }
}
