using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("Array of prefabs to spawn.")]
    public GameObject[] prefabs;

    [Tooltip("Number of prefabs to spawn.")]
    public int numberOfPrefabsToSpawn = 10;

    [Tooltip("Radius within which prefabs will be spawned.")]
    public float spawnRadius = 5f;

    void Start()
    {
        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        if (prefabs.Length == 0)
        {
            Debug.LogWarning("No prefabs assigned to the spawner!");
            return;
        }

        for (int i = 0; i < numberOfPrefabsToSpawn; i++)
        {
            // Pick a random prefab from the array
            GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

            // Determine a random position within the spawn radius
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = transform.position.y; // Keep it on the same vertical level

            // Spawn the prefab
            Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
        }
    }
}
