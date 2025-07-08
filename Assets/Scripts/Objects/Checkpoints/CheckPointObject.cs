// ==============================
// CheckPointObject.cs
// Interactable checkpoint that can be activated once to mark player progress
// ==============================

using UnityEngine;
using PlayerController;
using Managers;

namespace Objects
{
    public class CheckPointObject : MonoBehaviour, IInteractable
    {
        [Header("Checkpoint Data")]
        [Tooltip("Unique ID for this checkpoint (leave empty if not persistent)")]
        [SerializeField] private string objectID;

        [Header("Visuals")]
        [Tooltip("Sprite displayed when the checkpoint has already been used")]
        [SerializeField] private Sprite usedSprite;

        private bool _hasBeenUsed = false;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
                Debug.LogError("CheckPointObject: No SpriteRenderer found.");

            // Verificamos si ya se activó antes (solo si tiene ID)
            if (!string.IsNullOrEmpty(objectID) && GameManager.Instance != null)
            {
                if (GameManager.Instance.IsObjectUsed(objectID))
                {
                    _hasBeenUsed = true;
                    if (usedSprite != null)
                        _spriteRenderer.sprite = usedSprite;
                }
            }
        }

        public void Interact(Player player)
        {
            if (_hasBeenUsed)
            {
                Debug.Log("This checkpoint has already been used.");
                return;
            }

            _hasBeenUsed = true;

            // Guardamos el checkpoint
            GameManager.Instance?.SaveCheckpoint(
                spawnID: objectID, // se usa como spawnID
                position: player.transform.position,
                health: player.GetComponent<PlayerHealth>().CurrentHealth,
                inventory: player.Inventory
            );

            // Marcamos como usado (solo si tiene ID)
            if (!string.IsNullOrEmpty(objectID))
                GameManager.Instance.MarkObjectAsUsed(objectID);

            // Cambiamos sprite visual
            if (usedSprite != null && _spriteRenderer != null)
                _spriteRenderer.sprite = usedSprite;

            Debug.Log($"Checkpoint '{objectID}' activado!");
        }

        // Soporte para restauración por GameManager
        public string ObjectID => objectID;
    }
}
