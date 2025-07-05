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
        [Header("Movement")]
        public float moveSpeed = 3f; // Player movement speed

        // === Hitbox Configuration ===
        [Header("Hitbox")]
        public GameObject meleeHitbox; // Reference to melee hitbox

        // === Attack Configuration ===
        [Header("Ranged")]
        [SerializeField] private GameObject _weaponObject; // GameObject for ranged weapon visuals
        [SerializeField] private WeaponScript _weaponScript; // Controls ranged attack logic
        [SerializeField] private WeaponAim _weaponAim; // Controls aiming direction
        [SerializeField] private ItemSO dartItem; // Item used as ammo for ranged attacks
        public ItemSO DartItem => dartItem; // Public getter for dart ammo

        // === Internal Component References ===
        [HideInInspector] public bool isAttacking;
        [HideInInspector] public bool isShooting;
        [HideInInspector] public bool isInteracting;
        [HideInInspector] public PlayerInputHandler inputHandler; // Handles player inputs
        [HideInInspector] public Rigidbody2D rb2D; // Rigidbody for physics movement
        [HideInInspector] public PlayerAnimation PlayerAnimation; // Manages animations
        public Inventory Inventory { get; private set; } // Inventory system

        [Header("Unlock Items")]
        [SerializeField] private ItemSO meleeUnlockItem; // Item that unlocks melee attack
        [SerializeField] private ItemSO rangedUnlockItem; // Item that unlocks ranged attack

        [Header("HUD References")]
        [SerializeField] private GameObject meleeIconHUD;
        [SerializeField] private GameObject rangedIconHUD;

        // === State Instances ===
        [HideInInspector] public PlayerIdleState IdleState;
        [HideInInspector] public PlayerWalkState WalkState;
        [HideInInspector] public PlayerMeleAttackState MeleAttackState;
        [HideInInspector] public PlayerInteractState InteractState;
        [HideInInspector] public PlayerRangedAttackState RangedAttackState;
        [HideInInspector] public PlayerUseItemState UseItemState;
        [HideInInspector] public PlayerDeathState DeathState;

        // === Private Components ===
        private Animator _animator;
        private PlayerStateMachine _stateMachine; // Manages player state transitions
        private AttackMode _lastAttackMode;

        // === Aiming Direction ===
        public Vector2 AimDirection { get; private set; } = Vector2.right; // Default aim direction

        // SINGLETON (WIP)
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

            // Initialize core components
            inputHandler = GetComponent<PlayerInputHandler>();
            rb2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            PlayerAnimation = new PlayerAnimation(_animator);
            _stateMachine = new PlayerStateMachine();
            Inventory = new Inventory();

            // Initialize all player states with references
            IdleState = new PlayerIdleState(this, _stateMachine);
            WalkState = new PlayerWalkState(this, _stateMachine);
            MeleAttackState = new PlayerMeleAttackState(this, _stateMachine, meleeHitbox);
            InteractState = new PlayerInteractState(this, _stateMachine, meleeHitbox);
            RangedAttackState = new PlayerRangedAttackState(this, _stateMachine, _weaponScript, _weaponAim);
            UseItemState = new PlayerUseItemState(this, _stateMachine);
            DeathState = new PlayerDeathState(this, _stateMachine);
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
            // === Update aiming direction based on input mode ===
            UpdateAimDirection();

            HandleAttackModeSwitch();
            EnsureWeaponHiddenIfUnavailable();

            CheckUnlocks();

            _stateMachine.CurrentState.HandleInput();
            _stateMachine.CurrentState.LogicUpdate();
        }

        void FixedUpdate()
        {
            // Apply physics-based logic for current state
            _stateMachine.CurrentState.PhysicsUpdate();
        }

        public void Move(Vector2 direction)
        {
            // Move the player using Rigidbody2D
            rb2D.velocity = direction * moveSpeed;
        }

        // === New Aiming Direction Logic with Input Mode Awareness ===
        private void UpdateAimDirection()
        {
            if (inputHandler.IsUsingMouse)
            {
                // Mouse aiming: always follow mouse position
                AimDirection = inputHandler.MouseDirection;
            }
            else
            {
                // Gamepad/keyboard aiming: use last movement direction
                if (inputHandler.LastMovementDirection != Vector2.zero)
                {
                    AimDirection = inputHandler.LastMovementDirection;
                }
                // Else, keep existing AimDirection (idle)
            }
        }

        // === Refactored methods ===

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
                if (meleeIconHUD != null)
                {
                    meleeIconHUD.SetActive(true);
                }
            }
        }

        private void CheckRangedUnlock()
        {
            if (!RangedAttackState.IsUnlocked && Inventory.GetItemCount(rangedUnlockItem) > 0)
            {
                RangedAttackState.Unlock();
                Debug.Log("Ranged attack unlocked.");
                if (rangedIconHUD != null)
                {
                    rangedIconHUD.SetActive(true);
                }
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
