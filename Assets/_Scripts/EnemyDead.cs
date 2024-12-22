using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject enemyPrefab; // The prefab to spawn when the enemy dies

    [Header("Enemy Reference")]
    public CharacterActor enemyCharacter; // The reference to the enemy's CharacterActor script

    [Header("Spawn Settings")]
    public float spawnDelay = 2f; // Delay before spawning the enemy after death

    private void Start()
    {
        // Ensure enemyCharacter is assigned
        if (enemyCharacter == null)
        {
            Debug.LogError("Enemy CharacterActor is not assigned in the spawner!");
        }
    }

    private void Update()
    {
        // Check if the enemy is dead
        if (enemyCharacter != null && enemyCharacter.isDead)
        {
            // Spawn the enemy prefab after a delay
            Invoke(nameof(SpawnEnemy), spawnDelay);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            // Instantiate the enemy prefab at the enemy's position
            Instantiate(enemyPrefab, enemyCharacter.transform.position, Quaternion.identity);

            // Optional: Destroy the old enemy after spawning the new one
            Destroy(enemyCharacter.gameObject);
        }
        else
        {
            Debug.LogError("Enemy prefab is not assigned!");
        }
    }
}
