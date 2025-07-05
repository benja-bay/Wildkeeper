// ==============================
// Player.cs
// Main Player class managing input, animation, movement, and states
// ==============================

using System.Collections.Generic;
using Items;
using PlayerController.State;
using UnityEngine;
using Weapons;

namespace PlayerController
{
    public class Player : MonoBehaviour
    {
        // === Movement Configuration ===
        [Header("Movement Settings")]
        public float moveSpeed = 3f;
        
        [Header("Run Settings")]
        public float runSpeed = 6f;
        public float runDuration = 2f;
        public float runCooldown = 3f;
        [HideInInspector] public float runCooldownTimer = 0f;

        // === State Durations Configuration ===
        [Header("State Durations")]
        [Tooltip("Duration of the melee attack animation/effect")]
        public float meleeAttackDuration = 0.4f;
        
        [Tooltip("Duration of interaction (e.g. pressing E)")]
        public float interactionDuration = 0.3f;
        
        [Tooltip("Duration of using a consumable item")]
        public float useItemDuration = 0.5f;

        // === Hitbox Configuration ===
        [Header("Hitbox")]
        public GameObject meleeHitbox;

        // === Attack Configuration ===
        [Header("Ranged")]
        [SerializeField] private GameObject _weaponObject;
        [SerializeField] private WeaponScript _weaponScript;
        [SerializeField] private WeaponAim _weaponAim;
        [SerializeField] private ItemSO dartItem;
        public ItemSO DartItem => dartItem;

        // === Internal References ===
        [HideInInspector] public bool isAttacking;
        [HideInInspector] public bool isShooting;
        [HideInInspector] public bool isInteracting;
        [HideInInspector] public PlayerInputHandler inputHandler;
        [HideInInspector] public Rigidbody2D rb2D;
        [HideInInspector] public PlayerAnimation PlayerAnimation;
        public Inventory Inventory { get; private set; }

        [Header("Unlock Items")]
        [SerializeField] private ItemSO meleeUnlockItem;
        [SerializeField] private ItemSO rangedUnlockItem;

        [Header("HUD References")]
        [SerializeField] private GameObject meleeIconHUD;
        [SerializeField] private GameObject rangedIconHUD;

        // === States ===
        [HideInInspector] public PlayerIdleState IdleState;
        [HideInInspector] public PlayerWalkState WalkState;
        [HideInInspector] public PlayerMeleAttackState MeleAttackState;
        [HideInInspector] public PlayerInteractState InteractState;
        [HideInInspector] public PlayerRangedAttackState RangedAttackState;
        [HideInInspector] public PlayerUseItemState UseItemState;
        [HideInInspector] public PlayerDeathState DeathState;
        [HideInInspector] public PlayerRunState RunState;

        private Animator _animator;
        private PlayerStateMachine _stateMachine;
        private AttackMode _lastAttackMode;

        // === Aiming Direction ===
        public Vector2 AimDirection { get; private set; } = Vector2.right;

        // === Singleton ===
        public static Player Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            // Core components
            inputHandler = GetComponent<PlayerInputHandler>();
            rb2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            PlayerAnimation = new PlayerAnimation(_animator);
            _stateMachine = new PlayerStateMachine();
            Inventory = new Inventory();

            // States
            IdleState = new PlayerIdleState(this, _stateMachine);
            WalkState = new PlayerWalkState(this, _stateMachine);
            MeleAttackState = new PlayerMeleAttackState(this, _stateMachine, meleeHitbox);
            InteractState = new PlayerInteractState(this, _stateMachine, meleeHitbox);
            RangedAttackState = new PlayerRangedAttackState(this, _stateMachine, _weaponScript, _weaponAim);
            UseItemState = new PlayerUseItemState(this, _stateMachine);
            DeathState = new PlayerDeathState(this, _stateMachine);
            RunState = new PlayerRunState(this, _stateMachine);
        }

        void Start()
        {
            _stateMachine.Initialize(IdleState);

            if (GameManager.Instance != null)
            {
                Inventory.Clear();

                var itemsToLoad = new Dictionary<ItemSO, int>(GameManager.Instance.inventory);
                foreach (var item in itemsToLoad)
                {
                    Inventory.AddItem(item.Key, item.Value, false);
                }
            }
        }

        void Update()
        {
            // === Update Run Cooldown Timer ===
            if (runCooldownTimer > 0f)
            {
                runCooldownTimer -= Time.deltaTime;
            }

            // === Update Aim Direction ===
            UpdateAimDirection();

            HandleAttackModeSwitch();
            EnsureWeaponHiddenIfUnavailable();

            CheckUnlocks();

            _stateMachine.CurrentState.HandleInput();
            _stateMachine.CurrentState.LogicUpdate();
        }

        void FixedUpdate()
        {
            _stateMachine.CurrentState.PhysicsUpdate();
        }

        public void Move(Vector2 direction)
        {
            rb2D.velocity = direction * moveSpeed;
        }

        private void UpdateAimDirection()
        {
            if (inputHandler.IsUsingMouse)
            {
                AimDirection = inputHandler.MouseDirection;
            }
            else if (inputHandler.LastMovementDirection != Vector2.zero)
            {
                AimDirection = inputHandler.LastMovementDirection;
            }
        }

        private void HandleAttackModeSwitch()
        {
            if (_lastAttackMode != inputHandler.CurrentAttackMode)
            {
                _lastAttackMode = inputHandler.CurrentAttackMode;

                bool canUseRanged = RangedAttackState.IsUnlocked && Inventory.HasAmmo(DartItem);
                _weaponObject.SetActive(_lastAttackMode == AttackMode.KRanged && canUseRanged);
            }
        }

        private void EnsureWeaponHiddenIfUnavailable()
        {
            if (inputHandler.CurrentAttackMode == AttackMode.KRanged
                && (!RangedAttackState.IsUnlocked || !Inventory.HasAmmo(DartItem)))
            {
                _weaponObject.SetActive(false);
            }
        }

        private void CheckMeleeUnlock()
        {
            if (!MeleAttackState.IsUnlocked && Inventory.GetItemCount(meleeUnlockItem) > 0)
            {
                MeleAttackState.Unlock();
                Debug.Log("Melee attack unlocked.");
                meleeIconHUD?.SetActive(true);
            }
        }

        private void CheckRangedUnlock()
        {
            if (!RangedAttackState.IsUnlocked && Inventory.GetItemCount(rangedUnlockItem) > 0)
            {
                RangedAttackState.Unlock();
                Debug.Log("Ranged attack unlocked.");
                rangedIconHUD?.SetActive(true);
            }
        }

        private void CheckUnlocks()
        {
            CheckMeleeUnlock();
            CheckRangedUnlock();
        }

        public void ChangeToDeathState()
        {
            _stateMachine.ChangeState(DeathState);
        }
    }
}
