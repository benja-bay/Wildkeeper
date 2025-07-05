// ==============================
// PlayerInteractState.cs
// Handles interaction state using a hitbox, often reusing the melee hitbox
// ==============================

using UnityEngine;

namespace Player.State
{
    public class PlayerInteractState : PlayerState
    {
        // === Interaction Configuration ===
        private GameObject _meleeHitbox; // Reference to the melee hitbox reused for interaction
        private Hitbox _interactionHitbox; // Component handling hitbox logic
        private float _interactionDuration = 0.3f; // Total duration of the interaction
        private float _interactionTimer; // Timer to track interaction progress

        public PlayerInteractState(Player player, PlayerStateMachine stateMachine, GameObject meleeHitbox)
            : base(player, stateMachine)
        {
            _meleeHitbox = meleeHitbox;
            _interactionHitbox = meleeHitbox.GetComponent<Hitbox>();
        }

        // Called when the state is entered
        public override void Enter()
        {
            base.Enter();
            Player.isInteracting = true;

            // Stop movement and play idle animation
            Player.Move(Vector2.zero);
            Player.PlayerAnimation.PlayIdle();

            // Setup and activate interaction hitbox
            _interactionHitbox.Initialize(Player, Player.GetComponent<PlayerInputHandler>(), Player.transform);
            _interactionHitbox.SetMode(HitboxMode.KInteract);
            _interactionHitbox.UpdatePositionAndRotation();

            Debug.Log("Interaction hitbox activated");
            
            _meleeHitbox.SetActive(true); // Activate hitbox GameObject

            _interactionTimer = _interactionDuration;
        }

        // Called every frame to handle input and update logic
        public override void HandleInput()
        {
            base.HandleInput();

            _interactionTimer -= Time.deltaTime;

            // Keep the hitbox aligned to mouse direction
            _interactionHitbox.UpdatePositionAndRotation();

            // Allow cancelling with item usage
            if (Player.inputHandler.useItemPressed)
            {
                StateMachine.ChangeState(Player.UseItemState);
                return;
            }

            // Allow cancelling with attack
            if (Player.inputHandler.attackPressed)
            {
                if (Player.inputHandler.CurrentAttackMode == AttackMode.KMelee && Player.MeleAttackState.IsUnlocked)
                {
                    StateMachine.ChangeState(Player.MeleAttackState);
                    return;
                }
                else if (Player.inputHandler.CurrentAttackMode == AttackMode.KRanged && Player.RangedAttackState.IsUnlocked)
                {
                    StateMachine.ChangeState(Player.RangedAttackState);
                    return;
                }
            }

            // End interaction after timer runs out
            if (_interactionTimer <= 0f)
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

        // Called when exiting the interaction state
        public override void Exit()
        {
            base.Exit();
            Player.isInteracting = false;
            _meleeHitbox.SetActive(false); // Deactivate hitbox GameObject
        }
    }
}
