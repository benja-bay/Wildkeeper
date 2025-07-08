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

        private System.Collections.IEnumerator RegisterToPlayerWhenAvailable()
        {
            while (player == null)
            {
                player = FindObjectOfType<PlayerController.Player>();
                yield return null; // Espera un frame hasta que el player aparezca
            }

            if (!isRegistered && player != null)
            {
                player.RegisterObserver(this);
                isRegistered = true;
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
