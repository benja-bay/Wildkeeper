// ==============================
// CheckPointObject.cs
// Interactable checkpoint that can be activated once to mark player progress
// ==============================
// TODO - No corregir


using UnityEngine;

namespace Objects
{
    public class CheckPointObject : MonoBehaviour, IInteractable
    {
        [Header("Visuals")]
        [Tooltip("Sprite displayed when the checkpoint has already been used")]
        [SerializeField] private Sprite usedSprite;

        private bool _hasBeenUsed = false; // Ensures the checkpoint is only used once
        private SpriteRenderer _spriteRenderer; // Reference to the SpriteRenderer

        private void Awake()
        {
            // === Cache the SpriteRenderer attached to this object ===
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("CheckPointObject: No SpriteRenderer found on this object.");
            }
        }

        public void Interact(PlayerController.Player player)
        {
            // === Prevent multiple activations of the checkpoint ===
            if (_hasBeenUsed)
            {
                Debug.Log("This checkpoint has already been used.");
                return;
            }

            // === Simulate checkpoint activation ===
            Debug.Log("Checkpoint set");

            _hasBeenUsed = true;

            // === Change sprite to indicate checkpoint has been used ===
            if (usedSprite != null && _spriteRenderer != null)
            {
                _spriteRenderer.sprite = usedSprite;
            }
        }
    }
}
