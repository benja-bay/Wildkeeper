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
        public Vector2 movementInput;
        public bool attackPressed;
        public bool interactPressed;
        public bool useItemPressed;
        public bool runPressed;
        public AttackMode CurrentAttackMode { get; private set; } = AttackMode.KMelee;
        public Vector2 MouseDirection { get; private set; }

        // === Aim Mode Support ===
        public Vector2 LastMovementDirection { get; private set; } = Vector2.right;
        public bool IsUsingMouse { get; private set; } = true;

        // === Required References ===
        [Header("Aiming")]
        [SerializeField] private Transform _aimPivot; // Optional pivot for mouse direction
        [SerializeField] private Camera _camera;

        private void Awake()
        {
            // Fallback: if no custom pivot assigned, use own transform
            if (_aimPivot == null)
            {
                _aimPivot = transform;
            }
        }

        void Update()
        {
            // === Capture Movement & Input ===
            movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            attackPressed = Input.GetButtonDown("Attack");
            interactPressed = Input.GetButtonDown("Interact");
            useItemPressed = Input.GetButtonDown("Use");
            runPressed = Input.GetButtonDown("Run");

            // === Movement-based aiming for gamepad ===
            if (movementInput != Vector2.zero)
            {
                LastMovementDirection = movementInput;
                IsUsingMouse = false;
            }

            // === Mouse aiming ===
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                IsUsingMouse = true;
            }

            // === Switch attack mode ===
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            bool switchPositive = Input.GetButtonDown("SwitchAttackPositive");
            bool switchNegative = Input.GetButtonDown("SwitchAttackNegative");

            if (scroll != 0f || switchPositive || switchNegative)
            {
                CurrentAttackMode = CurrentAttackMode == AttackMode.KMelee ? AttackMode.KRanged : AttackMode.KMelee;
            }

            // === Update Aiming ===
            UpdateMouseDirection();
        }

        private void UpdateMouseDirection()
        {
            if (_camera == null || _aimPivot == null) return;

            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector3 dir = (mouseWorldPos - _aimPivot.position).normalized;
            MouseDirection = new Vector2(dir.x, dir.y);
        }

        // === Set pivot manually from code if needed ===
        public void SetAimPivot(Transform pivot)
        {
            _aimPivot = pivot;
        }
    }
}
