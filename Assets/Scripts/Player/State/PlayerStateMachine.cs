// ==============================
// PlayerStateMachine.cs
// Handles transitions between different player states
// ==============================

namespace PlayerController.State
{
    public class PlayerStateMachine
    {
        // === Current Player State ===
        public PlayerState CurrentState { get; private set; }

        public void Initialize(PlayerState startingState)
        {
            // === Set initial state and execute its entry logic ===
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            // === Exit current state and switch to new one ===
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
