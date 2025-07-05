// ==============================
// PlayerWalkState.cs
// Handles player behavior while walking (movement input detected)
// ==============================

using UnityEngine;

namespace Player.State
{
    public class PlayerWalkState : PlayerState
    {
        // === Constructor ===
        public PlayerWalkState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Enter()
        {
            // === Play walking animation on state entry ===
            Player.PlayerAnimation.PlayWalk(Player.inputHandler.movementInput);
        }

        public override void HandleInput()
        {
            // === Handle transitions based on input ===

            // Handle Use Item input
            if (Player.inputHandler.useItemPressed)
            {
                StateMachine.ChangeState(Player.UseItemState);
                return;
            }

            // Handle Interact input
            if (Player.inputHandler.interactPressed)
            {
                StateMachine.ChangeState(Player.InteractState);
                return;
            }

            // Handle Attack input
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
                else
                {
                    Debug.Log("Attack mode not unlocked.");
                }
            }

            // Handle movement input
            if (Player.inputHandler.movementInput == Vector2.zero)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void LogicUpdate()
        {
            // === Update animation direction based on movement input ===
            Vector2 moveInput = Player.inputHandler.movementInput;
            Player.PlayerAnimation.PlayWalk(moveInput);
        }

        public override void PhysicsUpdate()
        {
            // === Move player based on input direction ===
            Vector2 moveInput = Player.inputHandler.movementInput;
            Player.Move(moveInput);
        }
    }
}
