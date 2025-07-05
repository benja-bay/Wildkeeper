// ==============================
// PlayerIdleState.cs
// Handles player behavior while idle (no movement)
// ==============================

using UnityEngine;

namespace PlayerController.State
{
    public class PlayerIdleState : PlayerState
    {
        // === Constructor ===
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Enter()
        {
            // === Play idle animation and stop movement ===
            Player.PlayerAnimation.PlayIdle();
            Player.Move(Vector2.zero);
        }

        public override void HandleInput()
        {
            // === Handle transitions based on input ===

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
            if (Player.inputHandler.movementInput != Vector2.zero)
            {
                StateMachine.ChangeState(Player.WalkState);
            }
        }
    }
}
