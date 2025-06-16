// ==============================
// SceneTransitionTrigger.cs
// Trigger-based scene transition system that changes scenes and sets the player's spawn point.
// ==============================

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; // Required for SceneAsset in the Unity Editor
#endif

public class SceneTransitionTrigger : MonoBehaviour
{
    // === Name of the target scene to load (used at runtime) ===
    // This value is set automatically via the custom inspector using SceneAsset.
    [HideInInspector]
    public string targetScene;

#if UNITY_EDITOR
    // === Scene reference used only in the Editor for visual selection ===
    public SceneAsset sceneAsset;
#endif

    // === Reference to the ScriptableObject that holds valid spawn point IDs for the target scene ===
    public SceneSpawnData spawnData;

    // === Index of the selected spawn point from the spawnData list ===
    public int selectedSpawnIndex = 0;

    // === The currently selected spawn point ID (read-only property) ===
    // Used to determine where the player should appear in the new scene.
    public string SelectedSpawnID =>
        spawnData != null && spawnData.spawnPointIDs.Count > selectedSpawnIndex
        ? spawnData.spawnPointIDs[selectedSpawnIndex]
        : "";

    // === Trigger event that initiates the scene transition when the player enters ===
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only respond to the player entering the trigger zone
        if (collision.CompareTag("Player"))
        {
            // Set the next spawn point in the SceneSpawnManager
            SceneSpawnManager.Instance.SetNextSpawnPoint(SelectedSpawnID);

            // Load the target scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
        }
    }
}
