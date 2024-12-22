using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    [Header("Turret Settings")]
    [Tooltip("The range within which the turret detects the player.")]
    public float detectionRange = 15f;

    [Tooltip("Fire rate of the turret (shots per second).")]
    public float fireRate = 1f;

    [Tooltip("Projectile prefab to shoot.")]
    public GameObject projectilePrefab;

    [Tooltip("Spawn point for projectiles.")]
    public Transform projectileSpawnPoint;

    [Tooltip("Speed of the projectile.")]
    public float projectileSpeed = 10f;

    [Tooltip("Time before the projectile despawns.")]
    public float projectileLifetime = 5f;

    private float fireCooldown = 0f;

    void Update()
    {
        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Check if the player is within range
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Rotate to face the player
                Vector3 direction = (player.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                // Shoot at the player
                if (fireCooldown <= 0f)
                {
                    ShootAtPlayer(player);
                    fireCooldown = 1f / fireRate;
                }
            }
        }

        // Update fire cooldown
        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    void ShootAtPlayer(GameObject player)
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Set the projectile's velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (player.transform.position - projectileSpawnPoint.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }

            // Schedule the projectile to despawn
            Destroy(projectile, projectileLifetime);
        }
        else
        {
            Debug.LogWarning("ProjectilePrefab or ProjectileSpawnPoint is not assigned!");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
