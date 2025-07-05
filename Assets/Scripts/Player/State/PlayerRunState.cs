// ==============================
// PlayerRunState.cs
// Handles player behavior while sprinting
// ==============================

using UnityEngine;

namespace PlayerController.State
{
    public class PlayerRunState : PlayerState
    {
        private float _runTimer;

        public PlayerRunState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            _runTimer = Player.runDuration;
            Player.PlayerAnimation.PlayWalk(Player.inputHandler.movementInput);
        }

        public override void HandleInput()
        {
            base.HandleInput();

            _runTimer -= Time.deltaTime;
            if (_runTimer <= 0f)
            {
                Player.runCooldownTimer = Player.runCooldown;
                TransitionBack();
                return;
            }

            if (Player.inputHandler.useItemPressed)
            {
                StateMachine.ChangeState(Player.UseItemState);
                return;
            }

            if (Player.inputHandler.interactPressed)
            {
                StateMachine.ChangeState(Player.InteractState);
                return;
            }
        }

        public override void LogicUpdate()
        {
            Player.PlayerAnimation.PlayWalk(Player.inputHandler.movementInput);
        }

        public override void PhysicsUpdate()
        {
            Vector2 moveInput = Player.inputHandler.movementInput;
            Player.Move(moveInput * (Player.runSpeed / Player.moveSpeed));
        }

        private void TransitionBack()
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

        public bool CanRun()
        {
            return Player.runCooldownTimer <= 0f;
        }
    }
}