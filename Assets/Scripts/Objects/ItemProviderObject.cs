using UnityEngine;
using Items;

namespace Objects
{
    public class ItemProviderObject : MonoBehaviour, IInteractable
    {
        public enum QuantityMode { Fixed, Random }

        [Header("Object ID")]
        [SerializeField] private string objectID;

        [Header("Animator")]
        [SerializeField] private Animator animator;

        [Header("Item Settings")]
        public QuantityMode quantityMode = QuantityMode.Fixed;

        [System.Serializable]
        public struct ItemEntry
        {
            public ItemSO item;
            public int fixedQuantity;
            public int minRandom;
            public int maxRandom;
        }

        [SerializeField] private ItemEntry[] items;

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                if (animator == null)
                    Debug.LogError("ItemProviderObject: No Animator found.");
            }

            // Si ya fue usado, reproducir animación "Used"
            if (GameManager.Instance != null && GameManager.Instance.IsObjectUsed(objectID))
            {
                animator?.SetTrigger("Used");
            }
        }

        public void Interact(Player.Player player)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsObjectUsed(objectID))
            {
                Debug.Log("Este objeto ya fue usado.");
                return;
            }

            var inventory = player.Inventory;
            if (inventory == null) return;

            foreach (var entry in items)
            {
                int amount = quantityMode switch
                {
                    QuantityMode.Fixed => entry.fixedQuantity,
                    QuantityMode.Random => Random.Range(entry.minRandom, entry.maxRandom + 1),
                    _ => 0
                };

                inventory.AddItem(entry.item, amount);
                Debug.Log($"Entregado {amount}x {entry.item.itemName}");
            }

            GameManager.Instance.MarkObjectAsUsed(objectID);

            animator?.SetTrigger("Used");
        }
    }
}
