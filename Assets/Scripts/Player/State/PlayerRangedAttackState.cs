// ==============================
// PlayerRangedAttackState.cs
// Handles player behavior during ranged attacks using a weapon and aiming logic
// ==============================

using UnityEngine;
using Weapons;

namespace Player.State
{
    public class PlayerRangedAttackState : PlayerState
    {
        // === Weapon References ===
        private readonly WeaponScript _weapon;       // Script to handle shooting logic
        private readonly WeaponAim _aim;             // Script to aim weapon toward mouse
        private bool _unlocked = false;              // Whether ranged attack is unlocked

        public PlayerRangedAttackState(Player player, PlayerStateMachine stateMachine,
            WeaponScript weapon, WeaponAim aim)
            : base(player, stateMachine)
        {
            _weapon = weapon;
            _aim = aim;
        }

        // Called when entering the ranged attack state
        public override void Enter()
        {
            base.Enter();

            // Block if not unlocked or out of ammo
            if (!_unlocked)
            {
                Debug.Log("Ranged attack not unlocked.");
                StateMachine.ChangeState(Player.IdleState);
                return;
            }

            if (!Player.Inventory.HasAmmo(Player.DartItem))
            {
                Debug.Log("No ammo available.");
                StateMachine.ChangeState(Player.IdleState);
                return;
            }

            Player.isShooting = true;

            // Update weapon aim immediately
            _aim.UpdatePositionAndRotation();

            // Fire if weapon is ready
            if (_weapon.CanShoot())
                _weapon.Shoot();

            // Consume ammo after shot
            Player.Inventory.ConsumeAmmo(Player.DartItem);
        }

        // Called every frame to handle aiming updates and input
        public override void HandleInput()
        {
            base.HandleInput();

            // Update aiming direction
            _aim.UpdatePositionAndRotation();

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
        }

        // Called every frame to manage shooting and transitions
        public override void LogicUpdate()
        {
            // Fire continuously while attack button is pressed, respecting fire rate
            if (Player.inputHandler.attackPressed && _weapon.CanShoot())
            {
                _weapon.Shoot();
                Player.Inventory.ConsumeAmmo(Player.DartItem);
            }

            // If attack button is released, transition to movement or idle state
            if (!Player.inputHandler.attackPressed)
            {
                if (Player.inputHandler.movementInput != Vector2.zero)
                    StateMachine.ChangeState(Player.WalkState);
                else
                    StateMachine.ChangeState(Player.IdleState);
            }
        }

        // Called when exiting the ranged attack state
        public override void Exit()
        {
            base.Exit();
            Player.isShooting = false;
        }

        // Unlocks the ranged attack ability
        public void Unlock()
        {
            _unlocked = true;
        }

        // Property to check if the ability is unlocked
        public bool IsUnlocked => _unlocked;
    }
}
