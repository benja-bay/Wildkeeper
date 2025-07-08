using System.Collections.Generic;
using Items;
using UnityEngine;
using PlayerController;
using UnityEngine.SceneManagement;
using Objects; // ← para acceder a IInteractable

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
        private HashSet<string> savedUsedObjectIDs = new();
        
        private bool isRespawning = false;

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

        // === Guarda checkpoint con escena, spawn ID, salud, inventario y objetos usados ===
        public void SaveCheckpoint(string spawnID, Vector3 position, int health, Inventory inventory)
        {
            currentCheckpointID = spawnID;
            savedCheckpointPosition = position;
            savedCheckpointScene = SceneManager.GetActiveScene().name;
            savedHealth = health;
            savedInventory = inventory.CloneItemData();

            // Guardar objetos usados al momento del checkpoint
            savedUsedObjectIDs = new HashSet<string>(_usedObjectIDs);
        }

        public bool HasCheckpoint() => savedCheckpointPosition.HasValue && !string.IsNullOrEmpty(savedCheckpointScene);

        // === Controla el respawn después de morir ===
        public void RespawnPlayer()
        {
            if (!HasCheckpoint()) return;

            isRespawning = true; // ← importante

            SceneSpawnManager.Instance?.SetNextSpawnPoint(currentCheckpointID);
            SceneManager.LoadScene(savedCheckpointScene);
        }

        // === Llamado por SceneSpawnManager cuando se haya spawneado al jugador ===
        public void FinalizeRespawn()
        {
            if (!isRespawning || !HasCheckpoint()) return;

            isRespawning = false;

            Player player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
            if (player == null) return;

            RestoreCheckpoint(player);
            RestoreInteractables();

            var health = player.GetComponent<PlayerHealth>();
            health.enabled = true;
            health.Revive();

            player.inputHandler.enabled = true;
            player.enabled = true;
            player.ChangeToIdleState();
        }

        // === Aplica vida e inventario guardados ===
        public void RestoreCheckpoint(Player player)
        {
            if (!HasCheckpoint()) return;

            var health = player.GetComponent<PlayerHealth>();
            health.SetMaxHealth(savedHealth > 0 ? savedHealth : maxHealth);
            health.Regenerate(savedHealth > 0 ? savedHealth : maxHealth);

            player.Inventory.Clear();

            if (savedInventory.Count > 0)
            {
                foreach (var kvp in savedInventory)
                {
                    player.Inventory.AddItem(kvp.Key, kvp.Value);
                }
            }
            else
            {
                Debug.LogWarning("RestoreCheckpoint: saved inventory is empty.");
            }

            Debug.Log($"Restored to checkpoint '{currentCheckpointID}' in scene '{savedCheckpointScene}'");
        }

        // === Restaura objetos interactuables que NO fueron usados al momento del checkpoint ===
        private void RestoreInteractables()
        {
            // Buscar todos los MonoBehaviours activos e inactivos
            MonoBehaviour[] allBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>(true);

            foreach (var mb in allBehaviours)
            {
                if (mb is IInteractable interactable)
                {
                    string objID = interactable.ObjectID;
                    if (string.IsNullOrEmpty(objID)) continue; // No persistente → ignorar

                    if (!savedUsedObjectIDs.Contains(objID))
                    {
                        mb.gameObject.SetActive(true); // Restaurar si no fue usado al guardar
                    }
                }
            }

            Debug.Log("Interactables restored based on saved checkpoint state.");
        }
    }
}