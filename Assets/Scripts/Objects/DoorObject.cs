using HUD;
using UnityEngine;
using Objects;
using Items;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Collider2D))]
public class DoorObject : MonoBehaviour, IInteractable
{
    public SceneSpawnData spawnData;
    public int selectedSpawnIndex = 0;

    [HideInInspector]
    public string targetScene;

    public bool requiresKey = false;
    public ItemSO requiredKey; // Llave necesaria para abrir la puerta
    public string keyID = "";

#if UNITY_EDITOR
    public SceneAsset sceneAsset; // solo visible en el Editor
#endif

    private string SelectedSpawnID =>
        spawnData != null && spawnData.spawnPointIDs.Count > selectedSpawnIndex
            ? spawnData.spawnPointIDs[selectedSpawnIndex]
            : "";

    public void Interact(PlayerController.Player player)
    {
        if (requiresKey)
        {
            bool hasKey = false;

            if (requiredKey != null)
                hasKey = player.Inventory.GetItemCount(requiredKey) > 0;
            else if (!string.IsNullOrEmpty(keyID))
                hasKey = player.Inventory.HasKey(keyID);

            if (!hasKey)
            {
                InteractionUIManager.Instance?.ShowPromptTemporary("La puerta está cerrada. Necesitas una llave.", 2f);
                return;
            }
        }

        if (string.IsNullOrEmpty(targetScene) || string.IsNullOrEmpty(SelectedSpawnID))
        {
            Debug.LogWarning("Faltan datos para la transición de escena.");
            return;
        }

        SceneSpawnManager.Instance.SetNextSpawnPoint(SelectedSpawnID);
        UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
    }
}
