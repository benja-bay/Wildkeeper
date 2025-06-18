using System.Collections.Generic;
using Items;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private HashSet<string> _usedObjectIDs = new();
    
    [Header("Jugador")]
    public int currentHealth;
    public int maxHealth;
    public Dictionary<ItemSO, int> inventory = new();
    private Dictionary<string, bool> remoteObjectStates = new();
    
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

    public bool? GetRemoteObjectState(string objectID)
    {
        return remoteObjectStates.ContainsKey(objectID) ? remoteObjectStates[objectID] : null;
    }
}