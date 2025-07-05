// ==============================
// PlayerMeleAttackState.cs
// Handles player behavior during a melee attack
// ==============================

using UnityEngine;

namespace PlayerController.State
{
    public class PlayerMeleAttackState : PlayerState
    {
        // === Attack Configuration ===
        private GameObject _meleeHitbox;              // GameObject used for melee collision
        private Hitbox _attackHitbox;                 // Script component managing hitbox behavior
        private float _attackDuration;                // Total time the attack lasts
        private float _attackTimer;                   // Internal timer for attack duration
        private bool _unlocked = false;               // Whether the melee attack is unlocked

        public PlayerMeleAttackState(Player player, PlayerStateMachine stateMachine, GameObject meleeHitbox) 
            : base(player, stateMachine)
        {
            _meleeHitbox = meleeHitbox;
            _attackHitbox = meleeHitbox.GetComponent<Hitbox>();
        }

        // Called when entering the melee attack state
        public override void Enter()
        {
            base.Enter();

            if (!_unlocked)
            {
                Debug.Log("Melee attack not unlocked.");
                StateMachine.ChangeState(Player.IdleState);
                return;
            }

            Player.isAttacking = true;

            // Use player's configured attack duration
            _attackDuration = Player.meleeAttackDuration;
            _attackTimer = _attackDuration;

            // Setup and activate the attack hitbox
            _attackHitbox.Initialize(Player, Player.GetComponent<PlayerInputHandler>(), Player.transform);
            _attackHitbox.SetMode(HitboxMode.KAttack);
            _attackHitbox.UpdatePositionAndRotation();

            Debug.Log("Melee hitbox activated");

            Player.Move(Vector2.zero); // Stop player movement during attack
            _meleeHitbox.SetActive(true);

            // Use the player's current aim direction for animation
            Player.PlayerAnimation.PlayMeleeAttack(Player.AimDirection);
        }

        // Called every frame while this state is active
        public override void HandleInput()
        {
            base.HandleInput();
            _attackTimer -= Time.deltaTime;

            // Continuously update hitbox orientation
            _attackHitbox.UpdatePositionAndRotation();

            // Allow cancelling with Use Item input
            if (Player.inputHandler.useItemPressed)
            {
                StateMachine.ChangeState(Player.UseItemState);
                return;
            }

            // Allow cancelling with Interact input
            if (Player.inputHandler.interactPressed)
            {
                StateMachine.ChangeState(Player.InteractState);
                return;
            }

            // End attack when timer finishes and transition to next state
            if (_attackTimer <= 0f)
            {
                if (Player.inputHandler.movementInput != Vector2.zero)
                {
                    StateMachine.ChangeState(Player.WalkState);
                }
                else
                {
                    StateMachine.ChangeState(Player.IdleState);
                }
            }
        }

        // Called when exiting the melee attack state
        public override void Exit()
        {
            base.Exit();

            Player.isAttacking = false;
            Player.PlayerAnimation.StopMeleeAttack();
            _meleeHitbox.SetActive(false);
        }

        // Unlocks the melee attack ability
        public void Unlock()
        {
            _unlocked = true;
        }

        // Property to check if the attack is unlocked
        public bool IsUnlocked => _unlocked;
    }
}