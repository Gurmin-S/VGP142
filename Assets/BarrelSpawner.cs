using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject prefabToSpawn; // Prefab to spawn
    public float spawnInterval = 0.5f; // Time between spawns
    public float lineLength = 10f; // Length of the line along which prefabs are spawned
    public float prefabLifetime = 5f; // Time after which the spawned prefab is destroyed

    private float spawnTimer;

    void Update()
    {
        // Increment the timer
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn a new prefab
        if (spawnTimer >= spawnInterval)
        {
            SpawnPrefab();
            spawnTimer = 0f; // Reset the timer
        }
    }

    void SpawnPrefab()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Prefab to spawn is not assigned!");
            return;
        }

        // Generate a random position along the line
        float randomOffset = Random.Range(-lineLength / 2f, lineLength / 2f);
        Vector3 spawnPosition = transform.position + transform.right * randomOffset;

        // Calculate the rotation to face the Blue (Z) arrow (forward direction of the spawner)
        Quaternion spawnRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        // Instantiate the prefab with the calculated rotation
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, spawnRotation);

        // Destroy the prefab after its lifetime expires
        Destroy(spawnedPrefab, prefabLifetime);
    }

    // Draw a Gizmo to visualize the spawn line in the Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan; // Set the Gizmo color

        // Draw the line to visualize the spawn area
        Vector3 startPoint = transform.position - transform.right * (lineLength / 2f);
        Vector3 endPoint = transform.position + transform.right * (lineLength / 2f);
        Gizmos.DrawLine(startPoint, endPoint);
    }
}
