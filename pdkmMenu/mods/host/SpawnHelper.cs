using GameNetcodeStuff;
using pdkmMenu;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class SpawnHelper 
{
    private static List<SpawnableEnemyWithRarity> allEnemies = new List<SpawnableEnemyWithRarity>();
    private static List<GameObject> spawnedObjects = new List<GameObject>();

    public static List<SpawnableEnemyWithRarity> GetallEnemies()
    {
        allEnemies.Clear();
        SelectableLevel selectableLevel = StartOfRound.Instance.localPlayerController.quickMenuManager.testAllEnemiesLevel;
        if (selectableLevel == null)
        {
            selectableLevel = StartOfRound.Instance.currentLevel;
        }
        if (selectableLevel == null)
        {
            return allEnemies;
        }

        if (selectableLevel.Enemies != null && selectableLevel.Enemies.Count > 0)
        {
            foreach (var enemy in selectableLevel.Enemies)
            {
                allEnemies.Add(enemy);
            }
        }

        // Add daytime enemies
        if (selectableLevel.DaytimeEnemies != null && selectableLevel.DaytimeEnemies.Count > 0)
        {
            foreach (var enemy in selectableLevel.DaytimeEnemies)
            {
                allEnemies.Add(enemy);
            }
        }

        // Add outside enemies
        if (selectableLevel.OutsideEnemies != null && selectableLevel.OutsideEnemies.Count > 0)
        {
            foreach (var enemy in selectableLevel.OutsideEnemies)
            {
                allEnemies.Add(enemy);
            }
        }
        return allEnemies;
    }
    public static void SpawnEnemy(int index, PlayerControllerB player, bool safe = true)
    {
        if (allEnemies.Count == 0 || index >= allEnemies.Count)
        {
            Plugin.Logger.LogWarning("Invalid enemy index or no enemies available");
            return;
        }

        Vector3 spawnPosition = Vector3.zero;
        if (safe == false)
        {
            spawnPosition = player.transform.position;
        }
        else
        {
            spawnPosition = FindClosestSpawnPoint(player.transform.position);
        }

        if (spawnPosition == Vector3.zero)
        {
            Plugin.Logger.LogWarning("Failed to find valid spawn point");
            return;
        }

        GameObject enemy = UnityEngine.Object.Instantiate(
            allEnemies[index].enemyType.enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        enemy.GetComponentInChildren<NetworkObject>().Spawn(true);
        RoundManager.Instance.SpawnedEnemies.Add(enemy.GetComponent<EnemyAI>());
    }

    private static Vector3 FindClosestSpawnPoint(Vector3 target)
    {
        float minDistance = Mathf.Infinity;
        GameObject closestNode = null;

        // Merge inside and outside AI nodes
        GameObject[] allNodes = RoundManager.Instance.outsideAINodes.Concat(RoundManager.Instance.insideAINodes).ToArray();

        foreach (GameObject navNode in allNodes)
        {
            float distance = Vector3.Distance(target, navNode.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestNode = navNode;
            }
        }

        return closestNode?.transform.position ?? Vector3.zero;
    }
    public static void SpawnObject(int index, Vector3 p)
    {
        // Instantiate the object
        GameObject spawnedObject = UnityEngine.Object.Instantiate<GameObject>(RoundManager.Instance.currentLevel.spawnableMapObjects[index].prefabToSpawn, p, Quaternion.identity, StartOfRound.Instance.propsContainer);

        // Keep track of the spawned object
        spawnedObjects.Add(spawnedObject);

        // Spawn the object (if it's a networked object)
        NetworkObject networkObject = spawnedObject.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Spawn(false);
        }
    }
    public static void DespawnObjects()
    {
        // Iterate through the spawned objects and despawn them
        foreach (GameObject spawnedObject in spawnedObjects)
        {
            NetworkObject networkObject = spawnedObject.GetComponent<NetworkObject>();

            // Check if it's a networked object and despawn it
            if (networkObject != null)
            {
                networkObject.Despawn(true);
            }
            else
            {
                // If it's not a networked object, simply destroy it
                UnityEngine.Object.Destroy(spawnedObject);
            }
        }
        // Clear the list of spawned objects
        spawnedObjects.Clear();
    }

}
