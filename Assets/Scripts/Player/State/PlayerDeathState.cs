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

            Player.Move(Vector2.zero);
            Player.PlayerAnimation.PlayIdle();

            Player.inputHandler.enabled = false;

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

            // Rehabilitar input
            Player.inputHandler.enabled = true;

            // Resetear el trigger para evitar que se quede pegado
            Animator anim = Player.GetComponent<Animator>();
            if (anim != null)
            {
                anim.ResetTrigger("Die");
            }

            Debug.Log("PlayerDeathState: Player exited, controls re-enabled.");
        }
    }
}