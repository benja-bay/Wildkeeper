// ==============================
// PlayerInputHandler.cs
// Handles all player input including movement, attack, and mouse direction
// ==============================

using UnityEngine;

namespace PlayerController
{
    public enum AttackMode
    {
        KMelee,
        KRanged
    }

    public class PlayerInputHandler : MonoBehaviour
    {
        // === Public Input States ===
        public Vector2 movementInput; // Directional movement input from player
        public bool attackPressed; // Whether the attack input was pressed
        public bool interactPressed; // Whether the interact input was pressed
        public bool useItemPressed;
        public AttackMode CurrentAttackMode { get; private set; } = AttackMode.KMelee;
        public Vector2 MouseDirection { get; private set; } // Direction from player to mouse position

        // === New Aim Mode Support ===
        public Vector2 LastMovementDirection { get; private set; } = Vector2.right; // Stores last movement direction
        public bool IsUsingMouse { get; private set; } = true; // Determines if we're using mouse aiming

        // === Required References ===
        [SerializeField] private Transform _playerTransform; // Transform of the player
        [SerializeField] private Camera _camera; // Camera used to convert screen to world position

        void Update()
        {
            // === Capture Movement & Attack Input ===
            movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            attackPressed = Input.GetButtonDown("Attack");
            interactPressed = Input.GetButtonDown("Interact");
            useItemPressed = Input.GetButtonDown("Use");

            // === Remember last movement direction for gamepad/keyboard aiming ===
            if (movementInput != Vector2.zero)
            {
                LastMovementDirection = movementInput;
                IsUsingMouse = false; // If moving, switch to movement-based aiming
            }

            // === Detect if mouse was used for aiming (button down or hold) ===
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                IsUsingMouse = true;
            }

            // === Switch attack mode with mouse scroll wheel ===
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                CurrentAttackMode = CurrentAttackMode == AttackMode.KMelee ? AttackMode.KRanged : AttackMode.KMelee;
            }

            // === Update Mouse Direction for Aiming ===
            UpdateMouseDirection();
        }

        private void UpdateMouseDirection()
        {
            // === Convert Mouse Position to World and Calculate Direction from Player ===
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector3 dir = (mouseWorldPos - _playerTransform.position).normalized;
            MouseDirection = new Vector2(dir.x, dir.y);
        }
    }
}
