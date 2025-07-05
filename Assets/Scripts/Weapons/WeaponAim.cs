// ==============================
// WeaponAim.cs
// Controls weapon position and rotation to follow aim direction
// ==============================

using UnityEngine;
using PlayerController;

namespace Weapons
{
    public class WeaponAim : MonoBehaviour
    {
        // === References ===
        [SerializeField] private Transform _pivot; // Reference to the player's transform

        // === Configuration ===
        [SerializeField] private float _distance; // Distance from player to weapon (e.g., for aiming)

        private void Update()
        {
            // Update weapon aim and position every frame
            UpdatePositionAndRotation();
        }

        // Calculates direction from player to aim direction, rotates the weapon, and positions it accordingly
        public void UpdatePositionAndRotation()
        {
            if (PlayerController.Player.Instance == null || _pivot == null)
            {
                Debug.LogError("WeaponAim: Missing player reference or pivot.");
                return;
            }

            // Use the player's current aim direction
            Vector2 aimDir = PlayerController.Player.Instance.AimDirection;
            if (aimDir == Vector2.zero)
            {
                // Default to right if idle (avoids NaN angles)
                aimDir = Vector2.right;
            }

            // Calculate angle from direction
            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

            // Apply rotation
            transform.eulerAngles = new Vector3(0, 0, angle);

            // Position the weapon at a fixed distance in that direction
            transform.position = _pivot.position + (Vector3)(aimDir.normalized * _distance);

            // Flip weapon vertically if aiming left
            Vector3 localScale = Vector3.one;
            localScale.y = (angle > 90 || angle < -90) ? -1f : 1f;
            transform.localScale = localScale;
        }
    }
}
