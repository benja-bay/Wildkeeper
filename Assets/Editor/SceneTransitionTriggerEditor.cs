// ==============================
// SceneTransitionTriggerEditor.cs
// Custom inspector for SceneTransitionTrigger.
// Allows scene and spawn point selection via a visual editor UI.
// ==============================

using UnityEditor;
using UnityEngine;

// This attribute tells Unity to use this custom inspector for SceneTransitionTrigger components
[CustomEditor(typeof(SceneTransitionTrigger))]
public class SceneTransitionTriggerEditor : Editor
{
    // === Override Unity’s default Inspector GUI ===
    public override void OnInspectorGUI()
    {
        // Reference to the selected SceneTransitionTrigger in the Inspector
        var trigger = (SceneTransitionTrigger)target;

        // === Scene Selector ===
        // Allows the user to pick a scene asset from the project
        trigger.sceneAsset = (SceneAsset)EditorGUILayout.ObjectField("Target Scene", trigger.sceneAsset, typeof(SceneAsset), false);

        // If a scene asset is selected, extract its file name (without extension) and store it
        if (trigger.sceneAsset != null)
        {
            string path = AssetDatabase.GetAssetPath(trigger.sceneAsset);
            trigger.targetScene = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        // === Spawn Data Selector ===
        // Allows the user to assign a SceneSpawnData asset that contains the list of spawn point IDs
        trigger.spawnData = (SceneSpawnData)EditorGUILayout.ObjectField("Spawn Data", trigger.spawnData, typeof(SceneSpawnData), false);

        // === Spawn Point Dropdown ===
        // If a valid spawn data asset is assigned and contains spawn point IDs,
        // show a dropdown list to select one
        if (trigger.spawnData != null && trigger.spawnData.spawnPointIDs.Count > 0)
        {
            trigger.selectedSpawnIndex = EditorGUILayout.Popup("Spawn Point", trigger.selectedSpawnIndex, trigger.spawnData.spawnPointIDs.ToArray());
        }
        // If spawn data exists but has no spawn points, display a warning box
        else if (trigger.spawnData != null)
        {
            EditorGUILayout.HelpBox("This asset has no spawn points. Generate it via Tools > Scene Spawn Data Generator.", MessageType.Warning);
        }

        // === Save changes to the object if any value was modified ===
        if (GUI.changed)
        {
            EditorUtility.SetDirty(trigger);
        }
    }
}
