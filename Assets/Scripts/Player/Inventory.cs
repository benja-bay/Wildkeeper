// ==============================
// Inventory.cs
// Manages player's items, including usage, quantity tracking, and effects
// ==============================

using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Player
{
    public class Inventory
    {
        // === Stored Items ===
        public Dictionary<ItemSO, int> _items = new(); // Stores each item and its quantity

        // Adds item to inventory, capping at its max allowed amount
        public void AddItem(ItemSO item, int quantity = 1)
        {
            if (_items.ContainsKey(item))
            {
                _items[item] = Mathf.Min(_items[item] + quantity, item.maxAmount);
            }
            else
            {
                _items[item] = Mathf.Min(quantity, item.maxAmount);
            }

            Debug.Log($"Now you have {_items[item]}x {item.itemName}");
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddItem(item, quantity);
            }
        }

        // Uses an item and applies its effect to the player
        public bool UseItem(ItemSO item, Player player)
        {
            if (!_items.ContainsKey(item) || _items[item] <= 0)
                return false;

            _items[item]--;
            
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.inventory.ContainsKey(item))
                {
                    GameManager.Instance.inventory[item] = _items[item];
                }
            }

            switch (item.effectType)
            {
                case ItemSO.ItemEffectType.KHeal:
                    player.GetComponent<PlayerHealth>()?.Regenerate(item.value);
                    break;

                case ItemSO.ItemEffectType.KAmmo:
                    // Ammo usage handled elsewhere
                    break;

                case ItemSO.ItemEffectType.KUnlockMelee:
                    player.MeleAttackState.Unlock();
                    break;

                case ItemSO.ItemEffectType.KUnlockRanged:
                    player.RangedAttackState.Unlock();
                    break;
            }

            return true;
        }

        // Returns the current quantity of a given item
        public int GetItemCount(ItemSO item)
        {
            return _items.TryGetValue(item, out int count) ? count : 0;
        }

        // Finds the first usable item of a specific effect type
        public ItemSO GetFirstUsableItemOfType(ItemSO.ItemEffectType effectType)
        {
            foreach (var kvp in _items)
            {
                if (kvp.Key.effectType == effectType && kvp.Value > 0)
                    return kvp.Key;
            }
            return null;
        }

        // Reduces ammo count by one if available
        public bool ConsumeAmmo(ItemSO ammoItem)
        {
            if (!_items.ContainsKey(ammoItem) || _items[ammoItem] <= 0)
                return false;

            _items[ammoItem]--;
            return true;
        }

        // Checks if the player has ammo available
        public bool HasAmmo(ItemSO ammoItem)
        {
            return _items.TryGetValue(ammoItem, out int count) && count > 0;
        }
        
        public bool HasKey(string keyID)
        {
            foreach (var item in _items.Keys)
            {
                if (!string.IsNullOrEmpty(item.keyID) && item.keyID == keyID && _items[item] > 0)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
