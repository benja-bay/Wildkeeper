// ==============================
// SpawnPoint.cs
// Component used to mark GameObjects as valid player spawn points.
// Used by SceneSpawnDataGenerator and SceneSpawnManager.
// ==============================

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; // Needed only for logging warnings in the Unity Editor
#endif

public class SpawnPoint : MonoBehaviour
{
    // === Unique Identifier for This Spawn Point ===
    // This ID must match the ID selected in the SceneTransitionTrigger.
    public string spawnID;

    // === Automatically Called in the Editor When Values Are Changed ===
    private void OnValidate()
    {
        // If a valid ID is set, rename the GameObject in the hierarchy for clarity
        if (!string.IsNullOrEmpty(spawnID))
        {
            gameObject.name = "Spawn_" + spawnID;
        }
#if UNITY_EDITOR
        else
        {
            // If the spawnID is empty, display a warning in the console (Editor only)
            Debug.LogWarning($"SpawnPoint on {gameObject.name} is missing a spawnID.");
        }
#endif
    }
}
