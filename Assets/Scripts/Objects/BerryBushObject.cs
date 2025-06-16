// ==============================
// BerryBushObject.cs
// Interactable bush that gives randomized items to the player once, with animation
// ==============================

using Items;
using UnityEngine;

namespace Objects
{
    public class BerryBushObject : MonoBehaviour, IInteractable
    {
        [Header("Animator")]
        [Tooltip("Animator controlling the bush's animations")]
        [SerializeField] private Animator animator;
        
        [Header("ID")]
        [Tooltip("Unique object ID")]
        [SerializeField] private string objectID;

        [System.Serializable]
        public struct BushItem
        {
            public ItemSO item;       // Item to be collected
            public int minQuantity;   // Minimum amount given
            public int maxQuantity;   // Maximum amount given
        }

        [SerializeField] private BushItem[] contents; // List of collectible items in the bush

        private void Awake()
        {
            // === Cache Animator if not set in Inspector ===
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                if (animator == null)
                {
                    Debug.LogError("BerryBushObject: No Animator found on this object.");
                }
            }
            
            if (GameManager.Instance != null && GameManager.Instance.IsObjectUsed(objectID))
            {
                if (animator != null)
                {
                    animator.SetTrigger("Used");
                }
            }
        }

        public void Interact(Player.Player player)
        {
            // === Prevent bush from being used more than once ===
            if (GameManager.Instance != null && GameManager.Instance.IsObjectUsed(objectID))
            {
                Debug.Log("This bush has already been harvested.");
                return;
            }

            // === Access the player's inventory ===
            var inventory = player.Inventory;
            if (inventory == null)
            {
                Debug.LogError("Player does not have an Inventory component.");
                return;
            }

            // === Randomly determine and give items to player ===
            foreach (var bushItem in contents)
            {
                int amount = Random.Range(bushItem.minQuantity, bushItem.maxQuantity + 1);
                inventory.AddItem(bushItem.item, amount);
                Debug.Log($"Collected {amount}x {bushItem.item.itemName}");
            }

            GameManager.Instance.MarkObjectAsUsed(objectID);

            // === Play used animation if available ===
            if (animator != null)
            {
                animator.SetTrigger("Used");
            }
        }
    }
}