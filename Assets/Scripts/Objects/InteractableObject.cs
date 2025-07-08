// ==============================
// InteractableObject.cs
// Simple implementation of an interactable object triggered by player interaction
// ==============================

using UnityEngine;

namespace Objects
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerController.Player player)
        {
            // === Handle interaction logic triggered by the player ===
            if (player == null)
            {
                Debug.LogWarning("Attempted to interact with object without a valid player reference.");
                return;
            }

            Debug.Log($"Player {player.name} has interacted with the test object!");
        }
        
        public string ObjectID => null;
    }
}
