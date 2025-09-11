using UnityEditor;
using UnityEngine;
using Items;

[CustomEditor(typeof(DoorObject))]
public class SceneTransitionDoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var door = (DoorObject)target;

        // === Selector de escena visual ===
        door.sceneAsset = (SceneAsset)EditorGUILayout.ObjectField("Escena de destino", door.sceneAsset, typeof(SceneAsset), false);

        if (door.sceneAsset != null)
        {
            string path = AssetDatabase.GetAssetPath(door.sceneAsset);
            door.targetScene = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        // === Selector de datos de spawn ===
        door.spawnData = (SceneSpawnData)EditorGUILayout.ObjectField("Spawn Data", door.spawnData, typeof(SceneSpawnData), false);

        // === Selector de punto de aparición ===
        if (door.spawnData != null && door.spawnData.spawnPointIDs.Count > 0)
        {
            door.selectedSpawnIndex = EditorGUILayout.Popup("Punto de aparición", door.selectedSpawnIndex, door.spawnData.spawnPointIDs.ToArray());
        }
        else if (door.spawnData != null)
        {
            EditorGUILayout.HelpBox("Este asset no contiene puntos de spawn. Usa el generador en Tools > Scene Spawn Data Generator.", MessageType.Warning);
        }
        
        door.requiresKey = EditorGUILayout.Toggle("¿Requiere llave?", door.requiresKey);

        if (door.requiresKey)
        {
            door.requiredKey = (ItemSO)EditorGUILayout.ObjectField("Llave requerida", door.requiredKey, typeof(ItemSO), false);
        }
        
        // Guardar cambios
        if (GUI.changed)
        {
            EditorUtility.SetDirty(door);
        }
    }
}
