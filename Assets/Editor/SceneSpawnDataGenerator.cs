// ==============================
// SceneSpawnDataGenerator.cs
// Editor tool to automatically create or update SceneSpawnData assets
// by scanning a scene for all SpawnPoint components.
// ==============================

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class SceneSpawnDataGenerator : EditorWindow
{
    // === Reference to the selected scene in the inspector ===
    private SceneAsset sceneAsset;

    // === Adds a menu item to open this custom editor window ===
    [MenuItem("Tools/Scene Spawn Data Generator")]
    public static void ShowWindow()
    {
        GetWindow<SceneSpawnDataGenerator>("Spawn Data Generator");
    }

    // === Draws the custom UI for the editor window ===
    private void OnGUI()
    {
        GUILayout.Label("Generador Automático de Spawn Points", EditorStyles.boldLabel);

        // Scene selector field
        sceneAsset = (SceneAsset)EditorGUILayout.ObjectField("Escena", sceneAsset, typeof(SceneAsset), false);

        // Button to trigger data generation
        if (sceneAsset != null && GUILayout.Button("Generar SceneSpawnData"))
        {
            GenerateSpawnData(sceneAsset);
        }
    }

    // === Main function that generates or updates the SceneSpawnData asset ===
    private void GenerateSpawnData(SceneAsset sceneAsset)
    {
        // Get the file path of the selected scene
        string scenePath = AssetDatabase.GetAssetPath(sceneAsset);

        // Prompt to save current scenes if modified
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // Open the selected scene in single mode
            Scene openedScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            // Find all SpawnPoint components in the scene
            SpawnPoint[] spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();

            // Warn if no spawn points are found
            if (spawnPoints.Length == 0)
            {
                Debug.LogWarning("No se encontraron SpawnPoints en la escena: " + sceneAsset.name);
                return;
            }

            // Collect valid spawnIDs
            List<string> ids = new List<string>();
            foreach (SpawnPoint sp in spawnPoints)
            {
                if (!string.IsNullOrEmpty(sp.spawnID))
                {
                    ids.Add(sp.spawnID);
                }
                else
                {
                    Debug.LogWarning($"SpawnPoint sin ID en objeto: {sp.gameObject.name}");
                }
            }

            // Ensure the output directory exists
            string folderPath = "Assets/Data/SpawnPoints";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Generate the asset path for the ScriptableObject
            string assetPath = $"{folderPath}/SpawnPoints_{sceneAsset.name}.asset";

            // Load existing asset or create a new one
            SceneSpawnData spawnData = AssetDatabase.LoadAssetAtPath<SceneSpawnData>(assetPath);
            if (spawnData == null)
            {
                spawnData = ScriptableObject.CreateInstance<SceneSpawnData>();
                AssetDatabase.CreateAsset(spawnData, assetPath);
            }

            // Update the asset with the new data
            spawnData.sceneName = sceneAsset.name;
            spawnData.spawnPointIDs = ids;

            // Save the asset changes
            EditorUtility.SetDirty(spawnData);
            AssetDatabase.SaveAssets();

            Debug.Log($"SceneSpawnData generado con éxito para {sceneAsset.name} ({ids.Count} spawn points)");
        }
    }
}
