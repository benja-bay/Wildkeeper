// ==============================
// PlayerDeathState.cs
// Handles player behavior when dead
// ==============================

using UnityEngine;

namespace PlayerController.State
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        // Called once when this state becomes active
        public override void Enter()
        {
            base.Enter();

            // Stop movement
            Player.Move(Vector2.zero);

            // Ensure idle animation so movement stops visually
            Player.PlayerAnimation.PlayIdle();

            // Disable player input so no new commands come in
            Player.inputHandler.enabled = false;

            // Play death animation trigger
            Animator anim = Player.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Die");
            }

            Debug.Log("PlayerDeathState: Player has entered the death state.");
        }

        public override void HandleInput()
        {
            // Intentionally empty - no inputs accepted while dead
        }

        public override void LogicUpdate()
        {
            // No logic updates while dead
        }

        public override void PhysicsUpdate()
        {
            // No physics while dead
        }

        public override void Exit()
        {
            base.Exit();
            // ToDo exit, respawn logic
        }
    }
}