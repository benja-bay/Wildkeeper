// ==============================
// IInteractable.cs
// Interface for objects that can be interacted with by the player
// ==============================

namespace Objects
{
    public interface IInteractable
    {
        void Interact(PlayerController.Player player);

        /// <summary>
        /// Optional: unique identifier for saving persistent state (like healing or key pickups).
        /// Return null or empty if not persistent.
        /// </summary>
        string ObjectID { get; }
    }
}