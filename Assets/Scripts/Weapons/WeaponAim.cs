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
        [SerializeField] private Transform _pivot; // Player body pivot

        // === Configuration ===
        [SerializeField] private float _distance = 1f; // Distance from player center to weapon

        private void Update()
        {
            UpdatePositionAndRotation();
        }

        // Updates weapon's position and rotation to match player's aim direction
        public void UpdatePositionAndRotation()
        {
            var player = Player.Instance;
            if (player == null || _pivot == null)
            {
                Debug.LogError("WeaponAim: Missing player reference or pivot transform.");
                return;
            }

            Vector2 aimDir = player.AimDirection;
            if (aimDir == Vector2.zero)
                aimDir = Vector2.right; // Default to right

            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
            transform.position = _pivot.position + (Vector3)(aimDir.normalized * _distance);

            // Vertical flip for left aim
            Vector3 localScale = Vector3.one;
            localScale.y = (angle > 90 || angle < -90) ? -1f : 1f;
            transform.localScale = localScale;
        }

        // === Allows external assignment of the pivot reference ===
        public void SetPivot(Transform pivot)
        {
            _pivot = pivot;
        }
    }
}
