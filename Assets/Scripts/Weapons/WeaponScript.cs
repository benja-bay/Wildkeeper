// ==============================
// WeaponScript.cs
// Handles weapon firing logic including fire rate and bullet instantiation
// ==============================

using UnityEngine;
using PlayerController;

namespace Weapons
{
    public class WeaponScript : MonoBehaviour
    {
        [SerializeField] private Transform _barrel; // Bullet spawn point
        [SerializeField] private GameObject _bullet; // Bullet prefab to instantiate
        [SerializeField] private float _fireRate; // Time delay between each shot

        private float _fireTimer; // Timer to control fire rate

        public void Shoot()
        {
            if (!CanShoot()) return;

            var player = Player.Instance;
            if (player == null) return;

            // Check for ammo before firing
            if (!player.Inventory.HasAmmo(player.DartItem))
            {
                Debug.Log("No ammo left to shoot.");
                return;
            }

            // Fire and consume ammo
            _fireTimer = Time.time + _fireRate;
            Instantiate(_bullet, _barrel.position, _barrel.rotation);
            player.Inventory.ConsumeAmmo(player.DartItem);
        }

        public bool CanShoot()
        {
            return Time.time > _fireTimer;
        }
    }
}
