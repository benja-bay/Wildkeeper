// ==============================
// PlayerAnimation.cs
// Manages animation transitions for idle, walking, and melee attack
// ==============================

using UnityEngine;

namespace PlayerController
{
    public class PlayerAnimation
    {
        // === Animation Parameter Hashes ===
        private static int _isMoving = Animator.StringToHash("IsMoving");
        private static int _moveX = Animator.StringToHash("MoveX");
        private static int _moveY = Animator.StringToHash("MoveY");
        private static int _isAttacking = Animator.StringToHash("IsAttacking");

        // === Animator Reference ===
        private Animator _animator;

        public PlayerAnimation(Animator anim)
        {
            _animator = anim;
        }

        // Plays idle animation by disabling the "IsMoving" flag
        public void PlayIdle()
        {
            _animator.SetBool(_isMoving, false);
        }

        // Plays walk animation and sets direction based on movement input
        public void PlayWalk(Vector2 movementInput)
        {
            _animator.SetBool(_isMoving, true);
            _animator.SetFloat(_moveX, movementInput.x);
            _animator.SetFloat(_moveY, movementInput.y);
        }

        // Plays melee attack animation in the direction of the aim
        public void PlayMeleeAttack(Vector2 aimDirection)
        {
            _animator.SetBool(_isMoving, false);
            _animator.SetBool("IsAttacking", true);
            _animator.SetFloat("AttackX", aimDirection.x);
            _animator.SetFloat("AttackY", aimDirection.y);
            _animator.CrossFade("Attack", 0.1f); // Smooth transition to attack animation
        }

        // Stops the melee attack animation
        public void StopMeleeAttack()
        {
            _animator.SetBool(_isAttacking, false);
        }
    }
}
