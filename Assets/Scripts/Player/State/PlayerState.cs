// ==============================
// PlayerState.cs
// Abstract base class for all player states in the state machine pattern
// ==============================

namespace PlayerController.State
{
    public abstract class PlayerState
    {
        protected Player Player; // Reference to player entity
        protected PlayerStateMachine StateMachine; // Reference to player's state machine

        protected PlayerState(Player player, PlayerStateMachine stateMachine)
        {
            this.Player = player;
            this.StateMachine = stateMachine;
        }

        // === Lifecycle Hooks for States ===
        public virtual void Enter() { } // Called when state starts
        public virtual void Exit() { } // Called when state ends

        // === Update Logic Hooks ===
        public virtual void HandleInput() { } // Called during normal update
        public virtual void LogicUpdate() { } // Called during normal update

        // === Physics Hook ===
        public virtual void PhysicsUpdate() { } // Called during FixedUpdate
    }
}