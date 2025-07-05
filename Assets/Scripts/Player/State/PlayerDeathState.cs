// ==============================
// PlayerDeathState.cs
// Handles player behavior when dead
// ==============================

using UnityEngine;

namespace Player.State
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        // Called once when this state becomes active
        public override void Enter()
        {
            base.Enter();

            // Stop movement and disable interactions
            Player.Move(Vector2.zero);
            Player.inputHandler.enabled = false;

            // Play death animation
            Animator anim = Player.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Die");
            }

            Debug.Log("PlayerDeathState: Player has entered the death state.");
        }

        // Player can't do anything while dead
        public override void HandleInput()
        {
            // Intentionally empty - no inputs accepted
        }

        public override void LogicUpdate()
        {
            // No logic updates needed when dead
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