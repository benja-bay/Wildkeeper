// ==============================
// SceneSpawnManager.cs
// Singleton responsible for placing the player at the correct spawn point
// when transitioning between scenes, based on the spawnID provided.
// ==============================

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManager : MonoBehaviour
{
    // === Singleton instance for global access ===
    public static SceneSpawnManager Instance;

    // === The ID of the spawn point to use when the next scene is loaded ===
    private string nextSpawnPointID;

    // === Initialize the singleton and subscribe to the sceneLoaded event ===
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Persist this object across scene loads
            DontDestroyOnLoad(gameObject);

            // Register callback for when a new scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Destroy duplicate managers if one already exists
            Destroy(gameObject);
        }
    }

    // === Called by SceneTransitionTrigger to set the next spawn location by ID ===
    public void SetNextSpawnPoint(string spawnPointID)
    {
        nextSpawnPointID = spawnPointID;
    }

    // === Triggered automatically when a new scene finishes loading ===
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If a valid spawn point ID is set, search for a matching SpawnPoint in the new scene
        if (!string.IsNullOrEmpty(nextSpawnPointID))
        {
            // Get all GameObjects in the scene
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            // Look for the SpawnPoint component with the matching ID
            foreach (var obj in allObjects)
            {
                SpawnPoint sp = obj.GetComponent<SpawnPoint>();
                if (sp != null && sp.spawnID == nextSpawnPointID)
                {
                    // If found, move the player to that position
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        player.transform.position = sp.transform.position;
                        return;
                    }
                }
            }

            // Log a warning if no matching spawn point was found 
            Debug.LogWarning($"No se encontró un SpawnPoint con ID '{nextSpawnPointID}' en la escena '{scene.name}'.");
        }
    }
}
