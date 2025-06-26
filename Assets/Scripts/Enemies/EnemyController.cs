// ==============================
// EnemyController.cs
// Controlador principal del enemigo que gestiona estados, movimiento, ataque y percepción
// ==============================

using Enemies.Factories;
using Systems;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyController : MonoBehaviour
    {
        // Estado actual del enemigo
        private IEnemyState _currentState;
        private NavMeshAgent _agent; // Referencia al componente de navegación
        private EnemyConfig _config; // Configuración del enemigo cargada desde datos externos

        [Header("General Settings")]
        public Transform[] PatrolPoints; // Puntos entre los que el enemigo patrulla
        public float PatrolWaitTime { get; private set; } // Tiempo que espera en cada punto
        public Transform Target; // Objetivo actual (por ejemplo, el jugador)

        [Header("Enemy Stats")]
        [HideInInspector] public int DamageAmount; // Daño infligido
        [HideInInspector] public float DamageCooldown; // Tiempo entre ataques
        [HideInInspector] public float AttackDistance; // Distancia para poder atacar
        [HideInInspector] public float VisionDistance; // Distancia de visión para detectar al jugador
        public AttackType PrimaryAttackType { get; private set; } // Tipo de ataque principal

        [Header("Ranged Settings")]
        public GameObject ProjectilePrefab; // Prefab del proyectil
        public float ProjectileSpeed; // Velocidad del proyectil

        [Header("Components")]
        public EnemyMeleeHitbox MeleeHitbox; // Componente para ataques cuerpo a cuerpo

        // Estados que puede adoptar el enemigo
        private IEnemyState StartState;
        private IEnemyState VisionState;
        private IEnemyState AtackState;
        private EnemyStateFactory _stateFactory; // Fábrica para crear estados
        
        [Header("Boss Settings")]
        public bool isBoss = false;

        // Variables de ataque tipo "charge"
        public float ChargeSpeed { get; private set; }
        public float ChargeDuration { get; private set; }
        public float ChargeCooldown { get; private set; }

        private void Awake()
        {
            // Configura el agente de navegación
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.autoBraking = false;
        }

        private void Start()
        {
            // Configura el hitbox de ataque si existe
            if (MeleeHitbox != null)
            {
                MeleeHitbox.DamageAmount = DamageAmount;
                MeleeHitbox.DamageCooldown = DamageCooldown;
                MeleeHitbox.AttackDistance = AttackDistance;
                MeleeHitbox.gameObject.SetActive(false);
            }

            // Establece el estado inicial del enemigo
            SetInitialState();
        }

        private void Update()
        {
            // Llama al método de actualización del estado actual
            _currentState?.Update();
        }

        // Inicializa al enemigo con los valores definidos en el config
        public void Initialize(EnemyConfig config)
        {
            _config = config;

            // Crea la fábrica de estados y actualiza los estados de comportamiento
            _stateFactory = new EnemyStateFactory(this);
            UpdateBehaviorStates();

            // Asigna valores desde la configuración
            DamageAmount = config.Damage;
            DamageCooldown = config.AttackCooldown;
            AttackDistance = config.AttackDistance;
            VisionDistance = config.VisionDistance;
            PatrolWaitTime = config.PatrolWaitTime;
            ProjectilePrefab = config.ProjectilePrefab;
            ProjectileSpeed = config.ProjectileSpeed;
            ChargeSpeed = config.ChargeSpeed;
            ChargeDuration = config.ChargeDuration;
            ChargeCooldown = config.ChargeCooldown;

            //Configura la velocidad del agente si está presente
            if (_agent != null)
                _agent.speed = config.Speed;

            // Establece la salud del enemigo
            var health = GetComponent<Health>();
            if (health != null)
                health.SetMaxHealth(config.Health);

            // Asigna el tipo de ataque principal
            PrimaryAttackType = (config.AttackTypes != null && config.AttackTypes.Length > 0)
                ? config.AttackTypes[0]
                : AttackType.Melee;
        }

        // Cambia al estado inicial
        public void SetInitialState() => TransitionToState(StartState);

        // Cambia al estado cuando ve a su objetivo
        public void SetVisionState() => TransitionToState(VisionState);

        // Asigna un nuevo objetivo
        public void SetTarget(Transform target) => Target = target;

        // Cambia de estado
        public void TransitionToState(IEnemyState newState)
        {
            _currentState?.Exit(); // Sale del estado actual
            _currentState = newState; // Asigna el nuevo
            _currentState?.Enter(); // Entra al nuevo estado
        }

        // Activa o desactiva el hitbox de ataque cuerpo a cuerpo
        public void ActivateHitbox(bool active)
        {
            if (MeleeHitbox != null)
                MeleeHitbox.gameObject.SetActive(active);
        }

        // Actualiza los estados de comportamiento dependiendo de la salud
        public void UpdateBehaviorStates()
        {
            int index = 0;

            var healthComponent = GetComponent<EnemyHealth>();
            if (healthComponent != null)
            {
                float percent = (float)healthComponent.CurrentHealth / healthComponent.MaxHealth * 100f;
                if (percent <= _config.BehaviorSwitchHealthPercent)
                {
                    index = 1; // Cambia el índice si la salud es baja
                }
            }

            // Asigna el estado inicial y de visión basado en el índice
            StartState = _stateFactory.GetState(
                _config.StartState.Length > index ? _config.StartState[index] : BehaviorType.Idle);

            VisionState = _stateFactory.GetState(
                _config.OnVisionState.Length > index ? _config.OnVisionState[index] : BehaviorType.Chase);
        }
    }
}