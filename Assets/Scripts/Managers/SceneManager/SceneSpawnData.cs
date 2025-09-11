// ==============================
// SceneSpawnData.cs
//  Records all valid spawn points in a scene in a ScriptableObject
// ==============================

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnPoints_", menuName = "Scene Management/Spawn Point Database")]       // Menu to create the ScriptableObject
public class SceneSpawnData : ScriptableObject
{
    public string sceneName;                                    // Name of the scene 
    public List<string> spawnPointIDs = new List<string>();     // List of spawn points IDs
}
