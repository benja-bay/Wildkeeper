using System.Collections.Generic;
using Items;
using UnityEngine;
using PlayerController;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private HashSet<string> _usedObjectIDs = new();

        [Header("Jugador")]
        public int currentHealth;
        public int maxHealth;
        public Dictionary<ItemSO, int> inventory = new();
        private Dictionary<string, bool> remoteObjectStates = new();

        // === Checkpoint Data ===
        private Vector3? savedCheckpointPosition = null;
        private string currentCheckpointID = null;
        private string savedCheckpointScene = null;
        private Dictionary<ItemSO, int> savedInventory = new();
        private int savedHealth = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool IsObjectUsed(string objectID) => _usedObjectIDs.Contains(objectID);

        public void MarkObjectAsUsed(string objectID)
        {
            if (!string.IsNullOrEmpty(objectID))
                _usedObjectIDs.Add(objectID);
        }

        public void AddItem(ItemSO item, int quantity)
        {
            if (!inventory.ContainsKey(item))
                inventory[item] = 0;
            inventory[item] += quantity;
        }

        public int GetItemCount(ItemSO item)
        {
            return inventory.TryGetValue(item, out var count) ? count : 0;
        }

        public bool HasKey(string keyID)
        {
            foreach (var kvp in inventory)
            {
                if (!string.IsNullOrEmpty(kvp.Key.keyID) && kvp.Key.keyID == keyID && kvp.Value > 0)
                    return true;
            }
            return false;
        }

        public void SetRemoteObjectState(string objectID, bool active)
        {
            remoteObjectStates[objectID] = active;
        }

        public void NotifyBossDeath()
        {
            SceneManager.LoadScene("GameOver");
        }

        public bool? GetRemoteObjectState(string objectID)
        {
            return remoteObjectStates.ContainsKey(objectID) ? remoteObjectStates[objectID] : null;
        }

        // === Guarda checkpoint con escena, spawn ID, salud, inventario ===
        public void SaveCheckpoint(string spawnID, Vector3 position, int health, Inventory inventory)
        {
            currentCheckpointID = spawnID;
            savedCheckpointPosition = position;
            savedCheckpointScene = SceneManager.GetActiveScene().name;
            savedHealth = health;
            savedInventory = inventory.CloneItemData();
        }

        public bool HasCheckpoint() => savedCheckpointPosition.HasValue && !string.IsNullOrEmpty(savedCheckpointScene);

        // === Controla el respawn después de morir ===
        public void RespawnPlayer()
        {
            if (!HasCheckpoint()) return;

            // Le decimos al SceneSpawnManager que use este spawn point
            SceneSpawnManager.Instance?.SetNextSpawnPoint(currentCheckpointID);

            // Cargamos la escena del checkpoint
            SceneManager.LoadScene(savedCheckpointScene);
        }

        // === Llamado por SceneSpawnManager cuando se haya spawneado al jugador ===
        public void FinalizeRespawn()
        {
            Player player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
            if (player == null) return;

            RestoreCheckpoint(player);

            player.inputHandler.enabled = true;
            player.enabled = true;
            player.GetComponent<PlayerHealth>().enabled = true;
            player.ChangeToIdleState();
        }

        // === Aplica vida e inventario guardados ===
        public void RestoreCheckpoint(Player player)
        {
            if (!HasCheckpoint()) return;

            var health = player.GetComponent<PlayerHealth>();
            health.SetMaxHealth(savedHealth);
            health.Regenerate(savedHealth);

            player.Inventory.Clear();
            foreach (var kvp in savedInventory)
            {
                player.Inventory.AddItem(kvp.Key, kvp.Value);
            }

            Debug.Log($"Restored to checkpoint '{currentCheckpointID}' in scene '{savedCheckpointScene}'");
        }
    }
}