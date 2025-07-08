using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

namespace HUD
{
    public class PlayerHUD : MonoBehaviour, IPlayerObserver
    {
        [SerializeField] private GameObject meleeIcon;
        [SerializeField] private GameObject rangedIcon;
        [SerializeField] private PlayerController.Player player;

        private bool isRegistered = false;

        private void Start()
        {
            StartCoroutine(RegisterToPlayerWhenAvailable());
        }

        private IEnumerator RegisterToPlayerWhenAvailable()
        {
            while (player == null)
            {
                player = FindObjectOfType<PlayerController.Player>();
                yield return null;
            }

            if (!isRegistered && player != null)
            {
                player.RegisterObserver(this);
                isRegistered = true;

                // Verificar el estado actual del player y actualizar íconos si corresponde
                if (player.IsMeleeUnlocked)
                    OnMeleeUnlocked();

                if (player.IsRangedUnlocked)
                    OnRangedUnlocked();
            }
        }

        public void OnMeleeUnlocked()
        {
            if (meleeIcon != null)
                meleeIcon.SetActive(true);
        }

        public void OnRangedUnlocked()
        {
            if (rangedIcon != null)
                rangedIcon.SetActive(true);
        }
    }
}
