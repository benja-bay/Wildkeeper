// ==============================
// IInteractable.cs
// Interface for objects that can be interacted with by the player
// ==============================

namespace Objects
{
    public interface IInteractable
    {
        // === Defines interaction behavior for an object ===
        void Interact(Player.Player player);

    }
}