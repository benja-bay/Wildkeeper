using Managers;
using UnityEngine;
using PlayerController;

namespace Objects
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private string checkpointID;
        [SerializeField] private bool autoActivateOnTrigger = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!autoActivateOnTrigger) return;
            if (collision.CompareTag("Player"))
                ActivateCheckpoint(collision.GetComponent<Player>());
        }

        public void ActivateCheckpoint(Player player)
        {
            if (player == null || GameManager.Instance == null) return;

            GameManager.Instance.SaveCheckpoint(checkpointID, player.transform.position, player.GetComponent<PlayerHealth>().CurrentHealth, player.Inventory);
            Debug.Log($"Checkpoint '{checkpointID}' activated!");
        }
    }
}